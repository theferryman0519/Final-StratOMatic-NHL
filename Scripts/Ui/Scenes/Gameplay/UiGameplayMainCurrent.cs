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

namespace SoM.Ui {
public class UiGameplayMainCurrent : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Stamina Elements")]
    [SerializeField] private SoM_Dropdown _teamDropDown;
    [SerializeField] private Transform _container;
    [SerializeField] private GameplayStaminaPrefab _staminaPrefab;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private UiGameplayMain mainUi;

    private int teamSelection = 0;
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void UpdateCurrent(UiGameplayMain ui)
    {
        if (ui == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the current skaters.");

        mainUi = ui;

        _teamDropDown.SetListener(ChangeStaminaTeam);

        ChangeStaminaTeam(teamSelection);
    }
#endregion
#region -------------------- Private Methods --------------------
    private void ChangeStaminaTeam(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the stamina view to home or away team.");

        switch (option)
        {
            case 1: ShowStaminaTeam("Away"); break;
            case 0:
            default: ShowStaminaTeam("Home"); break;
        }
    }

    private void ShowStaminaTeam(string team)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the current line and pair stamina.");

        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }

        GameTeam chosenTeam = team == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        bool isExtra = GameplayController.Inst.GameData.PullGoalieTeam == team;
        bool isShorthanded = GameplayController.Inst.GameData.PowerplayTeam != "None" && GameplayController.Inst.GameData.PowerplayTeam != team;

        int currentLine = chosenTeam.CurrentLine;
        int currentPair = chosenTeam.CurrentPair;

        Dictionary<string, Skater> onIceSkaters = new();

        if (!isShorthanded)
        {
            Skater center = chosenTeam.SkaterLineup[$"C{currentLine}"];
            onIceSkaters.Add("C", center);
        }

        Skater leftWing = chosenTeam.SkaterLineup[$"LW{currentLine}"];
        
        if (leftWing == EventsController.Inst.GameplayEvents.PenaltyEvents.PenaltySkater)
        {
            leftWing = chosenTeam.SkaterLineup[$"LW2"];
        }
        
        onIceSkaters.Add("LW", leftWing);

        Skater rightWing = chosenTeam.SkaterLineup[$"RW{currentLine}"];
        
        if (rightWing == EventsController.Inst.GameplayEvents.PenaltyEvents.PenaltySkater)
        {
            rightWing = chosenTeam.SkaterLineup[$"RW2"];
        }
        
        onIceSkaters.Add("RW", rightWing);

        Skater leftDefense = chosenTeam.SkaterLineup[$"LD{currentPair}"];
        
        if (leftDefense == EventsController.Inst.GameplayEvents.PenaltyEvents.PenaltySkater)
        {
            leftDefense = chosenTeam.SkaterLineup[$"LD2"];
        }
        
        onIceSkaters.Add("LD", leftDefense);

        Skater rightDefense = chosenTeam.SkaterLineup[$"RD{currentPair}"];
        
        if (rightDefense == EventsController.Inst.GameplayEvents.PenaltyEvents.PenaltySkater)
        {
            rightDefense = chosenTeam.SkaterLineup[$"RD2"];
        }
        
        onIceSkaters.Add("RD", rightDefense);

        if (isExtra)
        {
            Skater extra = chosenTeam.SkaterLineup["C2"].Id == chosenTeam.SkaterLineup["C1"].Id ? chosenTeam.SkaterLineup["C3"] : chosenTeam.SkaterLineup["C2"];
            onIceSkaters.Add("EA", extra);
        }

        foreach (KeyValuePair<string, Skater> onIceSkater in onIceSkaters)
        {
            GameplayStaminaPrefab staminaObj = Instantiate(_staminaPrefab, _container);

            staminaObj.SetVisual(onIceSkater.Key, onIceSkater.Value);
            staminaObj.StaminaButton.onClick.AddListener(() =>
            {
                CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the skater stats.");

                mainUi.ShowSkaterStatsPanel(onIceSkater.Value);
            });
        }
    }
#endregion
}}
