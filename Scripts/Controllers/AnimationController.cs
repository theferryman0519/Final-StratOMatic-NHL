// Main Dependencies
using DG.Tweening;
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
public class AnimationController : Singleton<AnimationController> {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        CoreController.Inst.LoadingStepCompleted();
    }

    public void SetObjectsAlpha(List<CanvasGroup> elements, float alpha, Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the alpha for the objects.");
        
        foreach (CanvasGroup element in elements)
        {
            element.alpha = alpha;
        }

        continueAction?.Invoke();
    }

    public void FadeInObjects(List<CanvasGroup> elements, Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Fading objects in.");

        FadeObjects(elements, true, continueAction);
    }

    public void FadeOutObjects(List<CanvasGroup> elements, Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Fading objects out.");
        
        FadeObjects(elements, false, continueAction);
    }

    public void ShrinkButton(Button mainButton, Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Shrinking the button.");

        mainButton.interactable = false;

        ShrinkingButton(mainButton, continueAction);
    }
#endregion
#region -------------------- Private Methods --------------------
    private void FadeObjects(List<CanvasGroup> elements, bool isFadingIn, Action continueAction)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Fading objects.");
        
        Sequence seq = DOTween.Sequence();

        float initialAlpha = (isFadingIn) ? 0f : 1f;
        float finalAlpha = (isFadingIn) ? 1f : 0f;

        foreach (CanvasGroup element in elements)
        {
            element.alpha = initialAlpha;
            seq.Join(element.DOFade(finalAlpha, ConstantController.Fading_Multiplier));
        }

        seq.OnComplete(() =>
        {
            continueAction?.Invoke();
        });

        seq.Play();
    }

    private void ShrinkingButton(Button mainButton, Action continueAction)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Shrinking the main button.");
        
        Sequence seq = DOTween.Sequence();
        
        RectTransform rectTransform = mainButton.gameObject.GetComponent<RectTransform>();

        Vector3 initialSize = rectTransform.localScale;
        
        seq.Join(rectTransform.DOScale(initialSize * 0.85f, ConstantController.Shrinking_Multiplier).SetEase(Ease.OutQuad));
        seq.Append(rectTransform.DOScale(initialSize, ConstantController.Shrinking_Multiplier / 2f).SetEase(Ease.OutQuad));

        seq.OnComplete(() =>
        {
            continueAction?.Invoke();
            mainButton.interactable = true;
        });

        seq.Play();
    }
#endregion
}}
