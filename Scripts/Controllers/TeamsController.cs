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
using SoM.Teams;
using Random = Unity.Mathematics.Random;

namespace SoM.Controllers {
public class TeamsController : Singleton<TeamsController> {

#region -------------------- Serialized Variables --------------------
    [Header("Creation Elements")]
    [SerializeField] private TeamCreation _teamCreation;
#endregion
#region -------------------- Public Variables --------------------
    public List<Team> AllTeams = new();
    public List<Team> AllNhlTeams = new();
    public List<Team> AllPwhlTeams = new();
    public List<Team> AllNhlFranchiseTeams = new();
    public List<Team> AllPwhlFranchiseTeams = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        AllTeams.Clear();
        AllNhlTeams.Clear();
        AllPwhlTeams.Clear();
        AllNhlFranchiseTeams.Clear();
        AllPwhlFranchiseTeams.Clear();

        SetAllTeams();
    }

    public Team GetTeamFromCode(string code, ConstantController.LeagueType league)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the team from the team code.");

        List<Team> leagueList = new();

        switch (league)
        {
            case ConstantController.LeagueType.NHL: leagueList = AllNhlTeams; break;
            case ConstantController.LeagueType.NHLFranchise: leagueList = AllNhlFranchiseTeams; break;
            case ConstantController.LeagueType.PWHL: leagueList = AllPwhlTeams; break;
            case ConstantController.LeagueType.PWHLFranchise: leagueList = AllPwhlFranchiseTeams; break;
        }

        if (leagueList.Count < 1)
        {
            return null;
        }

        Team foundTeam = null;

        foreach (Team team in leagueList)
        {
            if (team.Info.Code == code)
            {
                foundTeam = team;
            }
        }

        return foundTeam;
    }

    public Dictionary<string, Skater> GetDefaultLineup(string code, ConstantController.LeagueType league)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the default lineup for a team.");

        Dictionary<string, Skater> defaultLineup = new();

        Team team = GetTeamFromCode(code, league);

        if (team == null) { return defaultLineup; }

        List<Skater> teamSkaters = new();
        List<Skater> teamForwards = new();
        List<Skater> teamDefense = new();

        List<string> forwardPositions = new() { "C1", "LW1", "RW1", "C2", "LW2", "RW2", "C3", "LW3", "RW3", "C4", "LW4", "RW4" };
        List<string> defensePositions = new() { "LD1", "RD1", "LD2", "RD2", "LD3", "RD3" };

        switch (league)
        {
            case ConstantController.LeagueType.NHL:
                teamSkaters = SkatersController.Inst.NhlSkaters[team.Info.Code];
                break;
            case ConstantController.LeagueType.NHLFranchise:
                teamSkaters = SkatersController.Inst.NhlFranchiseSkaters[team.Info.Code];
                break;
            case ConstantController.LeagueType.PWHL:
                teamSkaters = SkatersController.Inst.PwhlSkaters[team.Info.Code];
                break;
            case ConstantController.LeagueType.PWHLFranchise:
                teamSkaters = SkatersController.Inst.PwhlFranchiseSkaters[team.Info.Code];
                break;
        }
        
        teamForwards = teamSkaters.Where(s => s.Info.Position == "F")
            .OrderByDescending(s => s.Card.Offense + s.Card.Defense + s.Card.Breakaway)
            .ToList();
        
        teamDefense = teamSkaters.Where(s => s.Info.Position == "D")
            .OrderByDescending(s => s.Card.Offense + s.Card.Defense)
            .ToList();
        
        foreach (string pos in forwardPositions)
        {
            defaultLineup.Add(pos, teamForwards[0]);
            teamForwards.RemoveAt(0);
        }

        foreach (string pos in defensePositions)
        {
            defaultLineup.Add(pos, teamDefense[0]);
            teamDefense.RemoveAt(0);
        }

        return defaultLineup;
    }

    public Goalie GetDefaultStartingGoalie(string code, ConstantController.LeagueType league)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the default starting goalie for a team.");

        Team team = GetTeamFromCode(code, league);

        if (team == null) { return null; }

        List<Goalie> teamGoalies = new();

        switch (league)
        {
            case ConstantController.LeagueType.NHL:
                teamGoalies = GoaliesController.Inst.NhlGoalies[team.Info.Code];
                break;
            case ConstantController.LeagueType.NHLFranchise:
                teamGoalies = GoaliesController.Inst.NhlFranchiseGoalies[team.Info.Code];
                break;
            case ConstantController.LeagueType.PWHL:
                teamGoalies = GoaliesController.Inst.PwhlGoalies[team.Info.Code];
                break;
            case ConstantController.LeagueType.PWHLFranchise:
                teamGoalies = GoaliesController.Inst.PwhlFranchiseGoalies[team.Info.Code];
                break;
        }

        teamGoalies = teamGoalies.OrderByDescending(g => g.WinPercentage).ToList();

        return teamGoalies[0];
    }
#endregion
#region -------------------- Private Methods --------------------
    private async void SetAllTeams()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting all teams.");

        List<Team> allTeams = new();

        await FirebaseController.Inst.GetAllTeams(async allTeamsData =>
        {
            foreach (TeamDatabase teamData in allTeamsData)
            {
                Team team = await _teamCreation.CreateTeam(teamData);

                switch (team.Info.League)
                {
                    case "NHL":
                        AllNhlTeams.Add(team);
                        allTeams.Add(team);
                        break;
                    case "PWHL":
                        AllPwhlTeams.Add(team);
                        allTeams.Add(team);
                        break;
                    case "NHLFranchise":
                        AllNhlFranchiseTeams.Add(team);
                        allTeams.Add(team);
                        break;
                    case "PWHLFranchise":
                        AllPwhlFranchiseTeams.Add(team);
                        allTeams.Add(team);
                        break;
                }
            }

            AllTeams = allTeams.OrderBy(t => t.Info.Code).ToList();

            SkatersController.Inst.InitializeController();
			GoaliesController.Inst.InitializeController();
			SeasonsController.Inst.InitializeController();
			PlayoffsController.Inst.InitializeController();

            CoreController.Inst.LoadingStepCompleted();
        });
    }
#endregion
}}
