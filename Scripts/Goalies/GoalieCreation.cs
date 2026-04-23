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
using Random = UnityEngine.Random;

// Game Dependencies
using SoM.Controllers;
using SoM.Models;

namespace SoM.Goalies {
public class GoalieCreation : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private string goalieId = string.Empty;
    private string goaliePenalty = string.Empty;

    private int totalGames = 0;

    private float winPercentage = 0f;
    private float goalsAgainstPerGame = 0f;
    private float shotsAgainstPerGame = 0f;
    private float assistsPerGame = 0f;
    private float penaltyMinutesPerGame = 0f;

    private int[] orderedSums = { 7, 6, 8, 9, 5, 10, 4, 11, 3, 12, 2 };

    private SemaphoreSlim createGoalieLock = new(1, 1);
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public async Task<Goalie> CreateGoalie(GoalieDatabase goalieDatabase)
    {
        await createGoalieLock.WaitAsync();
        try
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie.");

            goalieId = goalieDatabase.Id;
            goaliePenalty = string.Empty;
            totalGames = 0;

            winPercentage = 0f;
            goalsAgainstPerGame = 0f;
            shotsAgainstPerGame = 0f;
            assistsPerGame = 0f;
            penaltyMinutesPerGame = 0f;

            Goalie newGoalie = new Goalie
            {
                Id = goalieDatabase.Id,
            };

            newGoalie.Info = await CreateInfo(goalieDatabase.InfoString);
            newGoalie.Game = await CreateGame();
            newGoalie.Season = await CreateSeason(goalieDatabase.SeasonStrings ?? new List<string>());
            newGoalie.Playoff = await CreatePlayoff(goalieDatabase.PlayoffStrings ?? new List<string>());
            newGoalie.Stats = await CreateStats(goalieDatabase.StatsStrings);
            newGoalie.Card = await CreateCard();
            newGoalie.WinPercentage = winPercentage;

            CoreController.Inst.WriteLog(this.GetType().Name, $"Goalie data for {newGoalie.Info.FirstName} {newGoalie.Info.LastName} has been created.");
            return newGoalie;
        }
        finally
        {
            createGoalieLock.Release();
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private async Task<GoalieInfo> CreateInfo(string infoString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie info.");

        string[] infoArray = infoString.Split('/');
        if (infoArray.Length < 4) { return null; }

        GoalieInfo newInfo = new GoalieInfo
        {
            FirstName = infoArray[0],
            LastName = infoArray[1],
            Team = infoArray[2],
            League = infoArray[3],
        };

        return newInfo;
    }

    private async Task<GoalieGame> CreateGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie game.");

        GoalieGame newGame = new GoalieGame
        {
            GoalsAgainst = 0,
            ShotsAgainst = 0,
            Assists = 0,
            PenaltyMinutes = 0,
        };

        return newGame;
    }

    private async Task<GoalieSeason> CreateSeason(List<string> seasonStrings)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie season.");
        
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
        
        GoalieSeason newSeason = new GoalieSeason
        {
            UserId = UsersController.Inst.UserData.Id,
            GamesPlayed = 0,
            Wins = 0,
            Losses = 0,
            Shutouts = 0,
            GoalsAgainst = 0,
            ShotsAgainst = 0,
            Assists = 0,
            PenaltyMinutes = 0,
            Stamina = 100,
        };

        if (!string.IsNullOrEmpty(userSeasonString))
        {
            string[] userSeasonArray = userSeasonString.Split('/');

            newSeason.GamesPlayed = Int32.Parse(userSeasonArray[1]);
            newSeason.Wins = Int32.Parse(userSeasonArray[2]);
            newSeason.Losses = Int32.Parse(userSeasonArray[3]);
            newSeason.Shutouts = Int32.Parse(userSeasonArray[4]);
            newSeason.GoalsAgainst = Int32.Parse(userSeasonArray[5]);
            newSeason.ShotsAgainst = Int32.Parse(userSeasonArray[6]);
            newSeason.Assists = Int32.Parse(userSeasonArray[7]);
            newSeason.PenaltyMinutes = Int32.Parse(userSeasonArray[8]);
            newSeason.Stamina = Int32.Parse(userSeasonArray[9]);
        }

