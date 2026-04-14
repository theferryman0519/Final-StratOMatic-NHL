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

namespace SoM.Gameplay {
public class GameplayStatsSet : MonoBehaviour {

#region -------------------- Game Stats Methods --------------------
    public void SetPossTeam(string team)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the possession team.");

        GameplayController.Inst.GameData.PossTeam = team;
    }

    public void ClearPossPos()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Clearing the possession position tracker.");

        GameplayController.Inst.GameData.PossPos.Clear();
    }

    public void AddPossPos(string pos)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to the possession position tracker.");

        GameplayController.Inst.GameData.PossPos.Add(pos);
    }
#endregion
#region -------------------- Team Stats Methods --------------------
    public void AddTeamPlusMinus(bool isHomeTeam, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to full team plus/minus.");

        GameTeam gameTeam = isHomeTeam ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        int lineNum = gameTeam.CurrentLine;
        int pairNum = gameTeam.CurrentPair;

        AddPlusMinus(gameTeam.SkaterLineup[$"C{lineNum}"], delta);
        AddPlusMinus(gameTeam.SkaterLineup[$"LW{lineNum}"], delta);
        AddPlusMinus(gameTeam.SkaterLineup[$"RW{lineNum}"], delta);
        AddPlusMinus(gameTeam.SkaterLineup[$"LD{pairNum}"], delta);
        AddPlusMinus(gameTeam.SkaterLineup[$"RD{pairNum}"], delta);
    }

    public void AddTeamStamina(bool isHomeTeam)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to full team shift stamina.");

        GameTeam gameTeam = isHomeTeam ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        int lineNum = gameTeam.CurrentLine;
        int pairNum = gameTeam.CurrentPair;

        AddShiftSegment(gameTeam.SkaterLineup[$"C{lineNum}"]);
        AddShiftSegment(gameTeam.SkaterLineup[$"LW{lineNum}"]);
        AddShiftSegment(gameTeam.SkaterLineup[$"RW{lineNum}"]);
        AddShiftSegment(gameTeam.SkaterLineup[$"LD{pairNum}"]);
        AddShiftSegment(gameTeam.SkaterLineup[$"RD{pairNum}"]);

        if (GameplayController.Inst.GameOptions.FatigueOn)
        {
            LowerStamina(gameTeam.SkaterLineup[$"C{lineNum}"]);
            LowerStamina(gameTeam.SkaterLineup[$"LW{lineNum}"]);
            LowerStamina(gameTeam.SkaterLineup[$"RW{lineNum}"]);
            LowerStamina(gameTeam.SkaterLineup[$"LD{pairNum}"]);
            LowerStamina(gameTeam.SkaterLineup[$"RD{pairNum}"]);
        }

        for (int l = 1; l < 5; l++)
        {
            if (l != lineNum)
            {
                ResetStamina(gameTeam.SkaterLineup[$"C{l}"]);
                ResetStamina(gameTeam.SkaterLineup[$"LW{l}"]);
                ResetStamina(gameTeam.SkaterLineup[$"RW{l}"]);
            }
        }

        for (int p = 1; p < 4; p++)
        {
            if (p != pairNum)
            {
                ResetStamina(gameTeam.SkaterLineup[$"LD{p}"]);
                ResetStamina(gameTeam.SkaterLineup[$"RD{p}"]);
            }
        }
    }

    public void ResetFullTeamStamina(bool isHomeTeam)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Resetting the full team stamina.");

        GameTeam gameTeam = isHomeTeam ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        foreach (KeyValuePair<string, Skater> skater in gameTeam.SkaterLineup)
        {
            ResetStamina(skater.Value);
        }
    }
