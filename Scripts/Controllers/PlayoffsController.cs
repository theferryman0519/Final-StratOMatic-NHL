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

                // TODO: Set Rounds
        
                ConstantController.LeagueType leagueType = ConstantController.LeagueType.None;
        
                if (PlayoffData.League == "NHL") { leagueType = ConstantController.LeagueType.NHL; }
                else { leagueType = ConstantController.LeagueType.PWHL; }
        
                Team userTeam = TeamsController.Inst.GetTeamFromCode(seasonData.Team, leagueType);
        
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
                PlayoffData.Team.SkaterLineup = await SetSkaterLineup(seasonData.SkaterLineup);
                PlayoffData.Team.GoalieLineup = await SetGoalieLineup(seasonData.GoalieLineup);
            }
        
            CoreController.Inst.LoadingStepCompleted();
        });
    }
#endregion
}}
