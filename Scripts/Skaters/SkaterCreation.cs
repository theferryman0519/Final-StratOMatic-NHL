// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

namespace SoM.Skaters {
public class SkaterCreation : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private string skaterId = string.Empty;
    private string skaterPos = string.Empty;
    private string skaterPass = string.Empty;
    private string skaterPenalty = string.Empty;

    private int skaterDef = 1;

    private int totalGames = 0;

    private float goalsPerGame = 0f;
    private float assistsPerGame = 0f;
    private float pointsPerGame = 0f;
    private float plusMinusPerGame = 0f;
    private float penaltyMinutesPerGame = 0f;
    private float ppgPerGame = 0f;
    private float shgPerGame = 0f;
    private float shotsPerGame = 0f;
    private float blockedShotsPerGame = 0f;
    private float hitsPerGame = 0f;
    private float faceoffPercentage = 0f;
    private float minutesPerGame = 0f;

    private int[] orderedSums = { 7, 6, 9, 5, 10, 4, 11, 3, 12, 2 };

    private SemaphoreSlim createSkaterLock = new(1, 1);
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public async Task<Skater> CreateSkater(SkaterDatabase skaterDatabase)
    {
        await createSkaterLock.WaitAsync();
        try
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the skater.");

            skaterId = skaterDatabase.Id;
            skaterPos = string.Empty;
            skaterPass = string.Empty;
            skaterPenalty = string.Empty;
            skaterDef = 1;
            totalGames = 0;

            goalsPerGame = 0f;
            assistsPerGame = 0f;
            pointsPerGame = 0f;
            plusMinusPerGame = 0f;
            penaltyMinutesPerGame = 0f;
            ppgPerGame = 0f;
            shgPerGame = 0f;
            shotsPerGame = 0f;
            blockedShotsPerGame = 0f;
            hitsPerGame = 0f;
            faceoffPercentage = 0f;
            minutesPerGame = 0f;

            Skater newSkater = new Skater
            {
                Id = skaterDatabase.Id,
            };

            newSkater.Info = await CreateInfo(skaterDatabase.InfoString);
            newSkater.Game = await CreateGame();
            newSkater.Season = await CreateSeason(skaterDatabase.SeasonString);
            newSkater.Playoff = await CreatePlayoff(skaterDatabase.PlayoffString);
            newSkater.Stats = await CreateStats(skaterDatabase.StatsStrings);
            newSkater.Card = await CreateCard();

            CoreController.Inst.WriteLog(this.GetType().Name, $"Skater data for {newSkater.Info.FirstName} {newSkater.Info.LastName} has been created.");
            return newSkater;
        }
        finally
        {
            createSkaterLock.Release();
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private async Task<SkaterInfo> CreateInfo(string infoString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the skater info.");

        string[] infoArray = infoString.Split('/');
        if (infoArray.Length < 4) { return null; }

        SkaterInfo newInfo = new SkaterInfo
        {
            Id = skaterId,
            FirstName = infoArray[0],
            LastName = infoArray[1],
            Position = infoArray[2],
            Team = infoArray[3],
            League = infoArray[4],
        };

        skaterPos = newInfo.Position;

        return newInfo;
    }

    private async Task<SkaterGame> CreateGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the skater game.");

        SkaterGame newGame = new SkaterGame
        {
            Id = skaterId,
            Goals = 0,
            Assists = 0,
            Points = 0,
            PlusMinus = 0,
            PenaltyMinutes = 0,
            PowerplayGoals = 0,
            PowerplayAssists = 0,
            PowerplayPoints = 0,
            ShorthandedGoals = 0,
            ShorthandedAssists = 0,
            ShorthandedPoints = 0,
            Shots = 0,
            Giveaways = 0,
            Takeaways = 0,
            FaceoffsWon = 0,
            FaceoffsLost = 0,
            SecondsPlayed = 0,
        };

        return newGame;
    }

