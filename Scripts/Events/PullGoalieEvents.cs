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
public class PullGoalieEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator PullGoalieShotsList()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsList to the queue.");

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

    public IEnumerator PullGoalieShotsStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsStart to the queue.");

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

    public IEnumerator PullGoalieShotsAttemptStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsAttemptStart to the queue.");

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

    public IEnumerator PullGoalieShotsAttemptResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsAttemptResult to the queue.");

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

    public IEnumerator PullGoalieShotsResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsResult to the queue.");

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
