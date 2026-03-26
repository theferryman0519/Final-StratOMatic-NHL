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
using SoM.Core;

namespace SoM.Controllers {
public class PanelController : Singleton<PanelController> {

#region -------------------- Serialized Variables --------------------
    [Header("Panel Elements")]
    [SerializeField] private UiBottomPanel _bottomPanel;
#endregion
#region -------------------- Public Variables --------------------
    public bool IsBottomVisible = false;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        IsBottomVisible = false;
    }

    public void ShowBottomPanel(ConstantController.PanelType panelType, Action actionA = null, Action actionB = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the bottom panel.");

        PanelData newPanel = GetBottomPanel(panelType, actionA, actionB);

        if (!IsBottomVisible && (newPanel != null))
        {
            _bottomPanel.gameObject.SetActive(true);
            _bottomPanel.InitializePanel(newPanel);
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private BottomPanel GetBottomPanel(ConstantController.PanelType panelType, Action actionA, Action actionB)
    {
        BottomPanel newPanel = new BottomPanel
        {
            ButtonA = "",
            ButtonB = "",
            HasCloseButton = false,
            ButtonCount = 0,
            SpriteA = 0,
            SpriteB = 0,
        };

        if (actionA == null) { newPanel.ActionA = () => { _bottomPanel.ClosePanel(); }; }
        else { newPanel.ActionA = () => { actionA?.Invoke(); }; }

        if (actionB == null) { newPanel.ActionB = () => { _bottomPanel.ClosePanel(); }; }
        else { newPanel.ActionB = () => { actionB?.Invoke(); }; }

        switch (panelType)
        {
            // Main
            case ConstantController.PanelType.OpeningInternetError:
                newPanel.Title = "No Internet";
                newPanel.Body = "You do not appear to be connected to the internet. Please connect to the internet, then restart the game and try again.";
                break;
            case ConstantController.PanelType.LoadingError:
                newPanel.Title = "Loading Error";
                newPanel.Body = "There appears to be an error loading the data. Please restart the game and try again.";
                break;
            case ConstantController.PanelType.LoadingOutdatedVersion:
                newPanel.Title = "Outdated Version";
                newPanel.Body = "It looks like your current game version is out of date. Please visit the appropriate app store to download the latest udpate.";
                break;
            case ConstantController.PanelType.LoginInvalidCredentials:
            case ConstantController.PanelType.SignUpInvalidCredentials:
                newPanel.Title = "Invalid Credentials";
                newPanel.Body = "It looks like your information is incorrect. Please return and enter the correct email and password to log into the game.";
                newPanel.ButtonA = "Return";
                newPanel.HasCloseButton = true;
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotIntialize:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            
            // Firebase - Get
            case ConstantController.PanelType.FirebaseCannotGetVersions:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve the current version. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetUser:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve the user account. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetUserGame:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve your current game. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetUserSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve your current season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetUserPlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve your current playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetTeam:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve the team data. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetTeams:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve all teams. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetTeamSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve your team season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetTeamPlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve your team playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetSkater:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve the skater data. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetSkaters:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve all skater. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetSkaterSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve your skater season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetSkaterPlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve your skater playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetGoalie:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve the goalie data. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetGoalies:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve all goalie. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetGoalieSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve your goalie season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotGetGoaliePlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and retrieve your goalie playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;

            // Firebase - Put
            case ConstantController.PanelType.FirebaseCannotPutVersions:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update the current version. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutUser:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update the user account. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutUserGame:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update your current game. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutUserSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update your current season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutUserPlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update your current playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutTeam:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update the team data. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutTeams:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update all teams. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutTeamSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update your team season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutTeamPlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update your team playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutSkater:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update the skater data. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutSkaters:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update all skater. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutSkaterSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update your skater season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutSkaterPlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update your skater playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutGoalie:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update the goalie data. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutGoalies:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update all goalie. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutGoalieSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update your goalie season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotPutGoaliePlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and update your goalie playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;

            // Firebase - Delete
            case ConstantController.PanelType.FirebaseCannotDeleteVersions:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete the current version. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteUser:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete the user account. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteUserGame:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete your current game. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteUserSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete your current season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteUserPlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete your current playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteTeamSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete your team season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteTeamPlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete your team playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteSkaterSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete your skater season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteSkaterPlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete your skater playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteGoalieSeason:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete your goalie season. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            case ConstantController.PanelType.FirebaseCannotDeleteGoaliePlayoff:
                newPanel.Title = "Firebase Error";
                newPanel.Body = "It looks like you are unable to connect to the server and delete your goalie playoff. Please contact support and try running the game again shortly.";
                newPanel.ButtonA = "Contact Support";
                newPanel.ButtonCount = 1;
                break;
            
            // Settings
            case ConstantController.PanelType.SettingsResetAccount:
                newPanel.Title = "Reset Statistics";
                newPanel.Body = "Are you sure you want to reset your user statistics? This will not delete your account, but all your records will be deleted.";
                newPanel.ButtonA = "Yes, Reset Statistics";
                newPanel.ButtonB = "No, Return";
                newPanel.HasCloseButton = true;
                newPanel.ButtonCount = 2;
                newPanel.SpriteA = 1;
                break;
            case ConstantController.PanelType.SettingsDeleteAccount:
                newPanel.Title = "Delete Account";
                newPanel.Body = "Are you sure you want to delete your user account? This will remove all data as well, and you will need to start a new account.";
                newPanel.ButtonA = "Yes, Delete Account";
                newPanel.ButtonB = "No, Return";
                newPanel.HasCloseButton = true;
                newPanel.ButtonCount = 2;
                newPanel.SpriteA = 1;
                break;
            
            // Multiplayer
            case ConstantController.PanelType.MultiplayerOpponentLeft:
                newPanel.Title = "Opponent Left";
                newPanel.Body = "It looks like your opponent has left the multiplayer game. Please return to the lobby.";
                newPanel.ButtonA = "Return";
                newPanel.ButtonCount = 1;
                break;
            
            // Default
            default:
                newPanel.Title = "Game Error";
                newPanel.Body = "There appears to be an error with the game, making you unable to continue. Please restart the game and try again.";
                break;
        }

        return newPanel;
    }
#endregion
}}

// case ConstantController.PanelType.xxxx:
//                 newPanel.Title = "xxxx";
//                 newPanel.Body = "xxxx.";
//                 break;
