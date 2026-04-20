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
			HidePanel();
			continueAction?.Invoke();
		});
	}
    
	public void HidePanel()
	{
		_mainElement.alpha = 0f;
		this.gameObject.SetActive(false);
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

		int awayGoals = GameplayController.Inst.GameData.AwayTeam.Stats.Goals;
		int awayShots = GameplayController.Inst.GameData.AwayTeam.Stats.Shots;
		int awayPPGs = GameplayController.Inst.GameData.AwayTeam.Stats.PowerplayGoals;
		int awaySHGs = GameplayController.Inst.GameData.AwayTeam.Stats.ShorthandedGoals;
		int awayFOWs = GameplayController.Inst.GameData.AwayTeam.Stats.FaceoffsWon;
		int awayHits = GameplayController.Inst.GameData.AwayTeam.Stats.Hits;
		int awayBSs = GameplayController.Inst.GameData.AwayTeam.Stats.BlockedShots;

		_homeTexts[0].text = homeGoals.ToString();
		_homeTexts[1].text = homeShots.ToString();
		_homeTexts[2].text = homePPGs.ToString();
		_homeTexts[3].text = homeSHGs.ToString();
		_homeTexts[4].text = homeFOWs.ToString();
		_homeTexts[5].text = homeHits.ToString();
		_homeTexts[6].text = homeBSs.ToString();

		_awayTexts[0].text = awayGoals.ToString();
		_awayTexts[1].text = awayShots.ToString();
		_awayTexts[2].text = awayPPGs.ToString();
		_awayTexts[3].text = awaySHGs.ToString();
		_awayTexts[4].text = awayFOWs.ToString();
		_awayTexts[5].text = awayHits.ToString();
		_awayTexts[6].text = awayBSs.ToString();

		_homeSliders[0].value = (homeGoals + awayGoals) == 0 ? 0f : (float)homeGoals / (float)(homeGoals + awayGoals);
		_homeSliders[1].value = (homeShots + awayShots) == 0 ? 0f : (float)homeShots / (float)(homeShots + awayShots);
		_homeSliders[2].value = (homePPGs + awayPPGs) == 0 ? 0f : (float)homePPGs / (float)(homePPGs + awayPPGs);
		_homeSliders[3].value = (homeSHGs + awaySHGs) == 0 ? 0f : (float)homeSHGs / (float)(homeSHGs + awaySHGs);
		_homeSliders[4].value = (homeFOWs + awayFOWs) == 0 ? 0f : (float)homeFOWs / (float)(homeFOWs + awayFOWs);
		_homeSliders[5].value = (homeHits + awayHits) == 0 ? 0f : (float)homeHits / (float)(homeHits + awayHits);
		_homeSliders[6].value = (homeBSs + awayBSs) == 0 ? 0f : (float)homeBSs / (float)(homeBSs + awayBSs);

		_awaySliders[0].value = (homeGoals + awayGoals) == 0 ? 0f : (float)awayGoals / (float)(homeGoals + awayGoals);
		_awaySliders[1].value = (homeShots + awayShots) == 0 ? 0f : (float)awayShots / (float)(homeShots + awayShots);
		_awaySliders[2].value = (homePPGs + awayPPGs) == 0 ? 0f : (float)awayPPGs / (float)(homePPGs + awayPPGs);
		_awaySliders[3].value = (homeSHGs + awaySHGs) == 0 ? 0f : (float)awaySHGs / (float)(homeSHGs + awaySHGs);
		_awaySliders[4].value = (homeFOWs + awayFOWs) == 0 ? 0f : (float)awayFOWs / (float)(homeFOWs + awayFOWs);
		_awaySliders[5].value = (homeHits + awayHits) == 0 ? 0f : (float)awayHits / (float)(homeHits + awayHits);
		_awaySliders[6].value = (homeBSs + awayBSs) == 0 ? 0f : (float)awayBSs / (float)(homeBSs + awayBSs);
	}
#endregion
}}
