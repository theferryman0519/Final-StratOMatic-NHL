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
public class EditLinePositionPanelPrefab : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Prefab Elements")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _offenseText;
    [SerializeField] private TMP_Text _defenseText;
    [SerializeField] private TMP_Text _fatigueText;
    [SerializeField] private TMP_Text _winPercentText;
    [SerializeField] private TMP_Text _staminaText;

    [SerializeField] private SoM_Button _viewCardButton;
    [SerializeField] private SoM_Button _selectButton;

    [SerializeField] private CanvasGroup _mainElement;
#endregion
#region -------------------- Public Variables --------------------
    public SoM_Button ViewCardButton => _viewCardButton;
    public SoM_Button SelectButton => _selectButton;
#endregion
#region -------------------- Private Variables --------------------

#endregion
#region -------------------- Initial Functions --------------------

#endregion
#region -------------------- Coroutines --------------------

#endregion
#region -------------------- Public Methods --------------------
    public void SetPositionDetails(Skater skater = null, Goalie goalie = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the position details for the prefab.");

        _mainElement.alpha = 1f;

        if (skater != null) { SetSkaterDetails(skater); }
        else if (goalie != null) { SetGoalieDetails(goalie); }

        if (skater == null && goalie == null) { _mainElement.alpha = 0f; }
    }
#endregion
#region -------------------- Private Methods --------------------
    private void SetSkaterDetails(Skater skater)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the skater position details.");

        SetShownDetails(true);

        _nameText.text = $"{skater.Info.FirstName} {skater.Info.LastName}";
        _offenseText.text = $"Offense: {skater.Card.Offense}";
        _defenseText.text = $"Defense: {skater.Card.Defense}";
        _fatigueText.text = $"Fatigue: {skater.Card.Fatigue}";
    }

    private void SetGoalieDetails(Goalie goalie)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the goalie position details.");

        SetShownDetails(false);

        float winPercent = goalie.WinPercentage * 100f;
        int stamina = 100;

        if (GameplayController.Inst.GameData.Type == "Season") { stamina = goalie.Season.Stamina; }
        else if (GameplayController.Inst.GameData.Type == "Playoff") { stamina = goalie.Playoff.Stamina; }

        _nameText.text = $"{goalie.Info.FirstName} {goalie.Info.LastName}";
        _winPercentText.text = $"Win Percentage: {winPercent.ToString("n0")}%";
        _staminaText.text = $"Stamina: {stamina}";
    }

    private void SetShownDetails(bool isSkater)
    {
        _offenseText.gameObject.SetActive(isSkater);
        _defenseText.gameObject.SetActive(isSkater);
        _fatigueText.gameObject.SetActive(isSkater);
        _winPercentText.gameObject.SetActive(!isSkater);
        _staminaText.gameObject.SetActive(!isSkater);

        string selectPosText = isSkater ? "Select Skater" : "Select Goalie";

        _selectButton.SetText(selectPosText);
    }
#endregion
}}
