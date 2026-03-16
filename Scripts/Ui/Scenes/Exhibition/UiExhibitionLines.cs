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
public class UiExhibitionLines : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _defaultLinesButton;
	[SerializeField] private SoM_Button _continueButton;
	[SerializeField] private SoM_Button _returnButton;

	[Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _positionDropdown;

	[Header("Game Object Elements")]
	[SerializeField] private GameObject _notCompleteObject;
	[SerializeField] private GameObject _continueObejct;

	[Header("List Elements")]
	[SerializeField] private List<EditLinePositionPrefab> _editLinePositions = new();
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
	private List<string> positionsList = new { "C1", "LW1", "RW1", "C2", "LW2", "RW2", "C3", "LW3", "RW3", "C4", "LW4", "RW4", 
		"LD1", "RD1", "LD2", "RD2", "LD3", "RD3", "G"};

	private Dictionary<string, EditLinePositionPrefab> positionObjectsDict = new();
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
		_defaultLinesButton.SetListener(SetLinesFromDefault);
		_continueButton.SetListener(GoToReady);
		_returnButton.SetListener(GoToOptions);

		_positionDropdown.SetListener(ChangePositionOption);

		ClearAllPositions();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SetLinesFromDefault()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the full line-up from default selection.");

		// TODO
    }

	private void GoToReady()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the exhibition game ready screen.");

		// TODO: Set team edit lines

		GoToScene(CoreController.Inst.Scene_Exhibition03);
    }

	private void GoToOptions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the exhibition game options screen.");

		GoToScene(CoreController.Inst.Scene_Exhibition01);
    }

	private void ChangePositionOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the edit line positions option.");

		switch (option)
		{
			case 1:
				// TODO: Set as defense
				break;
			case 2:
				// TODO: Set as goalies
				break;
			case 0:
			default:
				// TODO: Set as forwards
				break;
		}
    }

	private void ClearAllPositions()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Resetting the set line-up to clear all selections.");

		positionObjectsDict.Clear();

		for (int i = 0; i < positionsList.Count; i++)
		{
			int index = i;

			positionObjectsDict.Add(positionsList[index], _editLinePositions[index]);
		}

		foreach (EditLinePositionPrefab posPrefab in positionObjectsDict)
		{
			// TODO: Set each prefab to "not set" mode
			// TODO: Set each prefab to have all buttons set listeners
		}
	}
#endregion
}}
