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
using SoM.Controllers;
using SoM.Models;

namespace SoM.Ui {
public class UiSeasonGoalieStatistics : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _seasonNavigationDropdown;
    [SerializeField] private SoM_Dropdown _teamDropdown;

    [Header("Table Elements")]
    [SerializeField] private Transform _container;
    [SerializeField] private SeasonTableRow _tableRowPrefab;
    
    [Header("Table Header Elements")]
    [SerializeField] private Button _columnGamesButton;
    [SerializeField] private Button _columnWinsButton;
    [SerializeField] private Button _columnLossesButton;
    [SerializeField] private Button _columnShutoutsButton;
    [SerializeField] private Button _columnGoalsAgainstButton;
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
        _columnWinsButton.onClick.RemoveAllListeners();
        _columnLossesButton.onClick.RemoveAllListeners();
        _columnShutoutsButton.onClick.RemoveAllListeners();
        _columnGoalsAgainstButton.onClick.RemoveAllListeners();

        _columnGamesButton.onClick.AddListener(() => { SortTable(0); });
        _columnWinsButton.onClick.AddListener(() => { SortTable(1); });
        _columnLossesButton.onClick.AddListener(() => { SortTable(2); });
        _columnShutoutsButton.onClick.AddListener(() => { SortTable(3); });
        _columnGoalsAgainstButton.onClick.AddListener(() => { SortTable(4); });

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

		ChangeNavigationOption(4);
        SortTable(1);

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

    private void GoToSkaterStatistics()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the season skater statistics screen.");

        GoToNewScene(CoreController.Inst.Scene_Season05);
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
            case 3:
				GoToSkaterStatistics();
				break;
			case 4:
			default:
                // Navigation is already Goalie Statistics
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
        CoreController.Inst.WriteLog(this.GetType().Name, $"Sorting the table for the goalie statistics.");

        ClearContainer();

        string league = SeasonsController.Inst.SeasonData.League;
        bool isNhl = league.Contains("NHL");

        List<Goalie> seasonGoalies = new();
        List<Goalie> sortedGoalies = new();

        if (isNhl)
        {
            seasonGoalies = new(GoaliesController.Inst.NhlGoalies[SeasonsController.Inst.SeasonData.Team.Team.Code]);
            sortedGoalies = SortGoalieListBy(option, seasonGoalies);

            InstantiateRows(sortedGoalies);
        }

        else // PWHL
        {
            seasonGoalies = new(GoaliesController.Inst.PwhlGoalies[SeasonsController.Inst.SeasonData.Team.Team.Code]);
            sortedGoalies = SortGoalieListBy(option, seasonGoalies);

            InstantiateRows(sortedGoalies);
        }
    }

    private void InstantiateRows(List<Goalie> goalies)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Instantiating the table rows.");

        bool altBackground = false;

        foreach (Goalie goalie in goalies)
        {
            altBackground = !altBackground;

            SeasonTableRow row = Instantiate(_tableRowPrefab, _container);

            row.SetColumnA(goalie.Info.LastName);
            row.SetColumnB(goalie.Season.GamesPlayed.ToString("n0"));
            row.SetColumnC(goalie.Season.Wins.ToString("n0"));
            row.SetColumnD(goalie.Season.Losses.ToString("n0"));
            row.SetColumnE(goalie.Season.Shutouts.ToString("n0"));
            row.SetColumnF(goalie.Season.GoalsAgainst.ToString("n0"));

            row.Setbackground(altBackground);
        }
    }

    private List<Goalie> SortGoalieListBy(int option, List<Goalie> goalies)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Sorting the list of goalies.");

        switch (option)
        {
            case 0: // Games Played
                return goalies.OrderByDescending(g => g.Season.GamesPlayed).ToList();
            case 2: // Losses
                return goalies.OrderBy(g => g.Season.Losses).ToList();
            case 3: // Shuouts
                return goalies.OrderByDescending(g => g.Season.Shutouts).ToList();
            case 4: // Goals Against
                return goalies.OrderBy(g => g.Season.GoalsAgainst).ToList();
            case 1: // Wins
            default:
                return goalies.OrderByDescending(g => g.Season.Wins).ToList();
        }
    }
#endregion
}}
