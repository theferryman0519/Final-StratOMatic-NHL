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
public class EditLinePositionPrefab : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Prefab Elements")]
    [SerializeField] private TMP_Text _nameText;

    [SerializeField] private SoM_Button _selectButton;
    [SerializeField] private SoM_Button _removeButton;
#endregion
#region -------------------- Public Variables --------------------
    public SoM_Button SelectButton => _selectButton;
    public SoM_Button RemoveButton => _removeButton;

    public string ThisFullPos = string.Empty;

    public Skater ThisSkater = null;
    public Goalie ThisGoalie = null;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------

#endregion
#region -------------------- Coroutines --------------------

#endregion
#region -------------------- Public Methods --------------------
    public void SetPosition(string pos, bool isFilled, Skater skater = null, Goalie goalie = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the position for the prefab.");

        _selectButton.gameObject.SetActive(!isFilled);
        _removeButton.gameObject.SetActive(isFilled);
        _nameText.gameObject.SetActive(isFilled);

        if (skater != null)
        {
            ThisSkater = skater;

            _nameText.text = $"{pos}: {skater.Info.FirstName} {skater.Info.LastName}";
        }

        if (goalie != null)
        {
            ThisGoalie = goalie;

            _nameText.text = $"G: {goalie.Info.FirstName} {goalie.Info.LastName}";
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
