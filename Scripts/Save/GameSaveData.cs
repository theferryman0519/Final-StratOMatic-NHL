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
public class GameSaveData : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public GameDatabase SavedGame;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public GameDatabase GetCurrentGameSaveData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the current game.");

        GameDatabase newSavedGame = new GameDatabase
        {
            Id = GameplayController.Inst.GameData.Id,
            Type = GameplayController.Inst.GameData.Type,
            League = GameplayController.Inst.GameData.HomeTeam.Team.League,
            HomeTeam = GameplayController.Inst.GameData.HomeTeam.Team.Code,
            AwayTeam = GameplayController.Inst.GameData.AwayTeam.Team.Code,
            Period = GameplayController.Inst.GameData.Period,
            CardsDrawn = GameplayController.Inst.GameData.CardsDrawn,
            Possession = GameplayController.Inst.GameData.PossTeam,
            HomeSkaterStatsStrings = new(),
            HomeGoalieStatsStrings = new(),
            AwaySkaterStatsStrings = new(),
            AwayGoalieStatsStrings = new(),
        };

        string homeStats = string.Empty;
        string awayStats = string.Empty;

        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.Goals}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.Shots}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.PowerplayGoals}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.Powerplays}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.ShorthandedGoals}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.FaceoffsWon}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.FaceoffsLost}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.Hits}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.BlockedShots}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.Giveaways}/";
        homeStats += $"{GameplayController.Inst.GameData.HomeTeam.Stats.Takeaways}";

        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.Goals}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.Shots}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.PowerplayGoals}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.Powerplays}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.ShorthandedGoals}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.FaceoffsWon}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.FaceoffsLost}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.Hits}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.BlockedShots}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.Giveaways}/";
        awayStats += $"{GameplayController.Inst.GameData.AwayTeam.Stats.Takeaways}";

        newSavedGame.HomeStatsString = homeStats;
        newSavedGame.AwayStatsString = awayStats;

        foreach (Skater homeSkater in GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Values)
        {
            string skaterStats = string.Empty;

            skaterStats += $"{homeSkater.Id}/";
            skaterStats += $"{homeSkater.Game.Goals}/";
            skaterStats += $"{homeSkater.Game.Assists}/";
            skaterStats += $"{homeSkater.Game.Points}/";
            skaterStats += $"{homeSkater.Game.PlusMinus}/";
            skaterStats += $"{homeSkater.Game.PenaltyMinutes}/";
            skaterStats += $"{homeSkater.Game.PowerplayGoals}/";
            skaterStats += $"{homeSkater.Game.PowerplayAssists}/";
            skaterStats += $"{homeSkater.Game.PowerplayPoints}/";
            skaterStats += $"{homeSkater.Game.ShorthandedGoals}/";
            skaterStats += $"{homeSkater.Game.ShorthandedAssists}/";
            skaterStats += $"{homeSkater.Game.ShorthandedPoints}/";
            skaterStats += $"{homeSkater.Game.Shots}/";
            skaterStats += $"{homeSkater.Game.Giveaways}/";
            skaterStats += $"{homeSkater.Game.Takeaways}/";
            skaterStats += $"{homeSkater.Game.Hits}/";
            skaterStats += $"{homeSkater.Game.BlockedShots}/";
            skaterStats += $"{homeSkater.Game.FaceoffsWon}/";
            skaterStats += $"{homeSkater.Game.FaceoffsLost}/";
            skaterStats += $"{homeSkater.Game.SecondsPlayed}/";
            skaterStats += $"{homeSkater.Game.Stamina}";

            newSavedGame.HomeSkaterStatsStrings.Add(skaterStats);
        }

        foreach (Skater awaySkater in GameplayController.Inst.GameData.AwayTeam.SkaterLineup.Values)
        {
            string skaterStats = string.Empty;

            skaterStats += $"{awaySkater.Id}/";
            skaterStats += $"{awaySkater.Game.Goals}/";
            skaterStats += $"{awaySkater.Game.Assists}/";
            skaterStats += $"{awaySkater.Game.Points}/";
            skaterStats += $"{awaySkater.Game.PlusMinus}/";
            skaterStats += $"{awaySkater.Game.PenaltyMinutes}/";
            skaterStats += $"{awaySkater.Game.PowerplayGoals}/";
            skaterStats += $"{awaySkater.Game.PowerplayAssists}/";
            skaterStats += $"{awaySkater.Game.PowerplayPoints}/";
            skaterStats += $"{awaySkater.Game.ShorthandedGoals}/";
            skaterStats += $"{awaySkater.Game.ShorthandedAssists}/";
            skaterStats += $"{awaySkater.Game.ShorthandedPoints}/";
            skaterStats += $"{awaySkater.Game.Shots}/";
            skaterStats += $"{awaySkater.Game.Giveaways}/";
            skaterStats += $"{awaySkater.Game.Takeaways}/";
            skaterStats += $"{awaySkater.Game.Hits}/";
            skaterStats += $"{awaySkater.Game.BlockedShots}/";
            skaterStats += $"{awaySkater.Game.FaceoffsWon}/";
            skaterStats += $"{awaySkater.Game.FaceoffsLost}/";
            skaterStats += $"{awaySkater.Game.SecondsPlayed}/";
            skaterStats += $"{awaySkater.Game.Stamina}";

            newSavedGame.AwaySkaterStatsStrings.Add(skaterStats);
        }

        foreach (Goalie homeGoalie in GameplayController.Inst.GameData.HomeTeam.GoalieLineup.Values)
        {
            string goalieStats = string.Empty;

            goalieStats += $"{homeGoalie.Id}/";
            goalieStats += $"{homeGoalie.Game.GoalsAgainst}/";
            goalieStats += $"{homeGoalie.Game.ShotsAgainst}/";
            goalieStats += $"{homeGoalie.Game.Assists}/";
            goalieStats += $"{homeGoalie.Game.PenaltyMinutes}";

            newSavedGame.HomeGoalieStatsStrings.Add(goalieStats);
        }

        foreach (Goalie awayGoalie in GameplayController.Inst.GameData.AwayTeam.GoalieLineup.Values)
        {
            string goalieStats = string.Empty;

            goalieStats += $"{awayGoalie.Id}/";
            goalieStats += $"{awayGoalie.Game.GoalsAgainst}/";
            goalieStats += $"{awayGoalie.Game.ShotsAgainst}/";
            goalieStats += $"{awayGoalie.Game.Assists}/";
            goalieStats += $"{awayGoalie.Game.PenaltyMinutes}";

            newSavedGame.AwayGoalieStatsStrings.Add(goalieStats);
        }

        return newSavedGame;
    }

    public Game LoadGameFromSaveData(GameDatabase loadGame)
    {
        if (SavedGame != null) { return null; }

        SavedGame = loadGame;

        Game newGame = new Game
        {
            Id = SavedGame.Id,
            Type = SavedGame.Type,
            HomeUserType = "User",
            AwayUserType = "Ai",
            PowerplayTeam = "None",
            PossTeam = SavedGame.Possession,
            CardsDrawn = SavedGame.CardsDrawn,
            Period = SavedGame.Period,
            PossPos = new(),
            Logs = new(),
        };

        ConstantController.LeagueType league = ConstantController.LeagueType.None;

        if (SavedGame.League == "NHL") { league = ConstantController.LeagueType.NHL; }
        else if (SavedGame.League == "NHLFranchise") { league = ConstantController.LeagueType.NHLFranchise; }
        else if (SavedGame.League == "PWHL") { league = ConstantController.LeagueType.PWHL; }
        else { league = ConstantController.LeagueType.PWHLFranchise; }

        Team mainHomeTeam = TeamsController.Inst.GetTeamFromCode(SavedGame.HomeTeam, league);
        Team mainAwayTeam = TeamsController.Inst.GetTeamFromCode(SavedGame.AwayTeam, league);

        GameTeam homeTeam = new GameTeam
        {
            CurrentLine = 1,
            CurrentPair = 1,
            CurrentStrategy = 3,
            NextLine = 1,
            NextPair = 1,
            NextStrategy = 3,
            IsGoaliePulled = false,
            Team = mainHomeTeam.Info,
            SkaterLineup = SetSkaterData(SavedGame.HomeSkaterStatsStrings, true),
            GoalieLineup = SetGoalieData(SavedGame.HomeGoalieStatsStrings, true),
            Stats = SetTeamData(SavedGame.HomeStatsString),
        };

        GameTeam awayTeam = new GameTeam
        {
            CurrentLine = 1,
            CurrentPair = 1,
            CurrentStrategy = 3,
            NextLine = 1,
            NextPair = 1,
            NextStrategy = 3,
            IsGoaliePulled = false,
            Team = mainAwayTeam.Info,
            SkaterLineup = SetSkaterData(SavedGame.AwaySkaterStatsStrings, false),
            GoalieLineup = SetGoalieData(SavedGame.AwayGoalieStatsStrings, false),
            Stats = SetTeamData(SavedGame.AwayStatsString),
        };

        newGame.HomeTeam = homeTeam;
        newGame.AwayTeam = awayTeam;

        return newGame;
    }
