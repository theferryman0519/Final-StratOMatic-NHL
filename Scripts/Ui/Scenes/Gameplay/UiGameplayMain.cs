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
public class UiGameplayMain : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Section Block Elements")]
    [SerializeField] private UiGameplayMainScoreboard _scoreboard;
    [SerializeField] private UiGameplayMainActions _actions;
    [SerializeField] private UiGameplayMainRink _rink;
    [SerializeField] private UiGameplayMainButtons _buttons;
    [SerializeField] private UiGameplayMainCurrent _current;

    [Header("Button Elements")]
    [SerializeField] private Button _menuButton;

    [Header("Panel Elements")]
    [SerializeField] private GameplayPanel _menuPanel;
    [SerializeField] private GameplayPanel _logsPanel;
    [SerializeField] private GameplayPanel _forwardsPanel;
    [SerializeField] private GameplayPanel _defensePanel;
    [SerializeField] private GameplayPanel _strategiesPanel;
    [SerializeField] private GameplayPanel _skaterStatsPanel;
    [SerializeField] private GameplayPanel _goalieStatsPanel;
    [SerializeField] private GameplayPanel _gameStatsPanel;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        _menuButton.onClick.RemoveAllListeners();
        _menuButton.onClick.AddListener(() =>
        {
            AnimationController.Inst.ShrinkButton(_menuButton, ShowMenuPanel);
        });

        EventsController.Inst.MainUi = this;
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void UpdateVisuals()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the gameplay visuals.");

        UpdateScoreboard();
        UpdateActions();
        UpdateRink();
        UpdateButtons();
        UpdateCurrent();
    }
    
    public void UpdateScoreboard()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the gameplay scoreboard block.");

        _scoreboard.UpdateScoreboard(this);
    }

    public void UpdateActions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the gameplay actions block.");

        // TODO
    }

    public void UpdateRink()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the gameplay rink block.");

        // TODO
    }

    public void UpdateButtons()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the gameplay buttons block.");

        // TODO
    }

    public void UpdateCurrent()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the gameplay current block.");

        // TODO
    }

    public void ShowMenuPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay menu panel.");

        // TODO
    }

    public void ShowLogsPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay logs panel.");

        // TODO
    }

    public void ShowForwardsPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay change forwards panel.");

        // TODO
    }

    public void ShowDefensePanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay change defense panel.");

        // TODO
    }

    public void ShowStrategiesPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay change strategies panel.");

        // TODO
    }

    public void ShowSkaterStatsPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay skater stats panel.");

        // TODO
    }

    public void ShowGoalieStatsPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay goalie stats panel.");

        // TODO
    }

    public void ShowGameStatsPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay game stats panel.");

        // TODO
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
