// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// Game Dependencies
using SoM.Core;
using SoM.Models;
using SoM.Seasons;

namespace SoM.Controllers {
public class SeasonsController : Singleton<SeasonsController> {

#region -------------------- Serialized Variables --------------------
    [Header("Creation Elements")]
    [SerializeField] private SeasonCreation _seasonCreation;
#endregion
#region -------------------- Public Variables --------------------
    public Season SeasonData;

    public GameOptions SeasonOptions;

    public int SeasonGameNight = 0;
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

        LoadSeasonData();
    }

    public async Task CreateNewSeason(string team, string league, Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating a new season.");

        SeasonData = null;

        SeasonDatabase newSeasonDatabase = new SeasonDatabase
        {
            Id = Guid.NewGuid().ToString(),
            League = league,
            Team = team,
            Version = Random.Range(1,4),
            GameNight = SeasonGameNight,
            SkaterLineup = new(),
            GoalieLineup = new(),
        };

        SeasonData = await _seasonCreation.CreateSeason(newSeasonDatabase);

        continueAction?.Invoke();
    }

    public void LoadCurrentSeason(Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the current season.");

        LoadSeasonData();

        continueAction?.Invoke();
    }

    public TeamSeason GetTeamSeason(GameTeam team)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a team's season record.");

        ConstantController.LeagueType league = ConstantController.LeagueType.None;

        if (team.Team.League.Contains("NHL")) { league = ConstantController.LeagueType.NHL; }
        else { league = ConstantController.LeagueType.PWHL; }

        Team mainTeam = TeamsController.Inst.GetTeamFromCode(team.Team.Code, league);

        if (mainTeam.Season != null)
        {
            return mainTeam.Season;
        }

        return null;
    }
#endregion
#region -------------------- Private Methods --------------------
    private async void LoadSeasonData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the season data.");

        await FirebaseController.Inst.GetSeason(UsersController.Inst.UserData.Id, async seasonData =>
        {
            SeasonData = null;
        
            if (seasonData != null)
            {
                SeasonData = new Season
                {
                    Id = seasonData.Id,
                    League = seasonData.League,
                    Version = seasonData.Version,
                    GameNights = new(),
                };

                SeasonData.GameNights = await SaveController.Inst.LoadSeasonGameNightsData(seasonData);
        
                ConstantController.LeagueType leagueType = ConstantController.LeagueType.None;
        
                if (SeasonData.League == "NHL") { leagueType = ConstantController.LeagueType.NHL; }
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
        
                SeasonData.Team = userGameTeam;
                SeasonData.Team.SkaterLineup = await SetSkaterLineup(seasonData.SkaterLineup);
                SeasonData.Team.GoalieLineup = await SetGoalieLineup(seasonData.GoalieLineup);
        
                SeasonGameNight = seasonData.GameNight;
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

        if (SeasonData.League == "NHL") { teamSkaters = SkatersController.Inst.NhlSkaters[SeasonData.Team.Team.Code]; }
        else { teamSkaters = SkatersController.Inst.PwhlSkaters[SeasonData.Team.Team.Code]; }

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

        if (SeasonData.League == "NHL") { teamGoalies = GoaliesController.Inst.NhlGoalies[SeasonData.Team.Team.Code]; }
        else { teamGoalies = GoaliesController.Inst.PwhlGoalies[SeasonData.Team.Team.Code]; }

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
