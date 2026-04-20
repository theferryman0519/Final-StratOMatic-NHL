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
public class UiSettingsUpdateTeam : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Grid Elements")]
    [SerializeField] private Transform _container;
    [SerializeField] private FavoriteTeamPrefab _teamPrefab;

    [Header("Button Elements")]
    [SerializeField] private SoM_Button _saveButton;

    [Header("Team Select Elements")]
    [SerializeField] private TMP_Text _selectionText;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private Team favTeam;
    
    private List<string> usedTeams = new();
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
        _saveButton.SetListener(SaveChanges);

        ConstantController.LeagueType leagueType = ConstantController.LeagueType.None;
        
        if (UsersController.Inst.UserData.Info.League == "NHL") { leagueType = ConstantController.LeagueType.NHL; }
        else if (UsersController.Inst.UserData.Info.League == "PWHL") { leagueType = ConstantController.LeagueType.PWHL; }
        else if (UsersController.Inst.UserData.Info.League == "NHLFranchise") { leagueType = ConstantController.LeagueType.NHLFranchise; }
        else { leagueType = ConstantController.LeagueType.PWHLFranchise; }

        favTeam = TeamsController.Inst.GetTeamFromCode(UsersController.Inst.UserData.Info.Team, leagueType);

        SetContainer();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SaveChanges()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Attempting to save changes to the user favorite team.");

        UsersController.Inst.UserData.Info.Team = favTeam.Info.Code;
        UsersController.Inst.UserData.Info.League = favTeam.Info.League;

        UsersController.Inst.SaveUserData(() =>
        {
            GoToNewScene(CoreController.Inst.Scene_Settings02);
        });
    }
    
    private void SetContainer()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the favorite team container.");

        ClearContainer();

        foreach (Team team in TeamsController.Inst.AllTeams)
        {
            string usedTeamName = $"{team.Info.CityName}_{team.Info.NickName}";
            
            if (!usedTeams.Contains(usedTeamName))
            {
                FavoriteTeamPrefab icon = Instantiate(_teamPrefab, _container);

                icon.SetIcon(team, false);
                icon.IconButton.onClick.AddListener(() =>
                {
                    CoreController.Inst.WriteLog(this.GetType().Name, $"Choosing {team.Info.Code} of the {team.Info.League} as a favorite team.");

                    RefreshAllIcons();

                    favTeam = team;
                    icon.SetIcon(team, true);

                    string league = team.Info.League.Contains("NHL") ? "NHL" : "PWHL";

                    _selectionText.text = $"You have selected the {team.Info.CityName} {team.Info.NickName} of the {league}";
                    _saveButton.gameObject.SetActive(true);
                });
                
                usedTeams.Add(usedTeamName);
            }
        }
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
        CoreController.Inst.WriteLog(this.GetType().Name, $"Clearing the favorite team container.");

        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }

        _selectionText.text = "You have not selected a team";

        favTeam = null;
        usedTeams.Clear();

        _saveButton.gameObject.SetActive(false);
    }
#endregion
}}
