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
public class UiPlayoffResults : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _simulateButton;

    [Header("Text Elements")]
    [SerializeField] private TMP_Text _homeTeamText;
    [SerializeField] private TMP_Text _homeStatsText;
    [SerializeField] private TMP_Text _awayTeamText;
    [SerializeField] private TMP_Text _awayStatsText;

    [Header("Icon Elements")]
    [SerializeField] private Image _homeIcon;
    [SerializeField] private Image _awayIcon;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        InitializeUi();
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
		_simulateButton.SetListener(GoToNightSimulate);

        SetGameData();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToNightSimulate()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to playoff simulate night screen.");

		GoToScene(CoreController.Inst.Scene_Season11);
    }

    private void SetGameData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game data.");

        // TODO
    }
#endregion
}}
