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

		SetBanner();

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

	private void SetBanner()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the main banner.");

		if (SeasonsController.Inst.SeasonData != null)
		{
			string spriteName = $"{SeasonsController.Inst.SeasonData.League}_{SeasonsController.Inst.SeasonData.Team.Info.Code}";

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();

			return;
		}

		if (PlayoffsController.Inst.PlayoffData != null)
		{
			string spriteName = $"{PlayoffsController.Inst.PlayoffData.League}_{PlayoffsController.Inst.PlayoffData.Team.Info.Code}";

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();

			return;
		}

		if (GameplayController.Inst.GameData != null)
		{
			string spriteName = $"{GameplayController.Inst.GameData.HomeTeam.Info.League}_{GameplayController.Inst.GameData.HomeTeam.Info.Code}";

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();

			return;
		}

		Team userTeam = TeamsController.Inst.GetTeamFromCode(UsersController.Inst.UserData.Info.Team);
		string spriteName = $"{userTeam.Info.League}_{userTeam.Info.Code}";

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
