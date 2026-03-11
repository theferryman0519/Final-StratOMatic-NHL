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
    [SerializeField] protected CanvasGroup _bannerCanvas;
    [SerializeField] protected Image _bannerBackground;
    [SerializeField] protected Image _bannerIcon;
    [SerializeField] protected TMP_Text _bannerTitle;

    [Header("Main Content Elements")]
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
    protected virtual void InitializeUi()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the UI for the scene.");

        SetBanner();

        List<CanvasGroup> fadeInElements = new();

        foreach (CanvasGroup mainContent in _mainContent)
		{
			fadeInElements.Add(mainContent);
		}

        if (UiController.Inst.IsBannerFadingIn)
        {
            fadeInElements.Add(_bannerCanvas);
        }

        AnimationController.Inst.FadeInObjects(fadeInElements, () =>
		{
			UiController.Inst.IsBannerFadingIn = false;
		});
    }

    public void GotoNewScene(string sceneName)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to a new scene: {sceneName}.");

		List<CanvasGroup> fadeOutElements = new();

        foreach (CanvasGroup mainContent in _mainContent)
		{
			fadeOutElements.Add(mainContent);
		}

        if (UiController.Inst.IsBannerFadingOut)
        {
            fadeOutElements.Add(_bannerCanvas);
        }

        ContinueToScene(sceneName, finalElements);
    }
#endregion
#region -------------------- Private Methods --------------------
    private void SetBanner()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the banner.");

        if (UsersController.Inst.ChosenTeam == null)
        {
            _bannerBackground.sprite = ConstantController.Inst.BannerSprites["NHL"];
            _bannerIcon.sprite = ConstantController.Inst.LogoSprites["NHL"];
        }

        else
        {
            string teamCode = UsersController.Inst.ChosenTeam.Code;

            if (!ConstantController.Inst.BannerSprites.ContainsKey(teamCode) || !ConstantController.Inst.LogoSprites.ContainsKey(teamCode))
            {
                _bannerBackground.sprite = ConstantController.Inst.BannerSprites["NHL"];
                _bannerIcon.sprite = ConstantController.Inst.LogoSprites["NHL"];
            }

            else
            {
                _bannerBackground.sprite = ConstantController.Inst.BannerSprites[teamCode];
                _bannerIcon.sprite = ConstantController.Inst.LogoSprites[teamCode];
            }
        }
    }

    private void ContinueToScene(string sceneName, List<CanvasGroup> elements)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Continuing to scene: {sceneName}.");

		AnimationController.Inst.FadeOutObjects(elements, () =>
		{
			UiController.Inst.IsBannerFadingOut = false;

			CoreController.Inst.ChangeScene(sceneName);
		});
	}
#endregion
}}
