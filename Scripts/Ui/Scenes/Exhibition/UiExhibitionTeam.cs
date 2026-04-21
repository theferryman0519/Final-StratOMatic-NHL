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
public class UiExhibitionTeam : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Grid Elements")]
    [SerializeField] private Transform _container;
    [SerializeField] private FavoriteTeamPrefab _teamPrefab;

    [Header("Button Elements")]
    [SerializeField] private SoM_Button _continueButton;
	[SerializeField] private SoM_Button _returnButton;

    [Header("Team Select Elements")]
    [SerializeField] private TMP_Text _selectionText;

	[Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _leagueDropdown;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private Team selectedTeam;

	private List<Team> teamSelections = new();

	private ConstantController.LeagueType selectedLeague = ConstantController.LeagueType.NHL;
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
        _continueButton.SetListener(GoToOptions);
		_returnButton.SetListener(GoToHome);

		_leagueDropdown.SetListener(ChangeLeagueOption);

        GameplayController.Inst.CreateExhibitionGame();

        SetContainer();
        SetTeamFromDefault();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToOptions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to exhibition game options screen.");

        GameplayController.Inst.SetGameTeam(selectedTeam, true);

		GoToNewScene(CoreController.Inst.Scene_Exhibition01);
    }

	private void GoToHome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to home screen.");

        GameplayController.Inst.GameData = null;

		GoToNewScene(CoreController.Inst.Scene_Home00);
    }
    
    private void SetContainer()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the selected team container.");

        ClearContainer();

		teamSelections = SetTeamSelections();

        foreach (Team team in teamSelections)
        {
	        FavoriteTeamPrefab icon = Instantiate(_teamPrefab, _container);

            icon.SetIcon(team, false);
            icon.IconButton.onClick.AddListener(() =>
            {
                CoreController.Inst.WriteLog(this.GetType().Name, $"Choosing {team.Info.Code} as a selected team.");
                
                RefreshAllIcons();

                selectedTeam = team;
                icon.SetIcon(team, true);

				SetBanner(team);

                _selectionText.text = $"You have selected the {team.Info.CityName} {team.Info.NickName} of the {team.Info.League}";
                _continueButton.gameObject.SetActive(true);
            });
        }
    }

    private void SetTeamFromDefault()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the team from default.");

		if (PlayerPrefs.HasKey(ConstantController.Pref_DefaultExhibitionLeague) && PlayerPrefs.HasKey(ConstantController.Pref_DefaultExhibitionTeam))
		{
            string leagueDefault = string.Empty;
            string teamDefault = string.Empty;
            int leagueOption = -1;

			leagueDefault = PlayerPrefs.GetString(ConstantController.Pref_DefaultExhibitionLeague);
            teamDefault = PlayerPrefs.GetString(ConstantController.Pref_DefaultExhibitionTeam);

            if (leagueDefault == "NHL") { selectedLeague = ConstantController.LeagueType.NHL; leagueOption = 0; }
            else if (leagueDefault == "PWHL") { selectedLeague = ConstantController.LeagueType.PWHL; leagueOption = 1; }
            else if (leagueDefault == "NHLFranchise") { selectedLeague = ConstantController.LeagueType.NHLFranchise; leagueOption = 2; }
            else if (leagueDefault == "PWHLFranchise") { selectedLeague = ConstantController.LeagueType.PWHLFranchise; leagueOption = 3; }

            ChangeLeagueOption(leagueOption);

            _leagueDropdown.Dropdown.value = leagueOption;

            Team defaultTeam = TeamsController.Inst.GetTeamFromCode(teamDefault, selectedLeague);

            if (defaultTeam != null)
            {
                foreach (Transform teamObj in _container)
                {
                    if (teamObj.TryGetComponent<FavoriteTeamPrefab>(out FavoriteTeamPrefab teamPrefab))
                    {
                        if (teamPrefab.TeamString.Contains($"_{teamDefault}"))
                        {
	                        selectedTeam = defaultTeam;
	                        
                            teamPrefab.SetIcon(defaultTeam, true);

                            SetBanner(defaultTeam);

                            _selectionText.text = $"You have selected the {defaultTeam.Info.CityName} {defaultTeam.Info.NickName} of the {defaultTeam.Info.League}";
                            _continueButton.gameObject.SetActive(true);
                        }
                    }
                }
            }
		}
    }

    private void SetBanner(Team team)
    {
	    CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the team banner.");
	    
	    GameplayController.Inst.SetGameTeam(team, true);

	    SetBanner();
    }
    
    private void RefreshAllIcons()
    {
	    foreach (Transform obj in _container)
	    {
		    FavoriteTeamPrefab icon = obj.GetComponent<FavoriteTeamPrefab>();
            
		    icon.SwitchOff();
	    }
    }

    private void ClearContainer()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Clearing the selected team container.");

        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }

        _selectionText.text = "You have not selected a team";

        selectedTeam = null;
		teamSelections.Clear();

        _continueButton.gameObject.SetActive(false);
    }

	private List<Team> SetTeamSelections()
	{
		List<Team> teamList = new();

		switch (selectedLeague)
		{
			case ConstantController.LeagueType.PWHL:
				teamList = TeamsController.Inst.AllPwhlTeams;
				break;
			case ConstantController.LeagueType.NHLFranchise:
				teamList = TeamsController.Inst.AllNhlFranchiseTeams;
				break;
			case ConstantController.LeagueType.PWHLFranchise:
				teamList = TeamsController.Inst.AllPwhlFranchiseTeams;
				break;
			case ConstantController.LeagueType.NHL:
			default:
				teamList = TeamsController.Inst.AllNhlTeams;
				break;
		}

		return teamList.OrderBy(team => team.Info.CityName).ToList();
	}

	private void ChangeLeagueOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the league team option.");

		switch (option)
		{
			case 1:
				selectedLeague = ConstantController.LeagueType.PWHL;
				break;
			case 2:
				selectedLeague = ConstantController.LeagueType.NHLFranchise;
				break;
			// case 3:
			// 	selectedLeague = ConstantController.LeagueType.PWHLFranchise;
			// 	break;
			case 0:
			default:
				selectedLeague = ConstantController.LeagueType.NHL;
				break;
		}

		SetContainer();
    }
#endregion
}}
