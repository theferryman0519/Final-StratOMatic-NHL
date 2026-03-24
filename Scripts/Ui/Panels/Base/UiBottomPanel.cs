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
public class UiBottomPanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("UI Elements")]
	[SerializeField] private TMP_Text _titleText;
	[SerializeField] private TMP_Text _bodyText;

    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;
	[SerializeField] private SoM_Button _buttonA;
	[SerializeField] private SoM_Button _buttonB;

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
    public void InitializePanel(BottomPanel panelData)
	{
		_titleText.text = panelData.Title;
		_bodyText.text = panelData.Body;

        _closeButton.SetListener(ClosePanel);
        _buttonA.SetListener(() => { panelData.ActionA?.Invoke(); });
        _buttonB.SetListener(() => { panelData.ActionB?.Invoke(); });

        _buttonA.SetText(panelData.ButtonA);
        _buttonB.SetText(panelData.ButtonB);

        _buttonA.SetSprite(panelData.SpriteA);
        _buttonB.SetSprite(panelData.SpriteB);

        _closeButton.gameObject.SetActive(panelData.HasCloseButton);
        _buttonA.gameObject.SetActive(panelData.ButtonCount > 0);
        _buttonB.gameObject.SetActive(panelData.ButtonCount > 1);
	}

    public void ClosePanel(Action continueAction = null)
	{
		AnimationController.Inst.FadeOutPanel(CanvasGroup, MainPanel, () =>
		{
			CanvasGroup.alpha = 0f;
			this.gameObject.SetActive(false);

			continueAction?.Invoke();
		});
	}
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
