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
using SoM.Core;

namespace SoM.Controllers {
public class UiController : Singleton<UiController> {

#region -------------------- Serialized Variables --------------------
    [Header("Panel Elements")]
    [SerializeField] private UiBottomPanel _bottomPanel;

    [Header("UI Elements")]
    [SerializeField] private List<CanvasGroup> _backgroundElement;
#endregion
#region -------------------- Public Variables --------------------
    public bool IsFadingBannerIn = false;
    public bool IsFadingBannerOut = false;
	public bool IsNewUser = false;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        _bottomPanel.CanvasGroup.alpha = 0f;
        _bottomPanel.gameObject.SetActive(false);
        
        AnimationController.Inst.FadeOutObjects(_backgroundElement, () =>
        {
	        foreach (CanvasGroup obj in _backgroundElement)
	        {
		        obj.gameObject.SetActive(false);
	        }
        });
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        CoreController.Inst.LoadingStepCompleted();
    }

    public void ShowBottomPanel(UiBottomPanelBase panelBase)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the bottom panel.");
        
        _bottomPanel.gameObject.SetActive(true);
        _bottomPanel.InitializePanel(panelBase);
		_bottomPanel.CanvasGroup.alpha = 0f;

		AnimationController.Inst.FadeInPanel(_bottomPanel.CanvasGroup, _bottomPanel.MainPanel);
    }

	public void CloseBottomPanel(Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Closing the bottom panel.");
		
		_bottomPanel.ClosePanel(continueAction);
	}
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
