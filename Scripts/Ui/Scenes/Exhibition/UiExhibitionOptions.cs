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
    private string aiDifficulty = string.Empty;
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

		GameplayController.Inst.GameOptions = new GameOptions();

		SetDropdownDefaults();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SetOptionsAsDefaults()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the options as default for exhibition games.");

		string lineChangesSelection = GameplayController.Inst.GameOptions.LineChangesOn.ToString();
		string fatigueSelection = GameplayController.Inst.GameOptions.FatigueOn.ToString();
		string injuriesSelection = GameplayController.Inst.GameOptions.InjuriesOn.ToString();

		PlayerPrefs.SetString(ConstantController.Pref_ExhibitionOptions, $"{lineChangesSelection}/{fatigueSelection}/True/{injuriesSelection}/{aiDifficulty}");
    }

	private void GoToEditLines()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to exhibition edit lines screen.");

		GoToNewScene(CoreController.Inst.Scene_Exhibition02);
    }

	private void GoToTeamSelect()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to exhibition team select screen.");

        GoToNewScene(CoreController.Inst.Scene_Exhibition00);
    }

	private void SetDropdownDefaults()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the dropdown options from defaults for exhibition games.");

		string optionsDefault = string.Empty;

		if (PlayerPrefs.HasKey(ConstantController.Pref_ExhibitionOptions))
		{
			optionsDefault = PlayerPrefs.GetString(ConstantController.Pref_ExhibitionOptions);
		}

		else
		{
			optionsDefault = "True/True/True/True/Veteran";

			PlayerPrefs.SetString(ConstantController.Pref_ExhibitionOptions, optionsDefault);
		}
		
		string[] optionsArray = optionsDefault.Split("/");

		if (optionsArray[0] == "true" || optionsArray[0] == "True") { ChangeLineChangesOption(0); }
		else { ChangeLineChangesOption(1); }

		if (optionsArray[1] == "true" || optionsArray[1] == "True") { ChangeFatigueOption(0); }
		else { ChangeFatigueOption(1); }

		// optionsArray[2] is for season only

		if (optionsArray[3] == "true" || optionsArray[3] == "True") { ChangeInjuriesOption(0); }
		else { ChangeInjuriesOption(1); }

		if (optionsArray[4] == "Rookie") { ChangeDifficultyOption(0); }
		else if (optionsArray[4] == "Hall of Famer") { ChangeDifficultyOption(2); }
		else { ChangeDifficultyOption(1); }
	}

	private void ChangeLineChangesOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the line changes option.");

		switch (option)
		{
			case 1:
				GameplayController.Inst.GameOptions.LineChangesOn = false;
				break;
			case 0:
			default:
				GameplayController.Inst.GameOptions.LineChangesOn = true;
				break;
		}
		
		_lineChangesDropdown.Dropdown.value = option;
    }

	private void ChangeFatigueOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the player fatigue option.");

		switch (option)
		{
			case 1:
				GameplayController.Inst.GameOptions.FatigueOn = false;
				break;
			case 0:
			default:
				GameplayController.Inst.GameOptions.FatigueOn = true;
				break;
		}
		
		_fatigueDropdown.Dropdown.value = option;
    }

	private void ChangeInjuriesOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the player injury option.");

		switch (option)
		{
			case 1:
				GameplayController.Inst.GameOptions.InjuriesOn = false;
				break;
			case 0:
			default:
				GameplayController.Inst.GameOptions.InjuriesOn = true;
				break;
		}
		
		_injuriesDropdown.Dropdown.value = option;
    }

	private void ChangeDifficultyOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the AI difficulty option.");

		switch (option)
		{
			case 0:
				GameplayController.Inst.GameOptions.AiDifficulty = 0;
				aiDifficulty = "Rookie";
				break;
			case 2:
				GameplayController.Inst.GameOptions.AiDifficulty = 2;
				aiDifficulty = "Hall of Famer";
				break;
			case 1:
			default:
				GameplayController.Inst.GameOptions.AiDifficulty = 1;
				aiDifficulty = "Veteran";
				break;
		}
		
		_difficultyDropdown.Dropdown.value = option;
    }
#endregion
}}
