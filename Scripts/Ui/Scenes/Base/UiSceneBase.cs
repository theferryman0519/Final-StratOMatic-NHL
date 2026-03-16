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

namespace SoM.Ui {
public class UiSceneBase : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Banner Elements")]
    [SerializeField] protected Image _bannerBackground;
    [SerializeField] protected Image _bannerLogo;
    [SerializeField] protected TMP_Text _bannerTitle;

    [Header("Canvas Group Elements")]
    [SerializeField] protected CanvasGroup _banner;
    [SerializeField] protected List<CanvasGroup> _mainContent;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    protected virtual InitializeUi(Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the UI for the scene.");

        List<CanvasGroup> fadeInElements = new();

		foreach (CanvasGroup mainContent in _mainContent)
		{
			fadeInElements.Add(mainContent);
		}

		if (UiController.Inst.IsFadingBannerIn)
		{
			fadeInElements.Add(_banner.CanvasGroup);
		}

        AnimationController.Inst.FadeInObjects(fadeInElements, () =>
		{
			UiController.Inst.IsFadingBannerIn = false;

			continueAction?.Invoke();
		});
    }

	public void SetBanner(Team team)
	{
		// TODO
	}

    public void GoToNewScene(string sceneName)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Going to a new scene: {sceneName}.");

		List<CanvasGroup> fadeOutElements = new();

		foreach (CanvasGroup mainContent in _mainContent)
		{
			fadeOutElements.Add(mainContent);
		}

		if (UiController.Inst.IsFadingBannerOut)
		{
			fadeOutElements.Add(_banner.CanvasGroup);
		}

		ContinueToScene(sceneName, finalElements);
	}
#endregion
#region -------------------- Private Methods --------------------
    private void ContinueToScene(string sceneName, List<CanvasGroup> elements)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Continuing to scene: {sceneName}.");

		AnimationController.Inst.FadeOutObjects(elements, () =>
		{
			UiController.Inst.IsFadingBannerIn = false;

			CoreController.Inst.ChangeScene(sceneName);
		});
	}
#endregion
}}
