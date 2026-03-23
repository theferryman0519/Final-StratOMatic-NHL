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

    private int[] orderedSums = { 7, 6, 9, 5, 10, 4, 11, 3, 12, 2 };

    private SemaphoreSlim createGoalieLock = new(1, 1);
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public async Task<Skater> CreateGoalie(GoalieDatabase goalieDatabase)
    {
        await createGoalieLock.WaitAsync();
        try
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie.");

            goalieId = goalieDatabase.Id;
            goaliePenalty = string.Empty;
            totalGames = 0;

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
            newGoalie.Season = await CreateSeason(goalieDatabase.SeasonString);
            newGoalie.Playoff = await CreatePlayoff(goalieDatabase.PlayoffString);
            newGoalie.Stats = await CreateStats(goalieDatabase.StatsStrings);
            newGoalie.Card = await CreateCard();

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
        if (infoArray.Length < 3) { return null; }

        GoalieInfo newInfo = new GoalieInfo
        {
            Id = goalieId,
            FirstName = infoArray[0],
            LastName = infoArray[1],
            Team = infoArray[2],
        };

        return newInfo;
    }

    private async Task<GoalieGame> CreateGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie game.");

        GoalieGame newGame = new GoalieGame
        {
            Id = goalieId,
            GoalsAgainst = 0,
            ShotsAgainst = 0,
            Assists = 0,
            PenaltyMinutes = 0,
        };

        return newGame;
    }

    private async Task<GoalieSeason> CreateSeason(string seasonString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie season.");

        string[] seasonArray = seasonString.Split('/');
        if (seasonArray.Length < 8) { return null; }

        GoalieSeason newSeason = new GoalieSeason
        {
            Id = goalieId,
            GamesPlayed = Int32.Parse(seasonArray[0]),
            Wins = Int32.Parse(seasonArray[1]),
            Losses = Int32.Parse(seasonArray[2]),
            Shutouts = Int32.Parse(seasonArray[3]),
            GoalsAgainst = Int32.Parse(seasonArray[4]),
            ShotsAgainst = Int32.Parse(seasonArray[5]),
            Assists = Int32.Parse(seasonArray[6]),
            PenaltyMinutes = Int32.Parse(seasonArray[7]),
        };

        return newSeason;
    }

    private async Task<GoaliePlayoff> CreatePlayoff(string playoffString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie playoff.");

        string[] playoffArray = playoffString.Split('/');
        if (playoffArray.Length < 8) { return null; }

        GoaliePlayoff newPlayoff = new GoaliePlayoff
        {
            Id = goalieId,
            GamesPlayed = Int32.Parse(playoffArray[0]),
            Wins = Int32.Parse(playoffArray[1]),
            Losses = Int32.Parse(playoffArray[2]),
            Shutouts = Int32.Parse(playoffArray[3]),
            GoalsAgainst = Int32.Parse(playoffArray[4]),
            ShotsAgainst = Int32.Parse(playoffArray[5]),
            Assists = Int32.Parse(playoffArray[6]),
            PenaltyMinutes = Int32.Parse(playoffArray[7]),
        };

        return newPlayoff;
    }

    private async Task<List<GoalieStats>> CreateStats(List<string> statsStrings)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the goalie stats.");

        List<GoalieStats> newStats = new();

        foreach (string year in statsStrings)
        {
            GoalieStats newStat = await CreateSingleStats(year);

            newStats.Add(newStat);
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
            Id = goalieId,
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
            Id = goalieId,
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

        float penaltyMinutes = penaltyMinutesPerGame / 0.1f;
        float assists = assistsPerGame / 0.1f;
        float goalsAgainst = goalsAgainstPerGame / 2.85f;
        float shotsAgainst = shotsAgainstPerGame / 28f;

        float penaltyShare = Mathf.Clamp(0.03f + 0.05f * (penaltyMinutes - 1f), 0f, 0.15f);
        float breakawayShare = Mathf.Clamp(0.10f + 0.06f * (shotsAgainst - 1f) + 0.04f * (goalsAgainst - 1f) - 0.03f * (assists - 1f), 0.05f, 0.22f);
        float goalShare = Mathf.Clamp(0.22f + 0.12f * (goalsAgainst - 1f) + 0.04f * (shotsAgainst - 1f), 0.1f, 0.45f);

        float saveShare = 1f - penaltyShare - breakawayShare - goalShare;
        saveShare = Mathf.Clamp(saveShare, 0f, 1f);

        int penaltyCount = (int)Math.Round(11 * penaltyShare, MidpointRounding.AwayFromZero);
        int breakawayCount = (int)Math.Round(11 * breakawayShare, MidpointRounding.AwayFromZero);
        int goalCount = (int)Math.Round(11 * goalShare, MidpointRounding.AwayFromZero);
        int saveCount = 11 - penaltyCount - breakawayCount - goalCount;

        List<string> actionPool = new();
        List<string> weightedActions = new();
        Dictionary<int, string> weightedActionDict = new();

        actionPool.AddRange(Enumerable.Repeat("GOAL", goalCount));
        actionPool.AddRange(Enumerable.Repeat("PENALTY", penaltyCount));
        actionPool.AddRange(Enumerable.Repeat("SAVE", saveCount));
        actionPool.AddRange(Enumerable.Repeat("BREAKAWAY", breakawayCount));

        weightedActions = ApplyWinPercentageWeighting(actionPool);

        for (int i = 0; i < orderedSums.Count; i++)
        {
            int index = i;

            weightedActionDict.Add(orderedSums[index], weightedActions[index]);
        }

        ratingActions = weightedActionDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();

        return ratingActions;
    }

    private List<string> ApplyWinPercentageWeighting(List<string> actionPool)
    {
        if (actionPool == null || actionPool.Count != 11) { return; }

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
