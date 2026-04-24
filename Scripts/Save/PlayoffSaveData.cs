// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Game Dependencies
using SoM.Controllers;
using SoM.Models;

namespace SoM.Save {
public class PlayoffSaveData : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public PlayoffDatabase SaveUserPlayoffData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the playoff data.");

        PlayoffDatabase playoffDatabase = new PlayoffDatabase
        {
            Id = PlayoffsController.Inst.PlayoffData.Id,
            League = PlayoffsController.Inst.PlayoffData.League,
            Team = PlayoffsController.Inst.PlayoffData.Team.Team.Code,
            Round = PlayoffsController.Inst.PlayoffData.CurrentRound,
            GameNumber = PlayoffsController.Inst.CurrentNight,
            RoundData = new(),
            SkaterLineup = new(),
            GoalieLineup = new(),
        };

        foreach (PlayoffRound round in PlayoffsController.Inst.PlayoffData.Rounds)
        {
            string roundData = SetRoundDataString(round);

            playoffDatabase.RoundData.Add(roundData);
        }

        foreach (Skater skater in GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Values)
        {
            playoffDatabase.SkaterLineup.Add(skater.Id);
        }

        playoffDatabase.GoalieLineup.Add(GameplayController.Inst.GameData.HomeTeam.GoalieLineup["G"].Id);

