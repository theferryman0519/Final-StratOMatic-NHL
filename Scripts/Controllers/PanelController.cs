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

    public void ShowBottomPanel(ConstantController.PanelType panelType)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the bottom panel.");

        PanelData newPanel = GetBottomPanel(panelType);

        if (!IsBottomVisible && (newPanel != null))
        {
            _bottomPanel.gameObject.SetActive(true);
            _bottomPanel.InitializePanel(newPanel);
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private BottomPanel GetBottomPanel(ConstantController.PanelType panelType)
    {
        BottomPanel newPanel = new BottomPanel
        {
            ButtonA = "",
            ButtonB = "",
            HasCloseButton = false,
            ButtonCount = 0,
            SpriteA = 0,
            SpriteB = 0,
            ActionA = null,
            ActionB = null,
        };

        switch (panelType)
        {
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
                newPanel.Title = "Invalid Credentials";
                newPanel.Body = "It looks like your login information is incorrect. Please return and enter the correct email and password to log into the game.";
                newPanel.ButtonA = "Return";
                newPanel.HasCloseButton = true;
                newPanel.ButtonCount = 1;
                newPanel.ActionA = () => { _bottomPanel.ClosePanel(); };
                break;
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
