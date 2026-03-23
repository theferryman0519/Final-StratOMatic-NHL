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
using SoM.Core;
using SoM.Seasons;

namespace SoM.Controllers {
public class SeasonsController : Singleton<SeasonsController> {

#region -------------------- Serialized Variables --------------------
    [Header("Creation Elements")]
    [SerializeField] private SeasonCreation _seasonCreation;
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
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
