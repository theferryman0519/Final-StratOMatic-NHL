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
public class GameplayLogsPanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;
	[SerializeField] private SoM_Button _returnButton;

	[Header("Section Elements")]
	[SerializeField] private Transform _container;
	[SerializeField] private GameplayLogPrefab _logPrefab;

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
    public void InitializeGameLogsPanel()
	{
        _mainElement.alpha = 0f;

        _closeButton.SetListener(() => { ClosePanel(); });
		_returnButton.SetListener(() => { ClosePanel(); });

		SetContainer();

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
    private void SetContainer()
	{
		foreach (Transform child in _container)
		{
			Destroy(child.gameObject);
		}
		
		foreach (GameLog log in GameplayController.Inst.GameData.Logs)
		{
			GameplayLogPrefab prefab = Instantiate(_logPrefab, _container);
			prefab.SetTexts(log);
		}
	}
#endregion
}}
