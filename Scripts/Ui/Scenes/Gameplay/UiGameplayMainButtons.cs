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
    [SerializeField] private SoM_Button _outOptionShotButton;
    [SerializeField] private SoM_Button _outOptionPassButton;
    [SerializeField] private SoM_Button _outOptionDriveButton;
    [SerializeField] private SoM_Button _forwardsButton;
    [SerializeField] private SoM_Button _defenseButton;
    [SerializeField] private SoM_Button _strategiesButton;
    [SerializeField] private SoM_Button _pullGoalieButton;

    [SerializeField] private GameObject _actionObject;
    [SerializeField] private GameObject _outOptionShotObject;
    [SerializeField] private GameObject _outOptionPassObject;
    [SerializeField] private GameObject _outOptionDriveObject;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private UiGameplayMain mainUi;

    private bool lastActionInteractable;
#endregion
#region -------------------- Initial Functions --------------------
    void Update()
    {
        if (mainUi == null) { return; }

        bool isInteractable = !mainUi.IsMoving;

        if (lastActionInteractable != isInteractable)
        {
            _actionButton.SetInteractivity(isInteractable);

            lastActionInteractable = isInteractable;
        }
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void UpdateButtons(UiGameplayMain ui)
    {
        if (ui == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the buttons.");

        mainUi = ui;

        lastActionInteractable = !mainUi.IsMoving;
        _actionButton.SetInteractivity(lastActionInteractable);

        string actionText = EventsController.Inst.CurrentEventRun.ButtonText;
        string pullGoalieText = GameplayController.Inst.GameData.HomeTeam.IsGoaliePulled ? "Pulling Goalie..." : "Pull Goalie";

        _actionButton.SetText(actionText);
        _actionButton.SetListener(() =>
        {
            EventsController.Inst.ContinueAction?.Invoke();
        });

        _outOptionShotButton.SetListener(() =>
        {
            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Outside;
            EventsController.Inst.RunOffenseEvent(2);

            mainUi.IsOutsideOptions = false;
        });

        _outOptionPassButton.SetListener(() =>
        {
            EventsController.Inst.RunOffenseEvent(9);

            mainUi.IsOutsideOptions = false;
        });

        _outOptionDriveButton.SetListener(() =>
        {
            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Inside;
            EventsController.Inst.RunDefenseEvent(0);

            mainUi.IsOutsideOptions = false;
        });

        if (mainUi.IsOutsideOptions)
        {
            _actionObject.SetActive(false);
            _outOptionShotObject.SetActive(true);
            _outOptionPassObject.SetActive(true);
            _outOptionDriveObject.SetActive(true);
        }

        else
        {
            _actionObject.SetActive(true);
            _outOptionShotObject.SetActive(false);
            _outOptionPassObject.SetActive(false);
            _outOptionDriveObject.SetActive(false);
        }

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
