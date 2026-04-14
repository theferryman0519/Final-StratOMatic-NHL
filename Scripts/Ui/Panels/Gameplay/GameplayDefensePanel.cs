// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Game Dependencies
using SoM.Controllers;
using SoM.Models;

namespace SoM.Ui {
public class GameplayDefensePanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;
	[SerializeField] private SoM_Button _cancelButton;

	[Header("Section Elements")]
	[SerializeField] private List<GameObject> _sectionObjects = new();
	[SerializeField] private List<TMP_Text> _sectionTexts = new();
	[SerializeField] private List<SoM_Button> _sectionButtons = new();

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
    public void InitializeDefensePanel()
	{
        _mainElement.alpha = 0f;

        _closeButton.SetListener(() => { ClosePanel(); });
		_cancelButton.SetListener(() => { ClosePanel(); });

		SetSections();

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
    private void SetSections()
	{
		int currentPair = GameplayController.Inst.GameData.HomeTeam.CurrentPair;

		Dictionary<string, Skater> skaters = new(GameplayController.Inst.GameData.HomeTeam.SkaterLineup);

		for (int i = 0; i < 3; i++)
		{
			int index = i;

			_sectionTexts[index].text = $"{skaters[$"LD{index + 1}"].Info.LastName} - Fatigue: {skaters[$"LD{index + 1}"].Game.Stamina}%" + "\n" +
				$"{skaters[$"RD{index + 1}"].Info.LastName} - Fatigue: {skaters[$"RD{index + 1}"].Game.Stamina}%";
			
			_sectionObjects[index].SetActive((index + 1) != currentPair);
			_sectionButtons[index].SetListener(() =>
			{
				GameplayController.Inst.GameData.HomeTeam.NextPair = index + 1;
				ClosePanel();
			});
		}
	}
#endregion
}}
