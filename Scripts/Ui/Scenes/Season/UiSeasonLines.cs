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
using Random = UnityEngine.Random;

// Game Dependencies
using SoM.Controllers;
using SoM.Models;

namespace SoM.Ui {
public class UiSeasonLines : UiSceneBase {

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

	void Update()
    {
	    if (GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Count == 18 && GameplayController.Inst.GameData.HomeTeam.GoalieLineup.Count == 1)
	    {
		    _notCompleteObject.SetActive(false);
		    _continueObject.SetActive(true);
	    }

	    else
	    {
		    _notCompleteObject.SetActive(true);
		    _continueObject.SetActive(false);
	    }
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
		_defaultLinesButton.SetListener(SetLinesFromDefault);
		_continueButton.SetListener(GoToReady);
		_returnButton.SetListener(GoToSeasonHome);

		_positionDropdown.SetListener(ChangePositionOption);
		_editLinesPanel.HidePanel();

		ClearAllPositions();
		ChangePositionOption(0);
		SetLinesFromPrefs();

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
        else if (teamLeagueString == "PWHL") { teamLeague = ConstantController.LeagueType.PWHL; }
        
        Dictionary<string, Skater> defaultSkaters = TeamsController.Inst.GetDefaultLineup(teamCode, teamLeague);
        Goalie defaultGoalie = TeamsController.Inst.GetDefaultStartingGoalie(teamCode, teamLeague);
        
        GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Clear();
        GameplayController.Inst.GameData.HomeTeam.GoalieLineup.Clear();

        GameplayController.Inst.GameData.HomeTeam.SkaterLineup = defaultSkaters;
        GameplayController.Inst.GameData.HomeTeam.GoalieLineup["G"] = defaultGoalie;

        RefreshAllPositions();
    }

	private void SetLinesFromPrefs()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the lineup from saved prefs.");

		GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Clear();
        GameplayController.Inst.GameData.HomeTeam.GoalieLineup.Clear();

		if (PlayerPrefs.HasKey(ConstantController.Pref_DefaultSeasonLeague) && 
			PlayerPrefs.HasKey(ConstantController.Pref_DefaultSeasonTeam) && 
			PlayerPrefs.HasKey(ConstantController.Pref_DefaultSeasonLineup))
		{
            string leagueDefault = string.Empty;
            string teamDefault = string.Empty;
			string lineupDefault = string.Empty;

			leagueDefault = PlayerPrefs.GetString(ConstantController.Pref_DefaultSeasonLeague);
            teamDefault = PlayerPrefs.GetString(ConstantController.Pref_DefaultSeasonTeam);
			lineupDefault = PlayerPrefs.GetString(ConstantController.Pref_DefaultSeasonLineup);

			if (teamDefault == GameplayController.Inst.GameData.HomeTeam.Team.Code)
			{
				string[] lineupDefaultArray = lineupDefault.Split('/');

				List<Skater> defaultTeamSkaters = new();
				List<Goalie> defaultTeamGoalies = new();

				if (leagueDefault == "NHL")
				{
					defaultTeamSkaters = SkatersController.Inst.NhlSkaters[teamDefault];
					defaultTeamGoalies = GoaliesController.Inst.NhlGoalies[teamDefault];
				}

				else if (leagueDefault == "PWHL")
				{
					defaultTeamSkaters = SkatersController.Inst.PwhlSkaters[teamDefault];
					defaultTeamGoalies = GoaliesController.Inst.PwhlGoalies[teamDefault];
				}

				if (defaultTeamSkaters.Count > 0 && defaultTeamGoalies.Count > 0 && lineupDefaultArray.Length == 19)
				{
					for (int i = 0; i < positionsList.Count - 1; i++)
					{
						Skater skater = defaultTeamSkaters.FirstOrDefault(s => s.Id == lineupDefaultArray[i]);
						if (skater != null) { GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Add(positionsList[i], skater); }
					}

					Goalie goalie = defaultTeamGoalies.FirstOrDefault(g => g.Id == lineupDefaultArray[18]);
					if (goalie != null) { GameplayController.Inst.GameData.HomeTeam.GoalieLineup.Add("G", goalie); }
				}

				RefreshAllPositions();
			}
		}
    }

