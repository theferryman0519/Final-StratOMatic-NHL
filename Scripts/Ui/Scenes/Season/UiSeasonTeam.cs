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
public class UiSeasonTeam : UiSceneBase {

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

	private ConstantController.Inst.LeagueType selectedLeague = ConstantController.Inst.LeagueType.NHL;
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

        SetContainer();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToOptions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to season options screen.");

        // TODO: Set season team

		GoToScene(CoreController.Inst.Scene_Season01);
    }

	private void GoToHome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to home screen.");

		GoToScene(CoreController.Inst.Scene_Home00);
    }
    
    private void SetContainer()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the selected team container.");

        ClearContainer();

		teamSelections = SetTeamSelections();

        foreach (Team team in teamSelections)
        {
            GameObject icon = Instantiate(_teamPrefab, _container);

            icon.SetIcon(team, false);
            icon.SetListener(() =>
            {
                CoreController.Inst.WriteLog(this.GetType().Name, $"Choosing {team.Info.Code} as a selected team.");

                selectedTeam = team;
                icon.SetIcon(team, true);

				SetBanner(team);

                _selectionText.text = $"You have selected the {team.Info.CityName} {team.Info.NickName} of the {team.Info.League}";
                _continueButton.gameObject.SetActive(true);
            });
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
			case ConstantController.Inst.LeagueType.PWHL:
				teamList = TeamsController.Inst.AllPwhlTeams;
				break;
			case ConstantController.Inst.LeagueType.NHL:
			default:
				teamList = TeamsController.Inst.AllNhlTeams;
				break;
		}

		return teamList;
	}

	private void ChangeLeagueOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the league team option.");

		switch (option)
		{
			case 1:
				selectedLeague = ConstantController.Inst.LeagueType.PWHL;
				break;
			case 0:
			default:
				selectedLeague = ConstantController.Inst.LeagueType.NHL;
				break;
		}

		SetContainer();
    }
#endregion
}}
