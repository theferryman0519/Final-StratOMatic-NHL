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
public class UiMainSignUpTeam : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Grid Elements")]
    [SerializeField] private Transform _container;
    [SerializeField] private FavoriteTeamPrefab _teamPrefab;

    [Header("Button Elements")]
    [SerializeField] private SoM_Button _signUpButton;

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
        _signUpButton.SetListener(SignUpAccount);

        SetContainer();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SignUpAccount()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Signing up the account.");

        UsersController.Inst.UserData.Info.Name = UsersController.Inst.TempName;
        UsersController.Inst.UserData.Info.Email = UsersController.Inst.TempEmail;
        UsersController.Inst.UserData.Info.Password = UsersController.Inst.TempPassword;
        UsersController.Inst.UserData.Info.Team = favTeam.Info.Code;
        UsersController.Inst.UserData.Info.League = favTeam.Info.League;

        UsersController.Inst.SaveUserData(() =>
        {
            GoToNewScene(CoreController.Inst.Scene_Home00);
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
                    CoreController.Inst.WriteLog(this.GetType().Name, $"Choosing {team.Info.Code} as a favorite team.");

                    RefreshAllIcons();

                    favTeam = team;
                    icon.SetIcon(team, true);

                    string league = team.Info.League.Contains("NHL") ? "NHL" : "PWHL";

                    _selectionText.text = $"You have selected the {team.Info.CityName} {team.Info.NickName} of the {league}";
                    _signUpButton.gameObject.SetActive(true);
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

        _signUpButton.gameObject.SetActive(false);
    }
#endregion
}}
