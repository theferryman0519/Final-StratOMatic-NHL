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
public class UiGameplayMainButtons : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
    [SerializeField] private SoM_Button _actionButton;
    [SerializeField] private SoM_Button _forwardsButton;
    [SerializeField] private SoM_Button _defenseButton;
    [SerializeField] private SoM_Button _strategiesButton;
    [SerializeField] private SoM_Button _pullGoalieButton;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private UiGameplayMain mainUi;
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void UpdateButtons(UiGameplayMain ui)
    {
        if (ui == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the buttons.");

        mainUi = ui;

        string actionText = EventsController.Inst.CurrentEventRun.ButtonText;
        string pullGoalieText = GameplayController.Inst.GameData.HomeTeam.IsGoaliePulled ? "Pulling Goalie..." : "Pull Goalie";

        _actionButton.SetText(actionText);
        _actionButton.SetListener(() =>
        {
            EventsController.Inst.ContinueAction?.Invoke();
        });

        _forwardsButton.SetListener(() =>
        {
            mainUi.ShowForwardsPanel();
        });

        _defenseButton.SetListener(() =>
        {
            mainUi.ShowDefensePanel();
        });

        _strategiesButton.SetListener(() =>
        {
            mainUi.ShowStrategiesPanel();
        });

        _pullGoalieButton.SetText(pullGoalieText);
        _pullGoalieButton.SetListener(() =>
        {
            GameplayController.Inst.GameData.HomeTeam.IsGoaliePulled = !GameplayController.Inst.GameData.HomeTeam.IsGoaliePulled;
        });
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
