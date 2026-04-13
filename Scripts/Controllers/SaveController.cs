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
public class SaveController : Singleton<SaveController> {

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
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        CoreController.Inst.LoadingStepCompleted();
    }

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

            goalieStats += $"{homeGoalie.Game.GoalsAgainst}/";
            goalieStats += $"{homeGoalie.Game.ShotsAgainst}/";
            goalieStats += $"{homeGoalie.Game.Assists}/";
            goalieStats += $"{homeGoalie.Game.PenaltyMinutes}";

            newSavedGame.HomeGoalieStatsStrings.Add(goalieStats);
        }

        foreach (Goalie awayGoalie in GameplayController.Inst.GameData.AwayTeam.GoalieLineup.Values)
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
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
