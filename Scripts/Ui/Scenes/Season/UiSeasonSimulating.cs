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

namespace SoM.Ui {
public class UiSeasonSimulating : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Icon Elements")]
    [SerializeField] private Image _homeIcon;
    [SerializeField] private Image _awayIcon;

    [Header("Loading Elements")]
	[SerializeField] private Slider _loadingBar;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private bool isLoading = false;

    private int maxAmount = 0;
    private int simmedAmount = 0;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        InitializeUi();
    }

    void Update()
    {
        if (isLoading)
        {
            if (simmedAmount < maxAmount) { _loadingBar.value = (float)simmedAmount / (float)maxAmount; }
            else { _loadingBar.value = 1f; }
        }
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
		SetGameData();

        base.InitializeUi();

        StartSimulating();
    }
#endregion
#region -------------------- Private Methods --------------------
    private void SetGameData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game data.");

        GameTeam homeTeam = GameplayController.Inst.GameData.HomeTeam;
        GameTeam awayTeam = GameplayController.Inst.GameData.AwayTeam;

        string homeLeague = homeTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";
        string awayLeague = awayTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";

        string homeString = $"{homeLeague}_{homeTeam.Team.Code}_ON";
        string awayString = $"{awayLeague}_{awayTeam.Team.Code}_ON";

        _homeIcon.sprite = ConstantController.Inst.IconSprites[homeString];
        _awayIcon.sprite = ConstantController.Inst.IconSprites[awayString];
    }

    private async void StartSimulating()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Starting to simulate the rest of the night games.");

        int night = SeasonsController.Inst.SeasonGameNight;
        string userTeam = SeasonsController.Inst.SeasonData.Team.Team.Code;
        List<Game> nightGames = new(SeasonsController.Inst.SeasonData.GameNights.FirstOrDefault(g => g.Number == night).Games);

        maxAmount = nightGames.Count;
        simmedAmount += 1;

        isLoading = true;

        foreach (Game game in nightGames)
        {
            if (game.HomeTeam.Team.Code == userTeam || game.AwayTeam.Team.Code == userTeam)
            {
                continue;
            }

            ResetGameStats(game.HomeTeam);
            ResetGameStats(game.AwayTeam);

            SimulateSkaterStats(game.HomeTeam, game.AwayTeam.GoalieLineup["G"]);
            SimulateSkaterStats(game.AwayTeam, game.HomeTeam.GoalieLineup["G"]);

            SetGoalieGameStats(game.HomeTeam, game.AwayTeam);
            SetGoalieGameStats(game.AwayTeam, game.HomeTeam);
            SetTeamGameStats(game.HomeTeam);
            SetTeamGameStats(game.AwayTeam);

            ApplyGameToSeason(game.HomeTeam, game.AwayTeam);
            ApplyGameToSeason(game.AwayTeam, game.HomeTeam);

            // Home Team
            foreach (Skater homeSkater in game.HomeTeam.SkaterLineup.Values)
            {
                string homeSkaterSeasonString = SaveController.Inst.SaveSkaterSeasonData(homeSkater);

                await FirebaseController.Inst.PutSkaterSeason(homeSkater.Id, UsersController.Inst.UserData.Id, homeSkaterSeasonString);
            }

            string homeGoalieSeasonString = SaveController.Inst.SaveGoalieSeasonData(game.HomeTeam.GoalieLineup["G"]);
            string homeTeamSeasonString = SaveController.Inst.SaveTeamSeasonData(game.HomeTeam);

            await FirebaseController.Inst.PutGoalieSeason(game.HomeTeam.GoalieLineup["G"].Id, UsersController.Inst.UserData.Id, homeGoalieSeasonString);
            await FirebaseController.Inst.PutTeamSeason(homeMainTeam.Id, UsersController.Inst.UserData.Id, homeTeamSeasonString);

            // Away Team
            foreach (Skater awaySkater in game.AwayTeam.SkaterLineup.Values)
            {
                string homeSkaterSeasonString = SaveController.Inst.SaveSkaterSeasonData(awaySkater);

                await FirebaseController.Inst.PutSkaterSeason(awaySkater.Id, UsersController.Inst.UserData.Id, awaySkaterSeasonString);
            }

            string awayGoalieSeasonString = SaveController.Inst.SaveGoalieSeasonData(game.AwayTeam.GoalieLineup["G"]);
            string awayTeamSeasonString = SaveController.Inst.SaveTeamSeasonData(game.AwayTeam);

            await FirebaseController.Inst.PutGoalieSeason(game.AwayTeam.GoalieLineup["G"].Id, UsersController.Inst.UserData.Id, awayGoalieSeasonString);
            await FirebaseController.Inst.PutTeamSeason(awayMainTeam.Id, UsersController.Inst.UserData.Id, awayTeamSeasonString);

            simmedAmount += 1;
        }

        GameplayController.Inst.GameData = null;

        _loadingBar.value = 1f;
        isLoading = false;

        GoToNewScene(CoreController.Inst.Scene_Season02);
    }

    private void ResetGameStats(GameTeam gameTeam)
    {
        foreach (Skater skater in GetUniqueSkaters(gameTeam))
        {
            skater.Game.Goals = 0;
            skater.Game.Assists = 0;
            skater.Game.Points = 0;
            skater.Game.PlusMinus = 0;
            skater.Game.PenaltyMinutes = 0;
            skater.Game.PowerplayGoals = 0;
            skater.Game.PowerplayAssists = 0;
            skater.Game.PowerplayPoints = 0;
            skater.Game.ShorthandedGoals = 0;
            skater.Game.ShorthandedAssists = 0;
            skater.Game.ShorthandedPoints = 0;
            skater.Game.Shots = 0;
            skater.Game.Giveaways = 0;
            skater.Game.Takeaways = 0;
            skater.Game.Hits = 0;
            skater.Game.BlockedShots = 0;
            skater.Game.FaceoffsWon = 0;
            skater.Game.FaceoffsLost = 0;
            skater.Game.SecondsPlayed = 0;
            skater.Game.Stamina = 100;
        }

        Goalie goalie = gameTeam.GoalieLineup["G"];
        goalie.Game.GoalsAgainst = 0;
        goalie.Game.ShotsAgainst = 0;
        goalie.Game.Assists = 0;
        goalie.Game.PenaltyMinutes = 0;

        gameTeam.Stats.Goals = 0;
        gameTeam.Stats.Shots = 0;
        gameTeam.Stats.PowerplayGoals = 0;
        gameTeam.Stats.Powerplays = 0;
        gameTeam.Stats.ShorthandedGoals = 0;
        gameTeam.Stats.FaceoffsWon = 0;
        gameTeam.Stats.FaceoffsLost = 0;
        gameTeam.Stats.Hits = 0;
        gameTeam.Stats.BlockedShots = 0;
        gameTeam.Stats.Giveaways = 0;
        gameTeam.Stats.Takeaways = 0;
    }

    private void SimulateSkaterStats(GameTeam offenseTeam, Goalie defenseGoalie)
    {
        int goalieModifier = defenseGoalie.Card.GoalieRatingActions != null ? defenseGoalie.Card.GoalieRatingActions.Count : 0;
        HashSet<string> usedSkaterIds = new();

        foreach (KeyValuePair<string, Skater> lineupEntry in offenseTeam.SkaterLineup)
        {
            string lineupPosition = lineupEntry.Key;
            Skater skater = lineupEntry.Value;

            if (usedSkaterIds.Contains(skater.Id))
            {
                continue;
            }

            usedSkaterIds.Add(skater.Id);

            int offense = skater.Card.Offense;
            int defense = skater.Card.Defense;
            int breakaway = skater.Card.Breakaway;
            bool isForward = skater.Info.Position == "F";

            int shots = Mathf.Clamp(UnityEngine.Random.Range(1, 4) + Mathf.RoundToInt(offense / 2.5f) + (isForward ? 1 : 0) - (goalieModifier / 8), 0, 9);
            int goals = 0;

            for (int i = 0; i < shots; i++)
            {
                int goalRoll = UnityEngine.Random.Range(0, 100);
                int goalChance = Mathf.Clamp(6 + offense + Mathf.RoundToInt(breakaway / 2f) - goalieModifier, 2, 35);

                if (goalRoll < goalChance)
                {
                    goals += 1;
                }
            }

            int assists = 0;
            for (int i = 0; i < goals; i++)
            {
                assists += UnityEngine.Random.Range(0, 100) < 70 ? UnityEngine.Random.Range(0, 3) : 0;
            }

            skater.Game.Shots = shots;
            skater.Game.Goals = goals;
            skater.Game.Assists = assists;
            skater.Game.Points = goals + assists;
            skater.Game.Hits = Mathf.Clamp(UnityEngine.Random.Range(0, 3) + Mathf.RoundToInt(defense / 3f), 0, 6);
            skater.Game.BlockedShots = isForward ? UnityEngine.Random.Range(0, 2) : UnityEngine.Random.Range(0, 4);
            skater.Game.Giveaways = Mathf.Clamp(UnityEngine.Random.Range(0, 3) + Mathf.Max(0, 5 - defense / 3), 0, 5);
            skater.Game.Takeaways = Mathf.Clamp(UnityEngine.Random.Range(0, 3) + Mathf.RoundToInt(defense / 4f), 0, 5);
            skater.Game.SecondsPlayed = isForward ? UnityEngine.Random.Range(700, 1200) : UnityEngine.Random.Range(1100, 1600);

            if (lineupPosition.StartsWith("C"))
            {
                int faceoffs = UnityEngine.Random.Range(5, 18);
                int faceoffWins = Mathf.Clamp(Mathf.RoundToInt(faceoffs * Mathf.Clamp01((40f + (skater.Card.Faceoff * 6f)) / 100f)), 0, faceoffs);
                skater.Game.FaceoffsWon = faceoffWins;
                skater.Game.FaceoffsLost = faceoffs - faceoffWins;
            }
        }
    }

    private void SetGoalieGameStats(GameTeam goalieTeam, GameTeam opposingTeam)
    {
        Goalie goalie = goalieTeam.GoalieLineup["G"];
        List<Skater> opposingSkaters = GetUniqueSkaters(opposingTeam);

        goalie.Game.GoalsAgainst = opposingSkaters.Sum(s => s.Game.Goals);
        goalie.Game.ShotsAgainst = opposingSkaters.Sum(s => s.Game.Shots);
    }

    private void SetTeamGameStats(GameTeam gameTeam)
    {
        List<Skater> teamSkaters = GetUniqueSkaters(gameTeam);

        gameTeam.Stats.Goals = teamSkaters.Sum(s => s.Game.Goals);
        gameTeam.Stats.Shots = teamSkaters.Sum(s => s.Game.Shots);
        gameTeam.Stats.PowerplayGoals = teamSkaters.Sum(s => s.Game.PowerplayGoals);
        gameTeam.Stats.Powerplays = 0;
        gameTeam.Stats.ShorthandedGoals = teamSkaters.Sum(s => s.Game.ShorthandedGoals);
        gameTeam.Stats.FaceoffsWon = teamSkaters.Sum(s => s.Game.FaceoffsWon);
        gameTeam.Stats.FaceoffsLost = teamSkaters.Sum(s => s.Game.FaceoffsLost);
        gameTeam.Stats.Hits = teamSkaters.Sum(s => s.Game.Hits);
        gameTeam.Stats.BlockedShots = teamSkaters.Sum(s => s.Game.BlockedShots);
        gameTeam.Stats.Giveaways = teamSkaters.Sum(s => s.Game.Giveaways);
        gameTeam.Stats.Takeaways = teamSkaters.Sum(s => s.Game.Takeaways);
    }

    private void ApplyGameToSeason(GameTeam gameTeam, GameTeam opponentTeam)
    {
        bool isWin = gameTeam.Stats.Goals > opponentTeam.Stats.Goals;
        bool isLoss = gameTeam.Stats.Goals < opponentTeam.Stats.Goals;
        bool isTie = gameTeam.Stats.Goals == opponentTeam.Stats.Goals;

        ConstantController.LeagueType leagueType = gameTeam.Team.League.Contains("NHL") ? ConstantController.LeagueType.NHL : ConstantController.LeagueType.PWHL;
        Team mainTeam = TeamsController.Inst.GetTeamFromCode(gameTeam.Team.Code, leagueType);

        if (mainTeam == null || mainTeam.Season == null)
        {
            return;
        }

        mainTeam.Season.GamesPlayed += 1;
        mainTeam.Season.Wins += isWin ? 1 : 0;
        mainTeam.Season.Losses += isLoss ? 1 : 0;
        mainTeam.Season.Ties += isTie ? 1 : 0;
        mainTeam.Season.Points += isWin ? 2 : (isTie ? 1 : 0);
        mainTeam.Season.Goals += gameTeam.Stats.Goals;
        mainTeam.Season.Shots += gameTeam.Stats.Shots;
        mainTeam.Season.PowerplayGoals += gameTeam.Stats.PowerplayGoals;
        mainTeam.Season.Powerplays += gameTeam.Stats.Powerplays;
        mainTeam.Season.ShorthandedGoals += gameTeam.Stats.ShorthandedGoals;
        mainTeam.Season.FaceoffsWon += gameTeam.Stats.FaceoffsWon;
        mainTeam.Season.FaceoffsLost += gameTeam.Stats.FaceoffsLost;
        mainTeam.Season.Hits += gameTeam.Stats.Hits;
        mainTeam.Season.BlockedShots += gameTeam.Stats.BlockedShots;
        mainTeam.Season.Giveaways += gameTeam.Stats.Giveaways;
        mainTeam.Season.Takeaways += gameTeam.Stats.Takeaways;

        foreach (Skater skater in GetUniqueSkaters(gameTeam))
        {
            skater.Season.GamesPlayed += 1;
            skater.Season.Goals += skater.Game.Goals;
            skater.Season.Assists += skater.Game.Assists;
            skater.Season.Points += skater.Game.Points;
            skater.Season.PlusMinus += skater.Game.PlusMinus;
            skater.Season.PenaltyMinutes += skater.Game.PenaltyMinutes;
            skater.Season.PowerplayGoals += skater.Game.PowerplayGoals;
            skater.Season.PowerplayAssists += skater.Game.PowerplayAssists;
            skater.Season.PowerplayPoints += skater.Game.PowerplayPoints;
            skater.Season.ShorthandedGoals += skater.Game.ShorthandedGoals;
            skater.Season.ShorthandedAssists += skater.Game.ShorthandedAssists;
            skater.Season.ShorthandedPoints += skater.Game.ShorthandedPoints;
            skater.Season.Shots += skater.Game.Shots;
            skater.Season.Giveaways += skater.Game.Giveaways;
            skater.Season.Takeaways += skater.Game.Takeaways;
            skater.Season.FaceoffsWon += skater.Game.FaceoffsWon;
            skater.Season.FaceoffsLost += skater.Game.FaceoffsLost;
        }

        Goalie goalie = gameTeam.GoalieLineup["G"];
        goalie.Season.GamesPlayed += 1;
        goalie.Season.Wins += isWin ? 1 : 0;
        goalie.Season.Losses += isLoss ? 1 : 0;
        goalie.Season.Shutouts += goalie.Game.GoalsAgainst == 0 ? 1 : 0;
        goalie.Season.GoalsAgainst += goalie.Game.GoalsAgainst;
        goalie.Season.ShotsAgainst += goalie.Game.ShotsAgainst;
        goalie.Season.Assists += goalie.Game.Assists;
        goalie.Season.PenaltyMinutes += goalie.Game.PenaltyMinutes;
    }

    private List<Skater> GetUniqueSkaters(GameTeam gameTeam)
    {
        List<Skater> uniqueSkaters = new();
        HashSet<string> usedSkaterIds = new();

        foreach (Skater skater in gameTeam.SkaterLineup.Values)
        {
            if (usedSkaterIds.Contains(skater.Id))
            {
                continue;
            }

            usedSkaterIds.Add(skater.Id);
            uniqueSkaters.Add(skater);
        }

        return uniqueSkaters;
    }
#endregion
}}
