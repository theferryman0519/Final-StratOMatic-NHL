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
public class GameplayStaminaPrefab : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Prefab Elements")]
    [SerializeField] private Image _staminaImage;
    [SerializeField] private TMP_Text _playerText;

    [Header("Stamina Elements")]
    [SerializeField] private List<Sprite> _staminaSprites = new();
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
    public void SetVisual(string pos, Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the stamina graphic visual.");

        _playerText.text = pos + "\n" + skater.Info.LastName;

        int staminaDiff = (100 - skater.Game.Stamina) / 5;
        
        _staminaImage.sprite = _staminaSprites[staminaDiff];
    }
#endregion
#region -------------------- Private Methods --------------------

#endregion
}}
