// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Game Dependencies
using SoM.Core;
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
        };
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
