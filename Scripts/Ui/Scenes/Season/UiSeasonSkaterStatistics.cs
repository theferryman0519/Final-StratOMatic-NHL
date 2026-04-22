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
public class UiSeasonSkaterStatistics : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _seasonNavigationDropdown;
    [SerializeField] private SoM_Dropdown _teamDropdown;

    [Header("Table Elements")]
    [SerializeField] private Transform _container;
    [SerializeField] private SeasonTableRow _tableRowPrefab;
    
    [Header("Table Header Elements")]
    [SerializeField] private Button _columnGamesButton;
    [SerializeField] private Button _columnGoalsButton;
    [SerializeField] private Button _columnAssistsButton;
    [SerializeField] private Button _columnPointsButton;
    [SerializeField] private Button _columnPimButton;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private int teamOption = 0;
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
        _columnGamesButton.onClick.RemoveAllListeners();
        _columnGoalsButton.onClick.RemoveAllListeners();
        _columnAssistsButton.onClick.RemoveAllListeners();
        _columnPointsButton.onClick.RemoveAllListeners();
        _columnPimButton.onClick.RemoveAllListeners();

        _columnGamesButton.onClick.AddListener(() => { SortTable(0); });
        _columnGoalsButton.onClick.AddListener(() => { SortTable(1); });
        _columnAssistsButton.onClick.AddListener(() => { SortTable(2); });
        _columnPointsButton.onClick.AddListener(() => { SortTable(3); });
        _columnPimButton.onClick.AddListener(() => { SortTable(4); });

        _seasonNavigationDropdown.SetListener(ChangeNavigationOption);
        _seasonNavigationDropdown.Dropdown.value = 1;

        _teamDropdown.SetListener(ChangeTeamOption);

        List<Team> leagueTeams = new();

        if (SeasonsController.Inst.SeasonData.League.Contains("NHL")) { leagueTeams = new(TeamsController.Inst.AllNhlTeams); }
        else { leagueTeams = new(TeamsController.Inst.AllPwhlTeams); }

        leagueTeams = leagueTeams.OrderBy(t => t.Info.CityName).ToList();

        for (int i = 0; i < leagueTeams.Count; i++)
        {
            int index = i;

            if (leagueTeams[index].Info.Code == SeasonsController.Inst.SeasonData.Team.Team.Code) { teamOption = index; }
        }

        ChangeTeamOption(teamOption);
        _teamDropdown.Dropdown.value = teamOption;

		ChangeNavigationOption(3);
        SortTable(3);

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToNextGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the next season game screen.");

        GoToNewScene(CoreController.Inst.Scene_Season02);
    }

    private void GoToSeasonStandings()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season standings screen.");

        GoToNewScene(CoreController.Inst.Scene_Season03);
    }

    private void GoToTeamStatistics()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season team statistics screen.");

        GoToNewScene(CoreController.Inst.Scene_Season04);
    }

    private void GoToGoalieStatistics()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season goalie statistics screen.");

        GoToNewScene(CoreController.Inst.Scene_Season06);
    }

    private void ChangeNavigationOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the season navigation view.");

        switch (option)
		{
			case 0:
				GoToNextGame();
				break;
			case 1:
				GoToSeasonStandings();
				break;
			case 2:
				GoToTeamStatistics();
				break;
            case 4:
				GoToGoalieStatistics();
				break;
			case 3:
			default:
                // Navigation is already Skater Statistics
				break;
		}
    }

    private void ChangeTeamOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the team view.");

        teamOption = option;
        SortTable(3);
    }

    private void ClearContainer()
    {
        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }
    }

    private void SortTable(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Sorting the table for the skater statistics.");

        ClearContainer();

        string league = SeasonsController.Inst.SeasonData.League;
        bool isNhl = league.Contains("NHL");

        List<Skater> seasonSkaters = new();
        List<Skater> sortedSkaters = new();

        if (isNhl)
        {
            seasonSkaters = new(SkatersController.Inst.NhlSkaters[SeasonsController.Inst.SeasonData.Team.Team.Code]);
            sortedSkaters = SortSkaterListBy(option, seasonSkaters);

            InstantiateRows(sortedTeams);
        }

        else // PWHL
        {
            seasonSkaters = new(SkatersController.Inst.PwhlSkaters[SeasonsController.Inst.SeasonData.Team.Team.Code]);
            sortedSkaters = SortSkaterListBy(option, seasonSkaters);

            InstantiateRows(sortedTeams);
        }
    }

    private void InstantiateRows(List<Skater> skaters)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Instantiating the table rows.");

        bool altBackground = false;

        foreach (Skater skater in skaters)
        {
            altBackground = !altBackground;

            SeasonTableRow row = Instantiate(_tableRowPrefab, _container);

            row.SetColumnA(skater.Info.LastName);
            row.SetColumnB(skater.Season.GamesPlayed);
            row.SetColumnC(skater.Season.Goals);
            row.SetColumnD(skater.Season.Assists);
            row.SetColumnE(skater.Season.Points);
            row.SetColumnF(skater.Season.PenaltyMinutes);

            row.Setbackground(altBackground);
        }
    }

    private List<Team> SortSkaterListBy(int option, List<Skater> skaters)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Sorting the list of skaters.");

        switch (option)
        {
            case 0: // Games Played
                return skaters.OrderByDescending(s => s.Season.GamesPlayed).ToList();
            case 1: // Goals
                return skaters.OrderByDescending(s => s.Season.Goals).ToList();
            case 2: // Assists
                return skaters.OrderByDescending(s => s.Season.Assists).ToList();
            case 4: // Penalty Minutes
                return skaters.OrderByDescending(s => s.Season.PenaltyMinutes).ToList();
            case 3: // Points
            default:
                return skaters.OrderByDescending(s => s.Season.Points).ToList();
        }
    }
#endregion
}}