#endregion
#region -------------------- Private Methods --------------------
    private Dictionary<string, Skater> SetSkaterData(List<string> skaterStats, bool isHome)
    {
        Dictionary<string, Skater> setSkaters = new();

        List<string> posList = new() { "C1", "LW1", "RW1", "C2", "LW2", "RW2", "C3", "LW3", "RW3", "C4", "LW4", "RW4",
            "LD1", "RD1", "LD2", "RD2", "LD3", "RD3"};

        Dictionary<string, List<Skater>> leagueSkaters = new();
        List<Skater> teamSkaters = new();

        if (SavedGame.League == "NHL") { leagueSkaters = new(SkatersController.Inst.NhlSkaters); }
        else if (SavedGame.League == "NHLFranchise") { leagueSkaters = new(SkatersController.Inst.NhlFranchiseSkaters); }
        else if (SavedGame.League == "PWHL") { leagueSkaters = new(SkatersController.Inst.PwhlSkaters); }
        else { leagueSkaters = new(SkatersController.Inst.PwhlFranchiseSkaters); }

        teamSkaters = leagueSkaters[isHome ? SavedGame.HomeTeam : SavedGame.AwayTeam];

        for (int i = 0; i < skaterStats.Count; i++)
        {
            int index = i;
            string[] statArray = skaterStats[index].Split('/');

            Skater skater = teamSkaters.FirstOrDefault(s => s.Id == statArray[0]);

            skater.Game = new SkaterGame
            {
                Goals = Int32.Parse(statArray[1]),
                Assists = Int32.Parse(statArray[2]),
                Points = Int32.Parse(statArray[3]),
                PlusMinus = Int32.Parse(statArray[4]),
                PenaltyMinutes = Int32.Parse(statArray[5]),
                PowerplayGoals = Int32.Parse(statArray[6]),
                PowerplayAssists = Int32.Parse(statArray[7]),
                PowerplayPoints = Int32.Parse(statArray[8]),
                ShorthandedGoals = Int32.Parse(statArray[9]),
                ShorthandedAssists = Int32.Parse(statArray[10]),
                ShorthandedPoints = Int32.Parse(statArray[11]),
                Shots = Int32.Parse(statArray[12]),
                Giveaways = Int32.Parse(statArray[13]),
                Takeaways = Int32.Parse(statArray[14]),
                Hits = Int32.Parse(statArray[15]),
                BlockedShots = Int32.Parse(statArray[16]),
                FaceoffsWon = Int32.Parse(statArray[17]),
                FaceoffsLost = Int32.Parse(statArray[18]),
                SecondsPlayed = Int32.Parse(statArray[19]),
                Stamina = Int32.Parse(statArray[20]),
            };

            setSkaters.Add(posList[index], skater);
        }

        return setSkaters;
    }

    private Dictionary<string, Goalie> SetGoalieData(List<string> goalieStats, bool isHome)
    {
        Dictionary<string, Goalie> setGoalies = new();
        Dictionary<string, List<Goalie>> leagueGoalies = new();
        List<Goalie> teamGoalies = new();

        if (SavedGame.League == "NHL") { leagueGoalies = new(GoaliesController.Inst.NhlGoalies); }
        else if (SavedGame.League == "NHLFranchise") { leagueGoalies = new(GoaliesController.Inst.NhlFranchiseGoalies); }
        else if (SavedGame.League == "PWHL") { leagueGoalies = new(GoaliesController.Inst.PwhlGoalies); }
        else { leagueGoalies = new(GoaliesController.Inst.PwhlFranchiseGoalies); }

        teamGoalies = leagueGoalies[isHome ? SavedGame.HomeTeam : SavedGame.AwayTeam];

        for (int i = 0; i < goalieStats.Count; i++)
        {
            int index = i;
            string[] statArray = goalieStats[index].Split('/');

            Goalie goalie = teamGoalies.FirstOrDefault(s => s.Id == statArray[0]);

            goalie.Game = new GoalieGame
            {
                GoalsAgainst = Int32.Parse(statArray[1]),
                ShotsAgainst = Int32.Parse(statArray[2]),
                Assists = Int32.Parse(statArray[3]),
                PenaltyMinutes = Int32.Parse(statArray[4]),
            };

            setGoalies.Add("G", goalie);
        }

        return setGoalies;
    }

    private TeamGame SetTeamData(string teamStats)
    {
        string[] statArray = teamStats.Split('/');

        TeamGame newTeamGame = new TeamGame
        {
            Goals = Int32.Parse(statArray[0]),
            Shots = Int32.Parse(statArray[1]),
            PowerplayGoals = Int32.Parse(statArray[2]),
            Powerplays = Int32.Parse(statArray[3]),
            ShorthandedGoals = Int32.Parse(statArray[4]),
            FaceoffsWon = Int32.Parse(statArray[5]),
            FaceoffsLost = Int32.Parse(statArray[6]),
            Hits = Int32.Parse(statArray[7]),
            BlockedShots = Int32.Parse(statArray[8]),
            Giveaways = Int32.Parse(statArray[9]),
            Takeaways = Int32.Parse(statArray[10]),
        };

        return newTeamGame;
    }
#endregion
}}
