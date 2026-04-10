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
	[SerializeField] private GameObject _continueObject;
	[SerializeField] private GameObject _forwardsObject;
	[SerializeField] private GameObject _defenseObject;
	[SerializeField] private GameObject _goalieObject;

	[Header("Panel Elements")]
	[SerializeField] private EditLinesPanel _editLinesPanel;

	[Header("List Elements")]
	[SerializeField] private List<EditLinePositionPrefab> _editLinePositions = new();
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
	private List<string> positionsList = new() { "C1", "LW1", "RW1", "C2", "LW2", "RW2", "C3", "LW3", "RW3", "C4", "LW4", "RW4", 
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
		_editLinesPanel.HidePanel();

		ClearAllPositions();
		ChangePositionOption(0);

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SetLinesFromDefault()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the full line-up from default selection.");

		string teamCode = GameplayController.Inst.GameData.HomeTeam.Team.Code;
		string teamLeagueString = GameplayController.Inst.GameData.HomeTeam.Team.League;

		ConstantController.LeagueType teamLeague = ConstantController.LeagueType.None;

		if (teamLeagueString == "NHL") { teamLeague = ConstantController.LeagueType.NHL; }
		else if (teamLeagueString == "NHLFranchise") { teamLeague = ConstantController.LeagueType.NHLFranchise; }
		else if (teamLeagueString == "PWHL") { teamLeague = ConstantController.LeagueType.PWHL; }
		else if (teamLeagueString == "PWHLFranchise") { teamLeague = ConstantController.LeagueType.PWHLFranchise; }

		Dictionary<string, Skater> defaultSkaters = TeamsController.Inst.GetDefaultLineup(teamCode, teamLeague);

		Goalie defaultGoalie = TeamsController.Inst.GetDefaultStartingGoalie(teamCode, teamLeague);

		for (int s = 0; s < defaulSkaters.Count; s++)
		{
			_editLinePositions[s].ThisFullPos = defaulSkaters.ElementAt(s).Key;
			_editLinePositions[s].ThisSkater = defaulSkaters[s];
			_editLinePositions[s].ThisGoalie = null;

			string pos = defaulSkaters.ElementAt(s).Key.Contains("D") ? "D" : "F";

			_editLinePositions[s].SetPosition(pos, true, skater = defaulSkaters[s]);

			int posOption = 0;

			if (defaulSkaters.ElementAt(s).Key.Contains("D")) { posOption = 1; }
			else if (defaulSkaters.ElementAt(s).Key.Contains("G")) { posOption = 2; }

			_editLinePositions[s].RemoveButton.SetListener(() =>
			{
				ClearPosition(defaulSkaters.ElementAt(s).Key, _editLinePositions[s]);
			});

			_editLinePositions[s].SelectButton.SetListener(() =>
			{
				ShowSelectionPanel(posOption);
			});
		}

		int goalieIndex = _editLinePositions.Count - 1;

		_editLinePositions[goalieIndex].ThisFullPos = "G";
		_editLinePositions[goalieIndex].ThisSkater = null;
		_editLinePositions[goalieIndex].ThisGoalie = defaultGoalie;
		_editLinePositions[goalieIndex].SetPosition("G", true, goalie = defaultGoalie);

		_editLinePositions[goalieIndex].RemoveButton.SetListener(() =>
		{
			ClearPosition("G", _editLinePositions[goalieIndex]);
		});

		_editLinePositions[goalieIndex].SelectButton.SetListener(() =>
		{
			ShowSelectionPanel(2);
		});

		foreach (EditLinePositionPrefab prefab in _editLinePositions)
		{
			positionObjectsDict.Add(prefab.ThisFullPos, prefab);
		}
    }

	private void GoToReady()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the exhibition game ready screen.");

		// TODO: Set team edit lines

		GoToNewScene(CoreController.Inst.Scene_Exhibition03);
    }

	private void GoToOptions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the exhibition game options screen.");

        GoToNewScene(CoreController.Inst.Scene_Exhibition01);
    }

	private void ShowSelectionPanel(int posOption)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the position selection panel.");

        _editLinesPanel.gameObject.SetActive(true);
		_editLinesPanel.InitializeEditLinesPanel(posOption);
    }

	private void ChangePositionOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the edit line positions option.");

		switch (option)
		{
			case 1:
				_forwardsObject.SetActive(false);
				_defenseObject.SetActive(true);
				_goalieObject.SetActive(false);
				break;
			case 2:
				_forwardsObject.SetActive(false);
				_defenseObject.SetActive(false);
				_goalieObject.SetActive(true);
				break;
			case 0:
			default:
				_forwardsObject.SetActive(true);
				_defenseObject.SetActive(false);
				_goalieObject.SetActive(false);
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

		foreach (KeyValuePair<string, EditLinePositionPrefab> posPrefab in positionObjectsDict)
		{
			ClearPosition(posPrefab.Key, posPrefab.Value);
		}
	}

	private void ClearPosition(string pos, EditLinePositionPrefab prefab)
	{
		prefab.ThisFullPos = posPrefab.Key;
		prefab.ThisSkater = null;
		prefab.ThisGoalie = null;

		prefab.SetPosition(string.Empty, false);

		int posOption = 0;

		if (pos.Contains("D")) { posOption = 1; }
		else if (pos.Contains("G")) { posOption = 2; }

		prefab.RemoveButton.RemoveListener();
		prefab.SelectButton.SetListener(() =>
		{
			ShowSelectionPanel(posOption);
		});
	}
#endregion
}}
