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
public class GameplayGameStatsPanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;
	[SerializeField] private SoM_Button _returnButton;

	[Header("Section Elements")]
	[SerializeField] private List<TMP_Text> _homeTexts = new();
	[SerializeField] private List<TMP_Text> _awayTexts = new();
	[SerializeField] private List<Slider> _homeSliders = new();
	[SerializeField] private List<Slider> _awaySliders = new();

    [Header("Main Elements")]
	[SerializeField] private CanvasGroup _mainElement;
	[SerializeField] private RectTransform _mainPanel;
#endregion
#region -------------------- Public Variables --------------------
    public CanvasGroup CanvasGroup => _mainElement;
    
    public RectTransform MainPanel => _mainPanel;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeGameStatsPanel()
	{
        _mainElement.alpha = 0f;

        _closeButton.SetListener(() => { ClosePanel(); });
		_returnButton.SetListener(() => { ClosePanel(); });

		SetStats();

        AnimationController.Inst.FadeInPanel(_mainElement, _mainPanel, () =>
        {
            _mainElement.alpha = 1f;
        });
	}

    public void ClosePanel(Action continueAction = null)
	{
		AnimationController.Inst.FadeOutPanel(_mainElement, _mainPanel, () =>
		{
			_mainElement.alpha = 0f;
			this.gameObject.SetActive(false);

			continueAction?.Invoke();
		});
	}
#endregion
#region -------------------- Private Methods --------------------
	private void SetStats()
	{
		int homeGoals = GameplayController.Inst.GameData.HomeTeam.Stats.Goals;
		int homeShots = GameplayController.Inst.GameData.HomeTeam.Stats.Shots;
		int homePPGs = GameplayController.Inst.GameData.HomeTeam.Stats.PowerplayGoals;
		int homeSHGs = GameplayController.Inst.GameData.HomeTeam.Stats.ShorthandedGoals;
		int homeFOWs = GameplayController.Inst.GameData.HomeTeam.Stats.FaceoffsWon;
		int homeHits = GameplayController.Inst.GameData.HomeTeam.Stats.Hits;
		int homeBSs = GameplayController.Inst.GameData.HomeTeam.Stats.BlockedShots;
		int homeGAs = GameplayController.Inst.GameData.HomeTeam.Stats.Giveaways;
		int homeTAs = GameplayController.Inst.GameData.HomeTeam.Stats.Takeaways;

		int awayGoals = GameplayController.Inst.GameData.AwayTeam.Stats.Goals;
		int awayShots = GameplayController.Inst.GameData.AwayTeam.Stats.Shots;
		int awayPPGs = GameplayController.Inst.GameData.AwayTeam.Stats.PowerplayGoals;
		int awaySHGs = GameplayController.Inst.GameData.AwayTeam.Stats.ShorthandedGoals;
		int awayFOWs = GameplayController.Inst.GameData.AwayTeam.Stats.FaceoffsWon;
		int awayHits = GameplayController.Inst.GameData.AwayTeam.Stats.Hits;
		int awayBSs = GameplayController.Inst.GameData.AwayTeam.Stats.BlockedShots;
		int awayGAs = GameplayController.Inst.GameData.AwayTeam.Stats.Giveaways;
		int awayTAs = GameplayController.Inst.GameData.AwayTeam.Stats.Takeaways;

		_homeTexts[0].text = homeGoals.ToString();
		_homeTexts[1].text = homeShots.ToString();
		_homeTexts[2].text = homePPGs.ToString();
		_homeTexts[3].text = homeSHGs.ToString();
		_homeTexts[4].text = homeFOWs.ToString();
		_homeTexts[5].text = homeHits.ToString();
		_homeTexts[6].text = homeBSs.ToString();
		_homeTexts[7].text = homeGAs.ToString();
		_homeTexts[8].text = homeTAs.ToString();

		_awayTexts[0].text = awayGoals.ToString();
		_awayTexts[1].text = awayShots.ToString();
		_awayTexts[2].text = awayPPGs.ToString();
		_awayTexts[3].text = awaySHGs.ToString();
		_awayTexts[4].text = awayFOWs.ToString();
		_awayTexts[5].text = awayHits.ToString();
		_awayTexts[6].text = awayBSs.ToString();
		_awayTexts[7].text = awayGAs.ToString();
		_awayTexts[8].text = awayTAs.ToString();

		_homeSliders[0].value = (homeGoals + awayGoals) == 0 ? 0 : homeGoals / (homeGoals + awayGoals);
		_homeSliders[1].value = (homeShots + awayShots) == 0 ? 0 : homeShots / (homeShots + awayShots);
		_homeSliders[2].value = (homePPGs + awayPPGs) == 0 ? 0 : homePPGs / (homePPGs + awayPPGs);
		_homeSliders[3].value = (homeSHGs + awaySHGs) == 0 ? 0 : homeSHGs / (homeSHGs + awaySHGs);
		_homeSliders[4].value = (homeFOWs + awayFOWs) == 0 ? 0 : homeFOWs / (homeFOWs + awayFOWs);
		_homeSliders[5].value = (homeHits + awayHits) == 0 ? 0 : homeHits / (homeHits + awayHits);
		_homeSliders[6].value = (homeBSs + awayBSs) == 0 ? 0 : homeBSs / (homeBSs + awayBSs);
		_homeSliders[7].value = (homeGAs + awayGAs) == 0 ? 0 : homeGAs / (homeGAs + awayGAs);
		_homeSliders[8].value = (homeTAs + awayTAs) == 0 ? 0 : homeTAs / (homeTAs + awayTAs);

		_awaySliders[0].value = (homeGoals + awayGoals) == 0 ? 0 : awayGoals / (homeGoals + awayGoals);
		_awaySliders[1].value = (homeShots + awayShots) == 0 ? 0 : awayShots / (homeShots + awayShots);
		_awaySliders[2].value = (homePPGs + awayPPGs) == 0 ? 0 : awayPPGs / (homePPGs + awayPPGs);
		_awaySliders[3].value = (homeSHGs + awaySHGs) == 0 ? 0 : awaySHGs / (homeSHGs + awaySHGs);
		_awaySliders[4].value = (homeFOWs + awayFOWs) == 0 ? 0 : awayFOWs / (homeFOWs + awayFOWs);
		_awaySliders[5].value = (homeHits + awayHits) == 0 ? 0 : awayHits / (homeHits + awayHits);
		_awaySliders[6].value = (homeBSs + awayBSs) == 0 ? 0 : awayBSs / (homeBSs + awayBSs);
		_awaySliders[7].value = (homeGAs + awayGAs) == 0 ? 0 : awayGAs / (homeGAs + awayGAs);
		_awaySliders[8].value = (homeTAs + awayTAs) == 0 ? 0 : awayTAs / (homeTAs + awayTAs);
	}
#endregion
}}
