// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Game Dependencies
using SoM.Controllers;
using SoM.Models;

namespace SoM.Teams {
public class TeamCreation : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private string teamId = string.Empty;

    private SemaphoreSlim createTeamLock = new(1, 1);
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public async Task<Team> CreateTeam(TeamDatabase teamDatabase)
    {
        await createTeamLock.WaitAsync();
        try
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team.");

            teamId = teamDatabase.Id;

            Team newTeam = new Team
            {
                Id = teamDatabase.Id,
            };

            newTeam.Info = await CreateInfo(teamDatabase.InfoString);
            newTeam.Game = await CreateGame();
            newTeam.Season = await CreateSeason(teamDatabase.SeasonStrings ?? new List<string>());
            newTeam.Playoff = await CreatePlayoff(teamDatabase.PlayoffStrings ?? new List<string>());

            CoreController.Inst.WriteLog(this.GetType().Name, $"Team data for {newTeam.Info.CityName} {newTeam.Info.NickName} has been created.");
            return newTeam;
        }
        finally
        {
            createTeamLock.Release();
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private async Task<TeamInfo> CreateInfo(string infoString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team info.");

        string[] infoArray = infoString.Split('/');
        if (infoArray.Length < 4) { return null; }

        TeamInfo newInfo = new TeamInfo
        {
            Code = infoArray[0],
            CityName = infoArray[1],
            NickName = infoArray[2],
            League = infoArray[3],
        };

        return newInfo;
    }

    private async Task<TeamGame> CreateGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team game.");

        TeamGame newGame = new TeamGame
        {
            Goals = 0,
            Shots = 0,
            PowerplayGoals = 0,
            Powerplays = 0,
            ShorthandedGoals = 0,
            FaceoffsWon = 0,
            FaceoffsLost = 0,
            Hits = 0,
            BlockedShots = 0,
            Giveaways = 0,
            Takeaways = 0,
        };

        return newGame;
    }

    private async Task<TeamSeason> CreateSeason(List<string> seasonStrings)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team season.");
        
        if (seasonStrings.Count < 1) { return null; }

        string userSeasonString = string.Empty;

        foreach (string seasonString in seasonStrings)
        {
            string[] seasonArray = seasonString.Split('/');

            if (seasonArray[0] == UsersController.Inst.UserData.Id)
            {
                userSeasonString = seasonString;
            }
        }

        TeamSeason newSeason = new TeamSeason
        {
            UserId = UsersController.Inst.UserData.Id,
            GamesPlayed = 0,
            Wins = 0,
            Losses = 0,
            Ties = 0,
            OTLs = 0,
            Points = 0,
            Goals = 0,
            Shots = 0,
            PowerplayGoals = 0,
            Powerplays = 0,
            ShorthandedGoals = 0,
            FaceoffsWon = 0,
            FaceoffsLost = 0,
            Hits = 0,
            BlockedShots = 0,
            Giveaways = 0,
            Takeaways = 0,
        };

        if (!string.IsNullOrEmpty(userSeasonString))
        {
            string[] userSeasonArray = userSeasonString.Split('/');

            newSeason.GamesPlayed = Int32.Parse(userSeasonArray[1]);
            newSeason.Wins = Int32.Parse(userSeasonArray[2]);
            newSeason.Losses = Int32.Parse(userSeasonArray[3]);
            newSeason.Ties = Int32.Parse(userSeasonArray[4]);
            newSeason.OTLs = Int32.Parse(userSeasonArray[5]);
            newSeason.Points = Int32.Parse(userSeasonArray[6]);
            newSeason.Goals = Int32.Parse(userSeasonArray[7]);
            newSeason.Shots = Int32.Parse(userSeasonArray[8]);
            newSeason.PowerplayGoals = Int32.Parse(userSeasonArray[9]);
            newSeason.Powerplays = Int32.Parse(userSeasonArray[10]);
            newSeason.ShorthandedGoals = Int32.Parse(userSeasonArray[11]);
            newSeason.FaceoffsWon = Int32.Parse(userSeasonArray[12]);
            newSeason.FaceoffsLost = Int32.Parse(userSeasonArray[13]);
            newSeason.Hits = Int32.Parse(userSeasonArray[14]);
            newSeason.BlockedShots = Int32.Parse(userSeasonArray[15]);
            newSeason.Giveaways = Int32.Parse(userSeasonArray[16]);
            newSeason.Takeaways = Int32.Parse(userSeasonArray[17]);
        }

        return newSeason;
    }

    private async Task<TeamPlayoff> CreatePlayoff(List<string> playoffStrings)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team playoff.");
        
        if (playoffStrings.Count < 1) { return null; }

        string userPlayoffString = string.Empty;

        foreach (string playoffString in playoffStrings)
        {
            string[] playoffArray = playoffString.Split('/');

            if (playoffArray[0] == UsersController.Inst.UserData.Id)
            {
                userPlayoffString = playoffString;
            }
        }

        TeamPlayoff newPlayoff = new TeamPlayoff
        {
            UserId = UsersController.Inst.UserData.Id,
            GamesPlayed = 0,
            Wins = 0,
            Losses = 0,
            Goals = 0,
            Shots = 0,
            PowerplayGoals = 0,
            Powerplays = 0,
            ShorthandedGoals = 0,
            FaceoffsWon = 0,
            FaceoffsLost = 0,
            Hits = 0,
            BlockedShots = 0,
            Giveaways = 0,
            Takeaways = 0,
        };

        if (!string.IsNullOrEmpty(userPlayoffString))
        {
            string[] userPlayoffArray = userPlayoffString.Split('/');

            newPlayoff.GamesPlayed = Int32.Parse(userPlayoffArray[1]);
            newPlayoff.Wins = Int32.Parse(userPlayoffArray[2]);
            newPlayoff.Losses = Int32.Parse(userPlayoffArray[3]);
            newPlayoff.Goals = Int32.Parse(userPlayoffArray[4]);
            newPlayoff.Shots = Int32.Parse(userPlayoffArray[5]);
            newPlayoff.PowerplayGoals = Int32.Parse(userPlayoffArray[6]);
            newPlayoff.Powerplays = Int32.Parse(userPlayoffArray[7]);
            newPlayoff.ShorthandedGoals = Int32.Parse(userPlayoffArray[8]);
            newPlayoff.FaceoffsWon = Int32.Parse(userPlayoffArray[9]);
            newPlayoff.FaceoffsLost = Int32.Parse(userPlayoffArray[10]);
            newPlayoff.Hits = Int32.Parse(userPlayoffArray[11]);
            newPlayoff.BlockedShots = Int32.Parse(userPlayoffArray[12]);
            newPlayoff.Giveaways = Int32.Parse(userPlayoffArray[13]);
            newPlayoff.Takeaways = Int32.Parse(userPlayoffArray[14]);
        }
        
        return newPlayoff;
    }
#endregion
}}