#endregion
#region -------------------- Skater Stats Methods --------------------
    public void AddGoal(Skater skater, ConstantController.GoalType goalType, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to goals and points.");
        skater.Game.Goals += delta;
        skater.Game.Points += delta;

        if (goalType == ConstantController.GoalType.Powerplay)
        {
            skater.Game.PowerplayGoals += delta;
            skater.Game.PowerplayPoints += delta;
        }

        else if (goalType == ConstantController.GoalType.Shorthanded)
        {
            skater.Game.ShorthandedGoals += delta;
            skater.Game.ShorthandedPoints += delta;
        }

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddAssist(Skater skater, ConstantController.GoalType goalType, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to assists and points.");
        skater.Game.Assists += delta;
        skater.Game.Points += delta;

        if (goalType == ConstantController.GoalType.Powerplay)
        {
            skater.Game.PowerplayAssists += delta;
            skater.Game.PowerplayPoints += delta;
        }

        else if (goalType == ConstantController.GoalType.Shorthanded)
        {
            skater.Game.ShorthandedAssists += delta;
            skater.Game.ShorthandedPoints += delta;
        }

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddPlusMinus(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to plus/minus.");
        skater.Game.PlusMinus += delta;

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddPenaltyMinute(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to penalty minutes.");
        skater.Game.PenaltyMinutes += delta;

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddShot(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to shots.");
        skater.Game.Shots += delta;

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddGiveaway(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to giveaways.");
        skater.Game.Giveaways += delta;

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddTakeaway(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to takeaways.");
        skater.Game.Takeaways += delta;

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddHit(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to hits.");
        skater.Game.Hits += delta;

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddBlockedShot(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to blocked shot.");
        skater.Game.BlockedShots += delta;

        SetGoalieStats();
        SetTeamStats();
    }
    
    public void AddFaceoffWon(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to faceoff won.");
        skater.Game.FaceoffsWon += delta;

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddFaceoffLost(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to faceoff won.");
        skater.Game.FaceoffsLost += delta;

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddShiftSegment(Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding seconds played.");
        skater.Game.SecondsPlayed += 40;
    }

    public void LowerStamina(Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Lowering stamina.");

        if (GameplayController.Inst.GameOptions.FatigueOn)
        {
            string fatigueRating = skater.Card.Fatigue;

            switch (fatigueRating)
            {
                case "AA": skater.Game.Stamina -= 25; break;
                case "A": skater.Game.Stamina -= 20; break;
                case "B": skater.Game.Stamina -= 15; break;
                case "C": skater.Game.Stamina -= 10; break;
                case "D": skater.Game.Stamina -= 5; break;
            }

            if (skater.Game.Stamina < 0)
            {
                skater.Game.Stamina = 0;
            }
        }
    }

    public void ResetStamina(Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Resetting stamina.");
        skater.Game.Stamina = 100;
    }
#endregion
#region -------------------- Goalie Stats Methods --------------------
    public void AddGoalieAssist(Goalie goalie, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to assists.");
        goalie.Game.Assists += delta;

        SetGoalieStats();
        SetTeamStats();
    }

    public void AddGoaliePenaltyMinute(Goalie goalie, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to penalty minutes.");
        goalie.Game.PenaltyMinutes += delta;

        SetGoalieStats();
        SetTeamStats();
    }
#endregion
#region -------------------- Total Stats Methods --------------------
    public void SetGoalieStats()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting full goalie stats.");

        GoalieGame homeGoalieStats = GameplayController.Inst.GameData.HomeTeam.GoalieLineup.ElementAt(0).Value.Game;
        GoalieGame awayGoalieStats = GameplayController.Inst.GameData.AwayTeam.GoalieLineup.ElementAt(0).Value.Game;

        int homeGoals = 0;
        int awayGoals = 0;

        int homeShots = 0;
        int awayShots = 0;

        List<string> usedSkaterIds = new();

        foreach (Skater skater in GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Values)
        {
            if (!usedSkaterIds.Contains(skater.Id))
            {
                homeGoals += skater.Game.Goals;
                homeShots += skater.Game.Shots;

                usedSkaterIds.Add(skater.Id);
            }
        }

        usedSkaterIds.Clear();

        foreach (Skater skater in GameplayController.Inst.GameData.AwayTeam.SkaterLineup.Values)
        {
            if (!usedSkaterIds.Contains(skater.Id))
            {
                awayGoals += skater.Game.Goals;
                awayShots += skater.Game.Shots;

                usedSkaterIds.Add(skater.Id);
            }
        }

        homeGoalieStats.GoalsAgainst = awayGoals;
        homeGoalieStats.ShotsAgainst = awayShots;

        awayGoalieStats.GoalsAgainst = homeGoals;
        awayGoalieStats.ShotsAgainst = homeShots;
    }

    public void SetTeamStats()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting full team stats.");

        TeamGame homeStats = GameplayController.Inst.GameData.HomeTeam.Stats;
        TeamGame awayStats = GameplayController.Inst.GameData.AwayTeam.Stats;

        int homeGoals = 0;
        int homeShots = 0;
        int homePowerplayGoals = 0;
        int homeShorthandedGoals = 0;
        int homeFaceoffsWon = 0;
        int homeFaceoffsLost = 0;
        int homeHits = 0;
        int homeBlockedShots = 0;
        int homeGiveaways = 0;
        int homeTakeaways = 0;

        List<string> usedSkaterIds = new();

        foreach (Skater skater in GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Values)
        {
            if (!usedSkaterIds.Contains(skater.Id))
            {
                homeGoals += skater.Game.Goals;
                homeShots += skater.Game.Shots;
                homePowerplayGoals += skater.Game.PowerplayGoals;
                homeShorthandedGoals += skater.Game.ShorthandedGoals;
                homeFaceoffsWon += skater.Game.FaceoffsWon;
                homeFaceoffsLost += skater.Game.FaceoffsLost;
                homeHits += skater.Game.Hits;
                homeBlockedShots += skater.Game.BlockedShots;
                homeGiveaways += skater.Game.Giveaways;
                homeTakeaways += skater.Game.Takeaways;

                usedSkaterIds.Add(skater.Id);
            }
        }

        usedSkaterIds.Clear();

        int awayGoals = 0;
        int awayShots = 0;
        int awayPowerplayGoals = 0;
        int awayPowerplays = 0;
        int awayShorthandedGoals = 0;
        int awayFaceoffsWon = 0;
        int awayFaceoffsLost = 0;
        int awayHits = 0;
        int awayBlockedShots = 0;
        int awayGiveaways = 0;
        int awayTakeaways = 0;

        foreach (Skater skater in GameplayController.Inst.GameData.AwayTeam.SkaterLineup.Values)
        {
            if (!usedSkaterIds.Contains(skater.Id))
            {
                awayGoals += skater.Game.Goals;
                awayShots += skater.Game.Shots;
                awayPowerplayGoals += skater.Game.PowerplayGoals;
                awayShorthandedGoals += skater.Game.ShorthandedGoals;
                awayFaceoffsWon += skater.Game.FaceoffsWon;
                awayFaceoffsLost += skater.Game.FaceoffsLost;
                awayHits += skater.Game.Hits;
                awayBlockedShots += skater.Game.BlockedShots;
                awayGiveaways += skater.Game.Giveaways;
                awayTakeaways += skater.Game.Takeaways;

                usedSkaterIds.Add(skater.Id);
            }
        }

        homeStats.Goals = homeGoals;
        homeStats.Shots = homeShots;
        homeStats.PowerplayGoals = homePowerplayGoals;
        homeStats.ShorthandedGoals = homeShorthandedGoals;
        homeStats.FaceoffsWon = homeFaceoffsWon;
        homeStats.FaceoffsLost = homeFaceoffsLost;
        homeStats.Hits = homeHits;
        homeStats.BlockedShots = homeBlockedShots;
        homeStats.Giveaways = homeGiveaways;
        homeStats.Takeaways = homeTakeaways;

        awayStats.Goals = awayGoals;
        awayStats.Shots = awayShots;
        awayStats.PowerplayGoals = awayPowerplayGoals;
        awayStats.ShorthandedGoals = awayShorthandedGoals;
        awayStats.FaceoffsWon = awayFaceoffsWon;
        awayStats.FaceoffsLost = awayFaceoffsLost;
        awayStats.Hits = awayHits;
        awayStats.BlockedShots = awayBlockedShots;
        awayStats.Giveaways = awayGiveaways;
        awayStats.Takeaways = awayTakeaways;
    }
#endregion
}}
