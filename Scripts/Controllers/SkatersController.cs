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
using SoM.Skaters;

namespace SoM.Controllers {
public class SkatersController : Singleton<SkatersController> {

#region -------------------- Serialized Variables --------------------
    [Header("Creation Elements")]
    [SerializeField] private SkaterCreation _skaterCreation;
#endregion
#region -------------------- Public Variables --------------------
    public Dictionary<string, List<string>> NhlSkaters = new();
    public Dictionary<string, List<string>> PwhlSkaters = new();
    public Dictionary<string, List<string>> NhlFranchiseSkaters = new();
    public Dictionary<string, List<string>> PwhlFranchiseSkaters = new();
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

        NhlSkaters.Clear();
        PwhlSkaters.Clear();
        NhlFranchiseSkaters.Clear();
        PwhlFranchiseSkaters.Clear();

        SetAllSkaters();
    }
#endregion
#region -------------------- Private Methods --------------------
    private async void SetAllSkaters()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting all skaters.");

        await FirebaseController.Inst.GetAllSkaters(allSkatersData =>
        {
            foreach (SkaterDatabase skaterData in allSkatersData)
            {
                Skater skater = await _skaterCreation.CreateSkater(skaterData);

                switch (skater.Info.League)
                {
                    case "NHL":
                        if (!NhlSkaters.ContainsKey(skater.Info.Team))
                        {
                            NhlSkaters.Add(skater.Info.Team, new());
                        }

                        NhlSkaters[skater.Info.Team].Add(skater);
                        break;
                    case "PWHL":
                        if (!PwhlSkaters.ContainsKey(skater.Info.Team))
                        {
                            PwhlSkaters.Add(skater.Info.Team, new());
                        }

                        PwhlSkaters[skater.Info.Team].Add(skater);
                        break;
                    case "NHL-Franchise":
                        if (!NhlFranchiseSkaters.ContainsKey(skater.Info.Team))
                        {
                            NhlFranchiseSkaters.Add(skater.Info.Team, new());
                        }

                        NhlFranchiseSkaters[skater.Info.Team].Add(skater);
                        break;
                    case "PWHL-Franchise":
                        if (!PwhlFranchiseSkaters.ContainsKey(skater.Info.Team))
                        {
                            PwhlFranchiseSkaters.Add(skater.Info.Team, new());
                        }

                        PwhlFranchiseSkaters[skater.Info.Team].Add(skater);
                        break;
                }
            }

            CoreController.Inst.LoadingStepCompleted();
        });
    }
#endregion
}}
