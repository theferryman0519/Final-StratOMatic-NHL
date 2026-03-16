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
public class UiHomeMain : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Text Elements")]
	[SerializeField] private TMP_Text _welcomeText;
	[SerializeField] private TMP_Text _recordText;
	[SerializeField] private TMP_Text _seasonText;

	[Header("Button Elements")]
	[SerializeField] private SoM_Button _exhibitionButton;
	[SerializeField] private SoM_Button _multiplayerButton;
	[SerializeField] private SoM_Button _seasonButton;
	[SerializeField] private SoM_Button _newSeasonButton;
	[SerializeField] private SoM_Button _settingsButton;

	[Header("Game Object Elements")]
	[SerializeField] private GameObject _seasonObject;
	[SerializeField] private GameObject _newSeasonObject;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private bool hasSeason = false;
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
        _exhibitionButton.SetListener(GoToExhibitionGame);
		_multiplayerButton.SetListener(GoToMultiplayer);
		_seasonButton.SetListener(GoToSeason);
		_newSeasonButton.SetListener(GoToNewSeason);
		_settingsButton.SetListener(GoToSettings);

		SetTexts();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToExhibitionGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to exhibition game screen.");

		GoToNewScene(CoreController.Inst.Scene_Exhibition00);
    }

	private void GoToMultiplayer()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to multiplayer screen.");

		GoToNewScene(CoreController.Inst.Scene_Multiplayer00);
    }

	private void GoToSeason()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to season screen.");

		GoToNewScene(CoreController.Inst.Scene_Season02);
    }

	private void GoToNewSeason()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to a new season screen.");

		GoToNewScene(CoreController.Inst.Scene_Season00);
    }

	private void GoToSettings()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to user settings screen.");

		GoToNewScene(CoreController.Inst.Scene_Settings00);
    }

	private void SetTexts()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the home screen texts.");

		_welcomeText.text = $"Welcome, {UsersController.Inst.User.Info.Name}! Please choose whether you wish to play in an Exhibition match or in a Season.";

		UserStats userStats = UsersController.Inst.User.Stats;

		int wins = userStats.NhlWins + userStats.PwhlWins + userStats.NhlFranchiseWins + userStats.PwhlFranchiseWins;
		int losses = userStats.NhlLosses + userStats.PwhlLosses + userStats.NhlFranchiseLosses + userStats.PwhlFranchiseLosses;
		int ties = userStats.NhlTies + userStats.PwhlTies + userStats.NhlFranchiseTies + userStats.PwhlFranchiseTies;
		int otls = userStats.NhlOTLs + userStats.PwhlOTLs + userStats.NhlFranchiseOTLs + userStats.PwhlFranchiseOTLs;

		_recordText.text = "Single Game Record" + "\n" + $"{wins} - {losses} - {ties} - {otls}";

		hasSeason = UsersController.Inst.User.SeasonStats.IsInSeason;

		if (hasSeason)
		{
			int seasonWins = UsersController.Inst.User.SeasonStats.CurrentWins;
			int seasonLosses = UsersController.Inst.User.SeasonStats.CurrentLosses;
			int seasonTies = UsersController.Inst.User.SeasonStats.CurrentTies;
			int seasonOTLs = UsersController.Inst.User.SeasonStats.CurrentOTLs;

			string team = UsersController.Inst.User.SeasonStats.Team;
			string league = UsersController.Inst.User.SeasonStats.League;

			_seasonText.text = "Current Season Record" + "\n" + TeamsController.Inst.GetFullTeamName(team, league) + "\n"
				+ $"{seasonWins} - {seasonLosses} - {seasonTies} - {seasonOTLs}";
		}

		else
		{
			_seasonText.text = "You have not started a season yet";
		}

		_seasonObject.SetActive(hasSeason);
		_newSeasonButton.SetActive(!hasSeason);
	}
#endregion
}}
