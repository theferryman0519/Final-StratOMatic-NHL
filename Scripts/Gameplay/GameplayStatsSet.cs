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
    }

    public void AddPlusMinus(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to plus/minus.");
        skater.Game.PlusMinus += delta;
    }

    public void AddPenaltyMinute(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to penalty minutes.");
        skater.Game.PenaltyMinutes += delta;
    }

    public void AddShot(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to shots.");
        skater.Game.Shots += delta;
    }

    public void AddGiveaway(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to giveaways.");
        skater.Game.Giveaways += delta;
    }

    public void AddTakeaway(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to takeaways.");
        skater.Game.Takeaways += delta;
    }

    public void AddHit(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to hits.");
        skater.Game.Hits += delta;
    }

    public void AddBlockedShot(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to blocked shot.");
        skater.Game.BlockedShots += delta;
    }
    
    public void AddFaceoffWon(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to faceoff won.");
        skater.Game.FaceoffsWon += delta;
    }

    public void AddFaceoffLost(Skater skater, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to faceoff won.");
        skater.Game.FaceoffsLost += delta;
    }

    public void AddShiftSegment(Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding seconds played.");
        skater.Game.SecondsPlayed += 40;
    }

    public void LowerStamina(Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Lowering stamina.");

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
    }

    public void AddGoaliePenaltyMinute(Goalie goalie, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to penalty minutes.");
        goalie.Game.PenaltyMinutes += delta;
    }
#endregion
}}
