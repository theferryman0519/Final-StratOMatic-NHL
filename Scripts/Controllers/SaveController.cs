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
    [SerializeField] private PlayoffSaveData _playoffSaveData;
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
#region ---------- Initialization ----------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        if (_gameSaveData == null) { _gameSaveData = gameObject.AddComponent<GameSaveData>(); }
        if (_seasonSaveData == null) { _seasonSaveData = gameObject.AddComponent<SeasonSaveData>(); }
        if (_playoffSaveData == null) { _playoffSaveData = gameObject.AddComponent<PlayoffSaveData>(); }

        CoreController.Inst.LoadingStepCompleted();
    }
#endregion
#region ---------- Game Data ----------
    public GameDatabase GetCurrentGameSaveData()
    {
        return _gameSaveData.GetCurrentGameSaveData();
    }

    public Game LoadGameFromSaveData(GameDatabase loadGame)
    {
        return _gameSaveData.LoadGameFromSaveData(loadGame);
    }
#endregion
#region ---------- Season Data ----------
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

    public async Task<List<GameNight>> LoadSeasonGameNightsData(SeasonDatabase seasonDatabase)
    {
        return _seasonSaveData.LoadSeasonGameNightsData(seasonDatabase);
    }
#endregion
#region ---------- Playoff Data ----------
    public PlayoffDatabase SaveUserPlayoffData()
    {
        return _playoffSaveData.SaveUserPlayoffData();
    }

    public string SaveSkaterPlayoffData(Skater skater)
    {
        return _playoffSaveData.SaveSkaterPlayoffData(skater);
    }

    public string SaveGoaliePlayoffData(Goalie goalie)
    {
        return _playoffSaveData.SaveGoaliePlayoffData(goalie);
    }

    public string SaveTeamPlayoffData(GameTeam gameTeam)
    {
        return _playoffSaveData.SaveTeamPlayoffData(gameTeam);
    }

    public async Task<List<PlayoffRound>> LoadPlayoffRoundData(PlayoffDatabase playoffData)
    {
        return _playoffSaveData.LoadPlayoffRoundData(playoffData);
    }
#endregion
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
