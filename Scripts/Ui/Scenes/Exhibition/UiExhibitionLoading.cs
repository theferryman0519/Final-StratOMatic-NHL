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
public class UiExhibitionLoading : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Icon Elements")]
    [SerializeField] private Image _homeIcon;
    [SerializeField] private Image _awayIcon;

    [Header("Loading Elements")]
	[SerializeField] private Slider _loadingBar;
#endregion
#region -------------------- Public Variables --------------------
    private bool isLoading = false;
#endregion
#region -------------------- Private Variables --------------------
    
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
        while (isLoading && _loadingBar.value < 1f)
        {
            float randomPause = Random.Range(0f, 1f);
            float randomLoad = Random.Range(0.1f, 0.3f);

            yield return new WaitForSeconds(randomPause);

            _loadingBar.value += randomLoad;
        }

        if (isLoading)
        {
            isLoading = false;
            _loadingBar.value = 1f;

            GoToNewScene(CoreController.Inst.Scene_Gameplay00);
        }
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

        string homeString = $"{homeTeam.Team.League}_{homeTeam.Team.Code}_ON";
        string awayString = $"{awayTeam.Team.League}_{awayTeam.Team.Code}_ON";

        _homeIcon.sprite = ConstantController.Inst.IconSprites[homeString];
        _awayIcon.sprite = ConstantController.Inst.IconSprites[awayString];
    }

    private void StartLoading()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Starting to load the exhibition game.");

        isLoading = true;
        StartCoroutine(LoadingGame());
    }
#endregion
}}
