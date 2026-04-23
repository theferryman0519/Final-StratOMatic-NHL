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
public class UiSeasonStandings : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _seasonNavigationDropdown;
    [SerializeField] private SoM_Dropdown _divisionDropdown;

    [Header("Table Elements")]
    [SerializeField] private Transform _container;
    [SerializeField] private SeasonTableRow _tableRowPrefab;
    
    [Header("Table Header Elements")]
    [SerializeField] private Button _columnWinsButton;
    [SerializeField] private Button _columnLossesButton;
    [SerializeField] private Button _columnTiesButton;
    [SerializeField] private Button _columnOTLsButton;
    [SerializeField] private Button _columnPointsButton;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private Dictionary<string, List<string>> divisionTeams = new();

    private int divisionOption = 0;
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
        _columnWinsButton.onClick.RemoveAllListeners();
        _columnLossesButton.onClick.RemoveAllListeners();
        _columnTiesButton.onClick.RemoveAllListeners();
        _columnOTLsButton.onClick.RemoveAllListeners();
        _columnPointsButton.onClick.RemoveAllListeners();

        _columnWinsButton.onClick.AddListener(() => { SortTable(0); });
        _columnLossesButton.onClick.AddListener(() => { SortTable(1); });
        _columnTiesButton.onClick.AddListener(() => { SortTable(2); });
        _columnOTLsButton.onClick.AddListener(() => { SortTable(3); });
        _columnPointsButton.onClick.AddListener(() => { SortTable(4); });

        _seasonNavigationDropdown.SetListener(ChangeNavigationOption);
        _seasonNavigationDropdown.Dropdown.value = 1;

        _divisionDropdown.SetListener(ChangeDivisionOption);

        divisionTeams.Add("Atlantic", new(){ "BOS", "BUF", "DET", "FLA", "MTL", "OTT", "TBL", "TOR" });
        divisionTeams.Add("Metropolitan", new(){ "CAR", "CBJ", "NJD", "NYI", "NYR", "PHI", "PIT", "WSH" });
        divisionTeams.Add("Central", new(){ "CHI", "COL", "DAL", "MIN", "NSH", "STL", "UTA", "WPG" });
        divisionTeams.Add("Pacific", new(){ "ANA", "CGY", "EDM", "LAK", "SJS", "SEA", "VAN", "VGK" });

        if (SeasonsController.Inst.SeasonData.League.Contains("NHL"))
        {
            if (divisionTeams["Atlantic"].Contains(SeasonsController.Inst.SeasonData.Team.Team.Code)) { ChangeDivisionOption(3); _divisionDropdown.Dropdown.value = 3; }
            if (divisionTeams["Metropolitan"].Contains(SeasonsController.Inst.SeasonData.Team.Team.Code)) { ChangeDivisionOption(4); _divisionDropdown.Dropdown.value = 4; }
            if (divisionTeams["Central"].Contains(SeasonsController.Inst.SeasonData.Team.Team.Code)) { ChangeDivisionOption(5); _divisionDropdown.Dropdown.value = 5; }
            if (divisionTeams["Pacific"].Contains(SeasonsController.Inst.SeasonData.Team.Team.Code)) { ChangeDivisionOption(6); _divisionDropdown.Dropdown.value = 6; }
        }

		ChangeNavigationOption(1);
        SortTable(4);

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToNextGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the next season game screen.");

        GoToNewScene(CoreController.Inst.Scene_Season02);
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
			case 2:
				GoToTeamStatistics();
				break;
            case 3:
				GoToSkaterStatistics();
				break;
            case 4:
				GoToGoalieStatistics();
				break;
			case 1:
			default:
                // Navigation is already Season Standings
				break;
		}
    }

    private void ChangeDivisionOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the division view.");

        divisionOption = option;
        SortTable(4);

        // 0 = League

        // 1 = East
        // 2 = West

        // 3 = Atantic
        // 4 = Metropolitan
        // 5 = Central
        // 6 = Pacific
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
        CoreController.Inst.WriteLog(this.GetType().Name, $"Sorting the table for the season standings.");

        ClearContainer();

        string league = SeasonsController.Inst.SeasonData.League;
        bool isNhl = league.Contains("NHL");

        _divisionDropdown.gameObject.SetActive(isNhl);

        List<Team> seasonTeams = new();
        List<Team> sortedTeams = new();

        if (isNhl)
        {
            seasonTeams = TeamsController.Inst.AllNhlTeams;

            foreach (Team allTeam in seasonTeams)
            {
                switch (divisionOption)
                {
                    case 1:
                        if (divisionTeams["Atlantic"].Contains(allTeam.Info.Code)) { sortedTeams.Add(allTeam); }
                        if (divisionTeams["Metropolitan"].Contains(allTeam.Info.Code)) { sortedTeams.Add(allTeam); }
                        break;
                    case 2:
                        if (divisionTeams["Central"].Contains(allTeam.Info.Code)) { sortedTeams.Add(allTeam); }
                        if (divisionTeams["Pacific"].Contains(allTeam.Info.Code)) { sortedTeams.Add(allTeam); }
                        break;
                    case 3:
                        if (divisionTeams["Atlantic"].Contains(allTeam.Info.Code)) { sortedTeams.Add(allTeam); }
                        break;
                    case 4:
                        if (divisionTeams["Metropolitan"].Contains(allTeam.Info.Code)) { sortedTeams.Add(allTeam); }
                        break;
                    case 5:
                        if (divisionTeams["Central"].Contains(allTeam.Info.Code)) { sortedTeams.Add(allTeam); }
                        break;
                    case 6:
                        if (divisionTeams["Pacific"].Contains(allTeam.Info.Code)) { sortedTeams.Add(allTeam); }
                        break;
                    case 0:
                    default:
                        sortedTeams.Add(allTeam);
                        break;
                }
            }

            sortedTeams = SortTeamListBy(option, sortedTeams);

            InstantiateRows(sortedTeams);
        }

        else // PWHL
        {
            ChangeDivisionOption(0);

            seasonTeams = TeamsController.Inst.AllPwhlTeams;
            sortedTeams = SortTeamListBy(option, seasonTeams);

            InstantiateRows(sortedTeams);
        }
    }

    private void InstantiateRows(List<Team> teams)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Instantiating the table rows.");

        bool altBackground = false;

        foreach (Team team in teams)
        {
            altBackground = !altBackground;

            SeasonTableRow row = Instantiate(_tableRowPrefab, _container);

            row.SetColumnA(team.Info.Code);
            row.SetColumnB(team.Season.Wins.ToString("n0"));
            row.SetColumnC(team.Season.Losses.ToString("n0"));
            row.SetColumnD(team.Season.Ties.ToString("n0"));
            row.SetColumnE(team.Season.OTLs.ToString("n0"));
            row.SetColumnF(team.Season.Points.ToString("n0"));

            row.Setbackground(altBackground);
        }
    }

    private List<Team> SortTeamListBy(int option, List<Team> teams)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Sorting the list of teams.");

        switch (option)
        {
            case 0: // Wins
                return teams.OrderByDescending(t => t.Season.Wins).ToList();
            case 1: // Losses
                return teams.OrderByDescending(t => t.Season.Losses).ToList();
            case 2: // Ties
                return teams.OrderByDescending(t => t.Season.Ties).ToList();
            case 3: // OTLs
                return teams.OrderByDescending(t => t.Season.OTLs).ToList();
            case 4: // Points
            default:
                return teams.OrderByDescending(t => t.Season.Points).ThenByDescending(t => t.Season.Wins).ThenByDescending(t => t.Season.Ties).ToList();
        }
    }
#endregion
}}
