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
using SoM.Seasons;

namespace SoM.Controllers {
public class SeasonsController : Singleton<SeasonsController> {

#region -------------------- Serialized Variables --------------------
    [Header("Creation Elements")]
    [SerializeField] private SeasonCreation _seasonCreation;
#endregion
#region -------------------- Public Variables --------------------
    public Season SeasonData;

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

    public void CreateNewSeason()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating a new season.");

        // TODO
    }

    public void LoadCurrentSeason()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the current season.");

        // TODO
    }
#endregion
#region -------------------- Private Methods --------------------
    private async void LoadSeasonData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the season data.");

        await FirebaseController.Inst.GetSeason(UsersController.Inst.UserData.Id, seasonData =>
        {
            SeasonData = null;

            if (seasonData != null)
            {
                SeasonData = new Season
                {
                    Id = seasonData.Id,
                    League = seasonData.League,
                    Version = seasonData.Version,
                };

                ConstantController.LeagueType leagueType = ConstantController.LeagueType.None;

                if (SeasonData.League == "NHL") { leagueType == ConstantController.LeagueType.NHL; }
                else { leagueType == ConstantController.LeagueType.PWHL; }

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

                SeasonGameNight = seasonData.GameNight;
            }

            CoreController.Inst.LoadingStepCompleted();
        });
    }
#endregion
}}
