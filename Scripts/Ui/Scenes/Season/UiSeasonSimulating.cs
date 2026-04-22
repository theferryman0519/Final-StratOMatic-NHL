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
using SoM.Models;

namespace SoM.Ui {
public class UiSeasonSimulating : UiSceneBase {

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

    private int maxAmount = 0;
    private int simmedAmount = 0;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        InitializeUi();
    }

    void Update()
    {
        if (isLoading)
        {
            if (simmedAmount < maxAmount) { _loadingBar.value = (float)simmedAmount / (float)maxAmount; }
            else { _loadingBar.value = 1f; }
        }
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
		SetGameData();

        base.InitializeUi();

        StartSimulating();
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

    private void StartSimulating()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Starting to simulate the rest of the night games.");

        int night = SeasonsController.Inst.SeasonGameNight;
        string userTeam = SeasonsController.Inst.SeasonData.Team.Team.Code;
        List<Game> nightGames = new(SeasonsController.Inst.SeasonData.GameNights.FirstOrDefault(g => g.Number == night).Games);

        maxAmount = nightGames.Count;
        simmedAmount += 1;

        isLoading = true;

        foreach (Game game in nightGames)
        {
            if (game.HomeTeam.Team.Code != userTeam && game.AwayTeam.Team.Code != userTeam)
            {
                // TODO
                // Get default skaters and goalies for each home and away team
                // Simulate game stats for each skater on home team
                // Simulate game stats for each skater on away team
                // Update game stats for goalie on home team based on away skater stats
                // Update game stats for goalie on away team based on home skater stats
                // Update team stats for home team
                // Update team stats for away team

                simmedAmount += 1;
            }
        }
    }
#endregion
}}
