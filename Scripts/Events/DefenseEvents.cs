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
public class DefenseEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public Skater IntimidatingSkater;
    public Skater DefendingSkater;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator IntimidationStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationStart to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator IntimidationResultSteal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationResultSteal to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator IntimidationResultStealShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationResultStealShot to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator IntimidationResultShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationResultShot to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator DefendingStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingStart to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator DefendingResultSteal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultSteal to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator DefendingResultStealShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultStealShot to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator DefendingResultStealShotIntimidation()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultStealShotIntimidation to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator DefendingResultShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultShot to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator DefendingResultShotIntimidation()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultShotIntimidation to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator DefendingResultPenalty()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultPenalty to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public void DetermineIntimidationOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the intimidation outcome.");

        // TODO
    }

    public void DetermineDefendingOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the defending outcome.");

        // TODO
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
