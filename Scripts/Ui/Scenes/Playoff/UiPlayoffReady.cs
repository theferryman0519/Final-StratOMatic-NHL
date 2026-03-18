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
public class UiPlayoffReady : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _startButton;
	[SerializeField] private SoM_Button _returnButton;

    [Header("Text Elements")]
    [SerializeField] private TMP_Text _roundText;
    [SerializeField] private TMP_Text _homeTeamText;
    [SerializeField] private TMP_Text _homeLinesText;
    [SerializeField] private TMP_Text _awayTeamText;
    [SerializeField] private TMP_Text _awayLinesText;

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
		_startButton.SetListener(GoToLoading);
		_returnButton.SetListener(GoToEditLines);

        SetGameData();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToLoading()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to playoff game loading screen.");

		GoToScene(CoreController.Inst.Scene_Playoff08);
    }

    private void GoToEditLines()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to playoff edit lines screen.");

		GoToScene(CoreController.Inst.Scene_Playoff07);
    }

    private void SetGameData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game data.");

        // TODO
    }
#endregion
}}
