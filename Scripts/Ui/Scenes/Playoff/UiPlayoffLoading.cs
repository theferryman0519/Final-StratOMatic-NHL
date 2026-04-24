// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// Game Dependencies
using SoM.Controllers;
using SoM.Models;

namespace SoM.Ui {
public class UiPlayoffLoading : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Icon Elements")]
    [SerializeField] private Image _homeIcon;
    [SerializeField] private Image _awayIcon;

    [Header("Loading Elements")]
	[SerializeField] private Slider _loadingBar;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private bool isLoading = false;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        InitializeUi();
    }
#endregion
#region -------------------- Coroutines --------------------
    private IEnumerator LoadingGame()
    {
        float duration = 2f;
        float elapsed = 0f;

        _loadingBar.value = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            
            _loadingBar.value = Mathf.Clamp01(elapsed / duration);
            
            yield return null;
        }

        _loadingBar.value = 1f;

        AudioController.Inst.ChangeMusicVolume(true);
        
        GoToNewScene(CoreController.Inst.Scene_Gameplay00);
    }
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
		SetGameData();

        base.InitializeUi();

        StartLoading();
    }
#endregion
#region -------------------- Private Methods --------------------
    private void SetGameData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game data.");

        GameTeam homeTeam = GameplayController.Inst.GameData.HomeTeam;
        GameTeam awayTeam = GameplayController.Inst.GameData.AwayTeam;

        string homeLeague = homeTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";
        string awayLeague = awayTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";

        string homeString = $"{homeLeague}_{homeTeam.Team.Code}_ON";
        string awayString = $"{awayLeague}_{awayTeam.Team.Code}_ON";

        _homeIcon.sprite = ConstantController.Inst.IconSprites[homeString];
        _awayIcon.sprite = ConstantController.Inst.IconSprites[awayString];
    }

    private void StartLoading()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Starting to load the season game.");

        StartCoroutine(LoadingGame());
    }
#endregion
}}
