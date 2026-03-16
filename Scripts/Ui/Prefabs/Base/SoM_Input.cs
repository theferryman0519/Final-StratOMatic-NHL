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
public class SoM_Input : MonoBehavior {

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
    public void SetListeners(Action selectAction, Action deselectAction, Action endEditAction)
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
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
