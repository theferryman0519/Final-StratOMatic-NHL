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
public class UiSeasonReady : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _startButton;
	[SerializeField] private SoM_Button _returnButton;

    [Header("Text Elements")]
    [SerializeField] private TMP_Text _homeTeamText;
    [SerializeField] private TMP_Text _homeRecordText;
    [SerializeField] private TMP_Text _homeLinesText;
    [SerializeField] private TMP_Text _awayTeamText;
    [SerializeField] private TMP_Text _awayRecordText;
    [SerializeField] private TMP_Text _awayLinesText;

    [Header("Icon Elements")]
    [SerializeField] private Image _homeIcon;
    [SerializeField] private Image _awayIcon;
#endregion
#region -------------------- Public Variables --------------------
    
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
    
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
		_startButton.SetListener(GoToLoading);
		_returnButton.SetListener(GoToEditLines);

        SetGameData();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToLoading()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to season game loading screen.");

        GoToNewScene(CoreController.Inst.Scene_Season09);
    }

    private void GoToEditLines()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to season edit lines screen.");

        GoToNewScene(CoreController.Inst.Scene_Season08);
    }

    private void SetGameData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game data.");

        GameTeam homeTeam = GameplayController.Inst.GameData.HomeTeam;
        GameTeam awayTeam = GameplayController.Inst.GameData.AwayTeam;

        _homeTeamText.text = homeTeam.Team.Code;
        _awayTeamText.text = awayTeam.Team.Code;
        
        string homeLeague = homeTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";
        string awayLeague = awayTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";

        string homeString = $"{homeLeague}_{homeTeam.Team.Code}_ON";
        string awayString = $"{awayLeague}_{awayTeam.Team.Code}_ON";

        _homeIcon.sprite = ConstantController.Inst.IconSprites[homeString];
        _awayIcon.sprite = ConstantController.Inst.IconSprites[awayString];

        _homeLinesText.text = $"C: {homeTeam.SkaterLineup["C1"].Info.LastName}" + "\n" +
            $"LW: {homeTeam.SkaterLineup["LW1"].Info.LastName}" + "\n" +
            $"RW: {homeTeam.SkaterLineup["RW1"].Info.LastName}" + "\n\n" +
            $"LD: {homeTeam.SkaterLineup["LD1"].Info.LastName}" + "\n" +
            $"RD: {homeTeam.SkaterLineup["RD1"].Info.LastName}" + "\n\n" +
            $"G: {homeTeam.GoalieLineup["G"].Info.LastName}";
        
        _awayLinesText.text = $"C: {awayTeam.SkaterLineup["C1"].Info.LastName}" + "\n" +
            $"LW: {awayTeam.SkaterLineup["LW1"].Info.LastName}" + "\n" +
            $"RW: {awayTeam.SkaterLineup["RW1"].Info.LastName}" + "\n\n" +
            $"LD: {awayTeam.SkaterLineup["LD1"].Info.LastName}" + "\n" +
            $"RD: {awayTeam.SkaterLineup["RD1"].Info.LastName}" + "\n\n" +
            $"G: {awayTeam.GoalieLineup["G"].Info.LastName}";
        
        _homeRecordText.text = $"{UsersController.Inst.UserData.SeasonStats.CurrentWins} - {UsersController.Inst.UserData.SeasonStats.CurrentLosses} - " + "\n" +
            $"{UsersController.Inst.UserData.SeasonStats.CurrentTies} - {UsersController.Inst.UserData.SeasonStats.CurrentOTLs}";
        
        TeamSeason opponentSeason = SeasonsController.Inst.GetTeamSeason(awayTeam);

        _awayRecordText.text = $"{opponentSeason.Wins} - {opponentSeason.Losses} - {opponentSeason.Ties} - {opponentSeason.OTLs}";
    }
#endregion
}}
