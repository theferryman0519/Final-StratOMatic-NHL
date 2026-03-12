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
using SoM.Models;

namespace SoM.Ui {
public class UiMainAutoLogin : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Canvas Group Elements")]
    [SerializeField] private CanvasGroup _gameLogo;
    [SerializeField] private CanvasGroup _studioLogo;
    [SerializeField] private CanvasGroup _rink;
    [SerializeField] private CanvasGroup _version;

    [Header("Version Text Element")]
    [SerializeField] private TMP_Text _versionText;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private float fadeDuration = 1f;

    private Sequence sequence;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        CheckAutoLogin();
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    
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
            RunSequence(() => { CoreController.Inst.ChangeScene(CoreController.Inst.Scene_Main03); });
            return;
        }

        StartCoroutine(FirebaseController.Inst.InitializingFirebase(AutoLogin));
    }

    private void RunSequence(Action continueAction = null)
	{
        CoreController.Inst.WriteLog(this.GetType().Name, $"Running fade out sequence.");

		sequence = DOTween.Sequence();
		
		sequence.Append(_gameLogo.DOFade(0f, fadeDuration));
        sequence.Join(_studioLogo.DOFade(0f, fadeDuration));
        sequence.Join(_rink.DOFade(0f, fadeDuration));
        sequence.Join(_version.DOFade(0f, fadeDuration));
		
		// Complete
		sequence.OnComplete(() =>
		{
			continueAction?.Invoke();
		});
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
            SuccessAction = () => { RunSequence(() => { CoreController.Inst.ChangeScene(CoreController.Inst.Scene_Main02); }); },
            FailAction = () => { RunSequence(() => { CoreController.Inst.ChangeScene(CoreController.Inst.Scene_Main04); }); },
        };

        StartCoroutine(Firebase_Controller.Inst.SigningInUserToFirebase(loginData));
    }
#endregion
}}
