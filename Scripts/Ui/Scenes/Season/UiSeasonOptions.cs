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
	[SerializeField] private SoM_Dropdown _playerFatigueButton;
	[SerializeField] private SoM_Dropdown _goalieFatigueDropdown;
	[SerializeField] private SoM_Dropdown _injuriesDropdown;
	[SerializeField] private SoM_Dropdown _difficultyDropdown;
	[SerializeField] private SoM_Dropdown _rosterManagementDropdown;
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

		_playerFatigueButton.SetListener(ChangePlayerFatigueOption);
		_goalieFatigueDropdown.SetListener(ChangeGoalieFatigueOption);
		_injuriesDropdown.SetListener(ChangeInjuriesOption);
		_difficultyDropdown.SetListener(ChangeDifficultyOption);
		_rosterManagementDropdown.SetListener(ChangeRosterManagementOption);

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void AttemptToStartSeason()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Attempting to start new season.");

		// TODO

		GoToScene(CoreController.Inst.Scene_Season02);
    }

	private void GoToTeamSelect()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to season team select screen.");

		GoToScene(CoreController.Inst.Scene_Season00);
    }

	private void ChangePlayerFatigueOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the player fatigue option.");

		switch (option)
		{
			case 1:
				// TODO: Set as off
				break;
			case 0:
			default:
				// TODO: Set as on
				break;
		}
    }

	private void ChangeGoalieFatigueOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the goalie fatigue option.");

		switch (option)
		{
			case 1:
				// TODO: Set as off
				break;
			case 0:
			default:
				// TODO: Set as on
				break;
		}
    }

	private void ChangeInjuriesOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the player injury option.");

		switch (option)
		{
			case 1:
				// TODO: Set as off
				break;
			case 0:
			default:
				// TODO: Set as on
				break;
		}
    }

	private void ChangeDifficultyOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the AI difficulty option.");

		switch (option)
		{
			case 0:
				// TODO: Set as Rookie
				break;
			case 2:
				// TODO: Set as Hall of Famer
				break;
			case 1:
			default:
				// TODO: Set as Veteran
				break;
		}
    }
	
	private void ChangeRosterManagementOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the roster management option.");

		switch (option)
		{
			case 1:
				// TODO: Set as static from start
				break;
			case 0:
			default:
				// TODO: Set as match current
				break;
		}
    }
#endregion
}}
