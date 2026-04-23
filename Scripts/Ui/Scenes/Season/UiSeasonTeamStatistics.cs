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
public class UiSeasonTeamStatistics : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _seasonNavigationDropdown;
    [SerializeField] private SoM_Dropdown _divisionDropdown;

    [Header("Table Elements")]
    [SerializeField] private Transform _container;
    [SerializeField] private SeasonTableRow _tableRowPrefab;
    
    [Header("Table Header Elements")]
    [SerializeField] private Button _columnPowerplaysButton;
    [SerializeField] private Button _columnFaceoffsButton;
    [SerializeField] private Button _columnGoalsForButton;
    [SerializeField] private Button _columnShootingButton;
    [SerializeField] private Button _columnHitsButton;
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
        _columnPowerplaysButton.onClick.RemoveAllListeners();
        _columnFaceoffsButton.onClick.RemoveAllListeners();
        _columnGoalsForButton.onClick.RemoveAllListeners();
        _columnShootingButton.onClick.RemoveAllListeners();
        _columnHitsButton.onClick.RemoveAllListeners();

        _columnPowerplaysButton.onClick.AddListener(() => { SortTable(0); });
        _columnFaceoffsButton.onClick.AddListener(() => { SortTable(1); });
        _columnGoalsForButton.onClick.AddListener(() => { SortTable(2); });
        _columnShootingButton.onClick.AddListener(() => { SortTable(3); });
        _columnHitsButton.onClick.AddListener(() => { SortTable(4); });

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

		ChangeNavigationOption(2);
        SortTable(2);

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
			case 1:
				GoToSeasonStandings();
				break;
            case 3:
				GoToSkaterStatistics();
				break;
            case 4:
				GoToGoalieStatistics();
				break;
			case 2:
			default:
                // Navigation is already Team Statistics
				break;
		}
    }

    private void ChangeDivisionOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the division view.");

        divisionOption = option;
        SortTable(2);

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

            float ppp = team.Season.Powerplays > 0 ? ((float)team.Season.PowerplayGoals / (float)team.Season.Powerplays) * 100f : 0f;

            int totalFo = team.Season.FaceoffsWon + team.Season.FaceoffsLost;
            float fop = totalFo > 0 ? ((float)team.Season.FaceoffsWon / (float)totalFo) * 100f : 0f;

            float sp = team.Season.Shots > 0 ? ((float)team.Season.Goals / (float)team.Season.Shots) * 100f : 0f;

            row.SetColumnA(team.Info.Code);
            row.SetColumnB($"{ppp.ToString("n2")}%");
            row.SetColumnC($"{fop.ToString("n2")}%");
            row.SetColumnD(team.Season.Goals.ToString("n0"));
            row.SetColumnE($"{sp.ToString("n2")}%");
            row.SetColumnF(team.Season.Hits.ToString("n0"));

            row.Setbackground(altBackground);
        }
    }

    private List<Team> SortTeamListBy(int option, List<Team> teams)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Sorting the list of teams.");

        switch (option)
        {
            case 0: // Powerplay Goals
                return teams.OrderByDescending(t => t.Season.PowerplayGoals).ToList();
            case 1: // Faceoffs Won
                return teams.OrderByDescending(t => t.Season.FaceoffsWon).ToList();
            case 3: // Shots
                return teams.OrderByDescending(t => t.Season.Shots).ToList();
            case 4: // Hits
                return teams.OrderByDescending(t => t.Season.Hits).ToList();
            case 2: // Goals
            default:
                return teams.OrderByDescending(t => t.Season.Goals).ToList();
        }
    }
#endregion
}}
