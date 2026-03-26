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
public class UsersController : Singleton<UsersController> {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public User UserData;
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

        CoreController.Inst.LoadingStepCompleted();
    }

    public async void SetUserData(string id, Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the user data.");

        await FirebaseController.Inst.GetUser(id, userData =>
        {
            SetUser(userData, continueAction);
        });
    }

    public void CreateUserData(string id, Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the user data.");

        UserDatabase newUserData = new UserDatabase
        {
            Id = id,
            InfoString = string.Empty,
            StatsString = string.Empty,
            SeasonStatsString = string.Empty,
        };

        SetUser(newUserData, continueAction);
    }

    public async void SaveUserData(Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Saving the user data.");

        UserDatabase userDatabase = new UserDatabase
        {
            Id = UserData.Id,
            InfoString = SaveUserInfo(),
            StatsString = SaveUserStats(),
            SeasonStatsString = SaveUserSeasonStats(),
        };

        await FirebaseController.Inst.PutUser(userDatabase, UserData.Id, continueAction);
    }
#endregion
#region -------------------- Private Methods --------------------
    private void SetUser(UserDatabase userData, Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the user.");

        User user = new User
        {
            Id = userData.Id,
            Info = SetUserInfo(userData.Id, userData.InfoString),
            Stats = SetUserStats(userData.Id, userData.StatsString),
            SeasonStats = SetUserSeasonStats(userData.Id, userData.SeasonStatsString),
        };

        UserData = user;

        continueAction?.Invoke();
    }

    private UserInfo SetUserInfo(string id, string info)
    {
        UserInfo userInfo = new UserInfo
        {
            Id = id,
            Name = string.IsNullOrEmpty(info) ? string.Empty : info.Split('/')[1].Trim(),
            Email = string.IsNullOrEmpty(info) ? string.Empty : info.Split('/')[2].Trim(),
            Password = string.IsNullOrEmpty(info) ? string.Empty : info.Split('/')[3].Trim(),
            Team = string.IsNullOrEmpty(info) ? string.Empty : info.Split('/')[4].Trim(),
        };

        return userInfo;
    }

    private UserStats SetUserStats(string id, string stats)
    {
        UserStats userStats = new UserStats
        {
            Id = id,
            NhlWins = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[1].Trim()),
            NhlLosses = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[2].Trim()),
            NhlTies = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[3].Trim()),
            NhlOTLs = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[4].Trim()),
            PwhlWins = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[5].Trim()),
            PwhlLosses = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[6].Trim()),
            PwhlTies = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[7].Trim()),
            PwhlOTLs = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[8].Trim()),
            NhlFranchiseWins = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[9].Trim()),
            NhlFranchiseLosses = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[10].Trim()),
            NhlFranchiseTies = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[11].Trim()),
            NhlFranchiseOTLs = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[12].Trim()),
            PwhlFranchiseWins = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[13].Trim()),
            PwhlFranchiseLosses = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[14].Trim()),
            PwhlFranchiseTies = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[15].Trim()),
            PwhlFranchiseOTLs = string.IsNullOrEmpty(stats) ? 0 : Int32.Parse(stats.Split('/')[16].Trim()),
        };

        return userStats;
    }

    private UserSeasonStats SetUserSeasonStats(string id, string seasonStats)
    {
        UserSeasonStats userSeasonStats = new UserSeasonStats
        {
            Id = id,
            League = string.IsNullOrEmpty(seasonStats) ? string.Empty : seasonStats.Split('/')[1].Trim(),
            Team = string.IsNullOrEmpty(seasonStats) ? string.Empty : seasonStats.Split('/')[2].Trim(),
            IsInSeason = string.IsNullOrEmpty(seasonStats) ? false : (seasonStats.Split('/')[3].Trim() == "true"),
            CurrentWins = string.IsNullOrEmpty(seasonStats) ? 0 : Int32.Parse(seasonStats.Split('/')[4].Trim()),
            CurrentLosses = string.IsNullOrEmpty(seasonStats) ? 0 : Int32.Parse(seasonStats.Split('/')[5].Trim()),
            CurrentTies = string.IsNullOrEmpty(seasonStats) ? 0 : Int32.Parse(seasonStats.Split('/')[6].Trim()),
            CurrentOTLs = string.IsNullOrEmpty(seasonStats) ? 0 : Int32.Parse(seasonStats.Split('/')[7].Trim()),
            TotalCups = string.IsNullOrEmpty(seasonStats) ? 0 : Int32.Parse(seasonStats.Split('/')[8].Trim()),
        };

        return userSeasonStats;
    }

    private string SaveUserInfo()
    {
        string finalString = string.Empty;

        finalString += UserData.Info.Id + "/";
        finalString += UserData.Info.Name + "/";
        finalString += UserData.Info.Email + "/";
        finalString += UserData.Info.Password + "/";
        finalString += UserData.Info.Team;

        return finalString;
    }

    private string SaveUserStats()
    {
        string finalString = string.Empty;

        finalString += UserData.Stats.Id + "/";
        finalString += UserData.Stats.NhlWins.ToString() + "/";
        finalString += UserData.Stats.NhlLosses.ToString() + "/";
        finalString += UserData.Stats.NhlTies.ToString() + "/";
        finalString += UserData.Stats.NhlOTLs.ToString() + "/";
        finalString += UserData.Stats.PwhlWins.ToString() + "/";
        finalString += UserData.Stats.PwhlLosses.ToString() + "/";
        finalString += UserData.Stats.PwhlTies.ToString() + "/";
        finalString += UserData.Stats.PwhlOTLs.ToString() + "/";
        finalString += UserData.Stats.NhlFranchiseWins.ToString() + "/";
        finalString += UserData.Stats.NhlFranchiseLosses.ToString() + "/";
        finalString += UserData.Stats.NhlFranchiseTies.ToString() + "/";
        finalString += UserData.Stats.NhlFranchiseOTLs.ToString() + "/";
        finalString += UserData.Stats.PwhlFranchiseWins.ToString() + "/";
        finalString += UserData.Stats.PwhlFranchiseLosses.ToString() + "/";
        finalString += UserData.Stats.PwhlFranchiseTies.ToString() + "/";
        finalString += UserData.Stats.PwhlFranchiseOTLs.ToString();

        return finalString;
    }

    private string SaveUserSeasonStats()
    {
        string finalString = string.Empty;

        finalString += UserData.SeasonStats.Id + "/";
        finalString += UserData.SeasonStats.League + "/";
        finalString += UserData.SeasonStats.Team + "/";
        finalString += UserData.SeasonStats.IsInSeason.ToString() + "/";
        finalString += UserData.SeasonStats.CurrentWins.ToString() + "/";
        finalString += UserData.SeasonStats.CurrentLosses.ToString() + "/";
        finalString += UserData.SeasonStats.CurrentTies.ToString() + "/";
        finalString += UserData.SeasonStats.CurrentOTLs.ToString() + "/";
        finalString += UserData.SeasonStats.TotalCups.ToString();

        return finalString;
    }
#endregion
}}
