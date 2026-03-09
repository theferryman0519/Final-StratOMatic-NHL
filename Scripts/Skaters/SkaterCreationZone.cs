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

namespace SoM.Skaters {
public class SkaterCreationZone : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private List<string> allSkaterIds = new();
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public async void CreateSkaters()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating all skater data.");

        allSkaterIds = await FirebaseController.Inst.GetAllSkaterIds();

        CreateSkaterData();
    }

    public async void CreateSkaterCards()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating all skater card data.");

        List<SkaterData> allSkaters = new(SkatersController.Inst.FullSkaters);
        SkatersController.Inst.FullSkaters.Clear();

        foreach (SkaterData skater in allSkaters)
        {
            SkaterData tempSkater = skater;

            skater.Card = CreateSkaterCardData(tempSkater);
            skater.Game = CreateSkaterGameData(tempSkater);
            skater.Season = CreateSkaterSeasonData(tempSkater);

            SkatersController.Inst.FullSkaters.Add(skater);
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private async void CreateSkaterData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating all skater main data.");

        foreach (string id in allSkaterIds)
        {
            SkaterData newSkaterData = new SkaterData
            {
                Id = id
            };

            newSkaterData.FirstName = await FirebaseController.Inst.GetSkaterInfo(id, "FirstName");
            newSkaterData.LastName = await FirebaseController.Inst.GetSkaterInfo(id, "LastName");
            newSkaterData.Team = await FirebaseController.Inst.GetSkaterInfo(id, "Team");
            newSkaterData.Position = await FirebaseController.Inst.GetSkaterInfo(id, "Position");
            newSkaterData.Stats = await FirebaseController.Inst.GetSkaterStats(id);

            SkatersController.Inst.FullSkaters.Add(newSkaterData);
        }
    }

    private SkaterCardData CreateSkaterCardData(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating skater card data.");

        SkaterCardData newCardData = new SkaterCardData
        {
            Id = skater.Id,
            Intimidation = SetSkaterIntimidation(skater),
            Passing = SetSkaterPassing(skater),
            Penalty = SetSkaterPenalty(skater),
            Fatigue = SetSkaterFatigue(skater),
            Offense = SetSkaterOffense(skater),
            Defense = SetSkaterDefense(skater),
            Breakaway = SetSkaterBreakaway(skater),
            Faceoff = SetSkaterFaceoff(skater),
            OutsideShotActions = SetSkaterShotActions(skater, "Out"),
            InsideShotActions = SetSkaterShotActions(skater, "In"),
            ReboundShotActions = SetSkaterShotActions(skater, "Reb"),
            PassingActions = SetSkaterPassingActions(skater),
            DefendingActions = SetSkaterDefendingActions(skater),
        };

        return newCardData;
    }

    private SkaterGameData CreateSkaterGameData(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating skater game data.");

        SkaterGameData newGameData = new SkaterGameData
        {
            Id = skater.Id,
            Goals = 0,
            Assists = 0,
            Points = 0,
            PenaltyMinutes = 0,
            PlusMinus = 0,
            Shots = 0,
            PowerplayGoals = 0,
            PowerplayAssists = 0,
            PowerplayPoints = 0,
            ShorthandedGoals = 0,
            ShorthandedAssists = 0,
            ShorthandedPoints = 0,
            Hits = 0,
            BlockedShots = 0,
            Giveaways = 0,
            Takeaways = 0,
            FaceoffsWon = 0,
            FaceoffsLost = 0,
            TimeOnIce = 0,
        };

        return newGameData;
    }

    private SkaterSeasonData CreateSkaterSeasonData(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating skater season data.");

        SkaterSeasonData newSeasonData = new SkaterSeasonData
        {
            Id = skater.Id,
            GamesPlayed = 0,
            Goals = 0,
            Assists = 0,
            Points = 0,
            PenaltyMinutes = 0,
            PlusMinus = 0,
            Shots = 0,
            PowerplayGoals = 0,
            PowerplayAssists = 0,
            PowerplayPoints = 0,
            ShorthandedGoals = 0,
            ShorthandedAssists = 0,
            ShorthandedPoints = 0,
            Hits = 0,
            BlockedShots = 0,
            Giveaways = 0,
            Takeaways = 0,
            FaceoffsWon = 0,
            FaceoffsLost = 0,
            TimeOnIce = 0,
        };

        return newSeasonData;
    }

    private string SetSkaterIntimidation(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater intimidation data.");

        if (skater.Stats.TotalGames < 1) { return "0"; }

        float pimPer = skater.Stats.PenaltyMinutesPer;
        int intimidation = Mathf.RoundToInt(pimPer * 6f);

        if (skater.Position == "D") { intimidation += 1; }

        intimidation = Mathf.Clamp(intimidation, 0, 15);

        if (intimidation == 0) { return "0"; }
        else if (intimidation == 1) { return "1"; }
        else { return $"1-{intimidation}"; }
    }

    private string SetSkaterPassing(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater passing data.");

        if (skater.Stats.TotalGames < 1) { return "None"; }

        float assistsPer = skater.Stats.AssistsPer;
        float points = skater.Stats.PointsPer;
        float assists = skater.Stats.AssistsPer;

        float assistRatio = points > 0 ? assists / points : 0f;
        float passingRaw = (4.5f * assistsPer) + (2f * assistRatio);

        if (skater.Position == "D") { passingRaw += 0.3f; }

        if (passingRaw > 3.5f) { return "L"; }
        else if (passingRaw > 2.5f) { return "K"; }
        else if (passingRaw > 1.5f) { return "J"; }
        else { return "None"; }
    }

    private string SetSkaterPenalty(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater penalty data.");

        if (skater.Stats.TotalGames < 1) { return "D"; }

        float pimPer = skater.Stats.PenaltyMinutesPer;

        if (pimPer > 2.30f) { return "AA"; }
        else if (pimPer > 1.25f) { return "A"; }
        else if (pimPer > 0.60f) { return "B"; }
        else if (pimPer > 0.25f) { return "C"; }
        else { return "D"; }
    }

    private string SetSkaterFatigue(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater fatigue data.");

        if (skater.Stats.TotalGames < 1) { return "C"; }

        float toiMinutes = skater.Stats.AvgTimeOnIce / 60f;
        float gp = skater.Stats.TotalGames;
        float pimPer = skater.Stats.PenaltyMinutesPer;

        float toiScore = Mathf.Clamp01((toiMinutes - 8f) / 20f);
        float gpScore = Mathf.Clamp01(gp / 82f);
        float pimScore = Mathf.Clamp01(pimPer / 2.5f);

        float fatigueRaw = (0.75f * toiScore) + (0.20f * gpScore) - (0.05f * pimScore);

        if (fatigueRaw >= 0.75f) { return "D"; }
        else if (fatigueRaw >= 0.55f) { return "C"; }
        else if (fatigueRaw >= 0.35f) { return "B"; }
        else { return "A"; }
    }

    private int SetSkaterOffense(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater offense data.");

        if (skater.Stats.TotalGames < 1) { return 1; }

        float goalScore = 6f * skater.Stats.GoalsPer;
        float assistScore = 3.5f * skater.Stats.AssistsPer;
        float shotScore = 0.3f * skater.Stats.ShotsPer;
        float ppgScore = 0.75f * skater.Stats.PowerplayGoalsPer;
        float shgScore = 1f * skater.Stats.ShorthandedGoalsPer;

        float offenseScore = goalScore + assistScore + shotScore + ppgScore + shgScore;

        if (offenseScore > 6f) { return 4; }
        else if (offenseScore > 4f) { return 3; }
        else if (offenseScore > 2f) { return 2; }
        else { return 1; }
    }

    private int SetSkaterDefense(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater defense data.");

        if (skater.Stats.TotalGames < 1) { return 1; }

        float plusMinusPer = Mathf.Clamp(skater.Stats.PlusMinusPer, -1f, 1f);
        float pimPer = skater.Stats.PenaltyMinutesPer;
        float goalsPer = skater.Stats.GoalsPer;
        float assistsPer = skater.Stats.AssistsPer;
        float shotsPer = skater.Stats.ShotsPer;

        float expectedPm;

        if (skater.Position == "D") { expectedPm = (0.08f * goalsPer) + (0.08f * assistsPer) + (0.03f * shotsPer); }
        else { expectedPm = (0.35f * goalsPer) + (0.30f * assistsPer) + (0.08f * shotsPer); }

        float defenseRaw = (6f * (plusMinusPer - expectedPm)) - (1.1f * pimPer);

        if (defenseRaw > 0.75f) { return 5; }
        else if (defenseRaw > 0.35f) { return 4; }
        else if (defenseRaw > -0.35f) { return 3; }
        else if (defenseRaw > -0.75f) { return 2; }
        else { return 1; }
    }

    private int SetSkaterBreakaway(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater breakaway data.");

        if (skater.Stats.TotalGames < 1) { return 1; }

        float goalsPer = skater.Stats.GoalsPer;
        float shotsPer = skater.Stats.ShotsPer;
        float shgPer = skater.Stats.ShorthandedGoalsPer;

        float breakawayRaw = (5.5f * goalsPer) + (0.35f * shotsPer) + (3f * shgPer);

        if (breakawayRaw > 4f) { return 4; }
        else if (breakawayRaw > 2.75f) { return 3; }
        else if (breakawayRaw > 1.5f) { return 2; }
        else { return 1; }
    }

    private int SetSkaterFaceoff(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater faceoff data.");

        if (skater.Stats.TotalGames < 1) { return 0; }
        if (skater.Position != "F") { return 0; }

        if (skater.Stats.AssistsPer > 0.5f) { return UnityEngine.Random.Range(2,4); }
        else { return UnityEngine.Random.Range(0,3); }
    }

    private List<string> SetSkaterShotActions(SkaterData skater, string shotType)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater shot action data.");

        // Results for rolls 2 through 12
        List<string> actions = new List<string>()
        {
            "", "", "", "", "", "", "", "", "", "", ""
        };

        // Index map:
        // 0=2, 1=3, 2=4, 3=5, 4=6, 5=7, 6=8, 7=9, 8=10, 9=11, 10=12

        // 2d6 priority excluding 8, from most common to least common
        int[] priorityOrder = new int[] { 7, 6, 9, 5, 10, 4, 11, 3, 12, 2 };

        int RollToIndex(int roll) => roll - 2;

        // Roll 8 is always Rebound
        actions[RollToIndex(8)] = "Rebound";

        if (skater == null || skater.Stats == null || skater.Stats.TotalGames < 1 || skater.Stats.Shots <= 0)
        {
            // Safe fallback
            List<string> fallbackPool = new List<string>()
            {
                "Shot", "Shot", "Shot", "Shot",
                "Lose", "Lose", "Lose", "Lose", "Lose", "Lose"
            };

            for (int i = 0; i < priorityOrder.Length; i++)
            {
                actions[RollToIndex(priorityOrder[i])] = fallbackPool[i];
            }

            return actions;
        }

        float goalsPer = skater.Stats.GoalsPer;
        float assistsPer = skater.Stats.AssistsPer;
        float shotsPer = skater.Stats.ShotsPer;
        float shootingPct = skater.Stats.Shots > 0 ? (float)skater.Stats.Goals / skater.Stats.Shots : 0f;

        int goalCount = 0;
        int goalieRatingCount = 0;
        int loseCount = 0;
        int shotCount = 0;

        int goalBase = 0;

        // Build each column separately
        switch (shotType)
        {
            case "Out":
            {
                // Outside shots: tougher scoring, more goalie pressure, more lose
                if (goalsPer >= 0.55f) goalCount = 2;
                else if (goalsPer >= 0.25f) goalCount = 1;
                else goalCount = 0;

                float pressure = shotsPer + (0.7f * assistsPer);
                if (pressure >= 4.5f) goalieRatingCount = 4;
                else if (pressure >= 3.4f) goalieRatingCount = 3;
                else if (pressure >= 2.2f) goalieRatingCount = 2;
                else goalieRatingCount = 1;

                float control = (0.6f * assistsPer) + (0.3f * shotsPer) + (0.8f * goalsPer);
                if (control >= 2.4f) loseCount = 2;
                else if (control >= 1.5f) loseCount = 3;
                else loseCount = 4;

                shotCount = 10 - goalCount - goalieRatingCount - loseCount;

                goalBase = Mathf.Clamp(
                    Mathf.RoundToInt(20f * ((0.35f * shootingPct) + (0.20f * goalsPer))),
                    2, 10);

                break;
            }

            case "In":
            {
                // Inside shots: best balanced scoring column
                if (goalsPer >= 0.55f) goalCount = 3;
                else if (goalsPer >= 0.35f) goalCount = 2;
                else if (goalsPer >= 0.15f) goalCount = 1;
                else goalCount = 0;

                float pressure = shotsPer + (0.7f * assistsPer);
                if (pressure >= 4.5f) goalieRatingCount = 3;
                else if (pressure >= 3.0f) goalieRatingCount = 2;
                else goalieRatingCount = 1;

                float control = (0.6f * assistsPer) + (0.3f * shotsPer) + (0.8f * goalsPer);
                if (control >= 2.5f) loseCount = 1;
                else if (control >= 1.2f) loseCount = 2;
                else loseCount = 3;

                shotCount = 10 - goalCount - goalieRatingCount - loseCount;

                goalBase = Mathf.Clamp(
                    Mathf.RoundToInt(20f * ((0.50f * shootingPct) + (0.30f * goalsPer))),
                    4, 15);

                break;
            }

            case "Reb":
            {
                // Rebound shots: highest finishing, fewest loses
                if (goalsPer >= 0.55f) goalCount = 3;
                else if (goalsPer >= 0.30f) goalCount = 2;
                else if (goalsPer >= 0.12f) goalCount = 1;
                else goalCount = 0;

                float pressure = shotsPer + (0.7f * assistsPer);
                if (pressure >= 4.8f) goalieRatingCount = 2;
                else if (pressure >= 2.4f) goalieRatingCount = 1;
                else goalieRatingCount = 0;

                float control = (0.6f * assistsPer) + (0.3f * shotsPer) + (0.8f * goalsPer);
                if (control >= 2.2f) loseCount = 1;
                else if (control >= 1.0f) loseCount = 2;
                else loseCount = 3;

                shotCount = 10 - goalCount - goalieRatingCount - loseCount;

                goalBase = Mathf.Clamp(
                    Mathf.RoundToInt(20f * ((0.58f * shootingPct) + (0.34f * goalsPer))),
                    5, 17);

                break;
            }

            default:
            {
                // Unknown type fallback
                shotCount = 4;
                loseCount = 4;
                goalieRatingCount = 2;
                goalCount = 0;
                goalBase = 5;
                break;
            }
        }

        shotCount = Mathf.Clamp(shotCount, 0, 10);
        loseCount = Mathf.Clamp(loseCount, 0, 10);
        goalieRatingCount = Mathf.Clamp(goalieRatingCount, 0, 4);
        goalCount = Mathf.Clamp(goalCount, 0, 3);

        // Build goal strings, strongest first
        List<string> goals = new List<string>();

        for (int i = 0; i < goalCount; i++)
        {
            int val = goalBase;

            if (goalCount == 2)
            {
                val += (i == 0 ? 1 : -1);
            }
            else if (goalCount == 3)
            {
                if (i == 0) val += 2;
                else if (i == 1) val += 0;
                else val -= 2;
            }

            val = Mathf.Clamp(val, 1, 20);
            goals.Add($"Goal 1-{val}");
        }

        goals.Sort((a, b) =>
        {
            int aVal = int.Parse(a.Replace("Goal 1-", ""));
            int bVal = int.Parse(b.Replace("Goal 1-", ""));
            return bVal.CompareTo(aVal);
        });

        // Build result pool for non-8 rolls
        List<string> pool = new List<string>();

        // Strongest first so best results land on 7, 6, 9...
        pool.AddRange(goals);

        for (int i = 0; i < goalieRatingCount; i++)
        {
            pool.Add("Goalie Rating");
        }

        for (int i = 0; i < shotCount; i++)
        {
            pool.Add("Shot");
        }

        for (int i = 0; i < loseCount; i++)
        {
            pool.Add("Lose");
        }

        // Safety: make sure we have exactly 10 results for rolls other than 8
        while (pool.Count < 10)
        {
            pool.Add("Shot");
        }

        while (pool.Count > 10)
        {
            // Trim weakest first
            if (pool.Contains("Lose")) pool.Remove("Lose");
            else if (pool.Contains("Shot")) pool.Remove("Shot");
            else if (pool.Contains("Goalie Rating")) pool.Remove("Goalie Rating");
            else pool.RemoveAt(pool.Count - 1);
        }

        // Assign by roll probability
        for (int i = 0; i < priorityOrder.Length; i++)
        {
            int roll = priorityOrder[i];
            actions[RollToIndex(roll)] = pool[i];
        }

        return actions;
    }

    private List<string> SetSkaterPassingActions(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater passing action data.");
    }

    private List<string> SetSkaterDefendingActions(SkaterData skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater defending action data.");
    }
#endregion
}}
