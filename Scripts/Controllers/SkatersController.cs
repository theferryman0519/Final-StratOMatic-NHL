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
using SoM.Skaters;

namespace SoM.Controllers {
public class SkatersController : Singleton<SkatersController> {

#region -------------------- Serialized Variables --------------------
    [Header("Creation Elements")]
    [SerializeField] private SkaterCreationZone _skaterCreation;
    [SerializeField] private GoalieCreationZone _goalieCreation;
#endregion
#region -------------------- Public Variables --------------------
    public List<SkaterData> FullSkaters = new();
    public List<GoalieData> FullGoalies = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public async void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        FullSkaters.Clear();
        FullGoalies.Clear();

        await _skaterCreation.CreateSkaters();
        await _goalieCreation.CreateGoalies();

        CoreController.Inst.LoadingStepCompleted();
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
