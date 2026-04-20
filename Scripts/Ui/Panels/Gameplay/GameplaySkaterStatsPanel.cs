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
public class GameplaySkaterStatsPanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;
	[SerializeField] private SoM_Button _returnButton;

	[Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _statsDropdown;
	[SerializeField] private SoM_Dropdown _cardDropdown;

	[Header("Page Elements")]
	[SerializeField] private GameObject _infoPage;
	[SerializeField] private GameObject _cardPage;
	[SerializeField] private GameObject _cardOutsideShotsPage;
	[SerializeField] private GameObject _cardInsideShotsPage;
	[SerializeField] private GameObject _cardReboundShotsPage;
	[SerializeField] private GameObject _cardPassingPage;
	[SerializeField] private GameObject _cardDefendingPage;
	
	[SerializeField] private TMP_Text _titleText;

	[SerializeField] private List<TMP_Text> _infoTexts = new();
	[SerializeField] private List<TMP_Text> _cardTexts = new();

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
    public void InitializeSkaterStatsPanel(Skater skater)
	{
		if (skater == null) { return; }

        _mainElement.alpha = 0f;

        _closeButton.SetListener(() => { ClosePanel(); });
		_returnButton.SetListener(() => { ClosePanel(); });

		_statsDropdown.SetListener(ChangeStatsOption);
		_statsDropdown.Dropdown.value = 0;
		
		_cardDropdown.SetListener(ChangeCardsOption);
		_cardDropdown.Dropdown.value = 0;

		ChangeStatsOption(0);
		ChangeCardsOption(0);

		SetTexts(skater);

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
    private void ChangeStatsOption(int option)
    {
		switch (option)
		{
			case 1:
				_infoPage.SetActive(false);
				_cardPage.SetActive(true);
				break;
			case 0:
			default:
				_infoPage.SetActive(true);
				_cardPage.SetActive(false);
				break;
		}
    }

	private void ChangeCardsOption(int option)
    {
		switch (option)
		{
			case 1:
				_cardOutsideShotsPage.SetActive(false);
				_cardInsideShotsPage.SetActive(true);
				_cardReboundShotsPage.SetActive(false);
				_cardPassingPage.SetActive(false);
				_cardDefendingPage.SetActive(false);
				break;
			case 2:
				_cardOutsideShotsPage.SetActive(false);
				_cardInsideShotsPage.SetActive(false);
				_cardReboundShotsPage.SetActive(true);
				_cardPassingPage.SetActive(false);
				_cardDefendingPage.SetActive(false);
				break;
			case 3:
				_cardOutsideShotsPage.SetActive(false);
				_cardInsideShotsPage.SetActive(false);
				_cardReboundShotsPage.SetActive(false);
				_cardPassingPage.SetActive(true);
				_cardDefendingPage.SetActive(false);
				break;
			case 4:
				_cardOutsideShotsPage.SetActive(false);
				_cardInsideShotsPage.SetActive(false);
				_cardReboundShotsPage.SetActive(false);
				_cardPassingPage.SetActive(false);
				_cardDefendingPage.SetActive(true);
				break;
			case 0:
			default:
				_cardOutsideShotsPage.SetActive(true);
				_cardInsideShotsPage.SetActive(false);
				_cardReboundShotsPage.SetActive(false);
				_cardPassingPage.SetActive(false);
				_cardDefendingPage.SetActive(false);
				break;
		}
    }

	private void SetTexts(Skater skater)
	{
		_titleText.text = $"{skater.Info.FirstName} {skater.Info.LastName}";
		
		_infoTexts[0].text = $"Offense: {skater.Card.Offense}" + "\n" +
			$"Defense: {skater.Card.Defense}" + "\n" +
			$"Breakaway: {skater.Card.Breakaway}" + "\n" +
			$"Penalty: {skater.Card.Penalty}";
		
		_infoTexts[1].text = $"Intimidation: {skater.Card.Intimidation}" + "\n" +
			$"Passing: {skater.Card.Passing}" + "\n" +
			$"Faceoff: +{skater.Card.Faceoff}" + "\n" +
			$"Fatigue: {skater.Card.Fatigue}";
		
		_infoTexts[2].text = $"Goals: {skater.Game.Goals}" + "\n" +
			$"Assists: {skater.Game.Assists}" + "\n" +
			$"Points: {skater.Game.Points}" + "\n" +
			$"Shots: {skater.Game.Shots}" + "\n" +
			$"+/-: {skater.Game.PlusMinus}" + "\n" +
			$"Stamina: {skater.Game.Stamina}%";
		
		_infoTexts[3].text = $"PIM: {skater.Game.PenaltyMinutes}" + "\n" +
			$"Hits: {skater.Game.Hits}" + "\n" +
			$"Blocked Shots: {skater.Game.BlockedShots}" + "\n" +
			$"Giveaways: {skater.Game.Giveaways}" + "\n" +
			$"Takeaways: {skater.Game.Takeaways}" + "\n" +
			$"Faceoffs: {skater.Game.FaceoffsWon} for {skater.Game.FaceoffsWon + skater.Game.FaceoffsLost}";
		
		_cardTexts[0].text = $"02) {skater.Card.OutsideShotActions[0]}" + "\n" +
			$"03) {skater.Card.OutsideShotActions[1]}" + "\n" +
			$"04) {skater.Card.OutsideShotActions[2]}" + "\n" +
			$"05) {skater.Card.OutsideShotActions[3]}" + "\n" +
			$"06) {skater.Card.OutsideShotActions[4]}" + "\n" +
			$"07) {skater.Card.OutsideShotActions[5]}" + "\n" +
			$"08) {skater.Card.OutsideShotActions[6]}" + "\n" +
			$"09) {skater.Card.OutsideShotActions[7]}" + "\n" +
			$"10) {skater.Card.OutsideShotActions[8]}" + "\n" +
			$"11) {skater.Card.OutsideShotActions[9]}" + "\n" +
			$"12) {skater.Card.OutsideShotActions[10]}";
		
		_cardTexts[1].text = $"02) {skater.Card.InsideShotActions[0]}" + "\n" +
			$"03) {skater.Card.InsideShotActions[1]}" + "\n" +
			$"04) {skater.Card.InsideShotActions[2]}" + "\n" +
			$"05) {skater.Card.InsideShotActions[3]}" + "\n" +
			$"06) {skater.Card.InsideShotActions[4]}" + "\n" +
			$"07) {skater.Card.InsideShotActions[5]}" + "\n" +
			$"08) {skater.Card.InsideShotActions[6]}" + "\n" +
			$"09) {skater.Card.InsideShotActions[7]}" + "\n" +
			$"10) {skater.Card.InsideShotActions[8]}" + "\n" +
			$"11) {skater.Card.InsideShotActions[9]}" + "\n" +
			$"12) {skater.Card.InsideShotActions[10]}";
		
		_cardTexts[2].text = $"02) {skater.Card.ReboundShotActions[0]}" + "\n" +
			$"03) {skater.Card.ReboundShotActions[1]}" + "\n" +
			$"04) {skater.Card.ReboundShotActions[2]}" + "\n" +
			$"05) {skater.Card.ReboundShotActions[3]}" + "\n" +
			$"06) {skater.Card.ReboundShotActions[4]}" + "\n" +
			$"07) {skater.Card.ReboundShotActions[5]}" + "\n" +
			$"08) {skater.Card.ReboundShotActions[6]}" + "\n" +
			$"09) {skater.Card.ReboundShotActions[7]}" + "\n" +
			$"10) {skater.Card.ReboundShotActions[8]}" + "\n" +
			$"11) {skater.Card.ReboundShotActions[9]}" + "\n" +
			$"12) {skater.Card.ReboundShotActions[10]}";
		
		_cardTexts[3].text = $"A) {skater.Card.PassingActions[0]}" + "\n" +
			$"B) {skater.Card.PassingActions[1]}" + "\n" +
			$"C) {skater.Card.PassingActions[2]}" + "\n" +
			$"D) {skater.Card.PassingActions[3]}" + "\n" +
			$"E) {skater.Card.PassingActions[4]}" + "\n" +
			$"F) {skater.Card.PassingActions[5]}" + "\n" +
			$"G) {skater.Card.PassingActions[6]}" + "\n" +
			$"H) {skater.Card.PassingActions[7]}" + "\n" +
			$"I) {skater.Card.PassingActions[8]}" + "\n" +
			$"J) {skater.Card.PassingActions[9]}" + "\n" +
			$"K) {skater.Card.PassingActions[10]}" + "\n" +
			$"L) {skater.Card.PassingActions[11]}";
		
		_cardTexts[4].text = $"01) {skater.Card.DefendingActions[0]}" + "\n" +
			$"02) {skater.Card.DefendingActions[1]}" + "\n" +
			$"03) {skater.Card.DefendingActions[2]}" + "\n" +
			$"04) {skater.Card.DefendingActions[3]}" + "\n" +
			$"05) {skater.Card.DefendingActions[4]}" + "\n" +
			$"06) {skater.Card.DefendingActions[5]}" + "\n" +
			$"07) {skater.Card.DefendingActions[6]}" + "\n" +
			$"08) {skater.Card.DefendingActions[7]}" + "\n" +
			$"09) {skater.Card.DefendingActions[8]}" + "\n" +
			$"10) {skater.Card.DefendingActions[9]}" + "\n" +
			$"11) {skater.Card.DefendingActions[10]}" + "\n" +
			$"12) {skater.Card.DefendingActions[11]}" + "\n" +
			$"13) {skater.Card.DefendingActions[12]}" + "\n" +
			$"14) {skater.Card.DefendingActions[13]}";
	}
#endregion
}}
