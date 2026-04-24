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

namespace SoM.Controllers {
public class PlayoffsController : Singleton<PlayoffsController> {

#region -------------------- Serialized Variables --------------------
    [Header("Creation Elements")]
    [SerializeField] private PlayoffCreation _playoffCreation;
#endregion
#region -------------------- Public Variables --------------------
    public Playoff PlayoffData;

    public int CurrentNight;
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
        
        LoadPlayoffData();
    }

    public async Task CreateNewPlayoff(string team, string league, Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating a new playoff.");

        PlayoffData = null;

        PLayoffDatabase newPlayoffDatabase = new PLayoffDatabase
        {
            Id = Guid.NewGuid().ToString(),
            League = league,
            Team = team,
            Round = 1,
            GameNumber = 1,
            RoundData = new(),
            SkaterLineup = new(),
            GoalieLineup = new(),
        };

        PlayoffData = await _playoffCreation.CreatePlayoff(newPlayoffDatabase);

        CurrentNight = 1;

        continueAction?.Invoke();
    }

    public void LoadCurrentPlayoff(Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the current playoff.");

        // TODO

        continueAction?.Invoke();
    }

    public TeamPlayoff GetTeamPlayoff(GameTeam team)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a team's playoff record.");

        ConstantController.LeagueType league = ConstantController.LeagueType.None;

        if (team.Team.League.Contains("NHL")) { league = ConstantController.LeagueType.NHL; }
        else { league = ConstantController.LeagueType.PWHL; }

        Team mainTeam = TeamsController.Inst.GetTeamFromCode(team.Team.Code, league);

        if (mainTeam.Playoff != null)
        {
            return mainTeam.Playoff;
        }

        return null;
    }
#endregion
#region -------------------- Private Methods --------------------
    private async void LoadPlayoffData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the playoff data.");

        await FirebaseController.Inst.GetPlayoff(UsersController.Inst.UserData.Id, async playoffData =>
        {
            PlayoffData = null;
        
            if (playoffData != null)
            {
                PlayoffData = new Playoff
                {
                    Id = playoffData.Id,
                    League = playoffData.League,
                    CurrentRound = playoffData.Round,
                    Rounds = new(),
                };

                CurrentNight = playoffData.GameNumber;

                PlayoffData.Rounds = await SaveController.Inst.LoadPlayoffRoundData(playoffData);
        
                ConstantController.LeagueType leagueType = ConstantController.LeagueType.None;
        
                if (PlayoffData.League == "NHL") { leagueType = ConstantController.LeagueType.NHL; }
                else { leagueType = ConstantController.LeagueType.PWHL; }
        
                Team userTeam = TeamsController.Inst.GetTeamFromCode(playoffData.Team, leagueType);
        
                GameTeam userGameTeam = new GameTeam
                {
                    SkaterLineup = new(),
                    GoalieLineup = new(),
                    CurrentLine = 1,
                    CurrentPair = 1,
                    CurrentStrategy = 3,
                    NextLine = 1,
                    NextPair = 1,
                    NextStrategy = 3,
                    IsGoaliePulled = false,
                    Team = userTeam.Info,
                    Stats = userTeam.Game,
                };
        
                PlayoffData.Team = userGameTeam;
                PlayoffData.Team.SkaterLineup = await SetSkaterLineup(playoffData.SkaterLineup);
                PlayoffData.Team.GoalieLineup = await SetGoalieLineup(playoffData.GoalieLineup);
            }
        
            CoreController.Inst.LoadingStepCompleted();
        });
    }

    private async Task<Dictionary<string, Skater>> SetSkaterLineup(List<string> skaterIds)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater lineup.");

        Dictionary<string, Skater> skaterLineup = new();

        if (skaterIds.Count < 18)
        {
            return skaterLineup;
        }

        List<string> posStrings = new() { "C1", "LW1", "RW1", "C2", "LW2", "RW2", "C3", "LW3", "RW3", "C4", "LW4", "RW4", "LD1", "RD1", "LD2", "RD2", "LD3", "RD3" };

        for (int i = 0; i < posStrings.Count; i++)
        {
            Skater nextSkater = GetSkaterById(skaterIds[i]);

            if (nextSkater != null) { skaterLineup.Add(posStrings[i], nextSkater); }
        }

        return skaterLineup;
    }

    private async Task<Dictionary<string, Goalie>> SetGoalieLineup(List<string> goalieIds)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the goalie lineup.");

        Dictionary<string, Goalie> goalieLineup = new();

        if (goalieIds.Count < 1)
        {
            return goalieLineup;
        }

        Goalie startingGoalie = GetGoalieById(goalieIds[0]);

        if (startingGoalie != null) { goalieLineup.Add("G", startingGoalie); }

        return goalieLineup;
    }

    private Skater GetSkaterById(string id)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the skater by id.");

        Skater resultSkater = new Skater { };

        List<Skater> teamSkaters = new();

        if (PlayoffData.League == "NHL") { teamSkaters = SkatersController.Inst.NhlSkaters[PlayoffData.Team.Team.Code]; }
        else { teamSkaters = SkatersController.Inst.PwhlSkaters[PlayoffData.Team.Team.Code]; }

        foreach (Skater skater in teamSkaters)
        {
            if (id == skater.Id)
            {
                resultSkater = skater;
            }
        }

        return resultSkater;
    }

    private Goalie GetGoalieById(string id)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the goalie by id.");

        Goalie resultGoalie = new Goalie { };

        List<Goalie> teamGoalies = new();

        if (PlayoffData.League == "NHL") { teamGoalies = GoaliesController.Inst.NhlGoalies[PlayoffData.Team.Team.Code]; }
        else { teamGoalies = GoaliesController.Inst.PwhlGoalies[PlayoffData.Team.Team.Code]; }

        foreach (Goalie goalie in teamGoalies)
        {
            if (id == goalie.Id)
            {
                resultGoalie = goalie;
            }
        }

        return resultGoalie;
    }
#endregion
}}