    private async Task<SkaterSeason> CreateSeason(string seasonString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the skater season.");

        string[] seasonArray = seasonString.Split('/');
        if (seasonArray.Length < 17) { return null; }

        SkaterSeason newSeason = new SkaterSeason
        {
            Id = skaterId,
            GamesPlayed = Int32.Parse(seasonArray[0]),
            Goals = Int32.Parse(seasonArray[1]),
            Assists = Int32.Parse(seasonArray[2]),
            Points = Int32.Parse(seasonArray[3]),
            PlusMinus = Int32.Parse(seasonArray[4]),
            PenaltyMinutes = Int32.Parse(seasonArray[5]),
            PowerplayGoals = Int32.Parse(seasonArray[6]),
            PowerplayAssists = Int32.Parse(seasonArray[7]),
            PowerplayPoints = Int32.Parse(seasonArray[8]),
            ShorthandedGoals = Int32.Parse(seasonArray[9]),
            ShorthandedAssists = Int32.Parse(seasonArray[10]),
            ShorthandedPoints = Int32.Parse(seasonArray[11]),
            Shots = Int32.Parse(seasonArray[12]),
            Giveaways = Int32.Parse(seasonArray[13]),
            Takeaways = Int32.Parse(seasonArray[14]),
            FaceoffsWon = Int32.Parse(seasonArray[15]),
            FaceoffsLost = Int32.Parse(seasonArray[16]),
        };

        return newSeason;
    }

    private async Task<SkaterPlayoff> CreatePlayoff(string playoffString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the skater playoff.");

        string[] playoffArray = playoffString.Split('/');
        if (playoffArray.Length < 17) { return null; }

        SkaterPlayoff newPlayoff = new SkaterPlayoff
        {
            Id = skaterId,
            GamesPlayed = Int32.Parse(playoffArray[0]),
            Goals = Int32.Parse(playoffArray[1]),
            Assists = Int32.Parse(playoffArray[2]),
            Points = Int32.Parse(playoffArray[3]),
            PlusMinus = Int32.Parse(playoffArray[4]),
            PenaltyMinutes = Int32.Parse(playoffArray[5]),
            PowerplayGoals = Int32.Parse(playoffArray[6]),
            PowerplayAssists = Int32.Parse(playoffArray[7]),
            PowerplayPoints = Int32.Parse(playoffArray[8]),
            ShorthandedGoals = Int32.Parse(playoffArray[9]),
            ShorthandedAssists = Int32.Parse(playoffArray[10]),
            ShorthandedPoints = Int32.Parse(playoffArray[11]),
            Shots = Int32.Parse(playoffArray[12]),
            Giveaways = Int32.Parse(playoffArray[13]),
            Takeaways = Int32.Parse(playoffArray[14]),
            FaceoffsWon = Int32.Parse(playoffArray[15]),
            FaceoffsLost = Int32.Parse(playoffArray[16]),
        };

        return newPlayoff;
    }

    private async Task<List<SkaterStats>> CreateStats(List<string> statsStrings)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the skater stats.");

        List<SkaterStats> newStats = new();

        foreach (string year in statsStrings)
        {
            SkaterStats newStat = await CreateSingleStats(year);

            newStats.Add(newStat);
        }

        int totalGoals = 0;
        int totalAssists = 0;
        int totalPoints = 0;
        int totalPlusMinus = 0;
        int totalPenaltyMinutes = 0;
        int totalPPG = 0;
        int totalSHG = 0;
        int totalShots = 0;
        int totalBlockedShots = 0;
        int totalHits = 0;
        int totalFaceoffsWon = 0;
        int totalFaceoffsLost = 0;
        int totalMinutes = 0;

        foreach (SkaterStats stat in newStats)
        {
            totalGames += stat.GamesPlayed;
            totalGoals += stat.Goals;
            totalAssists += stat.Assists;
            totalPoints += stat.Points;
            totalPlusMinus += stat.PlusMinus;
            totalPenaltyMinutes += stat.PenaltyMinutes;
            totalPPG += stat.PowerplayGoals;
            totalSHG += stat.ShorthandedGoals;
            totalShots += stat.Shots;
            totalBlockedShots += stat.BlockedShots;
            totalHits += stat.Hits;
            totalFaceoffsWon += stat.FaceoffsWon;
            totalFaceoffsLost += stat.FaceoffsLost;
            totalMinutes += stat.TotalMinutes;
        }

        if (totalGames > 0)
        {
            goalsPerGame = (float)totalGoals / (float)totalGames;
            assistsPerGame = (float)totalAssists / (float)totalGames;
            pointsPerGame = (float)totalPoints / (float)totalGames;
            plusMinusPerGame = (float)totalPlusMinus / (float)totalGames;
            penaltyMinutesPerGame = (float)totalPenaltyMinutes / (float)totalGames;
            ppgPerGame = (float)totalPPG / (float)totalGames;
            shgPerGame = (float)totalSHG / (float)totalGames;
            shotsPerGame = (float)totalShots / (float)totalGames;
            blockedShotsPerGame = (float)totalBlockedShots / (float)totalGames;
            hitsPerGame = (float)totalHits / (float)totalGames;
            faceoffPercentage = (float)totalFaceoffsWon / (float)(totalFaceoffsWon + totalFaceoffsLost);
            minutesPerGame = (float)totalMinutes / (float)totalGames;
        }

