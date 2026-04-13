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
using SoM.Core;
using SoM.Gameplay;
using SoM.Models;

namespace SoM.Controllers {
public class GameplayController : Singleton<GameplayController> {

#region -------------------- Serialized Variables --------------------
    [Header("Stats Elements")]
    [SerializeField] private GameplayStatsSet _statsSet;
#endregion
#region -------------------- Public Variables --------------------
    public GameDatabase SavedGame;

    public Game GameData;

    public GameOptions GameOptions;

    public GameplayStatsSet StatsSet => _statsSet;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        CoreController.Inst.LoadingStepCompleted();
    }

    public void CreateExhibitionGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating a new exhibition game.");

        GameData = new Game
        {
            Id = Guid.NewGuid().ToString(),
            Type = "Exhibition",
            HomeUserType = "User",
            AwayUserType = "Ai",
            PowerplayTeam = "None",
            PossTeam = "None",
            CardsDrawn = 0,
            Period = 1,
            PossPos = new(),
            HomeTeam = null,
            AwayTeam = null,
            Logs = new(),
        };
    }

    public void CreateMultiplayerGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating a new multiplayer game.");

        GameData = new Game
        {
            Id = Guid.NewGuid().ToString(),
            Type = "Multiplayer",
            HomeUserType = "User",
            AwayUserType = "User",
            PowerplayTeam = "None",
            PossTeam = "None",
            CardsDrawn = 0,
            Period = 1,
            PossPos = new(),
            HomeTeam = null,
            AwayTeam = null,
            Logs = new(),
        };
    }

    public void CreateSeasonGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating a new season game.");

        GameData = new Game
        {
            Id = Guid.NewGuid().ToString(),
            Type = "Season",
            HomeUserType = "User",
            AwayUserType = "Ai",
            PowerplayTeam = "None",
            PossTeam = "None",
            CardsDrawn = 0,
            Period = 1,
            PossPos = new(),
            HomeTeam = null,
            AwayTeam = null,
            Logs = new(),
        };
    }

    public void CreatePlayoffGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating a new playoff game.");

        GameData = new Game
        {
            Id = Guid.NewGuid().ToString(),
            Type = "Playoff",
            HomeUserType = "User",
            AwayUserType = "Ai",
            PowerplayTeam = "None",
            PossTeam = "None",
            CardsDrawn = 0,
            Period = 1,
            PossPos = new(),
            HomeTeam = null,
            AwayTeam = null,
            Logs = new(),
        };
    }

    public GameDatabase GetCurrentGameSaveData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the current game.");

        GameDatabase newSavedGame = new GameDatabase
        {
            Id = GameData.Id,
            Type = GameData.Type,
            League = GameData.HomeTeam.Team.League,
            HomeTeam = GameData.HomeTeam.Team.Code,
            AwayTeam = GameData.AwayTeam.Team.Code,
            Period = GameData.Period,
            CardsDrawn = GameData.CardsDrawn,
            HomeSkaterStatsStrings = new(),
            HomeGoalieStatsStrings = new(),
            AwaySkaterStatsStrings = new(),
            AwayGoalieStatsStrings = new(),
        };

        string homeStats = string.Empty;
        string awayStats = string.Empty;

        homeStats += $"{GameData.HomeTeam.Stats.Goals}/";
        homeStats += $"{GameData.HomeTeam.Stats.Shots}/";
        homeStats += $"{GameData.HomeTeam.Stats.PowerplayGoals}/";
        homeStats += $"{GameData.HomeTeam.Stats.Powerplays}/";
        homeStats += $"{GameData.HomeTeam.Stats.ShorthandedGoals}/";
        homeStats += $"{GameData.HomeTeam.Stats.FaceoffsWon}/";
        homeStats += $"{GameData.HomeTeam.Stats.FaceoffsLost}/";
        homeStats += $"{GameData.HomeTeam.Stats.Hits}/";
        homeStats += $"{GameData.HomeTeam.Stats.BlockedShots}/";
        homeStats += $"{GameData.HomeTeam.Stats.Giveaways}/";
        homeStats += $"{GameData.HomeTeam.Stats.Takeaways}";

        awayStats += $"{GameData.AwayTeam.Stats.Goals}/";
        awayStats += $"{GameData.AwayTeam.Stats.Shots}/";
        awayStats += $"{GameData.AwayTeam.Stats.PowerplayGoals}/";
        awayStats += $"{GameData.AwayTeam.Stats.Powerplays}/";
        awayStats += $"{GameData.AwayTeam.Stats.ShorthandedGoals}/";
        awayStats += $"{GameData.AwayTeam.Stats.FaceoffsWon}/";
        awayStats += $"{GameData.AwayTeam.Stats.FaceoffsLost}/";
        awayStats += $"{GameData.AwayTeam.Stats.Hits}/";
        awayStats += $"{GameData.AwayTeam.Stats.BlockedShots}/";
        awayStats += $"{GameData.AwayTeam.Stats.Giveaways}/";
        awayStats += $"{GameData.AwayTeam.Stats.Takeaways}";

        newSavedGame.HomeStatsString = homeStats;
        newSavedGame.AwayStatsString = awayStats;

        foreach (Skater homeSkater in GameData.HomeTeam.SkaterLineup.Values)
        {
            string skaterStats = string.Empty;

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

        foreach (Skater awaySkater in GameData.AwayTeam.SkaterLineup.Values)
        {
            string skaterStats = string.Empty;

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

        foreach (Goalie homeGoalie in GameData.HomeTeam.GoalieLineup.Values)
        {
            string goalieStats = string.Empty;

            goalieStats += $"{homeGoalie.Game.GoalsAgainst}/";
            goalieStats += $"{homeGoalie.Game.ShotsAgainst}/";
            goalieStats += $"{homeGoalie.Game.Assists}/";
            goalieStats += $"{homeGoalie.Game.PenaltyMinutes}";

            newSavedGame.HomeGoalieStatsStrings.Add(goalieStats);
        }

        foreach (Goalie awayGoalie in GameData.AwayTeam.GoalieLineup.Values)
        {
            string goalieStats = string.Empty;

            goalieStats += $"{awayGoalie.Game.GoalsAgainst}/";
            goalieStats += $"{awayGoalie.Game.ShotsAgainst}/";
            goalieStats += $"{awayGoalie.Game.Assists}/";
            goalieStats += $"{awayGoalie.Game.PenaltyMinutes}";

            newSavedGame.AwayGoalieStatsStrings.Add(goalieStats);
        }

        return newSavedGame;
    }

    public void SetGameTeam(Team team, bool isHome)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game team.");

        GameTeam newTeam = new GameTeam
        {
            SkaterLineup = new(),
            GoalieLineup = new(),
            CurrentLine = 1,
            CurrentPair = 1,
            CurrentStrategy = 3,
            NextLine = 1,
            NextPair = 1,
            NextStrategy = 3,
            IsGoaliePulled = false,
            Team = team.Info,
            Stats = team.Game,
        };

        if (isHome)
        {
            GameData.HomeTeam = newTeam;
        }

        else
        {
            GameData.AwayTeam = newTeam;
        }
    }

    public void SetGameTeamLineup(Dictionary<string, Skater> skaters, Goalie goalie, bool isHome)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game team lineup.");

        Dictionary<string, Goalie> goalieLineup = new();
        goalieLineup.Add("G", goalie);

        if (isHome)
        {
            GameData.HomeTeam.SkaterLineup = skaters;
            GameData.HomeTeam.GoalieLineup = goalieLineup;
        }

        else
        {
            GameData.AwayTeam.SkaterLineup = skaters;
            GameData.AwayTeam.GoalieLineup = goalieLineup;
        }
    }

    public Skater GetPossSkater()
    {
        if (GameData.PossTeam == "None") { return null; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the current possession skater.");

        GameTeam possTeam = (GameData.PossTeam == "Home") ? GameData.HomeTeam : GameData.AwayTeam;
        string possPos = GameData.PossPos[GameData.PossPos.Count - 1];
        return possTeam.SkaterLineup[possPos];
    }

    public string GetSkaterPos(Skater skater, string teamString)
    {
        if (skater == null) { return null; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the skater's position.");

        GameTeam team = (teamString == "Home") ? GameData.HomeTeam : GameData.AwayTeam;
        int index = -1;

        for (int i = 0; i < team.SkaterLineup.Count; i++)
        {
            if (team.SkaterLineup.ElementAt(i).Value.Id == skater.Id)
            {
                index = i;
            }
        }

        if (index < 0) { return string.Empty; }
        else { return team.SkaterLineup.ElementAt(index).Key; }
    }

    public Skater GetDefendingSkater()
    {
        if (GameData.PossTeam == "None") { return null; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the current possession skater.");

        GameTeam defendingTeam = (GameData.PossTeam == "Home") ? GameData.AwayTeam : GameData.HomeTeam;
        int defendingLine = defendingTeam.CurrentLine;
        int defendingPair = defendingTeam.CurrentPair;

        string possPos = GameData.PossPos[GameData.PossPos.Count - 1];
        string defendPos = string.Empty;

        if (possPos.Contains("C")) { defendPos = $"C{defendingLine}"; }
        else if (possPos.Contains("LW")) { defendPos = $"RW{defendingLine}"; }
        else if (possPos.Contains("RW")) { defendPos = $"LW{defendingLine}"; }
        else if (possPos.Contains("LD")) { defendPos = $"RW{defendingPair}"; }
        else if (possPos.Contains("RD")) { defendPos = $"LW{defendingPair}"; }

        return defendingTeam.SkaterLineup[defendPos];
    }

    public Goalie GetOpposingGoalie()
    {
        if (GameData.PossTeam == "None") { return null; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the current opposing goalie.");

        GameTeam oppositeTeam = (GameData.PossTeam == "Home") ? GameData.AwayTeam : GameData.HomeTeam;
        return oppositeTeam.GoalieLineup["G"];
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
