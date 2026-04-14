// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Game Dependencies
using SoM.Core;
using SoM.Models;
using SoM.Save;

namespace SoM.Controllers {
public class SaveController : Singleton<SaveController> {

#region -------------------- Serialized Variables --------------------
    [Header("Save Data Elements")]
    [SerializeField] private GameSaveData _gameSaveData;
#endregion
#region -------------------- Public Variables --------------------
    public GameDatabase SavedGame;
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

        if (_gameSaveData == null) { _gameSaveData = gameObject.AddComponent<GameSaveData>(); }

        CoreController.Inst.LoadingStepCompleted();
    }

    public GameDatabase GetCurrentGameSaveData()
    {
        return _gameSaveData.GetCurrentGameSaveData();
    }

    public Game LoadGameFromSaveData(GameDatabase loadGame)
    {
        return _gameSaveData.LoadGameFromSaveData(loadGame);
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
