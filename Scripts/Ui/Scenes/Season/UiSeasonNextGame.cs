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

        Game nextGame = nightGames.FirstOrDefault(ng => ng.HomeTeam.Team.Code == SeasonController.Inst.SeasonData.Team.Team.Code);
        GameTeam nextTeam = nextGame.AwayTeam;

        string league = SeasonController.Inst.SeasonData.League.Contains("NHL") ? "NHL" : "PWHL";
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

        // TODO
    }

    private void GoToHome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the home screen.");

        // TODO
    }

    private void ShowDeletePanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the delete season panel.");

        // TODO
    }

    private void ChangeNavigationOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the season navigation view.");

        // TODO
    }
#endregion
}}
