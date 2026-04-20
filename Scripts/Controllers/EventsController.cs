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
using SoM.Events;
using SoM.Models;
using SoM.Ui;

namespace SoM.Controllers {
public class EventsController : Singleton<EventsController> {

#region -------------------- Serialized Variables --------------------
    [Header("Overlay Blocker")]
	[SerializeField] private GameObject _overlayBlocker;
#endregion
#region -------------------- Public Variables --------------------
    public GameplayEvents GameplayEvents;

    public Action ContinueAction;

    public EventRun CurrentEventRun;

    public UiGameplayMain MainUi;
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

        if (GameplayEvents == null)
        {
            GameplayEvents = gameObject.AddComponent<GameplayEvents>();
        }

        ToggleOverlay(false);

        CoreController.Inst.LoadingStepCompleted();
    }

    public void InitializeEvents(UiGameplayMain mainUi)
    {
        if (mainUi == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the gameplay events.");

        GameplayEvents.InitializeEvents();
        ContinueAction = null;
        MainUi = mainUi;
    }

    public void RunFaceoffEvent(int index)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running a FaceoffEvent ----------");

        GameplayEvents.RunFaceoffEvent(index);
    }

    public void RunOffenseEvent(int index)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running a OffenseEvent ----------");

        GameplayEvents.RunOffenseEvent(index);
    }

    public void RunDefenseEvent(int index)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running a DefenseEvent ----------");

        GameplayEvents.RunDefenseEvent(index);
    }

    public void RunPenaltyEvent(int index)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running a PenaltyEvent ----------");

        GameplayEvents.RunPenaltyEvent(index);
    }

    public void RunGoalEvent(int index)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running a GoalEvent ----------");

        GameplayEvents.RunGoalEvent(index);
    }

    public void RunPullGoalieEvent(int index)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running a PullGoalieEvent ----------");

        GameplayEvents.RunPullGoalieEvent(index);
    }

    public void RunGameFlowEvent(int index)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running a GameFlowEvent ----------");

        GameplayEvents.RunGameFlowEvent(index);
    }

    public void ToggleOverlay(bool isOverlayOn)
	{
		_overlayBlocker.SetActive(isOverlayOn);
	}
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
