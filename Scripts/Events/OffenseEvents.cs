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
public class OffenseEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator ActionCard()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationStart to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator OutsideOptions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationStart to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator ShotStart(ConstantController.ShotType shotType)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationStart to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator ShotResult(ConstantController.ShotType shotType)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationStart to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator ReboundCheck()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationStart to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator PassingStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationStart to the queue.");

        // TODO

        yield return null;
    }

    public IEnumerator PassingResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationStart to the queue.");

        // TODO

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
