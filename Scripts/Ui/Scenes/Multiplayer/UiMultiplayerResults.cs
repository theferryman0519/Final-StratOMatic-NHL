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
public class UiMultiplayerResults : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _returnButton;

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
		_returnButton.SetListener(GoToHome);

        SetGameData();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToHome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to home screen.");

		GoToScene(CoreController.Inst.Scene_Home00);
    }

    private void SetGameData()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the game data.");

        // TODO
    }
#endregion
}}
