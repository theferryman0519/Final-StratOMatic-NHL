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

namespace SoM.Events {
public class FaceoffEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator PuckDrop()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PuckDrop to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = "",
            ActionText = "",
            PossPos = string.Empty,
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator FaceoffStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding FaceoffStart to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = "",
            ActionText = "",
            PossPos = string.Empty,
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator FaceoffResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding FaceoffResult to the queue.");

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
