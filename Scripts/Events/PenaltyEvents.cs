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
public class PenaltyEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator PenaltyCheck()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyCheck to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = "",
            ActionText = "",
            PossPos = "",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PenaltyShotsList()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsList to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = "",
            ActionText = "",
            PossPos = "",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PenaltyShotsStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsStart to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = "",
            ActionText = "",
            PossPos = "",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PenaltyShotsAttemptStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsAttemptStart to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = "",
            ActionText = "",
            PossPos = "",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PenaltyShotsAttemptResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsAttemptResult to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = "",
            ActionText = "",
            PossPos = "",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PenaltyShotsResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsResult to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = "",
            ActionText = "",
            PossPos = "",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
