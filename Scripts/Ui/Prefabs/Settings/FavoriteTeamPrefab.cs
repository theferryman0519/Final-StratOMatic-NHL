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
public class FavoriteTeamPrefab : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Team Icon Elements")]
    [SerializeField] private Image _icon;
    [SerializeField] private Button _iconButton;
#endregion
#region -------------------- Public Variables --------------------
    public Button IconButton => _iconButton;

    public string TeamString = string.Empty;
#endregion
#region -------------------- Private Variables --------------------

#endregion
#region -------------------- Initial Functions --------------------

#endregion
#region -------------------- Coroutines --------------------

#endregion
#region -------------------- Public Methods --------------------
    public void SetIcon(Team team, bool isSelected)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the team icon.");

        string shortLeague = team.Info.League.Contains("NHL") ? "NHL" : "PWHL";
        
        TeamString = $"{shortLeague}_{team.Info.Code}";
        string isSelectedString = isSelected ? "_ON" : "_OFF";
        
        _icon.sprite = ConstantController.Inst.IconSprites[TeamString + isSelectedString];
    }

    public void SwitchOff()
    {
        if (!string.IsNullOrEmpty(TeamString))
        {
            string newTeamString = TeamString;

            _icon.sprite = ConstantController.Inst.IconSprites[TeamString + "_OFF"];
        }
    }
#endregion
#region -------------------- Private Methods --------------------

#endregion
}}
