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
    private List<Func<IEnumerator>> gameplayQueues;

    private bool isQueueRunning;
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    private IEnumerator RunningEvents()
    {
        isQueueRunning = true;
        
        List<Func<IEnumerator>> currentQueue = gameplayQueues;
        int index = 0;

        while (index < currentQueue.Count)
        {
            yield return StartCoroutine(currentQueue[index]());

            if (EventsController.Inst.MainUi != null) { EventsController.Inst.MainUi.UpdateVisual(); }

            yield return new WaitForSeconds(0.15f);

            index++;
        }

        gameplayQueues.Clear();

        isQueueRunning = false;

        EventsController.Inst.ToggleOverlay(false);

        Action nextAction = EventsController.Inst.ContinueAction;
        EventsController.Inst.ContinueAction = null;
        
        nextAction?.Invoke();
    }
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeEvents()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the gameplay events.");

        gameplayQueues = new();
        isQueueRunning = false;

        if (FaceoffEvents == null) { FaceoffEvents = gameObject.AddComponent<FaceoffEvents>(); }
        if (OffenseEvents == null) { OffenseEvents = gameObject.AddComponent<OffenseEvents>(); }
        if (DefenseEvents == null) { DefenseEvents = gameObject.AddComponent<DefenseEvents>(); }
        if (PenaltyEvents == null) { PenaltyEvents = gameObject.AddComponent<PenaltyEvents>(); }
        if (GoalEvents == null) { GoalEvents = gameObject.AddComponent<GoalEvents>(); }
        if (PullGoalieEvents == null) { PullGoalieEvents = gameObject.AddComponent<PullGoalieEvents>(); }
        if (GameFlowEvents == null) { GameFlowEvents = gameObject.AddComponent<GameFlowEvents>(); }
    }

    public void Enqueue(Func<IEnumerator> actionFactory, bool isFirst = false)
    {
        if (actionFactory == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding gameplay events to the queue.");

        if (gameplayQueues == null)
        {
            InitializeEvents();
        }

        if (isFirst)
        {
            gameplayQueues.Insert(0, actionFactory);
        }

        else
        {
            gameplayQueues.Add(actionFactory);
        }
    }

    public void RunEvents()
    {
        if (gameplayQueues.Count < 1) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Running gameplay events from the queue.");

        EventsController.Inst.ToggleOverlay(true);

        StartCoroutine(RunningEvents());
    }

    public void StopAllEvents()
    {
        if (!isQueueRunning) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Stopping all gameplay events from running.");

        StopAllCoroutines();

        gameplayQueues.Clear();

        EventsController.Inst.ContinueAction = null;
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
