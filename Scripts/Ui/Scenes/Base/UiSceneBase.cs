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
    public Action ContinueAction;
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

		if (UiController.Inst.IsFadingBannerIn)
		{
			fadeInElements.Add(_banner);
		}

        AnimationController.Inst.FadeInObjects(fadeInElements, () =>
		{
			UiController.Inst.IsFadingBannerIn = false;

			ContinueAction?.Invoke();
		});
    }

    protected void GoToNewScene(string sceneName)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Going to a new scene: {sceneName}.");

		List<CanvasGroup> fadeOutElements = new();

		foreach (CanvasGroup mainContent in _mainContent)
		{
			fadeOutElements.Add(mainContent);
		}

		if (UiController.Inst.IsFadingBannerOut)
		{
			fadeOutElements.Add(_banner);
		}

		ContinueToScene(sceneName, fadeOutElements);
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

	private void SetBanner()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the main banner.");
		
		string spriteName = string.Empty;

		if (SeasonsController.Inst.SeasonData != null)
		{
			spriteName = $"{SeasonsController.Inst.SeasonData.League}_{SeasonsController.Inst.SeasonData.Team.Info.Code}";

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();

			return;
		}

		if (PlayoffsController.Inst.PlayoffData != null)
		{
			spriteName = $"{PlayoffsController.Inst.PlayoffData.League}_{PlayoffsController.Inst.PlayoffData.Team.Info.Code}";

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();

			return;
		}

		if (GameplayController.Inst.GameData != null)
		{
			spriteName = $"{GameplayController.Inst.GameData.HomeTeam.Team.League}_{GameplayController.Inst.GameData.HomeTeam.Team.Code}";

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();

			return;
		}

		Team userTeam = TeamsController.Inst.GetTeamFromCode(UsersController.Inst.UserData.Info.Team);
		spriteName = $"{userTeam.Info.League}_{userTeam.Info.Code}";

		_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
		_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

		SetBannerTitle();
	}

	private void SetBannerTitle()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the main banner title.");
		
		string sceneName = CoreController.Inst.GetSceneName();

		if (sceneName.Contains("Settings")) { _bannerTitle.text = "Settings"; }
		else if (sceneName.Contains("Exhibition")) { _bannerTitle.text = "Exhibition"; }
		else if (sceneName.Contains("Season")) { _bannerTitle.text = "Season"; }
		else if (sceneName.Contains("Playoff")) { _bannerTitle.text = "Playoff"; }
		else if (sceneName.Contains("Multiplayer")) { _bannerTitle.text = "Multiplayer"; }
		else { _bannerTitle.text = string.Empty; }
	}
#endregion
}}
