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
public class SoM_Dropdown : MonoBehavior {

#region -------------------- Serialized Variables --------------------
    [Header("Input Field Elements")]
    [SerializeField] private TMP_Dropdown _dropdown;
#endregion
#region -------------------- Public Variables --------------------
    public TMP_Dropdown Dropdown => _dropdown;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void SetListener(Action changeAction)
    {
        _dropdown.onValueChanged.RemoveAllListeners();

        _dropdown.onValueChanged.AddListener(changeAction);
    }

    public string GetValue()
    {
        return _dropdown.value;
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
