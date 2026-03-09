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

namespace SoM.Skaters {
public class GoalieCreationZone : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private List<string> allGoalieIds = new();
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public async void CreateGoalies()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating all goalie data.");

        allGoalieIds = await FirebaseController.Inst.GetAllGoalieIds();

        CreateGoalieData();
    }

    public async void CreateGoalieCards()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating all goalie card data.");

        List<GoalieData> allGoalies = new(SkatersController.Inst.FullGoalies);
        SkatersController.Inst.FullGoalies.Clear();

        foreach (GoalieData goalie in allGoalies)
        {
            GoalieData tempGoalie = goalie;

            goalie.Card = await CreateGoalieCardData(tempGoalie);
            goalie.Game = await CreateGoalieGameData(tempGoalie);
            goalie.Season = await CreateGoalieSeasonData(tempGoalie);

            SkatersController.Inst.FullGoalies.Add(goalie);
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private async void CreateGoalieData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Creating all goalie main data.");

        foreach (string id in allGoalieIds)
        {
            GoalieData newGoalieData = new GoalieData
            {
                Id = id
            };

            newGoalieData.FirstName = await FirebaseController.Inst.GetGoalieInfo(id, "FirstName");
            newGoalieData.LastName = await FirebaseController.Inst.GetGoalieInfo(id, "LastName");
            newGoalieData.Team = await FirebaseController.Inst.GetGoalieInfo(id, "Team");
            newGoalieData.Position = await FirebaseController.Inst.GetGoalieInfo(id, "Position");
            newGoalieData.Stats = await FirebaseController.Inst.GetGoalieStats(id);

            SkatersController.Inst.FullGoalies.Add(newGoalieData);
        }
    }
#endregion
}}
