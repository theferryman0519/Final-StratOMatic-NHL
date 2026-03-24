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
        };

        switch (panelType)
        {
            case ConstantController.PanelType.OpeningInternetError:
                newPanel.Title = "No Internet";
                newPanel.Body = "You do not appear to be connected to the internet. Please connect to the internet, then restart the game and try again.";
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