	private void GoToReady()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season game ready screen.");
        
        string teamCode = GameplayController.Inst.GameData.HomeTeam.Team.Code;
        string teamLeagueString = GameplayController.Inst.GameData.HomeTeam.Team.League;
        int maxChoice = 0;
        
        ConstantController.LeagueType teamLeague = ConstantController.LeagueType.None;

        if (teamLeagueString == "NHL")
        {
	        teamLeague = ConstantController.LeagueType.NHL;
	        maxChoice = ConstantController.NhlTeamCount;
        }
        
        else if (teamLeagueString == "PWHL")
        {
	        teamLeague = ConstantController.LeagueType.PWHL;
	        maxChoice = ConstantController.PwhlTeamCount;
        }
        
        Dictionary<string, Skater> defaultSkaters = TeamsController.Inst.GetDefaultLineup(GameplayController.Inst.GameData.AwayTeam.Team.Code, teamLeague);
        Goalie defaultGoalie = TeamsController.Inst.GetDefaultStartingGoalie(GameplayController.Inst.GameData.AwayTeam.Team.Code, teamLeague);
        
        GameplayController.Inst.GameData.AwayTeam.SkaterLineup = defaultSkaters;
        GameplayController.Inst.GameData.AwayTeam.GoalieLineup["G"] = defaultGoalie;

		SetLinesToPlayerPrefs();

