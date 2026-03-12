// Main Dependencies
using DG.Tweening;
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

namespace SoM.Ui {
public class UiMainOpening : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Version Text Element")]
    [SerializeField] private TMP_Text _versionText;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private float keepDuration = 3f;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
	{
		InitializeUi();
	}
#endregion
#region -------------------- Coroutines --------------------
    private IEnumerator PauseToStart()
    {
        yield return new WaitForSeconds(keepDuration);

        _mainContent.Clear();

        GoToNewScene(CoreController.Inst.Scene_Main01);
    }
#endregion
#region -------------------- Public Methods --------------------
    
#endregion
#region -------------------- Private Methods --------------------
    protected override void InitializeUi()
	{
        _versionText.text = $"Version: {Application.version}";

        base.InitializeUi(() => { StartCoroutine(PauseToStart()); });
	}
#endregion
}}
