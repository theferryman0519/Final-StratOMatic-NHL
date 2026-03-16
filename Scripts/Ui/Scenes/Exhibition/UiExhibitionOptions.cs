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
public class UiExhibitionOptions : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
    [SerializeField] private SoM_Button _defaultsButton;
	[SerializeField] private SoM_Button _continueButton;
	[SerializeField] private SoM_Button _returnButton;

	[Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _lineChangesDropdown;
	[SerializeField] private SoM_Dropdown _fatigueDropdown;
	[SerializeField] private SoM_Dropdown _injuriesDropdown;
	[SerializeField] private SoM_Dropdown _difficultyDropdown;
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
        _defaultsButton.SetListener(SetOptionsAsDefaults);
		_continueButton.SetListener(GoToEditLines);
		_returnButton.SetListener(GoToTeamSelect);

		_lineChangesDropdown.SetListener(ChangeLineChangesOption);
		_fatigueDropdown.SetListener(ChangeFatigueOption);
		_injuriesDropdown.SetListener(ChangeInjuriesOption);
		_difficultyDropdown.SetListener(ChangeDifficultyOption);

		SetDropdownDefaults();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SetOptionsAsDefaults()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the options as default for exhibition games.");

		// TODO
    }

	private void GoToEditLines()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to exhibition edit lines screen.");

		GoToScene(CoreController.Inst.Scene_Exhibition02);
    }

	private void GoToTeamSelect()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to exhibition team select screen.");

		GoToScene(CoreController.Inst.Scene_Exhibition00);
    }

	private void SetDropdownDefaults()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the dropdown options from defaults for exhibition games.");

		// TODO
	}

	private void ChangeLineChangesOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the line changes option.");

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

	private void ChangeFatigueOption(int option)
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
#endregion
}}