        return playoffDatabase;
    }

    public string SaveSkaterPlayoffData(Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the playoff skater data.");

        string skaterData = string.Empty;

        skaterData += UsersController.Inst.UserData.Id + "/";
        skaterData += skater.Playoff.GamesPlayed.ToString() + "/";
        skaterData += skater.Playoff.Goals.ToString() + "/";
        skaterData += skater.Playoff.Assists.ToString() + "/";
        skaterData += skater.Playoff.Points.ToString() + "/";
        skaterData += skater.Playoff.PlusMinus.ToString() + "/";
        skaterData += skater.Playoff.PenaltyMinutes.ToString() + "/";
        skaterData += skater.Playoff.PowerplayGoals.ToString() + "/";
        skaterData += skater.Playoff.PowerplayAssists.ToString() + "/";
        skaterData += skater.Playoff.PowerplayPoints.ToString() + "/";
        skaterData += skater.Playoff.ShorthandedGoals.ToString() + "/";
        skaterData += skater.Playoff.ShorthandedAssists.ToString() + "/";
        skaterData += skater.Playoff.ShorthandedPoints.ToString() + "/";
        skaterData += skater.Playoff.Shots.ToString() + "/";
        skaterData += skater.Playoff.Giveaways.ToString() + "/";
        skaterData += skater.Playoff.Takeaways.ToString() + "/";
        skaterData += skater.Playoff.FaceoffsWon.ToString() + "/";
        skaterData += skater.Playoff.FaceoffsLost.ToString();

        return skaterData;
    }

    public string SaveGoaliePlayoffData(Goalie goalie)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the playoff goalie data.");

        string goalieData = string.Empty;

        goalieData += UsersController.Inst.UserData.Id + "/";
        goalieData += goalie.Playoff.GamesPlayed.ToString() + "/";
        goalieData += goalie.Playoff.Wins.ToString() + "/";
        goalieData += goalie.Playoff.Losses.ToString() + "/";
        goalieData += goalie.Playoff.Shutouts.ToString() + "/";
        goalieData += goalie.Playoff.GoalsAgainst.ToString() + "/";
        goalieData += goalie.Playoff.ShotsAgainst.ToString() + "/";
        goalieData += goalie.Playoff.Assists.ToString() + "/";
        goalieData += goalie.Playoff.PenaltyMinutes.ToString() + "/";
        goalieData += goalie.Playoff.Stamina.ToString();

        return goalieData;
    }

    public string SaveTeamPlayoffData(GameTeam gameTeam)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the playoff team data.");

        string teamData = string.Empty;

        teamData += UsersController.Inst.UserData.Id + "/";
        teamData += gameTeam.Playoff.GamesPlayed.ToString() + "/";
        teamData += gameTeam.Playoff.Wins.ToString() + "/";
        teamData += gameTeam.Playoff.Losses.ToString() + "/";
        teamData += gameTeam.Playoff.Goals.ToString() + "/";
        teamData += gameTeam.Playoff.Shots.ToString() + "/";
        teamData += gameTeam.Playoff.PowerplayGoals.ToString() + "/";
        teamData += gameTeam.Playoff.Powerplays.ToString() + "/";
        teamData += gameTeam.Playoff.ShorthandedGoals.ToString() + "/";
        teamData += gameTeam.Playoff.FaceoffsWon.ToString() + "/";
        teamData += gameTeam.Playoff.FaceoffsLost.ToString() + "/";
        teamData += gameTeam.Playoff.Hits.ToString() + "/";
        teamData += gameTeam.Playoff.BlockedShots.ToString() + "/";
        teamData += gameTeam.Playoff.Giveaways.ToString() + "/";
        teamData += gameTeam.Playoff.Takeaways.ToString();

        return teamData;
    }

    public async Task<List<PlayoffRound>> LoadPlayoffRoundData(PlayoffDatabase playoffData)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the playoff round data.");

        List<PlayoffRound> playoffRounds = new();
        List<string> roundData = playoffData.RoundData;

        ConstantController.LeagueType leagueType = ConstantController.LeagueType.None;

        if (playoffData.League == "NHL") { leagueType = ConstantController.LeagueType.NHL; }
        else { leagueType = ConstantController.LeagueType.PWHL; }

        foreach (string round in roundData)
        {
            PlayoffRound playoffRound = new PlayoffRound
            {
                Teams = new(),
                RoundRecords = new(),
            };

            string[] roundArray = round.Split('/').Trim();

            int roundNumber = Int32.Parse(roundArray[0]);
            string westTeamWithWinsA = roundArray[1]; // Format: "[CODE]-[WINS]"
            string westTeamWithWinsB = roundArray[2]; // Format: "[CODE]-[WINS]"
            string westTeamWithWinsC = roundArray[3]; // Format: "[CODE]-[WINS]"
            string westTeamWithWinsD = roundArray[4]; // Format: "[CODE]-[WINS]"
            string westTeamWithWinsE = roundArray[5]; // Format: "[CODE]-[WINS]"
            string westTeamWithWinsF = roundArray[6]; // Format: "[CODE]-[WINS]"
            string westTeamWithWinsG = roundArray[7]; // Format: "[CODE]-[WINS]"
            string westTeamWithWinsH = roundArray[8]; // Format: "[CODE]-[WINS]"
            string eastTeamWithWinsA = roundArray[9]; // Format: "[CODE]-[WINS]"
            string eastTeamWithWinsB = roundArray[10]; // Format: "[CODE]-[WINS]"
            string eastTeamWithWinsC = roundArray[11]; // Format: "[CODE]-[WINS]"
            string eastTeamWithWinsD = roundArray[12]; // Format: "[CODE]-[WINS]"
            string eastTeamWithWinsE = roundArray[13]; // Format: "[CODE]-[WINS]"
            string eastTeamWithWinsF = roundArray[14]; // Format: "[CODE]-[WINS]"
            string eastTeamWithWinsG = roundArray[15]; // Format: "[CODE]-[WINS]"
            string eastTeamWithWinsH = roundArray[16]; // Format: "[CODE]-[WINS]"

            playoffRound.Round = roundNumber;

            Team westTeamA = !westTeamWithWinsA.Contains("None") ? TeamsController.Inst.GetTeamFromCode(westTeamWithWinsA.Split('-')[0], leagueType) : null;
            Team westTeamB = !westTeamWithWinsB.Contains("None") ? TeamsController.Inst.GetTeamFromCode(westTeamWithWinsB.Split('-')[0], leagueType) : null;
            Team westTeamC = !westTeamWithWinsC.Contains("None") ? TeamsController.Inst.GetTeamFromCode(westTeamWithWinsC.Split('-')[0], leagueType) : null;
            Team westTeamD = !westTeamWithWinsD.Contains("None") ? TeamsController.Inst.GetTeamFromCode(westTeamWithWinsD.Split('-')[0], leagueType) : null;
            Team westTeamE = !westTeamWithWinsE.Contains("None") ? TeamsController.Inst.GetTeamFromCode(westTeamWithWinsE.Split('-')[0], leagueType) : null;
            Team westTeamF = !westTeamWithWinsF.Contains("None") ? TeamsController.Inst.GetTeamFromCode(westTeamWithWinsF.Split('-')[0], leagueType) : null;
            Team westTeamG = !westTeamWithWinsG.Contains("None") ? TeamsController.Inst.GetTeamFromCode(westTeamWithWinsG.Split('-')[0], leagueType) : null;
            Team westTeamH = !westTeamWithWinsH.Contains("None") ? TeamsController.Inst.GetTeamFromCode(westTeamWithWinsH.Split('-')[0], leagueType) : null;
            Team eastTeamA = !eastTeamWithWinsA.Contains("None") ? TeamsController.Inst.GetTeamFromCode(eastTeamWithWinsA.Split('-')[0], leagueType) : null;
            Team eastTeamB = !eastTeamWithWinsB.Contains("None") ? TeamsController.Inst.GetTeamFromCode(eastTeamWithWinsB.Split('-')[0], leagueType) : null;
            Team eastTeamC = !eastTeamWithWinsC.Contains("None") ? TeamsController.Inst.GetTeamFromCode(eastTeamWithWinsC.Split('-')[0], leagueType) : null;
            Team eastTeamD = !eastTeamWithWinsD.Contains("None") ? TeamsController.Inst.GetTeamFromCode(eastTeamWithWinsD.Split('-')[0], leagueType) : null;
            Team eastTeamE = !eastTeamWithWinsE.Contains("None") ? TeamsController.Inst.GetTeamFromCode(eastTeamWithWinsE.Split('-')[0], leagueType) : null;
            Team eastTeamF = !eastTeamWithWinsF.Contains("None") ? TeamsController.Inst.GetTeamFromCode(eastTeamWithWinsF.Split('-')[0], leagueType) : null;
            Team eastTeamG = !eastTeamWithWinsG.Contains("None") ? TeamsController.Inst.GetTeamFromCode(eastTeamWithWinsG.Split('-')[0], leagueType) : null;
            Team eastTeamH = !eastTeamWithWinsH.Contains("None") ? TeamsController.Inst.GetTeamFromCode(eastTeamWithWinsH.Split('-')[0], leagueType) : null;

            if (westTeamA != null) { playoffRound.Teams.Add(westTeamA); }
            if (westTeamB != null) { playoffRound.Teams.Add(westTeamB); }
            if (westTeamC != null) { playoffRound.Teams.Add(westTeamC); }
            if (westTeamD != null) { playoffRound.Teams.Add(westTeamD); }
            if (westTeamE != null) { playoffRound.Teams.Add(westTeamE); }
            if (westTeamF != null) { playoffRound.Teams.Add(westTeamF); }
            if (westTeamG != null) { playoffRound.Teams.Add(westTeamG); }
            if (westTeamH != null) { playoffRound.Teams.Add(westTeamH); }
            if (eastTeamA != null) { playoffRound.Teams.Add(eastTeamA); }
            if (eastTeamB != null) { playoffRound.Teams.Add(eastTeamB); }
            if (eastTeamC != null) { playoffRound.Teams.Add(eastTeamC); }
            if (eastTeamD != null) { playoffRound.Teams.Add(eastTeamD); }
            if (eastTeamE != null) { playoffRound.Teams.Add(eastTeamE); }
            if (eastTeamF != null) { playoffRound.Teams.Add(eastTeamF); }
            if (eastTeamG != null) { playoffRound.Teams.Add(eastTeamG); }
            if (eastTeamH != null) { playoffRound.Teams.Add(eastTeamH); }

            if (roundNumber < 4)
            {
                (int TeamA, int TeamB) westRecordsAB =
                    !westTeamWithWinsA.Contains("None") && !westTeamWithWinsB.Contains("None") ? 
                    (Int32.Parse(westTeamWithWinsA.Split('-')[1]), Int32.Parse(westTeamWithWinsB.Split('-')[1])) : (0, 0);
                
                (int TeamA, int TeamB) westRecordsCD =
                    !westTeamWithWinsC.Contains("None") && !westTeamWithWinsD.Contains("None") ? 
                    (Int32.Parse(westTeamWithWinsC.Split('-')[1]), Int32.Parse(westTeamWithWinsD.Split('-')[1])) : (0, 0);
                
                (int TeamA, int TeamB) westRecordsEF =
                    !westTeamWithWinsE.Contains("None") && !westTeamWithWinsF.Contains("None") ? 
                    (Int32.Parse(westTeamWithWinsE.Split('-')[1]), Int32.Parse(westTeamWithWinsF.Split('-')[1])) : (0, 0);
                
                (int TeamA, int TeamB) westRecordsGH =
                    !westTeamWithWinsG.Contains("None") && !westTeamWithWinsH.Contains("None") ? 
                    (Int32.Parse(westTeamWithWinsG.Split('-')[1]), Int32.Parse(westTeamWithWinsH.Split('-')[1])) : (0, 0);
                
                (int TeamA, int TeamB) eastRecordsAB =
                    !eastTeamWithWinsA.Contains("None") && !eastTeamWithWinsB.Contains("None") ? 
                    (Int32.Parse(eastTeamWithWinsA.Split('-')[1]), Int32.Parse(eastTeamWithWinsB.Split('-')[1])) : (0, 0);
                
                (int TeamA, int TeamB) eastRecordsCD =
                    !eastTeamWithWinsC.Contains("None") && !eastTeamWithWinsD.Contains("None") ? 
                    (Int32.Parse(eastTeamWithWinsC.Split('-')[1]), Int32.Parse(eastTeamWithWinsD.Split('-')[1])) : (0, 0);
                
                (int TeamA, int TeamB) eastRecordsEF =
                    !eastTeamWithWinsE.Contains("None") && !eastTeamWithWinsF.Contains("None") ? 
                    (Int32.Parse(eastTeamWithWinsE.Split('-')[1]), Int32.Parse(eastTeamWithWinsF.Split('-')[1])) : (0, 0);
                
                (int TeamA, int TeamB) eastRecordsGH =
                    !eastTeamWithWinsG.Contains("None") && !eastTeamWithWinsH.Contains("None") ? 
                    (Int32.Parse(eastTeamWithWinsG.Split('-')[1]), Int32.Parse(eastTeamWithWinsH.Split('-')[1])) : (0, 0);
                
                if (westRecordsAB != (0, 0)) { playoffRound.RoundRecords.Add(westRecordsAB); }
                if (westRecordsCD != (0, 0)) { playoffRound.RoundRecords.Add(westRecordsCD); }
                if (westRecordsEF != (0, 0)) { playoffRound.RoundRecords.Add(westRecordsEF); }
                if (westRecordsGH != (0, 0)) { playoffRound.RoundRecords.Add(westRecordsGH); }
                if (eastRecordsAB != (0, 0)) { playoffRound.RoundRecords.Add(eastRecordsAB); }
                if (eastRecordsCD != (0, 0)) { playoffRound.RoundRecords.Add(eastRecordsCD); }
                if (eastRecordsEF != (0, 0)) { playoffRound.RoundRecords.Add(eastRecordsEF); }
                if (eastRecordsGH != (0, 0)) { playoffRound.RoundRecords.Add(eastRecordsGH); }
            }

            else
            {
                (int TeamA, int TeamB) cupRecord =
                    !westTeamWithWinsA.Contains("None") && !eastTeamWithWinsA.Contains("None") ? 
                    (Int32.Parse(westTeamWithWinsA.Split('-')[1]), Int32.Parse(eastTeamWithWinsA.Split('-')[1])) : (0, 0);
                
                playoffRound.RoundRecords.Add(cupRecord);
            }

            playoffRounds.Add(playoffRound);
        }

        return playoffRounds;
    }
