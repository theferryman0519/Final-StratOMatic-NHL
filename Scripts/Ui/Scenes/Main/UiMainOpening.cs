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

        CheckAutoLogin();
    }
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
        _versionText.text = $"Version: {Application.version}";

        base.InitializeUi(() => { StartCoroutine(PauseToStart()); });
	}
#endregion
#region -------------------- Private Methods --------------------
    private void CheckAutoLogin()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Checking for auto login capability.");

        CoreController.Inst.Initialize();
		ConstantController.Inst.Initialize();
		
		bool hasInternet = await CoreController.Inst.HasInternetConnection();

		if (!hasInternet)
		{
            // TODO: Show "Internet Error" bottom panel
			return;
		}

        bool hasEmail = PlayerPrefs.HasKey(ConstantController.Pref_Email);
        bool hasPassword = PlayerPrefs.HasKey(ConstantController.Pref_Password);

        if (!hasEmail || !hasPassword)
        {
            GoToNewScene(CoreController.Inst.Scene_Main02);
        }

        else
        {
            StartCoroutine(FirebaseController.Inst.InitializingFirebase(AutoLogin));
        }
    }

    private void AutoLogin()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Automatically logging into the game.");

        string email = PlayerPrefs.GetString(ConstantController.Pref_Email);
        string password = PlayerPrefs.GetString(ConstantController.Pref_Password);

        FirebaseLogin loginData = new FirebaseLogin
        {
            Email = email,
            Password = password,
            SuccessAction = () => { _mainContent.Clear(); GoToNewScene(CoreController.Inst.Scene_Main01); },
            FailAction = () => { GoToNewScene(CoreController.Inst.Scene_Main03); },
        };

        StartCoroutine(Firebase_Controller.Inst.SigningInUserToFirebase(loginData));
    }
#endregion
}}
