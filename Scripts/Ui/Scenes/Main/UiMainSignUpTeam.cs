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

        // TODO
    }
    
    private void SetContainer()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the favorite team container.");

        ClearContainer();

        foreach (Team team in TeamsController.Inst.AllTeams)
        {
            GameObject icon = Instantiate(_teamPrefab, _container);

            icon.SetIcon(team, false);
            icon.SetListener(() =>
            {
                CoreController.Inst.WriteLog(this.GetType().Name, $"Choosing {team.Info.Code} as a favorite team.");

                favTeam = team;
                icon.SetIcon(team, true);

                _selectionText.text = $"You have selected the {team.Info.CityName} {team.Info.NickName} of the {team.Info.League}";
                _signUpButton.gameObject.SetActive(true);
            });
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

        _signUpButton.gameObject.SetActive(false);
    }
#endregion
}}
