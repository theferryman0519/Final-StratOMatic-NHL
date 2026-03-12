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

namespace SoM.Ui {
public class SoM_Button : MonoBehavior {

#region -------------------- Serialized Variables --------------------
    [Header("Button Elements")]
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
#endregion
#region -------------------- Public Variables --------------------
    public Button Button => _button;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void SetListener(Action buttonAction)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClickAddListener(() =>
        {
            AnimationController.Inst.ShrinkButton(_button, buttonAction);
        });
    }

    public void RemoveListener()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void SetText(string buttonText)
    {
        _text.text = buttonText;
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
