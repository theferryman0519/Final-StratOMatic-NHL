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
public class UiGameplayMainScoreboard : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Main Elements")]
    [SerializeField] private TMP_Text _periodText;
    [SerializeField] private TMP_Text _cardsDrawnText;
    [SerializeField] private TMP_Text _powerplayText;

    [Header("Home Elements")]
    [SerializeField] private Button _homeButton;
    [SerializeField] private Image _homeIcon;
    [SerializeField] private TMP_Text _homeScoreText;
    [SerializeField] private TMP_Text _homeShotsText;

    [Header("Away Elements")]
    [SerializeField] private Button _awayButton;
    [SerializeField] private Image _awayIcon;
    [SerializeField] private TMP_Text _awayScoreText;
    [SerializeField] private TMP_Text _awayShotsText;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private UiGameplayMain mainUi;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        _homeButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(() =>
        {
            AnimationController.Inst.ShrinkButton(_homeButton, () => { mainUi.ShowGameStatsPanel(); });
        });

        _awayButton.onClick.RemoveAllListeners();
        _awayButton.onClick.AddListener(() =>
        {
            AnimationController.Inst.ShrinkButton(_awayButton, () => { mainUi.ShowGameStatsPanel(); });
        });
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void UpdateScoreboard(UiGameplayMain ui)
    {
        if (ui == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the scoreboard.");

        mainUi = ui;

        UpdateIcons();
        UpdateScores();
        UpdateShots();
        UpdateTime();
        ShowPowerplay();
    }
#endregion
#region -------------------- Private Methods --------------------
    private void UpdateIcons()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the home and away icons.");

        GameTeam homeTeam = GameplayController.Inst.GameData.HomeTeam;
        GameTeam awayTeam = GameplayController.Inst.GameData.AwayTeam;

        string homeString = $"{homeTeam.Team.League}_{homeTeam.Team.Code}_ON";
        string awayString = $"{awayTeam.Team.League}_{awayTeam.Team.Code}_ON";

        _homeIcon.sprite = ConstantController.Inst.IconSprites[homeString];
        _awayIcon.sprite = ConstantController.Inst.IconSprites[awayString];
    }

    private void UpdateScores()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the home and away scores.");

        int homeGoals = GetTeamGoals(true);
        int awayGoals = GetTeamGoals(false);

        _homeScoreText.text = homeGoals.ToString("n0");
        _awayScoreText.text = awayGoals.ToString("n0");
    }

    private void UpdateShots()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the home and away shots on goal.");

        int homeShots = GetTeamShots(true);
        int awayShots = GetTeamShots(false);

        _homeShotsText.text = $"Shots: {homeShots.ToString("n0")}";
        _awayShotsText.text = $"Shots: {awayShots.ToString("n0")}";
    }

    private void UpdateTime()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the time and cards drawn.");

        int period = GameplayController.Inst.GameData.Period;
        int cardsDrawn = GameplayController.Inst.GameData.CardsDrawn;

        string periodString = string.Empty;
        string timeString = GetGameTime(cardsDrawn);

        if (period == 1 || period == 0) { periodString = "1st Period"; }
        else if (period == 2) { periodString = "2nd Period"; }
        else if (period == 3) { periodString = "3rd Period"; }
        else { periodString = "Overtime"; }

        _periodText.text = $"{periodString}: {timeString}";
        _cardsDrawnText.text = $"Cards Drawn: {cardsDrawn}";
    }

    private void ShowPowerplay()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the powerplay indicator.");

        string powerplayTeam = GameplayController.Inst.GameData.PowerplayTeam;

        if (powerplayTeam == "Home")
        {
            string homeCode = GameplayController.Inst.GameData.HomeTeam.Team.Code;

            _powerplayText.text = $"PP: {homeCode}";
            _powerplayText.gameObject.SetActive(true);
        }

        else if (powerplayTeam == "Away")
        {
            string awayCode = GameplayController.Inst.GameData.AwayTeam.Team.Code;

            _powerplayText.text = $"PP: {awayCode}";
            _powerplayText.gameObject.SetActive(true);
        }

        else
        {
            _powerplayText.text = "None";
            _powerplayText.gameObject.SetActive(false);
        }
    }

    private int GetTeamGoals(bool isHome)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the team total goal count.");

        GameTeam team = isHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        int totalGoals = 0;

        foreach (Skater skater in team.SkaterLineup.Values)
        {
            totalGoals += skater.Game.Goals;
        }

        return totalGoals;
    }

    private int GetTeamShots(bool isHome)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the team total shot count.");

        GameTeam team = isHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        int totalShots = 0;

        foreach (Skater skater in team.SkaterLineup.Values)
        {
            totalShots += skater.Game.Shots;
        }

        return totalShots;
    }

    private string GetGameTime(int cardsDrawn)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the game time based on cards drawn.");

        switch (cardsDrawn)
        {
            case 0: return "20:00";
            case 1: return "19:20";
            case 2: return "18:40";
            case 3: return "18:00";
            case 4: return "17:20";
            case 5: return "16:40";
            case 6: return "16:00";
            case 7: return "15:20";
            case 8: return "14:40";
            case 9: return "14:00";
            case 10: return "13:20";
            case 11: return "12:40";
            case 12: return "12:00";
            case 13: return "11:20";
            case 14: return "10:40";
            case 15: return "10:00";
            case 16: return "9:20";
            case 17: return "8:40";
            case 18: return "8:00";
            case 19: return "7:20";
            case 20: return "6:40";
            case 21: return "6:00";
            case 22: return "5:20";
            case 23: return "4:40";
            case 24: return "4:00";
            case 25: return "3:20";
            case 26: return "2:40";
            case 27: return "2:00";
            case 28: return "1:20";
            case 29: return "0:40";
            case 30:
            default: return "0:05";
        }
    }
#endregion
}}
