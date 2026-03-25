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

namespace SoM.Teams {
public class TeamCreation : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private string teamId = string.Empty;

    private SemaphoreSlim createTeamLock = new(1, 1);
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public async Task<Team> CreateTeam(TeamDatabase teamDatabase)
    {
        await createTeamLock.WaitAsync();
        try
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team.");

            teamId = teamDatabase.Id;

            Team newTeam = new Team
            {
                Id = teamDatabase.Id,
            };

            newTeam.Info = await CreateInfo(teamDatabase.InfoString);
            newTeam.Game = await CreateGame();
            newTeam.Season = await CreateSeason(teamDatabase.SeasonString);
            newTeam.Playoff = await CreatePlayoff(teamDatabase.PlayoffString);

            CoreController.Inst.WriteLog(this.GetType().Name, $"Team data for {newTeam.Info.CityName} {newTeam.Info.NickName} has been created.");
            return newTeam;
        }
        finally
        {
            createTeamLock.Release();
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private async Task<TeamInfo> CreateInfo(string infoString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team info.");

        string[] infoArray = infoString.Split('/');
        if (infoArray.Length < 4) { return null; }

        TeamInfo newInfo = new TeamInfo
        {
            Id = teamId,
            Code = infoArray[0],
            CityName = infoArray[1],
            NickName = infoArray[2],
            League = infoArray[3],
        };

        return newInfo;
    }

    private async Task<TeamGame> CreateGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team game.");

        TeamGame newGame = new TeamGame
        {
            Id = teamId,
            Goals = 0,
            Shots = 0,
            PowerplayGoals = 0,
            Powerplays = 0,
            ShorthandedGoals = 0,
            FaceoffsWon = 0,
            FaceoffsLost = 0,
            Hits = 0,
            BlockedShots = 0,
            Giveaways = 0,
            Takeaways = 0,
        };

        return newGame;
    }

    private async Task<TeamSeason> CreateSeason(string seasonString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team season.");

        string[] seasonArray = seasonString.Split('/');
        if (seasonArray.Length < 17) { return null; }

        TeamSeason newSeason = new TeamSeason
        {
            Id = teamId,
            GamesPlayed = Int32.Parse(seasonArray[0]),
            Wins = Int32.Parse(seasonArray[1]),
            Losses = Int32.Parse(seasonArray[2]),
            Ties = Int32.Parse(seasonArray[3]),
            OTLs = Int32.Parse(seasonArray[4]),
            Points = Int32.Parse(seasonArray[5]),
            Goals = Int32.Parse(seasonArray[6]),
            Shots = Int32.Parse(seasonArray[7]),
            PowerplayGoals = Int32.Parse(seasonArray[8]),
            Powerplays = Int32.Parse(seasonArray[9]),
            ShorthandedGoals = Int32.Parse(seasonArray[10]),
            FaceoffsWon = Int32.Parse(seasonArray[11]),
            FaceoffsLost = Int32.Parse(seasonArray[12]),
            Hits = Int32.Parse(seasonArray[13]),
            BlockedShots = Int32.Parse(seasonArray[14]),
            Giveaways = Int32.Parse(seasonArray[15]),
            Takeaways = Int32.Parse(seasonArray[16]),
        };

        return newSeason;
    }

    private async Task<TeamPlayoff> CreatePlayoff(string playoffString)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the team playoff.");

        string[] playoffArray = playoffString.Split('/');
        if (playoffArray.Length < 14) { return null; }

        TeamPlayoff newPlayoff = new TeamPlayoff
        {
            Id = teamId,            
            GamesPlayed = Int32.Parse(playoffArray[0]),
            Wins = Int32.Parse(playoffArray[1]),
            Losses = Int32.Parse(playoffArray[2]),
            Goals = Int32.Parse(playoffArray[3]),
            Shots = Int32.Parse(playoffArray[4]),
            PowerplayGoals = Int32.Parse(playoffArray[5]),
            Powerplays = Int32.Parse(playoffArray[6]),
            ShorthandedGoals = Int32.Parse(playoffArray[7]),
            FaceoffsWon = Int32.Parse(playoffArray[8]),
            FaceoffsLost = Int32.Parse(playoffArray[9]),
            Hits = Int32.Parse(playoffArray[10]),
            BlockedShots = Int32.Parse(playoffArray[11]),
            Giveaways = Int32.Parse(playoffArray[12]),
            Takeaways = Int32.Parse(playoffArray[13]),
        };

        return newPlayoff;
    }
#endregion
}}
