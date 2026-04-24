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
    [SerializeField] private SeasonSaveData _seasonSaveData;
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
        if (_seasonSaveData == null) { _seasonSaveData = gameObject.AddComponent<SeasonSaveData>(); }

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

    public SeasonDatabase SaveUserSeasonData()
    {
        return _seasonSaveData.SaveUserSeasonData();
    }

    public string SaveSkaterSeasonData(Skater skater)
    {
        return _seasonSaveData.SaveSkaterSeasonData(skater);
    }

    public string SaveGoalieSeasonData(Goalie goalie)
    {
        return _seasonSaveData.SaveGoalieSeasonData(goalie);
    }

    public string SaveTeamSeasonData(GameTeam gameTeam)
    {
        return _seasonSaveData.SaveTeamSeasonData(gameTeam);
    }

    public List<GameNight> LoadSeasonGameNightsData(SeasonDatabase seasonDatabase)
    {
        return _seasonSaveData.LoadSeasonGameNightsData(seasonDatabase);
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
