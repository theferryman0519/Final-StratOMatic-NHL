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
public class GameplayLogPrefab : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Prefab Elements")]
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _actionText;
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
    public void SetTexts(GameLog log)
    {
        string periodText = string.Empty;

        if (log.Period == 1 || log.Period == 0) { periodText = "1st Period"; }
        else if (log.Period == 2) { periodText = "2nd Period"; }
        else if (log.Period == 3) { periodText = "3rd Period"; }
        else { periodText = "Overtime"; }

        _timeText.text = $"{periodText}: {log.GameTime}";
        _actionText.text = log.Action;
    }
#endregion
#region -------------------- Private Methods --------------------

#endregion
}}
