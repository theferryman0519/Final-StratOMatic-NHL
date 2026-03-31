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
public class GoalEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator GoalieRatingStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingStart to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator GoalieRatingResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingResult to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator GoalCheck()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalCheck to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator Goal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding Goal to the queue.");

        // TODO

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
