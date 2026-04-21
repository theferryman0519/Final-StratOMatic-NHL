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
    
    public void SetBanner()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the main banner.");

		string sceneName = CoreController.Inst.GetSceneName();

		if (sceneName.Contains("Exhibition")) { SetExhibitionBanner(); }
		else if (sceneName.Contains("Season")) { SetSeasonBanner(); }
		else if (sceneName.Contains("Playoff")) { SetSeasonBanner(); }
		else { SetMainBanner(); }
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

	private void SetMainBanner()
	{
		string spriteName = string.Empty;

		if (UsersController.Inst.UserData != null)
		{
			ConstantController.LeagueType leagueType = ConstantController.LeagueType.None;
		
			if (UsersController.Inst.UserData.Info.League == "NHL") { leagueType = ConstantController.LeagueType.NHL; }
			else if (UsersController.Inst.UserData.Info.League == "NHLFranchise") { leagueType = ConstantController.LeagueType.NHLFranchise; }
			else if (UsersController.Inst.UserData.Info.League == "PWHL") { leagueType = ConstantController.LeagueType.PWHL; }
			else if (UsersController.Inst.UserData.Info.League == "PWHLFranchise") { leagueType = ConstantController.LeagueType.PWHLFranchise; }
			
			Team userTeam = TeamsController.Inst.GetTeamFromCode(UsersController.Inst.UserData.Info.Team, leagueType);

			if (userTeam == null)
			{
				spriteName = $"NHL_NHL";
			}
			
			else
			{
				string shortLeague = userTeam.Info.League.Contains("NHL") ? "NHL" : "PWHL";
				spriteName = $"{shortLeague}_{userTeam.Info.Code}";
			}

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();
		}
	}

	private void SetExhibitionBanner()
	{
		string spriteName = string.Empty;

		if (GameplayController.Inst.GameData != null)
		{
			if (GameplayController.Inst.GameData.HomeTeam != null)
			{
				string shortLeague = GameplayController.Inst.GameData.HomeTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";
				spriteName = $"{shortLeague}_{GameplayController.Inst.GameData.HomeTeam.Team.Code}";
			}

			else
			{
				spriteName = $"NHL_NHL";
			}

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();

			return;
		}

		SetMainBanner();
	}

	private void SetSeasonBanner()
	{
		string spriteName = string.Empty;

		if (SeasonsController.Inst.SeasonData != null)
		{
			string shortLeague = SeasonsController.Inst.SeasonData.League.Contains("NHL") ? "NHL" : "PWHL";
			spriteName = $"{shortLeague}_{SeasonsController.Inst.SeasonData.Team.Team.Code}";

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();

			return;
		}

		if (PlayoffsController.Inst.PlayoffData != null)
		{
			string shortLeague = PlayoffsController.Inst.PlayoffData.League.Contains("NHL") ? "NHL" : "PWHL";
			spriteName = $"{shortLeague}_{PlayoffsController.Inst.PlayoffData.Team.Team.Code}";

			_bannerBackground.sprite = ConstantController.Inst.BannerSprites[spriteName];
			_bannerLogo.sprite = ConstantController.Inst.LogoSprites[spriteName];

			SetBannerTitle();

			return;
		}

		SetMainBanner();
	}
#endregion
}}
