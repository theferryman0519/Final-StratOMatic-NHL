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
public class MainOpeningUi : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Logo Elements")]
    [SerializeField] private CanvasGroup _gameLogo;
    [SerializeField] private CanvasGroup _studioLogo;

    [Header("Main Elements")]
    [SerializeField] private CanvasGroup _rink;
    [SerializeField] private CanvasGroup _version;

    [SerializeField] private TMP_Text _versionText;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private Sequence sequence;

    private float fadeDuration = 1f;
    private float keepDuration = 3f;
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
    
#endregion
#region -------------------- Private Methods --------------------
    private void InitializeUi()
	{
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the UI for the scene.");

		SetAllAlphas();
		RunSequence();
	}

    private void SetAllAlphas()
	{
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the alphas for the scene elements.");

		_gameLogo.alpha = 0f;
		_studioLogo.alpha = 0f;
		_rink.alpha = 0f;
        _version.alpha = 0f;
	}

    private void RunSequence()
	{
        CoreController.Inst.WriteLog(this.GetType().Name, $"Running the fade sequence.");

		sequence = DOTween.Sequence();
		
		sequence.Append(_gameLogo.DOFade(1f, fadeDuration));
        sequence.Join(_studioLogo.DOFade(1f, fadeDuration));
        sequence.Join(_rink.DOFade(1f, fadeDuration));
        sequence.Join(_version.DOFade(1f, fadeDuration));
		sequence.AppendInterval(keepDuration);
		
		sequence.Append(_gameLogo.DOFade(0f, fadeDuration));
        sequence.Join(_studioLogo.DOFade(0f, fadeDuration));

		sequence.OnComplete(() =>
		{
			CoreController.Inst.ChangeScene(CoreController.Inst.Scene_Main01);
		});
	}
#endregion
}}
