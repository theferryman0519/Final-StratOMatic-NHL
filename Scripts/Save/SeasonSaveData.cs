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

namespace SoM.Save {
public class SeasonSaveData : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public SeasonDatabase SavedSeason;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public SeasonDatabase SaveUserSeasonData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the season data.");

        SeasonDatabase seasonDatabase = new SeasonDatabase
        {
            Id = SeasonsController.Inst.SeasonData.Id,
            League = SeasonsController.Inst.SeasonData.League,
            Team = SeasonsController.Inst.SeasonData.Team.Team.Code,
            Version = SeasonsController.Inst.SeasonData.Version,
            GameNight = SeasonsController.Inst.SeasonGameNight + 1,
            SkaterLineup = new(),
            GoalieLineup = new(),
        };

        foreach (Skater skater in GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Values)
        {
            seasonDatabase.SkaterLineup.Add(skater.Id);
        }

        seasonDatabase.GoalieLineup.Add(GameplayController.Inst.GameData.HomeTeam.GoalieLineup["G"].Id);

        return seasonDatabase;
    }

    public string SaveSkaterSeasonData(Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the season skater data.");

        string skaterData = string.Empty;

        skaterData += UsersController.Inst.UserData.Id + "/";
        skaterData += skater.Season.GamesPlayed.ToString() + "/";
        skaterData += skater.Season.Goals.ToString() + "/";
        skaterData += skater.Season.Assists.ToString() + "/";
        skaterData += skater.Season.Points.ToString() + "/";
        skaterData += skater.Season.PlusMinus.ToString() + "/";
        skaterData += skater.Season.PenaltyMinutes.ToString() + "/";
        skaterData += skater.Season.PowerplayGoals.ToString() + "/";
        skaterData += skater.Season.PowerplayAssists.ToString() + "/";
        skaterData += skater.Season.PowerplayPoints.ToString() + "/";
        skaterData += skater.Season.ShorthandedGoals.ToString() + "/";
        skaterData += skater.Season.ShorthandedAssists.ToString() + "/";
        skaterData += skater.Season.ShorthandedPoints.ToString() + "/";
        skaterData += skater.Season.Shots.ToString() + "/";
        skaterData += skater.Season.Giveaways.ToString() + "/";
        skaterData += skater.Season.Takeaways.ToString() + "/";
        skaterData += skater.Season.FaceoffsWon.ToString() + "/";
        skaterData += skater.Season.FaceoffsLost.ToString();

        return skaterData;
    }

    public string SaveGoalieSeasonData(Goalie goalie)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the season goalie data.");

        string goalieData = string.Empty;

        goalieData += UsersController.Inst.UserData.Id + "/";
        goalieData += goalie.Season.GamesPlayed.ToString() + "/";
        goalieData += goalie.Season.Wins.ToString() + "/";
        goalieData += goalie.Season.Losses.ToString() + "/";
        goalieData += goalie.Season.Shutouts.ToString() + "/";
        goalieData += goalie.Season.GoalsAgainst.ToString() + "/";
        goalieData += goalie.Season.ShotsAgainst.ToString() + "/";
        goalieData += goalie.Season.Assists.ToString() + "/";
        goalieData += goalie.Season.PenaltyMinutes.ToString() + "/";
        goalieData += goalie.Season.Stamina.ToString();

        return goalieData;
    }

    public string SaveTeamSeasonData(GameTeam gameTeam)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the season team data.");

        string teamData = string.Empty;

        teamData += UsersController.Inst.UserData.Id + "/";
        teamData += gameTeam.Season.GamesPlayed.ToString() + "/";

        teamData += gameTeam.Season.Wins.ToString() + "/";
        teamData += gameTeam.Season.Losses.ToString() + "/";
        teamData += gameTeam.Season.Ties.ToString() + "/";
        teamData += gameTeam.Season.OTLs.ToString() + "/";
        teamData += gameTeam.Season.Points.ToString() + "/";
        teamData += gameTeam.Season.Goals.ToString() + "/";
        teamData += gameTeam.Season.Shots.ToString() + "/";
        teamData += gameTeam.Season.PowerplayGoals.ToString() + "/";
        teamData += gameTeam.Season.Powerplays.ToString() + "/";
        teamData += gameTeam.Season.ShorthandedGoals.ToString() + "/";
        teamData += gameTeam.Season.FaceoffsWon.ToString() + "/";
        teamData += gameTeam.Season.FaceoffsLost.ToString() + "/";
        teamData += gameTeam.Season.Hits.ToString() + "/";
        teamData += gameTeam.Season.BlockedShots.ToString() + "/";
        teamData += gameTeam.Season.Giveaways.ToString() + "/";
        teamData += gameTeam.Season.Takeaways.ToString();

        return teamData;
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
