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
public class GameplayMenuPanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;
	[SerializeField] private SoM_Button _resumeButton;
	[SerializeField] private SoM_Button _logsButton;
	[SerializeField] private SoM_Button _quitButton;

    [Header("Main Elements")]
	[SerializeField] private CanvasGroup _mainElement;
	[SerializeField] private RectTransform _mainPanel;
#endregion
#region -------------------- Public Variables --------------------
    public CanvasGroup CanvasGroup => _mainElement;
    
    public RectTransform MainPanel => _mainPanel;
#endregion
#region -------------------- Private Variables --------------------
    private UiGameplayMain mainUi;
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeMenuPanel(UiGameplayMain ui)
	{
		if (ui == null) { return; }

		CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the menu panel.");

		mainUi = ui;

        _mainElement.alpha = 0f;

        _closeButton.SetListener(() => { ClosePanel(); });
		_resumeButton.SetListener(() => { ClosePanel(); });
		_logsButton.SetListener(ShowLogsPanel);
		_quitButton.SetListener(QuitGame);

        AnimationController.Inst.FadeInPanel(_mainElement, _mainPanel, () =>
        {
            _mainElement.alpha = 1f;
        });
	}

    public void ClosePanel(Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Closing the menu panel.");

		AnimationController.Inst.FadeOutPanel(_mainElement, _mainPanel, () =>
		{
			_mainElement.alpha = 0f;
			this.gameObject.SetActive(false);

			continueAction?.Invoke();
		});
	}

	public void ShowLogsPanel()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the logs panel.");
		
		mainUi.ShowLogsPanel();
	}

	public void QuitGame()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Quitting the game.");
		
		// TODO
		// Save the game data
		// Go back to the home screen
	}
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