        return newStats;
    }

    private async Task<SkaterStats> CreateSingleStats(string statsString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the skater single season stats.");

        string[] statsArray = statsString.Split('/');
        if (statsArray.Length < 15) { return null; }

        SkaterStats newStats = new SkaterStats
        {
            Id = skaterId,
            Year = Int32.Parse(statsArray[0]),
            GamesPlayed = Int32.Parse(statsArray[1]),
            TotalMinutes = Int32.Parse(statsArray[2]),
            Goals = Int32.Parse(statsArray[3]),
            Assists = Int32.Parse(statsArray[4]),
            Points = Int32.Parse(statsArray[5]),
            PlusMinus = Int32.Parse(statsArray[6]),
            PenaltyMinutes = Int32.Parse(statsArray[7]),
            PowerplayGoals = Int32.Parse(statsArray[8]),
            ShorthandedGoals = Int32.Parse(statsArray[9]),
            Shots = Int32.Parse(statsArray[10]),
            BlockedShots = Int32.Parse(statsArray[11]),
            Hits = Int32.Parse(statsArray[12]),
            FaceoffsWon = Int32.Parse(statsArray[13]),
            FaceoffsLost = Int32.Parse(statsArray[14]),
        };

        return newStats;
    }

    private async Task<SkaterCard> CreateCard()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the skater card.");

        SkaterCard newCard = new SkaterCard
        {
            Id = skaterId,
            Penalty = await SetPenalty(),
            Intimidation = await SetIntimidation(),
            Passing = await SetPassing(),
            Fatigue = await SetFatigue(),
            Offense = await SetOffense(),
            Defense = await SetDefense(),
            Breakaway = await SetBreakaway(),
            Faceoff = await SetFaceoff(),
            OutsideShotActions = await SetShotActions(0),
            InsideShotActions = await SetShotActions(1),
            ReboundShotActions = await SetShotActions(2),
            PassingActions = await SetPassingActions(),
            DefendingActions = await SetDefendingActions(),
        };

        return newCard;
    }

    private async Task<string> SetPenalty()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater penalty.");

        if (totalGames < 1) { skaterPenalty = "D"; return "D"; }

        if (penaltyMinutesPerGame >= 2f) { skaterPenalty = "AA"; return "AA"; }
        if (penaltyMinutesPerGame >= 1.2f) { skaterPenalty = "A"; return "A"; }
        if (penaltyMinutesPerGame >= 0.7f) { skaterPenalty = "B"; return "B"; }
        if (penaltyMinutesPerGame >= 0.3f) { skaterPenalty = "C"; return "C"; }

        skaterPenalty = "D";
        return "D";
    }

    private async Task<string> SetIntimidation()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater intimidation.");
        
        if (totalGames < 1) { return "0"; }
        
        float hits = 2.6f * hitsPerGame;
        float penaltyMinutes = 0.8f * penaltyMinutesPerGame;
        float blockedShots = 0.5f * blockedShotsPerGame;
        float time = 0.08f * minutesPerGame;
        
        float raw = hits + penaltyMinutes + blockedShots + time;
        float rookieFactor = Mathf.Clamp(totalGames, 0f, 20f) / 20f;
        float adj = 1.5f + rookieFactor * (raw - 1.5f);
        
        int scaled = Mathf.Clamp(Mathf.RoundToInt(adj * 0.85f), 0, 15);
        
        if (scaled <= 0) { return "0"; }
        if (scaled == 1) { return "1"; }
        return $"1-{scaled}"; }

    private async Task<string> SetPassing()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater passing.");

        if (totalGames < 1) { skaterPass = "None"; return "None"; }

        float assists = 30f * assistsPerGame;
        float points = 8f * pointsPerGame;
        float shots = 1.2f * shotsPerGame;

        float raw = assists + points + shots;
        float rookieFactor = Mathf.Clamp(totalGames, 0f, 20f) / 20f;
        float adj = 9f + rookieFactor * (raw - 9f);

        if (adj >= 22f) { skaterPass = "L"; return "L"; }
        if (adj >= 15f) { skaterPass = "K"; return "K"; }
        if (adj >= 9f) { skaterPass = "J"; return "J"; }

        skaterPass = "None";
        return "None";
    }

    private async Task<string> SetFatigue()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater fatigue.");

        if (totalGames < 1) { return "B"; }

        if (minutesPerGame >= 18f) { return "D"; }
        if (minutesPerGame >= 14f) { return "C"; }
        if (minutesPerGame >= 10f) { return "B"; }
        if (minutesPerGame >= 6f) { return "A"; }
        return "AA";
    }

    private async Task<int> SetOffense()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater offense.");

        if (totalGames < 1) { return 1; }

        float goals = 32f * goalsPerGame;
        float assists = 20f * assistsPerGame;
        float shots = 4f * shotsPerGame;
        float ppg = 10f * ppgPerGame;
        float time = 0.35f * minutesPerGame;

        float raw = goals + assists + shots + ppg + time;
        float rookieFactor = Mathf.Clamp(totalGames, 0f, 20f) / 20f;
        float adj = 18f + rookieFactor * (raw - 18f);

        if (adj >= 42f) { return 4; }
        if (adj >= 24f) { return 3; }
        if (adj >= 14f) { return 2; }        
        return 1;
    }

    private async Task<int> SetDefense()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater defense.");

        if (totalGames < 1) { skaterDef = 1; return 1; }

        float time = 0.95f * minutesPerGame;
        float blockedShots = 7.5f * blockedShotsPerGame;
        float hits = 2.5f * hitsPerGame;
        float plusMinus = 10f * plusMinusPerGame;
        float penaltyMinutes = -3.5f * penaltyMinutesPerGame;
        float points = 1.5f * pointsPerGame;
        float shots = 0.35f * shotsPerGame;

        float raw = time + blockedShots + hits + plusMinus + penaltyMinutes + points + shots;
        float rookieFactor = Mathf.Clamp(totalGames, 0f, 20f) / 20f;
        float adj = 18f + rookieFactor * (raw - 18f);

        if (adj >= 34f) { skaterDef = 5; return 5; }
        if (adj >= 24f) { skaterDef = 4; return 4; }
        if (adj >= 19f) { skaterDef = 3; return 3; }
        if (adj >= 13f) { skaterDef = 2; return 2; }

        skaterDef = 1;
        return 1;
    }

    private async Task<int> SetBreakaway()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater breakaway.");

        if (totalGames < 1) { return 1; }

        float goals = 20f * goalsPerGame;
        float assists = 10f * assistsPerGame;
        float shots = 3.2f * shotsPerGame;
        float shgs = 18f * shgPerGame;
        float time = 0.45f * minutesPerGame;
        float blockedShots = -0.60f * blockedShotsPerGame;
        float hits = -0.12f * hitsPerGame;

        float raw = goals + assists + shots + shgs + time + blockedShots + hits;
        float rookieFactor = Mathf.Clamp(totalGames, 0f, 20f) / 20f;
        float adj = 14f + rookieFactor * (raw - 14f);

        if (adj >= 21f) { return 4; }
        if (adj >= 15f) { return 3; }
        if (adj >= 9f) { return 2; }
        return 1;
    }

    private async Task<int> SetFaceoff()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater faceoff.");

        if (totalGames < 1) { return 0; }
        if (skaterPos == "D" || skaterPos == "G") { return 0; }

        if (faceoffPercentage >= 0.55f) { return 3; }
        if (faceoffPercentage >= 0.50f) { return 2; }
        if (faceoffPercentage >= 0.45f) { return 1; }
        return 0;
    }

    private async Task<List<string>> SetShotActions(int shot)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater shot actions.");

        if (totalGames < 1)
        {
            return new List<string>() {
                "SHOT", "SHOT", "SHOT", "SHOT", "SHOT", "SHOT", "REBOUND", "SHOT", "SHOT", "SHOT", "SHOT"
            };
        }

        float overallRating = await GetShotOverallRating(shot);
        float finishRating = await GetShotFinishRating(shot);
        float pressureRating = await GetShotPressureRating(shot);

        float shotProgression = 0f;

        if (shot == 0) { shotProgression = -0.05; }
        else if (shot == 2) { shotProgression = 0.05; }

        overallRating += shotProgression;
        finishRating += shotProgression;
        pressureRating += shotProgression;

        int goalCount = await GetShotActionCount(finishRating, 2, 1.15);
        int loseCount = await GetShotActionCount(1.0 - overallRating, 4, 1.10);

        double goalieRatingScore = pressureRating * (1.0 - 0.15 * (goalCount / 4.0));
        int goalieRatingCount = await GetShotActionCount(goalieRatingScore, 2, 1.05);

        int used = goalCount + goalieRatingCount + loseCount;
        if (used > 10)
        {
            int overflow = used - 10;

            int trimGoalie = Math.Min(goalieRatingCount, overflow);
            goalieRatingCount -= trimGoalie;
            overflow -= trimGoalie;

            if (overflow > 0)
            {
                int trimLose = Math.Min(loseCount, overflow);
                loseCount -= trimLose;
                overflow -= trimLose;
            }

            if (overflow > 0)
            {
                int trimGoal = Math.Min(goalCount, overflow);
                goalCount -= trimGoal;
            }
        }

        return await GetShotListActions(goalCount, goalieRatingCount, loseCount);
    }

    private async Task<List<string>> SetPassingActions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater passing actions.");

        if (totalGames < 1)
        {
            return new List<string>() {
                "LOSE", "LOSE", "OUT", "OUT", "LOSE OUT", "LOSE OUT", "LOSE", "OUT", "OUT", "LOSE", "LOSE", "LOSE"
            };
        }

        List<string> passOptions = new() { "LOSE IN", "LOSE OUT", "LOSE", "OUT", "IN" };
        List<string> passingAction = new();

        for (int i = 0; i < 9; i++)
        {
            int index = -1;

            if (skaterPass == "L") { index = Random.Range(2, 5); }
            else if (skaterPass == "K") { index = Random.Range(1, 5); }
            else if (skaterPass == "J") { index = Random.Range(0, 4); }
            else { index = Random.Range(0, 4); }

            passingAction.Add(passOptions[index]);
        }

        if (skaterPass == "L")
        {
            passingAction.Add("IN");
            passingAction.Add("IN");
            passingAction.Add("IN");
        }

        else if (skaterPass == "K")
        {
            passingAction.Add("IN");
            passingAction.Add("IN");
            passingAction.Add("LOSE");
        }

        else if (skaterPass == "J")
        {
            passingAction.Add("IN");
            passingAction.Add("LOSE");
            passingAction.Add("LOSE");
        }

        else
        {
            passingAction.Add("LOSE");
            passingAction.Add("LOSE");
            passingAction.Add("LOSE");
        }

        return passingAction;
    }

    private async Task<List<string>> SetDefendingActions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater defending actions.");

        if (totalGames < 1)
        {
            return new List<string>() {
                "TA-OUT", "TA-OUT", "OUT", "OUT", "IN", "IN", "TA", "TA", "TA", "TA", 
                "PENALTY", "OUT", "IN", "IN"
            };
        }

        List<string> defOptions = new() { "TA-IN", "TA-OUT", "TA", "OUT", "IN" };
        List<string> defActions = new();

        for (int i = 0; i < 7; i++)
        {
            int index = -1;

            if (skaterDef == 5) { index = Random.Range(0, 2); }
            else if (skaterDef == 4) { index = Random.Range(0, 3); }
            else if (skaterDef == 3) { index = Random.Range(1, 4); }
            else if (skaterDef == 2) { index = Random.Range(2, 5); }
            else { index = Random.Range(3, 5); }

            defActions.Add(defOptions[index]);
        }

        if (skaterPenalty == "AA")
        {
            defActions.Add("PENALTY"); defActions.Add("PENALTY"); defActions.Add("PENALTY"); defActions.Add("PENALTY");
        }

        else if (skaterPenalty == "A")
        {
            defActions.Add("TA"); defActions.Add("PENALTY"); defActions.Add("PENALTY"); defActions.Add("PENALTY");
        }

        else if (skaterPenalty == "B")
        {
            defActions.Add("TA"); defActions.Add("TA"); defActions.Add("PENALTY"); defActions.Add("PENALTY");
        }

        else if (skaterPenalty == "C")
        {
            defActions.Add("TA"); defActions.Add("TA"); defActions.Add("TA"); defActions.Add("PENALTY");
        }

        else
        {
            defActions.Add("TA"); defActions.Add("TA"); defActions.Add("TA"); defActions.Add("TA");
        }

        defActions.Add("PENALTY");

        for (int i = 0; i < 3; i++)
        {
            int index = -1;

            if (skaterDef == 5) { index = Random.Range(0, 2); }
            else if (skaterDef == 4) { index = Random.Range(0, 3); }
            else if (skaterDef == 3) { index = Random.Range(1, 4); }
            else if (skaterDef == 2) { index = Random.Range(2, 5); }
            else { index = Random.Range(3, 5); }

            defActions.Add(defOptions[index]);
        }

        return defActions;
    }

    private async Task<float> GetShotOverallRating(int shot)
    {
        List<float> goalMultipliers = new() { 0.3f, 0.35f, 0.4f };
        List<float> shotMultipliers = new() { 0.15f, 0.2f, 0.25f };
        List<float> assistMultipliers = new() { 0.1f, 0.15f, 0.2f };
        List<float> pointMultipliers = new() { 0.1f, 0.15f, 0.2f };

        float goals = goalsPerGame * goalMultipliers[shot];
        float shots = shotsPerGame * shotMultipliers[shot];
        float assists = assistsPerGame * assistMultipliers[shot];
        float points = pointsPerGame * pointMultipliers[shot];

        return goals + shots + assists + points;
    }

    private async Task<float> GetShotFinishRating(int shot)
    {
        List<float> goalMultipliers = new() { 0.4f, 0.45f, 0.5f };
        List<float> pointMultipliers = new() { 0.15f, 0.2f, 0.25f };

        float goals = goalsPerGame * goalMultipliers[shot];
        float points = pointsPerGame * pointMultipliers[shot];

        return goals + points;
    }

    private async Task<float> GetShotPressureRating(int shot)
    {
        List<float> goalMultipliers = new() { 0.25f, 0.3f, 0.35f };
        List<float> shotMultipliers = new() { 0.35f, 0.4f, 0.45f };
        List<float> pointMultipliers = new() { 0.25f, 0.3f, 0.35f };

        float goals = goalsPerGame * goalMultipliers[shot];
        float shots = shotsPerGame * shotMultipliers[shot];
        float points = pointsPerGame * pointMultipliers[shot];

        return goals + shots + points;
    }

    private async Task<int> GetShotActionCount(double score, int maxCount, double exponent = 1.0)
    {
        score = Math.Max(0.0, Math.Min(1.0, score));

        double curved = Math.Pow(score, exponent);

        return (int)Math.Round(curved * maxCount, MidpointRounding.AwayFromZero);
    }

    private async Task<List<string>> GetShotListActions(int goalCount, int goalieRatingCount, int loseCount)
    {
        Dictionary<int, string> result = Enumerable.Range(2, 11).ToDictionary(x => x, _ => "SHOT");

        result[8] = "REBOUND";

        HashSet<int> available = new HashSet<int>(orderedSums);

        double fixedCenter = pointsPerGame / 1.5f;
        double goalCenter = PositiveCenter(fixedCenter);
        double goalieCenter = PositiveCenter(fixedCenter * 0.50);
        double loseCenter = NegativeCenter(fixedCenter);

        foreach (int sum in TakeClosestAvailable(goalCount, goalCenter, available))
        {
            result[sum] = "GOAL";
        }

        foreach (int sum in TakeClosestAvailable(goalieRatingCount, goalieCenter, available))
        {
            result[sum] = "GOALIE RATING";
        }

        foreach (int sum in TakeClosestAvailable(loseCount, loseCenter, available))
        {
            result[sum] = "LOSE";
        }

        return result.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
    }

    private List<int> TakeClosestAvailable(int count, double center, HashSet<int> available)
    {
        List<int> chosen = new();
        if (count <= 0) { return chosen; }

        var orderedIndices = Enumerable.Range(0, orderedSums.Length).OrderBy(i => Math.Abs(i - center)).ThenBy(i => i);

        foreach (int index in orderedIndices)
        {
            int sum = orderedSums[index];
            if (!available.Contains(sum)) { continue; }

            available.Remove(sum);
            chosen.Add(sum);

            if (chosen.Count >= count) { break; }
        }

        return chosen;
    }

    private double PositiveCenter(double rating)
    {
        return (1.0 - Math.Max(0.0, Math.Min(1.0, rating))) * (orderedSums.Length - 1);
    }

    private double NegativeCenter(double rating)
    {
        return Math.Max(0.0, Math.Min(1.0, rating)) * (orderedSums.Length - 1);
    }
#endregion
}}