#endregion
#region -------------------- Private Methods --------------------
    private string SetRoundDataString(PlayoffRound round)
    {
        string roundString = string.Empty;
        string roundNumString = round.Round.ToString();

        roundString += roundNumString + "/";

        if (round.Round == 4)
        {
            string teamA = round.Teams[0].Info.Code + "-" + round.RoundRecords[0].TeamA.ToString();
            string teamB = round.Teams[1].Info.Code + "-" + round.RoundRecords[0].TeamB.ToString();

            roundString += teamA + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += teamB + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
        }

        else if (round.Round == 3)
        {
            string teamWestA = round.Teams[0].Info.Code + "-" + round.RoundRecords[0].TeamA.ToString();
            string teamWestB = round.Teams[1].Info.Code + "-" + round.RoundRecords[0].TeamB.ToString();
            string teamEastA = round.Teams[2].Info.Code + "-" + round.RoundRecords[1].TeamA.ToString();
            string teamEastB = round.Teams[3].Info.Code + "-" + round.RoundRecords[1].TeamB.ToString();

            roundString += teamWestA + "/";
            roundString += teamWestB + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += teamEastA + "/";
            roundString += teamEastB + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
        }

        else if (round.Round == 2)
        {
            string teamWestA = round.Teams[0].Info.Code + "-" + round.RoundRecords[0].TeamA.ToString();
            string teamWestB = round.Teams[1].Info.Code + "-" + round.RoundRecords[0].TeamB.ToString();
            string teamWestC = round.Teams[2].Info.Code + "-" + round.RoundRecords[1].TeamA.ToString();
            string teamWestD = round.Teams[3].Info.Code + "-" + round.RoundRecords[1].TeamB.ToString();
            string teamEastA = round.Teams[4].Info.Code + "-" + round.RoundRecords[2].TeamA.ToString();
            string teamEastB = round.Teams[5].Info.Code + "-" + round.RoundRecords[2].TeamB.ToString();
            string teamEastC = round.Teams[6].Info.Code + "-" + round.RoundRecords[3].TeamA.ToString();
            string teamEastD = round.Teams[7].Info.Code + "-" + round.RoundRecords[3].TeamB.ToString();

            roundString += teamWestA + "/";
            roundString += teamWestB + "/";
            roundString += teamWestC + "/";
            roundString += teamWestD + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += teamEastA + "/";
            roundString += teamEastB + "/";
            roundString += teamEastC + "/";
            roundString += teamEastD + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
            roundString += "None" + "/";
        }

        else
        {
            string teamWestA = round.Teams[0].Info.Code + "-" + round.RoundRecords[0].TeamA.ToString();
            string teamWestB = round.Teams[1].Info.Code + "-" + round.RoundRecords[0].TeamB.ToString();
            string teamWestC = round.Teams[2].Info.Code + "-" + round.RoundRecords[1].TeamA.ToString();
            string teamWestD = round.Teams[3].Info.Code + "-" + round.RoundRecords[1].TeamB.ToString();
            string teamWestE = round.Teams[4].Info.Code + "-" + round.RoundRecords[2].TeamA.ToString();
            string teamWestF = round.Teams[5].Info.Code + "-" + round.RoundRecords[2].TeamB.ToString();
            string teamWestG = round.Teams[6].Info.Code + "-" + round.RoundRecords[3].TeamA.ToString();
            string teamWestH = round.Teams[7].Info.Code + "-" + round.RoundRecords[3].TeamB.ToString();
            string teamEastA = round.Teams[8].Info.Code + "-" + round.RoundRecords[4].TeamA.ToString();
            string teamEastB = round.Teams[9].Info.Code + "-" + round.RoundRecords[4].TeamB.ToString();
            string teamEastC = round.Teams[10].Info.Code + "-" + round.RoundRecords[5].TeamA.ToString();
            string teamEastD = round.Teams[11].Info.Code + "-" + round.RoundRecords[5].TeamB.ToString();
            string teamEastE = round.Teams[12].Info.Code + "-" + round.RoundRecords[6].TeamA.ToString();
            string teamEastF = round.Teams[13].Info.Code + "-" + round.RoundRecords[6].TeamB.ToString();
            string teamEastG = round.Teams[14].Info.Code + "-" + round.RoundRecords[7].TeamA.ToString();
            string teamEastH = round.Teams[15].Info.Code + "-" + round.RoundRecords[7].TeamB.ToString();

            roundString += teamWestA + "/";
            roundString += teamWestB + "/";
            roundString += teamWestC + "/";
            roundString += teamWestD + "/";
            roundString += teamWestE + "/";
            roundString += teamWestF + "/";
            roundString += teamWestG + "/";
            roundString += teamWestH + "/";
            roundString += teamEastA + "/";
            roundString += teamEastB + "/";
            roundString += teamEastC + "/";
            roundString += teamEastD + "/";
            roundString += teamEastE + "/";
            roundString += teamEastF + "/";
            roundString += teamEastG + "/";
            roundString += teamEastH + "/";
        }

        return roundString;
    }
#endregion
}}
