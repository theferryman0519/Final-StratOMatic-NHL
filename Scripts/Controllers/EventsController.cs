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
public class EventsController : Singleton<EventsController> {

#region -------------------- Serialized Variables --------------------
    [Header("Overlay Blocker")]
	[SerializeField] private GameObject _overlayBlocker;
#endregion
#region -------------------- Public Variables --------------------
    public GameplayEvents GameplayEvents;

    public Action ContinueAction;

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

    public void FaceoffEvents()
    {
        if (GameplayEvents == null || GameplayEvents.FaceoffEvents == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running FaceoffEvents ----------.");

        // ADD ENQUEUE HERE

        GameplayEvents.RunFaceoffEvents();
    }

    public void OffenseEvents()
    {
        if (GameplayEvents == null || GameplayEvents.OffenseEvents == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running OffenseEvents ----------.");

        // ADD ENQUEUE HERE

        GameplayEvents.RunOffenseEvents();
    }

    public void DefenseEvents()
    {
        if (GameplayEvents == null || GameplayEvents.DefenseEvents == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running DefenseEvents ----------.");

        // ADD ENQUEUE HERE

        GameplayEvents.RunDefenseEvents();
    }

    public void PenaltyEvents()
    {
        if (GameplayEvents == null || GameplayEvents.PenaltyEvents == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running PenaltyEvents ----------.");

        // ADD ENQUEUE HERE

        GameplayEvents.RunPenaltyEvents();
    }

    public void GoalEvents()
    {
        if (GameplayEvents == null || GameplayEvents.GoalEvents == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running GoalEvents ----------.");

        // ADD ENQUEUE HERE

        GameplayEvents.RunGoalEvents();
    }

    public void PullGoalieEvents()
    {
        if (GameplayEvents == null || GameplayEvents.PullGoalieEvents == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running PullGoalieEvents ----------.");

        // ADD ENQUEUE HERE

        GameplayEvents.RunPullGoalieEvents();
    }

    public void GameFlowEvents()
    {
        if (GameplayEvents == null || GameplayEvents.GameFlowEvents == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"---------- Running GameFlowEvents ----------.");

        // ADD ENQUEUE HERE

        GameplayEvents.RunGameFlowEvents();
    }

    public void ToggleOverlay(bool isOverlayOn)
	{
		_overlayBlocker.SetActive(isOverlayOn);
	}
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
