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
public class GameFlowEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public IEnumerator StartOfGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding StartOfGame to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator StartOfPeriod()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding StartOfPeriod to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator Injury()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding Injury to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator EndOfPeriod()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding EndOfPeriod to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator OvertimeStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding OvertimeStart to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator EndOfGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding EndOfGame to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator CompleteGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding CompleteGame to the queue.");

        // TODO

        yield return null;
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
