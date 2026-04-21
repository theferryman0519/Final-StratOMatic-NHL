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
public class UiSeasonOptions : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _startButton;
	[SerializeField] private SoM_Button _returnButton;

	[Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _playerFatigueDropdown;
	[SerializeField] private SoM_Dropdown _goalieFatigueDropdown;
	[SerializeField] private SoM_Dropdown _injuriesDropdown;
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
		_startButton.SetListener(AttemptToStartSeason);
		_returnButton.SetListener(GoToTeamSelect);

		_playerFatigueDropdown.SetListener(ChangePlayerFatigueOption);
		_goalieFatigueDropdown.SetListener(ChangeGoalieFatigueOption);
		_injuriesDropdown.SetListener(ChangeInjuriesOption);

		SeasonsController.Inst.SeasonOptions = new GameOptions();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void AttemptToStartSeason()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Attempting to start new season.");

		string fatigueSelection = SeasonsController.Inst.SeasonOptions.FatigueOn.ToString();
		string goalieFatigueSelection = SeasonsController.Inst.SeasonOptions.GoalieFatigueOn.ToString();
		string injuriesSelection = SeasonsController.Inst.SeasonOptions.InjuriesOn.ToString();

		SeasonsController.Inst.SeasonOptions.LineChangesOn = true;
		SeasonsController.Inst.SeasonOptions.AiDifficulty = 2;

		PlayerPrefs.SetString(ConstantController.Pref_SeasonOptions, $"True/{fatigueSelection}/{goalieFatigueSelection}/{injuriesSelection}/Hall of Famer");

		UsersController.Inst.UserData.SeasonStats.IsInSeason = true;
		UsersController.Inst.UserData.SeasonStats.Id = Guid.NewGuid();
		UsersController.Inst.UserData.SeasonStats.League = SeasonsController.Inst.SeasonData.League;
		UsersController.Inst.UserData.SeasonStats.Team = SeasonsController.Inst.SeasonData.Team.Team.Code;
		UsersController.Inst.UserData.SeasonStats.CurrentWins = 0;
		UsersController.Inst.UserData.SeasonStats.CurrentLosses = 0;
		UsersController.Inst.UserData.SeasonStats.CurrentTies = 0;
		UsersController.Inst.UserData.SeasonStats.CurrentOTLs = 0;

		UsersController.Inst.SaveUserData(() =>
		{
			GoToNewScene(CoreController.Inst.Scene_Season02);
		});
    }

	private void GoToTeamSelect()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to season team select screen.");

        GoToNewScene(CoreController.Inst.Scene_Season00);
    }

	private void ChangePlayerFatigueOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the player fatigue option.");

		switch (option)
		{
			case 1:
				SeasonsController.Inst.SeasonOptions.FatigueOn = false;
				break;
			case 0:
			default:
				SeasonsController.Inst.SeasonOptions.FatigueOn = true;
				break;
		}
		
		_playerFatigueDropdown.Dropdown.value = option;
    }

	private void ChangeGoalieFatigueOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the goalie fatigue option.");

		switch (option)
		{
			case 1:
				SeasonsController.Inst.SeasonOptions.GoalieFatigueOn = false;
				break;
			case 0:
			default:
				SeasonsController.Inst.SeasonOptions.GoalieFatigueOn = true;
				break;
		}
		
		_goalieFatigueDropdown.Dropdown.value = option;
    }

	private void ChangeInjuriesOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the player injury option.");

		switch (option)
		{
			case 1:
				SeasonsController.Inst.SeasonOptions.InjuriesOn = false;
				break;
			case 0:
			default:
				SeasonsController.Inst.SeasonOptions.InjuriesOn = true;
				break;
		}
		
		_injuriesDropdown.Dropdown.value = option;
    }
#endregion
}}
