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
public class EditLinesCardPanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;
    [SerializeField] private SoM_Button _selectButton;
    [SerializeField] private SoM_Button _returnButton;

    [Header("Main Elements")]
	[SerializeField] private CanvasGroup _mainElement;
	[SerializeField] private RectTransform _mainPanel;
#endregion
#region -------------------- Public Variables --------------------
    public SoM_Button SelectButton => _selectButton;

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
    public void InitializeEditLinesCardPanel(Skater skater = null, Goalie goalie = null)
	{
        _mainElement.alpha = 0f;

        _closeButton.SetListener(() => { ClosePanel(); });
        _returnButton.SetListener(() => { ClosePanel(); });

        if (skater != null) { SetSkaterDetails(skater); }
        if (goalie != null) { SetGoalieDetails(goalie); }

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
    private void SetSkaterDetails(Skater skater)
    {
        // TODO
    }

    private void SetGoalieDetails(Goalie goalie)
    {
        // TODO
    }
#endregion
}}
