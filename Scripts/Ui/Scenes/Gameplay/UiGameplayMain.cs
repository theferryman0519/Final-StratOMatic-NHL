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
    [SerializeField] private GameplayMenuPanel _menuPanel;
    [SerializeField] private GameplayLogsPanel _logsPanel;
    [SerializeField] private GameplayForwardsPanel _forwardsPanel;
    [SerializeField] private GameplayDefensePanel _defensePanel;
    [SerializeField] private GameplayStrategiesPanel _strategiesPanel;
    [SerializeField] private GameplaySkaterStatsPanel _skaterStatsPanel;
    [SerializeField] private GameplayGoalieStatsPanel _goalieStatsPanel;
    [SerializeField] private GameplayGameStatsPanel _gameStatsPanel;

    [Header("Overlay Elements")]
    [SerializeField] private List<CanvasGroup> _overlays = new();
#endregion
#region -------------------- Public Variables --------------------
    public bool IsOutsideOptions = false;
    public bool IsMoving => _rink.IsMoving;
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

        AnimationController.Inst.FadeOutObjects(_overlays, () =>
        {
            EventsController.Inst.RunGameFlowEvent(0);
        });
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

        _actions.UpdateActions(this);
    }

    public void UpdateRink()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the gameplay rink block.");

        _rink.UpdateRink(this);
    }

    public void UpdateButtons()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the gameplay buttons block.");

        _buttons.UpdateButtons(this);
    }

    public void UpdateCurrent()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the gameplay current block.");

        _current.UpdateCurrent(this);
    }

    public void ShowMenuPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay menu panel.");

        _menuPanel.InitializeMenuPanel(this);
    }

    public void ShowLogsPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay logs panel.");

        _logsPanel.InitializeGameLogsPanel();
    }

    public void ShowForwardsPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay change forwards panel.");

        _forwardsPanel.InitializeForwardsPanel();
    }

    public void ShowDefensePanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay change defense panel.");

        _defensePanel.InitializeDefensePanel();
    }

    public void ShowStrategiesPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay change strategies panel.");

        _strategiesPanel.InitializeStrategiesPanel();
    }

    public void ShowSkaterStatsPanel(Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay skater stats panel.");

        _skaterStatsPanel.InitializeSkaterStatsPanel(skater);
    }

    public void ShowGoalieStatsPanel(Goalie goalie)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay goalie stats panel.");

        _goalieStatsPanel.InitializeGoalieStatsPanel(goalie);
    }

    public void ShowGameStatsPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the gameplay game stats panel.");

        _gameStatsPanel.InitializeGameStatsPanel();
    }

    public void EndGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Ending the game.");

        AnimationController.Inst.FadeInObjects(_overlays, () =>
        {
            if (GameplayController.Inst.GameData.Type == "Exhibition")
            {
                CoreController.Inst.ChangeScene(CoreController.Inst.Scene_Exhibition05);
            }

            else if (GameplayController.Inst.GameData.Type == "Season")
            {
                CoreController.Inst.ChangeScene(CoreController.Inst.Scene_Season10);
            }

            else if (GameplayController.Inst.GameData.Type == "Playoff")
            {
                CoreController.Inst.ChangeScene(CoreController.Inst.Scene_Playoff09);
            }

            else // Multiplayer
            {
                CoreController.Inst.ChangeScene(CoreController.Inst.Scene_Multiplayer09);
            }
        });
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