		GoToNewScene(CoreController.Inst.Scene_Season07);
    }

	private void GoToSeasonHome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season next game screen.");

        GoToNewScene(CoreController.Inst.Scene_Season02);
    }

	private void SetLinesToPlayerPrefs()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Setting lines to player prefs.");

		string userTeam = GameplayController.Inst.GameData.HomeTeam.Team.Code;
        string userLeague = GameplayController.Inst.GameData.HomeTeam.Team.League;
		string userLineup = string.Empty;

		foreach (Skater skater in GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Values)
		{
			userLineup += $"{skater.Id}/";
		}

		userLineup += $"{GameplayController.Inst.GameData.HomeTeam.GoalieLineup["G"].Id}";

		PlayerPrefs.SetString(ConstantController.Pref_DefaultSeasonLeague, userLeague);
		PlayerPrefs.SetString(ConstantController.Pref_DefaultSeasonTeam, userTeam);
		PlayerPrefs.SetString(ConstantController.Pref_DefaultSeasonLineup, userLineup);
	}

	private void ShowSelectionPanel(int posOption, string pos)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the position selection panel.");
        
        _editLinesPanel.gameObject.SetActive(true);
		_editLinesPanel.SelectedPosition = pos;
		_editLinesPanel.RefreshAction = RefreshAllPositions;
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

	private void RefreshAllPositions()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Refreshing the set line-up for all selections.");

		ClearAllPositions();

		foreach (KeyValuePair<string, EditLinePositionPrefab> posObject in positionObjectsDict)
		{
			posObject.Value.ThisFullPos = posObject.Key;
			posObject.Value.ThisGoalie = null;
			
			if (GameplayController.Inst.GameData.HomeTeam.SkaterLineup.ContainsKey(posObject.Key))
			{
				posObject.Value.ThisSkater = GameplayController.Inst.GameData.HomeTeam.SkaterLineup[posObject.Key];
				posObject.Value.SetPosition(posObject.Key.Substring(0, posObject.Key.Length - 1), true, posObject.Value.ThisSkater, null);
				
				int posOption = 0;

				if (IsDefensePos(posObject.Key)) { posOption = 1; }
				
				posObject.Value.RemoveButton.SetListener(() =>
				{
					GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Remove(posObject.Key);
					ClearPosition(posObject.Key, posObject.Value);
				});

				posObject.Value.SelectButton.SetListener(() =>
				{
					ShowSelectionPanel(posOption, posObject.Key);
				});
			}

			else
			{
				posObject.Value.ThisSkater = null;
			}
		}
		
		positionObjectsDict.ElementAt(18).Value.ThisFullPos = "G";
		positionObjectsDict.ElementAt(18).Value.ThisSkater = null;
		positionObjectsDict.ElementAt(18).Value.ThisGoalie = GameplayController.Inst.GameData.HomeTeam.GoalieLineup["G"];
		positionObjectsDict.ElementAt(18).Value.SetPosition("G", true, null, GameplayController.Inst.GameData.HomeTeam.GoalieLineup["G"]);

		positionObjectsDict.ElementAt(18).Value.RemoveButton.SetListener(() =>
		{
			GameplayController.Inst.GameData.HomeTeam.GoalieLineup.Remove("G");
			ClearPosition("G", positionObjectsDict.ElementAt(18).Value);
		});

		positionObjectsDict.ElementAt(18).Value.SelectButton.SetListener(() =>
		{
			ShowSelectionPanel(2, "G");
		});
	}

	private void ClearAllPositions()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Resetting the set line-up to clear all selections.");

		positionObjectsDict.Clear();
		
		positionObjectsDict.Add("C1", _editLinePositions[0]);
		positionObjectsDict.Add("LW1", _editLinePositions[1]);
		positionObjectsDict.Add("RW1", _editLinePositions[2]);
		positionObjectsDict.Add("C2", _editLinePositions[3]);
		positionObjectsDict.Add("LW2", _editLinePositions[4]);
		positionObjectsDict.Add("RW2", _editLinePositions[5]);
		positionObjectsDict.Add("C3", _editLinePositions[6]);
		positionObjectsDict.Add("LW3", _editLinePositions[7]);
		positionObjectsDict.Add("RW3", _editLinePositions[8]);
		positionObjectsDict.Add("C4", _editLinePositions[9]);
		positionObjectsDict.Add("LW4", _editLinePositions[10]);
		positionObjectsDict.Add("RW4", _editLinePositions[11]);
		positionObjectsDict.Add("LD1", _editLinePositions[12]);
		positionObjectsDict.Add("RD1", _editLinePositions[13]);
		positionObjectsDict.Add("LD2", _editLinePositions[14]);
		positionObjectsDict.Add("RD2", _editLinePositions[15]);
		positionObjectsDict.Add("LD3", _editLinePositions[16]);
		positionObjectsDict.Add("RD3", _editLinePositions[17]);
		positionObjectsDict.Add("G", _editLinePositions[18]);

		foreach (KeyValuePair<string, EditLinePositionPrefab> prefab in positionObjectsDict)
		{
			ClearPosition(prefab.Key, prefab.Value, true);
		}
	}

	private void ClearPosition(string pos, EditLinePositionPrefab prefab, bool isClearingAll = false)
	{
		prefab.ThisFullPos = pos;
		prefab.ThisSkater = null;
		prefab.ThisGoalie = null;

		prefab.SetPosition(string.Empty, false);

		int posOption = 0;

		if (IsDefensePos(pos)) { posOption = 1; }
		if (pos == "G") { posOption = 2; }

		prefab.RemoveButton.SetListener(() =>
		{
			GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Remove(pos);
			ClearPosition(pos, prefab);
		});
		prefab.SelectButton.SetListener(() =>
		{
			ShowSelectionPanel(posOption, pos);
		});

		if (!isClearingAll)
		{
			RefreshAllPositions();
		}
	}

	private bool IsDefensePos(string pos)
	{
		if (pos.StartsWith("LD") || pos.StartsWith("RD"))
		{
			return true;
		}
		
		return false;
	}
#endregion
}}
