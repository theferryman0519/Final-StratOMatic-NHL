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
public class UiSeasonResults : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _simulateButton;

    [Header("Text Elements")]
    [SerializeField] private TMP_Text _homeTeamText;
    [SerializeField] private TMP_Text _homeStatsText;
    [SerializeField] private TMP_Text _awayTeamText;
    [SerializeField] private TMP_Text _awayStatsText;

    [Header("Icon Elements")]
    [SerializeField] private Image _homeIcon;
    [SerializeField] private Image _awayIcon;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        InitializeUi();
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
        AudioController.Inst.ChangeMusicVolume(false);

		_simulateButton.SetListener(SaveSeasonData);

        SetGameData();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SaveSeasonData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the season data.");

        GameTeam homeTeam = GameplayController.Inst.GameData.HomeTeam;
        GameTeam awayTeam = GameplayController.Inst.GameData.AwayTeam;

        ConstantController.LeagueType league = homeTeam.Team.League.Contains("NHL") ? ConstantController.LeagueType.NHL : ConstantController.LeagueType.PWHL;

        Team homeMainTeam = TeamsController.Inst.GetTeamFromCode(GameplayController.Inst.GameData.HomeTeam.Team.Code, league);
        Team awayMainTeam = TeamsController.Inst.GetTeamFromCode(GameplayController.Inst.GameData.AwayTeam.Team.Code, league);

        string result = "Win";
        bool isOvertime = EventsController.Inst.GameplayEvents.GameFlowEvents.IsOvertimeGame;

        if (homeTeam.Stats.Goals < awayTeam.Stats.Goals) { result = isOvertime ? "OTL" : "Lose"; }
        else if (homeTeam.Stats.Goals == awayTeam.Stats.Goals) { result = "Tie"; }

        if (result == "Win") { UsersController.Inst.UserData.SeasonStats.CurrentWins += 1; }
        else if (result == "Lose") { UsersController.Inst.UserData.SeasonStats.CurrentLosses += 1; }
        else if (result == "Tie") { UsersController.Inst.UserData.SeasonStats.CurrentTies += 1; }
        else if (result == "OTL") { UsersController.Inst.UserData.SeasonStats.CurrentOTLs += 1; }

        ApplyGameToSeason(homeTeam, awayTeam);
        ApplyGameToSeason(awayTeam, homeTeam);

        UsersController.Inst.SaveUserData(async () =>
        {
            // Home Team
            foreach (Skater homeSkater in homeTeam.SkaterLineup.Values)
            {
                string homeSkaterSeasonString = SaveController.Inst.SaveSkaterSeasonData(homeSkater);

                await FirebaseController.Inst.PutSkaterSeason(homeSkater.Id, UsersController.Inst.UserData.Id, homeSkaterSeasonString);
            }

            string homeGoalieSeasonString = SaveController.Inst.SaveGoalieSeasonData(homeTeam.GoalieLineup["G"]);
            string homeTeamSeasonString = SaveController.Inst.SaveTeamSeasonData(homeTeam);

            await FirebaseController.Inst.PutGoalieSeason(homeTeam.GoalieLineup["G"].Id, UsersController.Inst.UserData.Id, homeGoalieSeasonString);
            await FirebaseController.Inst.PutTeamSeason(homeMainTeam.Id, UsersController.Inst.UserData.Id, homeTeamSeasonString);

            // Away Team
            foreach (Skater awaySkater in awayTeam.SkaterLineup.Values)
            {
                string awaySkaterSeasonString = SaveController.Inst.SaveSkaterSeasonData(awaySkater);

                await FirebaseController.Inst.PutSkaterSeason(awaySkater.Id, UsersController.Inst.UserData.Id, awaySkaterSeasonString);
            }

            string awayGoalieSeasonString = SaveController.Inst.SaveGoalieSeasonData(awayTeam.GoalieLineup["G"]);
            string awayTeamSeasonString = SaveController.Inst.SaveTeamSeasonData(awayTeam);

            await FirebaseController.Inst.PutGoalieSeason(awayTeam.GoalieLineup["G"].Id, UsersController.Inst.UserData.Id, awayGoalieSeasonString);
            await FirebaseController.Inst.PutTeamSeason(awayMainTeam.Id, UsersController.Inst.UserData.Id, awayTeamSeasonString);

            // User
            SeasonDatabase userSeasonData = SaveController.Inst.SaveUserSeasonData();

            await FirebaseController.Inst.PutSeason(userSeasonData, UsersController.Inst.UserData.Id, GoToNightSimulate);
        });
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

    private void GoToNightSimulate()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to season simulate night screen.");

        GoToNewScene(CoreController.Inst.Scene_Season11);
    }

    private void SetGameData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game data.");

        GameTeam homeTeam = GameplayController.Inst.GameData.HomeTeam;
        GameTeam awayTeam = GameplayController.Inst.GameData.AwayTeam;

        _homeTeamText.text = homeTeam.Team.Code;
        _awayTeamText.text = awayTeam.Team.Code;

        string homeLeague = homeTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";
        string awayLeague = awayTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";

        string homeString = $"{homeLeague}_{homeTeam.Team.Code}_ON";
        string awayString = $"{awayLeague}_{awayTeam.Team.Code}_ON";

        _homeIcon.sprite = ConstantController.Inst.IconSprites[homeString];
        _awayIcon.sprite = ConstantController.Inst.IconSprites[awayString];

        int homeGoals = homeTeam.Stats.Goals;
		int homeShots = homeTeam.Stats.Shots;
		int homePPGs = homeTeam.Stats.PowerplayGoals;
		int homePPs = homeTeam.Stats.Powerplays;
		int homeFOWs = homeTeam.Stats.FaceoffsWon;
		int homeHits = homeTeam.Stats.Hits;

		int awayGoals = awayTeam.Stats.Goals;
		int awayShots = awayTeam.Stats.Shots;
		int awayPPGs = awayTeam.Stats.PowerplayGoals;
		int awayPPs = awayTeam.Stats.Powerplays;
		int awayFOWs = awayTeam.Stats.FaceoffsWon;
		int awayHits = awayTeam.Stats.Hits;

        _homeStatsText.text = $"{homeGoals}" + "\n" +
            $"{homeShots}" + "\n" +
            $"{homePPGs} - {homePPs}" + "\n" +
            $"{homeHits}" + "\n" +
            $"{homeFOWs} of {homeFOWs + awayFOWs}";
        
        _awayStatsText.text = $"{awayGoals}" + "\n" +
            $"{awayShots}" + "\n" +
            $"{awayPPGs} - {awayPPs}" + "\n" +
            $"{awayHits}" + "\n" +
            $"{awayFOWs} of {homeFOWs + awayFOWs}";
    }
#endregion
}}
