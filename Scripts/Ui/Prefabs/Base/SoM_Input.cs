// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Game Dependencies
using SoM.Controllers;

namespace SoM.Ui {
public class SoM_Input : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Input Field Elements")]
    [SerializeField] private TMP_InputField _inputField;
#endregion
#region -------------------- Public Variables --------------------
    public TMP_InputField Input => _inputField;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void SetListeners(UnityAction<string> selectAction, UnityAction<string> deselectAction, UnityAction<string> endEditAction)
    {
        _inputField.onSelect.RemoveAllListeners();
        _inputField.onDeselect.RemoveAllListeners();
        _inputField.onEndEdit.RemoveAllListeners();

        _inputField.onSelect.AddListener(selectAction);
        _inputField.onDeselect.AddListener(deselectAction);
        _inputField.onEndEdit.AddListener(endEditAction);
    }

    public string GetInput()
    {
        return _inputField.text;
    }

    public void SetInput(string input)
    {
        _inputField.text = input;
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
