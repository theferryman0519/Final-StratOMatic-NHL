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
using SoM.Core;
using SoM.Models;
using SoM.Teams;

namespace SoM.Controllers {
public class TeamsController : Singleton<TeamsController> {

#region -------------------- Serialized Variables --------------------
    [Header("Creation Elements")]
    [SerializeField] private TeamCreation _teamCreation;
#endregion
#region -------------------- Public Variables --------------------
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

        AllNhlTeams.Clear();
        AllPwhlTeams.Clear();
        AllNhlFranchiseTeams.Clear();
        AllPwhlFranchiseTeams.Clear();

        SetAllTeams();
    }

    public Team GetTeamFromCode(string code, ConstantController.LeagueType league)
    {
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
#endregion
#region -------------------- Private Methods --------------------
    private async void SetAllTeams()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting all teams.");

        await FirebaseController.Inst.GetAllTeams(async allTeamsData =>
        {
            foreach (TeamDatabase teamData in allTeamsData)
            {
                Team team = await _teamCreation.CreateTeam(teamData);

                switch (team.Info.League)
                {
                    case "NHL": AllNhlTeams.Add(team); break;
                    case "PWHL": AllPwhlTeams.Add(team); break;
                    case "NHL-Franchise": AllNhlFranchiseTeams.Add(team); break;
                    case "PWHL-Franchise": AllPwhlFranchiseTeams.Add(team); break;
                }
            }

            CoreController.Inst.LoadingStepCompleted();
        });
    }
#endregion
}}
