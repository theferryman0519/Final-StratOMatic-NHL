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

        yield return null;
    }

    public IEnumerator IntimidationResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationResult to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator DefendingStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingStart to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator DefendingResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResult to the queue.");

        // TODO

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
