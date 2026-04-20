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
public class GameplayGoalieStatsPanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;
	[SerializeField] private SoM_Button _returnButton;

	[Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _statsDropdown;

	[Header("Page Elements")]
	[SerializeField] private GameObject _infoPage;
	[SerializeField] private GameObject _cardPage;

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
    public void InitializeGoalieStatsPanel(Goalie goalie)
	{
		if (goalie == null) { return; }

        _mainElement.alpha = 0f;

        _closeButton.SetListener(() => { ClosePanel(); });
		_returnButton.SetListener(() => { ClosePanel(); });

		_statsDropdown.SetListener(ChangeStatsOption);
		_statsDropdown.Dropdown.value = 0;

		ChangeStatsOption(0);

		SetTexts(goalie);

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

	private void SetTexts(Goalie goalie)
	{
		_titleText.text = $"{goalie.Info.FirstName} {goalie.Info.LastName}";
		
		_infoTexts[0].text = $"Penalty: {goalie.Card.Penalty}";
		_infoTexts[1].text = $"Fatigue: {goalie.Card.Fatigue}";
		
		_infoTexts[2].text = $"Goals Against: {goalie.Game.GoalsAgainst}" + "\n" +
			$"Shots Against: {goalie.Game.ShotsAgainst}";
		
		_infoTexts[3].text = $"Assists: {goalie.Game.Assists}" + "\n" +
			$"PIM: {goalie.Game.PenaltyMinutes}";
		
		_cardTexts[0].text = $"02) {goalie.Card.GoalieRatingActions[0]}" + "\n" +
			$"03) {goalie.Card.GoalieRatingActions[1]}" + "\n" +
			$"04) {goalie.Card.GoalieRatingActions[2]}" + "\n" +
			$"05) {goalie.Card.GoalieRatingActions[3]}" + "\n" +
			$"06) {goalie.Card.GoalieRatingActions[4]}" + "\n" +
			$"07) {goalie.Card.GoalieRatingActions[5]}" + "\n" +
			$"08) {goalie.Card.GoalieRatingActions[6]}" + "\n" +
			$"09) {goalie.Card.GoalieRatingActions[7]}" + "\n" +
			$"10) {goalie.Card.GoalieRatingActions[8]}" + "\n" +
			$"11) {goalie.Card.GoalieRatingActions[9]}" + "\n" +
			$"12) {goalie.Card.GoalieRatingActions[10]}";
	}
#endregion
}}
