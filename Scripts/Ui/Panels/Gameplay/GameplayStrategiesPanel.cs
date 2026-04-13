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
public class GameplayStrategiesPanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;
	[SerializeField] private SoM_Button _cancelButton;

	[Header("Section Elements")]
	[SerializeField] private List<GameObject> _sectionObjects = new();
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
    public void InitializeStrategiesPanel()
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
		int currentStrategy = GameplayController.Inst.GameData.HomeTeam.CurrentStrategy;

		for (int i = 0; i < 5; i++)
		{
			int index = i;
			
			_sectionObjects[index].SetActive((index + 1) != currentStrategy);
			_sectionButtons[index].SetListener(() =>
			{
				GameplayController.Inst.GameData.HomeTeam.NextStrategy = index + 1;
				ClosePanel();
			});
		}
	}
#endregion
}}