        return newSeason;
    }

    private async Task<GoaliePlayoff> CreatePlayoff(List<string> playoffStrings)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie playoff.");
        
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
        
        GoaliePlayoff newPlayoff = new GoaliePlayoff
        {
            UserId = UsersController.Inst.UserData.Id,
            GamesPlayed = 0,
            Wins = 0,
            Losses = 0,
            Shutouts = 0,
            GoalsAgainst = 0,
            ShotsAgainst = 0,
            Assists = 0,
            PenaltyMinutes = 0,
            Stamina = 100,
        };

        if (!string.IsNullOrEmpty(userPlayoffString))
        {
            string[] userPlayoffArray = userPlayoffString.Split('/');

            newPlayoff.GamesPlayed = Int32.Parse(userPlayoffArray[1]);
            newPlayoff.Wins = Int32.Parse(userPlayoffArray[2]);
            newPlayoff.Losses = Int32.Parse(userPlayoffArray[3]);
            newPlayoff.Shutouts = Int32.Parse(userPlayoffArray[4]);
            newPlayoff.GoalsAgainst = Int32.Parse(userPlayoffArray[5]);
            newPlayoff.ShotsAgainst = Int32.Parse(userPlayoffArray[6]);
            newPlayoff.Assists = Int32.Parse(userPlayoffArray[7]);
            newPlayoff.PenaltyMinutes = Int32.Parse(userPlayoffArray[8]);
            newPlayoff.Stamina = Int32.Parse(userPlayoffArray[9]);
        }

        return newPlayoff;
    }

    private async Task<List<GoalieStats>> CreateStats(List<string> statsStrings)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie stats.");

        List<GoalieStats> newStats = new();

        foreach (string year in statsStrings)
        {
            if (!string.IsNullOrEmpty(year))
            {
                GoalieStats newStat = await CreateSingleStats(year);

                newStats.Add(newStat);
            }
        }

        int totalWins = 0;
        int totalLosses = 0;
        int totalShutouts = 0;
        int totalGoalsAgainst = 0;
        int totalShotsAgainst = 0;
        int totalAssists = 0;
        int totalPenaltyMinutes = 0;

        foreach (GoalieStats stat in newStats)
        {
            totalGames += stat.GamesPlayed;
            totalWins += stat.Wins;
            totalLosses += stat.Losses;
            totalShutouts += stat.Shutouts;
            totalGoalsAgainst += stat.GoalsAgainst;
            totalShotsAgainst += stat.ShotsAgainst;
            totalAssists += stat.Assists;
            totalPenaltyMinutes += stat.PenaltyMinutes;
        }

        if (totalGames > 0)
        {
            winPercentage = (float)totalWins / (float)totalGames;
            goalsAgainstPerGame = (float)totalGoalsAgainst / (float)totalGames;
            shotsAgainstPerGame = (float)totalShotsAgainst / (float)totalGames;
            assistsPerGame = (float)totalAssists / (float)totalGames;
            penaltyMinutesPerGame = (float)totalPenaltyMinutes / (float)totalGames;
        }

        return newStats;
    }

    private async Task<GoalieStats> CreateSingleStats(string statsString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie single season stats.");

        string[] statsArray = statsString.Split('/');
        if (statsArray.Length < 9) { return null; }

        GoalieStats newStats = new GoalieStats
        {
            Year = Int32.Parse(statsArray[0]),
            GamesPlayed = Int32.Parse(statsArray[1]),
            Wins = Int32.Parse(statsArray[2]),
            Losses = Int32.Parse(statsArray[3]),
            Shutouts = Int32.Parse(statsArray[4]),
            GoalsAgainst = Int32.Parse(statsArray[5]),
            ShotsAgainst = Int32.Parse(statsArray[6]),
            Assists = Int32.Parse(statsArray[7]),
            PenaltyMinutes = Int32.Parse(statsArray[8]),
        };

        return newStats;
    }

    private async Task<GoalieCard> CreateCard()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie card.");

        GoalieCard newCard = new GoalieCard
        {
            Penalty = await SetPenalty(),
            Fatigue = await SetFatigue(),
            GoalieRatingActions = await SetGoalieRatingActions(),
        };

        return newCard;
    }

    private async Task<string> SetPenalty()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the goalie penalty.");

        if (totalGames < 1) { goaliePenalty = "D"; return "D"; }

        if (penaltyMinutesPerGame >= 1.8f) { goaliePenalty = "AA"; return "AA"; }
        if (penaltyMinutesPerGame >= 1f) { goaliePenalty = "A"; return "A"; }
        if (penaltyMinutesPerGame >= 0.6f) { goaliePenalty = "B"; return "B"; }
        if (penaltyMinutesPerGame >= 0.2f) { goaliePenalty = "C"; return "C"; }

        goaliePenalty = "D";
        return "D";
    }

    private async Task<string> SetFatigue()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the goalie fatigue.");

        if (totalGames < 1) { return "B"; }
        return "D";
    }

    private async Task<List<string>> SetGoalieRatingActions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the goalie goalie rating actions.");

        List<string> ratingActions = new();

        if (totalGames < 1)
        {
            return new List<string>
            {
                "SAVE", "SAVE", "SAVE", "SAVE", "SAVE", "SAVE",
                "GOAL", "GOAL", "BREAKAWAY", "SAVE", "PENALTY"
            };
        }

        float savePct = 0f;
        if (shotsAgainstPerGame > 0f)
        {
            savePct = (shotsAgainstPerGame - goalsAgainstPerGame) / shotsAgainstPerGame;
        }

        float savePctScore = Mathf.InverseLerp(0.840f, 0.930f, savePct);
        float gaaScore = 1.5f - Mathf.InverseLerp(2.0f, 4.2f, goalsAgainstPerGame);
        float shotsScore = Mathf.InverseLerp(22f, 34f, shotsAgainstPerGame);
        float assistsScore = Mathf.InverseLerp(0f, 0.35f, assistsPerGame);
        float pimScore = Mathf.InverseLerp(0f, 1.8f, penaltyMinutesPerGame);

        float goalieQuality =
            (savePctScore * 0.55f) +
            (gaaScore * Random.Range(0f, 1.2f)) +
            ((1f - shotsScore) * 0.10f) +
            (assistsScore * 0.05f) +
            ((1f - pimScore) * 0.05f);

        goalieQuality = Mathf.Clamp01(goalieQuality);

        float penaltyShare = Mathf.Lerp(0.02f, 0.10f, pimScore);
        float breakawayShare = Mathf.Lerp(0.20f, 0.08f, goalieQuality) + (shotsScore * 0.04f);
        float goalShare = Mathf.Lerp(0.42f, 0.12f, goalieQuality);
        float saveShare = 1f - penaltyShare - breakawayShare - goalShare;

        penaltyShare = Mathf.Clamp(penaltyShare, 0.01f, 0.15f);
        breakawayShare = Mathf.Clamp(breakawayShare, 0.06f, 0.24f);
        goalShare = Mathf.Clamp(goalShare, 0.08f, 0.46f);
        saveShare = Mathf.Clamp(saveShare, 0.20f, 0.75f);
        
        float noiseStrength = 0.025f;

        float goalNoise = Random.Range(0, noiseStrength);
        float breakawayNoise = Random.Range(-noiseStrength, noiseStrength);
        float penaltyNoise = Random.Range(-noiseStrength, noiseStrength);

        goalShare += goalNoise;
        breakawayShare += breakawayNoise;
        penaltyShare += penaltyNoise;

        saveShare = 1f - goalShare - breakawayShare - penaltyShare;

        goalShare = Mathf.Clamp(goalShare, 0.08f, 0.46f);
        breakawayShare = Mathf.Clamp(breakawayShare, 0.06f, 0.24f);
        penaltyShare = Mathf.Clamp(penaltyShare, 0.01f, 0.15f);
        saveShare = Mathf.Clamp(saveShare, 0.20f, 0.75f);

        float totalShare = goalShare + breakawayShare + penaltyShare + saveShare;
        goalShare /= totalShare;
        breakawayShare /= totalShare;
        penaltyShare /= totalShare;
        saveShare /= totalShare;

        Dictionary<string, int> counts = DistributeCounts(
            11f * goalShare,
            11f * penaltyShare,
            11f * saveShare,
            11f * breakawayShare
        );

        List<string> actionPool = new();
        actionPool.AddRange(Enumerable.Repeat("GOAL", counts["GOAL"]));
        actionPool.AddRange(Enumerable.Repeat("PENALTY", counts["PENALTY"]));
        actionPool.AddRange(Enumerable.Repeat("SAVE", counts["SAVE"]));
        actionPool.AddRange(Enumerable.Repeat("BREAKAWAY", counts["BREAKAWAY"]));

        List<string> weightedActions = ApplyWinPercentageWeighting(actionPool);

        Dictionary<int, string> weightedActionDict = new();

        for (int i = 0; i < orderedSums.Length; i++)
        {
            weightedActionDict.Add(orderedSums[i], weightedActions[i]);
        }

        ratingActions = weightedActionDict
            .OrderBy(kvp => kvp.Key)
            .Select(kvp => kvp.Value)
            .ToList();

        ratingActions = SpreadGoalResults(ratingActions);

        return ratingActions;
    }

    private Dictionary<string, int> DistributeCounts(
        float goalRaw,
        float penaltyRaw,
        float saveRaw,
        float breakawayRaw)
    {
        Dictionary<string, float> rawCounts = new()
        {
            { "GOAL", goalRaw },
            { "PENALTY", penaltyRaw },
            { "SAVE", saveRaw },
            { "BREAKAWAY", breakawayRaw }
        };

        Dictionary<string, int> finalCounts = rawCounts.ToDictionary(
            kvp => kvp.Key,
            kvp => Mathf.FloorToInt(kvp.Value)
        );

        int usedSlots = finalCounts.Values.Sum();
        int remainingSlots = 11 - usedSlots;

        foreach (var kvp in rawCounts
                     .OrderByDescending(x => x.Value - Mathf.Floor(x.Value))
                     .ThenBy(x => GetActionPriorityForRemainder(x.Key))
                     .Take(remainingSlots))
        {
            finalCounts[kvp.Key]++;
        }

        return finalCounts;
    }

    private int GetActionPriorityForRemainder(string action)
    {
        return action switch
        {
            "GOAL" => 0,
            "BREAKAWAY" => 1,
            "SAVE" => 2,
            "PENALTY" => 3,
            _ => 4
        };
    }

    private List<string> ApplyWinPercentageWeighting(List<string> actionPool)
    {
        if (actionPool == null || actionPool.Count != 11) { return new List<string>(); }

        List<string> sortedActions = actionPool.OrderBy(action => GetActionRank(action)).ToList();

        actionPool.Clear();

        float normalizedWin = Mathf.Clamp01((winPercentage - 0.3f) / 0.5f);
        float qualityBias = Mathf.Lerp(-0.75f, 1.25f, normalizedWin);

        List<(int SlotIndex, float TargetScore)> slotScores = new();

        for (int i = 0; i < 11; i++)
        {
            float probabilityStrength = 1f - (i / 10f);
            float centerOffset = probabilityStrength - 0.5f;
            float targetScore = centerOffset * qualityBias;

            slotScores.Add((i, targetScore));
        }

        slotScores = slotScores.OrderByDescending(slot => slot.TargetScore).ToList();

        string[] mappedActions = new string[11];

        int low = 0;
        int high = sortedActions.Count - 1;

        foreach ((int slotIndex, float targetScore) in slotScores)
        {
            bool wantsBetterAction = targetScore >= 0f;

            if (wantsBetterAction)
            {
                mappedActions[slotIndex] = sortedActions[high];
                high--;
            }
            else
            {
                mappedActions[slotIndex] = sortedActions[low];
                low++;
            }
        }

        actionPool.AddRange(mappedActions);
        return actionPool;
    }

    private List<string> SpreadGoalResults(List<string> ratingActions)
    {
        if (ratingActions == null || ratingActions.Count != 11) { return ratingActions; }

        List<int> goalIndices = ratingActions
            .Select((action, index) => new { action, index })
            .Where(x => x.action == "GOAL")
            .Select(x => x.index)
            .ToList();

        if (goalIndices.Count <= 1) { return ratingActions; }

        List<string> adjusted = new(ratingActions);

        foreach (int index in goalIndices)
        {
            adjusted[index] = "SAVE";
        }
        
        int goalCount = goalIndices.Count;
        
        while (goalCount > 0)
        {
            int index = Random.Range(0, 11);
            adjusted[index] = "GOAL";

            goalCount--;
        }

        return adjusted;
    }

    private int GetActionRank(string action)
    {
        return action switch
        {
            "GOAL" => 0,
            "PENALTY" => 1,
            "SAVE" => 2,
            "BREAKAWAY" => 3,
            _ => 0
        };
    }
#endregion
}}