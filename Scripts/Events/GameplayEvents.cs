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

namespace SoM.Events {
public class GameplayEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public FaceoffEvents FaceoffEvents;
    public OffenseEvents OffenseEvents;
    public DefenseEvents DefenseEvents;
    public PenaltyEvents PenaltyEvents;
    public GoalEvents GoalEvents;
    public PullGoalieEvents PullGoalieEvents;
    public GameFlowEvents GameFlowEvents;
#endregion
#region -------------------- Private Variables --------------------
    private bool isQueueRunning;
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    private IEnumerator RunningEvent(IEnumerator runEvent)
    {
        isQueueRunning = true;

        yield return StartCoroutine(runEvent);

        if (EventsController.Inst.MainUi != null) { EventsController.Inst.MainUi.UpdateVisual(); }

        yield return new WaitForSeconds(0.15f);

        isQueueRunning = false;

        EventsController.Inst.ToggleOverlay(false);
    }
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeEvents()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the gameplay events.");

        isQueueRunning = false;

        if (FaceoffEvents == null) { FaceoffEvents = gameObject.AddComponent<FaceoffEvents>(); }
        if (OffenseEvents == null) { OffenseEvents = gameObject.AddComponent<OffenseEvents>(); }
        if (DefenseEvents == null) { DefenseEvents = gameObject.AddComponent<DefenseEvents>(); }
        if (PenaltyEvents == null) { PenaltyEvents = gameObject.AddComponent<PenaltyEvents>(); }
        if (GoalEvents == null) { GoalEvents = gameObject.AddComponent<GoalEvents>(); }
        if (PullGoalieEvents == null) { PullGoalieEvents = gameObject.AddComponent<PullGoalieEvents>(); }
        if (GameFlowEvents == null) { GameFlowEvents = gameObject.AddComponent<GameFlowEvents>(); }
    }

    public void StopAllEvents()
    {
        if (!isQueueRunning) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Stopping all gameplay events from running.");

        StopAllCoroutines();
    }

    public void RunFaceoffEvent(int index)
    {
        if (FaceoffEvents == null) { InitializeEvents(); }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Running a FaceoffEvent.");

        switch (index)
        {
            case 0: RunEvent(FaceoffEvents.PuckDrop); break;
            case 1: RunEvent(FaceoffEvents.FaceoffStart); break;
            case 2: RunEvent(FaceoffEvents.FaceoffResult); break;
            default: break;
        }
    }

    public void RunOffenseEvent(int index, ConstantController.ShotType shotType)
    {
        if (OffenseEvents == null) { InitializeEvents(); }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Running a OffenseEvent.");

        OffenseEvents.SelectedShotType = shotType;

        switch (index)
        {
            case 0: RunEvent(OffenseEvents.ActionCard); break;
            case 1: RunEvent(OffenseEvents.OutsideOptions); break;
            case 2: RunEvent(OffenseEvents.ShotStart); break;
            case 3: RunEvent(OffenseEvents.ShotResultLose); break;
            case 4: RunEvent(OffenseEvents.ShotResultSave); break;
            case 5: RunEvent(OffenseEvents.ShotResultRebound); break;
            case 6: RunEvent(OffenseEvents.ShotResultGoalieRating); break;
            case 7: RunEvent(OffenseEvents.ShotResultGoal); break;
            case 8: RunEvent(OffenseEvents.ReboundCheck); break;
            case 9: RunEvent(OffenseEvents.PassingStart); break;
            case 10: RunEvent(OffenseEvents.PassingResultLose); break;
            case 11: RunEvent(OffenseEvents.PassingResultLoseShot); break;
            case 12: RunEvent(OffenseEvents.PassingResultShot); break;
            case 13: RunEvent(OffenseEvents.PassingResultShotIntimidation); break;
            case 14: RunEvent(OffenseEvents.PassingResultOptions); break;
            default: break;
        }
    }

    public void RunDefenseEvent(int index)
    {
        if (DefenseEvents == null) { InitializeEvents(); }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Running a DefenseEvent.");

        switch (index)
        {
            case 0: RunEvent(DefenseEvents.IntimidationStart); break;
            case 1: RunEvent(DefenseEvents.IntimidationResult); break;
            case 2: RunEvent(DefenseEvents.DefendingStart); break;
            case 3: RunEvent(DefenseEvents.DefendingResult); break;
            default: break;
        }
    }

    public void RunPenaltyEvent(int index)
    {
        if (PenaltyEvents == null) { InitializeEvents(); }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Running a PenaltyEvent.");

        switch (index)
        {
            case 0: RunEvent(PenaltyEvents.PenaltyCheck); break;
            case 1: RunEvent(PenaltyEvents.PenaltyShotsList); break;
            case 2: RunEvent(PenaltyEvents.PenaltyShotsStart); break;
            case 3: RunEvent(PenaltyEvents.PenaltyShotsAttemptStart); break;
            case 4: RunEvent(PenaltyEvents.PenaltyShotsAttemptResult); break;
            case 5: RunEvent(PenaltyEvents.PenaltyShotsResult); break;
            default: break;
        }
    }

    public void RunGoalEvent(int index)
    {
        if (GoalEvents == null) { InitializeEvents(); }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Running a GoalEvent.");

        switch (index)
        {
            case 0: RunEvent(GoalEvents.GoalieRatingStart); break;
            case 1: RunEvent(GoalEvents.GoalieRatingResult); break;
            case 2: RunEvent(GoalEvents.GoalCheck); break;
            case 3: RunEvent(GoalEvents.Goal); break;
            default: break;
        }
    }

    public void RunPullGoalieEvent(int index)
    {
        if (PullGoalieEvents == null) { InitializeEvents(); }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Running a PullGoalieEvent.");

        switch (index)
        {
            case 0: RunEvent(PullGoalieEvents.PullGoalieShotsList); break;
            case 1: RunEvent(PullGoalieEvents.PullGoalieShotsStart); break;
            case 2: RunEvent(PullGoalieEvents.PullGoalieShotsAttemptStart); break;
            case 3: RunEvent(PullGoalieEvents.PullGoalieShotsAttemptResult); break;
            case 4: RunEvent(PullGoalieEvents.PullGoalieShotsResult); break;
            default: break;
        }
    }

    public void RunGameFlowEvent(int index)
    {
        if (GameFlowEvents == null) { InitializeEvents(); }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Running a GameFlowEvent.");

        switch (index)
        {
            case 0: RunEvent(GameFlowEvents.StartOfGame); break;
            case 1: RunEvent(GameFlowEvents.StartOfPeriod); break;
            case 2: RunEvent(GameFlowEvents.Injury); break;
            case 3: RunEvent(GameFlowEvents.EndOfPeriod); break;
            case 4: RunEvent(GameFlowEvents.OvertimeStart); break;
            case 5: RunEvent(GameFlowEvents.EndOfGame); break;
            case 6: RunEvent(GameFlowEvents.CompleteGame); break;
            default: break;
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private void RunEvent(IEnumerator runEvent)
    {
        if (runEvent = null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Running gameplay event.");

        EventsController.Inst.ToggleOverlay(true);

        StartCoroutine(RunningEvent(runEvent));
    }
#endregion
}}
