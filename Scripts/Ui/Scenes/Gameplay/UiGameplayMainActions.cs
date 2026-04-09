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
public class UiGameplayMainActions : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _actionText;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private UiGameplayMain mainUi;
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void UpdateActions(UiGameplayMain ui)
    {
        if (ui == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the actions.");

        mainUi = ui;

        UpdateActionText();
    }
#endregion
#region -------------------- Private Methods --------------------
    private void UpdateActionText()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the actions.");

        string actionText = EventsController.Inst.CurrentEventRun.ActionText;

        _actionText.text = actionText;
    }
#endregion
}}
