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
public class EditLinesPanel : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Content Elements")]
    [SerializeField] private Transform _container;
    [SerializeField] private EditLinePositionPanelPrefab _panelPrefab;

    [Header("Button Elements")]
	[SerializeField] private SoM_Button _closeButton;

    [Header("Main Elements")]
	[SerializeField] private CanvasGroup _mainElement;
	[SerializeField] private RectTransform _mainPanel;
#endregion
#region -------------------- Public Variables --------------------
    public CanvasGroup CanvasGroup => _mainElement;
    
    public RectTransform MainPanel => _mainPanel;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeEditLinesPanel(int posOption)
	{
        _mainElement.alpha = 0f;

        _closeButton.SetListener(() => { ClosePanel(); });

        SetContainer(posOption);

        AnimationController.Inst.FadeInPanel(_mainElement, _mainPanel, () =>
        {
            _mainElement.alpha = 1f;
        });
	}

    public void ClosePanel(Action continueAction = null)
	{
		AnimationController.Inst.FadeOutPanel(_mainElement, _mainPanel, () =>
		{
			HidePanel();
			continueAction?.Invoke();
		});
	}

    public void HidePanel()
    {
        _mainElement.alpha = 0f;
        this.gameObject.SetActive(false);
    }
#endregion
#region -------------------- Private Methods --------------------
    private void SetContainer(int posOption)
    {
        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }

        // TODO: Adjust "HomeTeam" if multiplayer
        string teamCode = GameplayController.Inst.GameData.HomeTeam.Team.Code;
		string teamLeagueString = GameplayController.Inst.GameData.HomeTeam.Team.League;

        List<Skater> teamSkaters = new();
        List<Goalie> teamGoalies = new();

        if (teamLeagueString == "NHL")
        {
            teamSkaters = new(SkatersController.Inst.NhlSkaters[teamCode]);
            teamGoalies = new(GoaliesController.Inst.NhlGoalies[teamCode]);
        }

        else if (teamLeagueString == "NHLFranchise")
        {
            teamSkaters = new(SkatersController.Inst.NhlFranchiseSkaters[teamCode]);
            teamGoalies = new(GoaliesController.Inst.NhlFranchiseGoalies[teamCode]);
        }

        else if (teamLeagueString == "PWHL")
        {
            teamSkaters = new(SkatersController.Inst.PwhlSkaters[teamCode]);
            teamGoalies = new(GoaliesController.Inst.PwhlGoalies[teamCode]);
        }

        else if (teamLeagueString == "PWHLFranchise")
        {
            teamSkaters = new(SkatersController.Inst.PwhlFranchiseSkaters[teamCode]);
            teamGoalies = new(GoaliesController.Inst.PwhlFranchiseGoalies[teamCode]);
        }

        switch (posOption)
        {
            case 1:
                foreach (Skater defenseSkater in teamSkaters)
                {
                    if (defenseSkater.Info.Position == "D")
                    {
                        // TODO: Instantiate _panelPrefab
                    }
                }

                break;
            case 2:
                foreach (Goalie goalie in teamGoalies)
                {
                    // TODO: Instantiate _panelPrefab
                }

                break;
            case 0:
            default:
                foreach (Skater forwardSkater in teamSkaters)
                {
                    if (forwardSkater.Info.Position == "F")
                    {
                        // TODO: Instantiate _panelPrefab
                    }
                }

                break;
        }
    }
#endregion
}}
