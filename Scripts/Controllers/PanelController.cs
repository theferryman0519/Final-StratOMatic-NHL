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
