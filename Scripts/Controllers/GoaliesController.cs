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
using SoM.Goalies;

namespace SoM.Controllers {
public class GoaliesController : Singleton<GoaliesController> {

#region -------------------- Serialized Variables --------------------
    [Header("Creation Elements")]
    [SerializeField] private GoalieCreation _goalieCreation;
#endregion
#region -------------------- Public Variables --------------------
    public Dictionary<string, List<string>> NhlGoalies = new();
    public Dictionary<string, List<string>> PwhlGoalies = new();
    public Dictionary<string, List<string>> NhlFranchiseGoalies = new();
    public Dictionary<string, List<string>> PwhlFranchiseGoalies = new();
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

        NhlGoalies.Clear();
        PwhlGoalies.Clear();
        NhlFranchiseGoalies.Clear();
        PwhlFranchiseGoalies.Clear();

        SetAllGoalies();
    }
#endregion
#region -------------------- Private Methods --------------------
    private async void SetAllGoalies()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting all goalies.");

        await FirebaseController.Inst.GetAllGoalies(allGoaliesData =>
        {
            foreach (GoalieDatabase goalieData in allGoaliesData)
            {
                Goalie goalie = await _GoalieCreation.CreateGoalie(goalieData);

                switch (goalie.Info.League)
                {
                    case "NHL":
                        if (!NhlGoalies.ContainsKey(goalie.Info.Team))
                        {
                            NhlGoalies.Add(goalie.Info.Team, new());
                        }

                        NhlGoalies[goalie.Info.Team].Add(goalie);
                        break;
                    case "PWHL":
                        if (!PwhlGoalies.ContainsKey(goalie.Info.Team))
                        {
                            PwhlGoalies.Add(goalie.Info.Team, new());
                        }

                        PwhlGoalies[goalie.Info.Team].Add(goalie);
                        break;
                    case "NHL-Franchise":
                        if (!NhlFranchiseGoalies.ContainsKey(goalie.Info.Team))
                        {
                            NhlFranchiseGoalies.Add(goalie.Info.Team, new());
                        }

                        NhlFranchiseGoalies[goalie.Info.Team].Add(goalie);
                        break;
                    case "PWHL-Franchise":
                        if (!PwhlFranchiseGoalies.ContainsKey(goalie.Info.Team))
                        {
                            PwhlFranchiseGoalies.Add(goalie.Info.Team, new());
                        }

                        PwhlFranchiseGoalies[goalie.Info.Team].Add(goalie);
                        break;
                }
            }

            CoreController.Inst.LoadingStepCompleted();
        });
    }
#endregion
}}
