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
public class UiMainSavedGame : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _continueButton;
	[SerializeField] private SoM_Button _homeButton;

    [Header("Text Elements")]
    [SerializeField] private TMP_Text _homeTeamText;
    [SerializeField] private TMP_Text _homeGoalsText;
    [SerializeField] private TMP_Text _homeShotsText;
    [SerializeField] private TMP_Text _awayTeamText;
    [SerializeField] private TMP_Text _awayGoalsText;
    [SerializeField] private TMP_Text _awayShotsText;
    [SerializeField] private TMP_Text _gameTimeText;

    [Header("Icon Elements")]
    [SerializeField] private Image _homeIcon;
    [SerializeField] private Image _awayIcon;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private GameDatabase gameDatabase;
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
        _continueButton.SetListener(GoToGame);
		_homeButton.SetListener(ShowDeletePanel);

        SetData();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to gameplay screen.");

        GoToNewScene(CoreController.Inst.Scene_Gameplay00);
    }

    private void GoToHome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to home screen.");

        GoToNewScene(CoreController.Inst.Scene_Home00);
    }

    private void ShowDeletePanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the delete saved game panel.");

        PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.SavedGameDeleteGame, DeleteSavedGame);
    }

    private void DeleteSavedGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting the saved game.");

        FirebaseController.Inst.DeleteCurrentGame(UsersController.Inst.UserData.Id, GoToHome);
    }

    private void SetData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game data.");

        GameTeam homeTeam = GameplayController.Inst.GameData.HomeTeam;
        GameTeam awayTeam = GameplayController.Inst.GameData.AwayTeam;

        _homeTeamText.text = homeTeam.Team.Code;
        _awayTeamText.text = awayTeam.Team.Code;
        
        string homeLeague = homeTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";
        string awayLeague = awayTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";

        string homeString = $"{homeLeague}_{homeTeam.Team.Code}_ON";
        string awayString = $"{awayLeague}_{awayTeam.Team.Code}_ON";

        _homeIcon.sprite = ConstantController.Inst.IconSprites[homeString];
        _awayIcon.sprite = ConstantController.Inst.IconSprites[awayString];

        _homeGoalsText.text = GameplayController.Inst.GameData.HomeTeam.Team.Goals.ToString();
        _awayGoalsText.text = GameplayController.Inst.GameData.AwayTeam.Team.Goals.ToString();
        _homeShotsText.text = "Shots: " + GameplayController.Inst.GameData.HomeTeam.Team.Shots.ToString();
        _awayShotsText.text = "Shots: " + GameplayController.Inst.GameData.AwayTeam.Team.Shots.ToString();

        string timeString = SetTimeString();

        _gameTimeText.text = $"{homeTeam.Team.League} {GameplayController.Inst.GameData.Type} Game" + "\n" + $"{timeString}";
    }

    private string SetTimeString()
    {
        string periodText = string.Empty;
        string timeText = string.Empty;

        if (GameplayController.Inst.GameData.Period == 1) { periodText = "1st Period"; }
        else if (GameplayController.Inst.GameData.Period == 2) { periodText = "2nd Period"; }
        else if (GameplayController.Inst.GameData.Period == 3) { periodText = "3rd Period"; }
        else { periodText = "Overtime"; }

        switch (GameplayController.Inst.GameData.CardsDrawn)
        {
            case 0: timeText = "20:00"; break;
            case 1: timeText =  "19:20"; break;
            case 2: timeText =  "18:40"; break;
            case 3: timeText =  "18:00"; break;
            case 4: timeText =  "17:20"; break;
            case 5: timeText =  "16:40"; break;
            case 6: timeText =  "16:00"; break;
            case 7: timeText =  "15:20"; break;
            case 8: timeText =  "14:40"; break;
            case 9: timeText =  "14:00"; break;
            case 10: timeText =  "13:20"; break;
            case 11: timeText =  "12:40"; break;
            case 12: timeText =  "12:00"; break;
            case 13: timeText =  "11:20"; break;
            case 14: timeText =  "10:40"; break;
            case 15: timeText =  "10:00"; break;
            case 16: timeText =  "9:20"; break;
            case 17: timeText =  "8:40"; break;
            case 18: timeText =  "8:00"; break;
            case 19: timeText =  "7:20"; break;
            case 20: timeText =  "6:40"; break;
            case 21: timeText =  "6:00"; break;
            case 22: timeText =  "5:20"; break;
            case 23: timeText =  "4:40"; break;
            case 24: timeText =  "4:00"; break;
            case 25: timeText =  "3:20"; break;
            case 26: timeText =  "2:40"; break;
            case 27: timeText =  "2:00"; break;
            case 28: timeText =  "1:20"; break;
            case 29: timeText =  "0:40"; break;
            case 30: timeText =  "0:05"; break;
            case 31:
            default: timeText =  "0:00"; break;
        }

        return $"{timeText} {periodText}";
    }
#endregion
}}
