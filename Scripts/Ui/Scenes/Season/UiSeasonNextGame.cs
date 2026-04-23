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

namespace SoM.Ui {
public class UiSeasonNextGame : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _playNextButton;
	[SerializeField] private SoM_Button _quitButton;
	[SerializeField] private SoM_Button _deleteButton;

	[Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _seasonNavigationDropdown;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _recordText;
    [SerializeField] private TMP_Text _gameNumberText;
    [SerializeField] private TMP_Text _opponentTeamText;
    [SerializeField] private TMP_Text _opponentRecordText;
    
    [SerializeField] private Image _opponentIcon;
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
        _playNextButton.SetListener(GoToEditLines);
		_quitButton.SetListener(GoToHome);
		_deleteButton.SetListener(ShowDeletePanel);

		_seasonNavigationDropdown.SetListener(ChangeNavigationOption);
        _seasonNavigationDropdown.Dropdown.value = 0;

		ChangeNavigationOption(0);
        SetData();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SetData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the data for the next game.");

        int night = SeasonsController.Inst.SeasonGameNight;
        List<Game> nightGames = new(SeasonsController.Inst.SeasonData.GameNights.FirstOrDefault(g => g.Number == night).Games);

        Game nextGame = nightGames.FirstOrDefault(ng => ng.HomeTeam.Team.Code == SeasonsController.Inst.SeasonData.Team.Team.Code);

        GameplayController.Inst.CreateSeasonGame();
        GameplayController.Inst.GameData = nextGame;

        GameTeam nextTeam = nextGame.AwayTeam;

        string league = SeasonsController.Inst.SeasonData.League.Contains("NHL") ? "NHL" : "PWHL";
        string nextTeamString = $"{league}_{nextTeam.Team.Code}_ON";

        _opponentIcon.sprite = ConstantController.Inst.IconSprites[nextTeamString];

        _recordText.text = $"{UsersController.Inst.UserData.SeasonStats.CurrentWins} - {UsersController.Inst.UserData.SeasonStats.CurrentLosses} - " + "\n" +
            $"{UsersController.Inst.UserData.SeasonStats.CurrentTies} - {UsersController.Inst.UserData.SeasonStats.CurrentOTLs}";
        
        TeamSeason opponentSeason = SeasonsController.Inst.GetTeamSeason(nextTeam);

        _gameNumberText.text = $"Game #{night}";
        _opponentTeamText.text = $"{nextTeam.Team.CityName} {nextTeam.Team.NickName}";
        _opponentRecordText.text = $"{opponentSeason.Wins} - {opponentSeason.Losses} - {opponentSeason.Ties} - {opponentSeason.OTLs}";
    }

    private void GoToEditLines()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season edit lines screen.");

        GoToNewScene(CoreController.Inst.Scene_Season08);
    }

    private void GoToHome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the home screen.");

        GoToNewScene(CoreController.Inst.Scene_Home00);
    }

    private void GoToSeasonStandings()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season standings screen.");

        GoToNewScene(CoreController.Inst.Scene_Season03);
    }

    private void GoToTeamStatistics()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season team statistics screen.");

        GoToNewScene(CoreController.Inst.Scene_Season04);
    }

    private void GoToSkaterStatistics()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season skater statistics screen.");

        GoToNewScene(CoreController.Inst.Scene_Season05);
    }

    private void GoToGoalieStatistics()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season goalie statistics screen.");

        GoToNewScene(CoreController.Inst.Scene_Season06);
    }

    private void ShowDeletePanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the delete season panel.");

        PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.SeasonDeleteSeason, DeleteSeason);
    }

    private void ChangeNavigationOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the season navigation view.");

        switch (option)
		{
			case 1:
				GoToSeasonStandings();
				break;
			case 2:
				GoToTeamStatistics();
				break;
            case 3:
				GoToSkaterStatistics();
				break;
            case 4:
				GoToGoalieStatistics();
				break;
			case 0:
			default:
                // Navigation is already Next Game
				break;
		}
    }

    private async void DeleteSeason()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting the user season.");

		string userId = UsersController.Inst.UserData.Id;

		PlayoffDatabase userPlayoffs = null;
		SeasonDatabase userSeason = null;

		await FirebaseController.Inst.GetPlayoffs(userId, async playoffs =>
		{
			userPlayoffs = playoffs;

			await FirebaseController.Inst.GetSeason(userId, season =>
			{
				userSeason = season;
			});
		});

		if (userPlayoffs != null)
		{
			await FirebaseController.Inst.DeletePlayoffs(userId);
		}

		if (userSeason != null)
		{
			await FirebaseController.Inst.DeleteSeason(userId);
		}

        GoToHome();
	}
#endregion
}}
