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
public class UiExhibitionResults : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _returnButton;

    [Header("Text Elements")]
    [SerializeField] private TMP_Text _homeTeamText;
    [SerializeField] private TMP_Text _homeStatsText;
    [SerializeField] private TMP_Text _awayTeamText;
    [SerializeField] private TMP_Text _awayStatsText;

    [Header("Icon Elements")]
    [SerializeField] private Image _homeIcon;
    [SerializeField] private Image _awayIcon;
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
		_returnButton.SetListener(SetUserStats);

        SetGameData();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SetUserStats()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the user stats.");

        GameTeam homeTeam = GameplayController.Inst.GameData.HomeTeam;
        GameTeam awayTeam = GameplayController.Inst.GameData.AwayTeam;

        string result = "Win";
        string league = homeTeam.Team.League;
        bool isOvertime = EventsController.Inst.GameplayEvents.GameFlowEvents.IsOvertimeGame;

        if (homeTeam.Stats.Goals < awayTeam.Stats.Goals) { result = isOvertime ? "OTL" : "Lose"; }
        else if (homeTeam.Stats.Goals == awayTeam.Stats.Goals) { result = "Tie"; }

        if (league == "NHLFranchise")
        {
            if (result == "Win") { UsersController.UserData.Stats.NhlFranchiseWins += 1; }
            else if (result == "Lose") { UsersController.UserData.Stats.NhlFranchiseLosses += 1; }
            else if (result == "Tie") { UsersController.UserData.Stats.NhlFranchiseTies += 1; }
            else { UsersController.UserData.Stats.NhlFranchiseOTLs += 1; }
        }

        else if (league == "PWHL")
        {
            if (result == "Win") { UsersController.UserData.Stats.PwhlWins += 1; }
            else if (result == "Lose") { UsersController.UserData.Stats.PwhlLosses += 1; }
            else if (result == "Tie") { UsersController.UserData.Stats.PwhlTies += 1; }
            else { UsersController.UserData.Stats.PwhlOTLs += 1; }
        }

        else if (league == "PWHLFranchise")
        {
            if (result == "Win") { UsersController.UserData.Stats.PwhlFranchiseWins += 1; }
            else if (result == "Lose") { UsersController.UserData.Stats.PwhlFranchiseLosses += 1; }
            else if (result == "Tie") { UsersController.UserData.Stats.PwhlFranchiseTies += 1; }
            else { UsersController.UserData.Stats.PwhlFranchiseOTLs += 1; }
        }

        else // NHL
        {
            if (result == "Win") { UsersController.UserData.Stats.NhlWins += 1; }
            else if (result == "Lose") { UsersController.UserData.Stats.NhlLosses += 1; }
            else if (result == "Tie") { UsersController.UserData.Stats.NhlTies += 1; }
            else { UsersController.UserData.Stats.NhlOTLs += 1; }
        }

        UserDatabase userData = SaveController.Inst.GetCurrentUserSaveData();

        await FirebaseController.Inst.PutUser(userData, UsersController.Inst.UserData.Id, GoToHome);
    }
    
    private void GoToHome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to home screen.");

        GoToNewScene(CoreController.Inst.Scene_Home00);
    }

    private void SetGameData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game data.");

        GameTeam homeTeam = GameplayController.Inst.GameData.HomeTeam;
        GameTeam awayTeam = GameplayController.Inst.GameData.AwayTeam;

        _homeTeamText.text = homeTeam.Team.Code;
        _awayTeamText.text = awayTeam.Team.Code;

        string homeString = $"{homeTeam.Team.League}_{homeTeam.Team.Code}_ON";
        string awayString = $"{awayTeam.Team.League}_{awayTeam.Team.Code}_ON";

        _homeIcon.sprite = ConstantController.Inst.IconSprites[homeString];
        _awayIcon.sprite = ConstantController.Inst.IconSprites[awayString];

        int homeGoals = homeTeam.Stats.Goals;
		int homeShots = homeTeam.Stats.Shots;
		int homePPGs = homeTeam.Stats.PowerplayGoals;
		int homePPs = homeTeam.Stats.Powerplays;
		int homeFOWs = homeTeam.Stats.FaceoffsWon;
		int homeHits = homeTeam.Stats.Hits;

		int awayGoals = awayTeam.Stats.Goals;
		int awayShots = awayTeam.Stats.Shots;
		int awayPPGs = awayTeam.Stats.PowerplayGoals;
		int awayPPs = awayTeam.Stats.Powerplays;
		int awayFOWs = awayTeam.Stats.FaceoffsWon;
		int awayHits = awayTeam.Stats.Hits;

        _homeStatsText.text = $"{homeGoals}" + "\n" +
            $"{homeShots}" + "\n" +
            $"{homePPGs} - {homePPs}" + "\n" +
            $"{homeHits}" + "\n" +
            $"{homeFOWs} of {homeFOWs + awayFOWs}";
        
        _awayStatsText.text = $"{awayGoals}" + "\n" +
            $"{awayShots}" + "\n" +
            $"{awayPPGs} - {awayPPs}" + "\n" +
            $"{awayHits}" + "\n" +
            $"{awayFOWs} of {homeFOWs + awayFOWs}";
    }
#endregion
}}
