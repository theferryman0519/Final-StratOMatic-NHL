// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private EditLinesCardPanel _cardPanel;
#endregion
#region -------------------- Public Variables --------------------
    public string SelectedPosition = string.Empty;

    public Action RefreshAction = null;

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
        _cardPanel.HidePanel();

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
        _cardPanel.HidePanel();
        
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
        
        teamSkaters = teamSkaters.OrderByDescending(s => s.Card.Defense + s.Card.Offense).ToList();
        teamGoalies = teamGoalies.OrderByDescending(g => g.WinPercentage).ToList();

        foreach (Skater skater in GameplayController.Inst.GameData.HomeTeam.SkaterLineup.Values)
        {
            teamSkaters.Remove(skater);
        }

        switch (posOption)
        {
            case 1:
                foreach (Skater defenseSkater in teamSkaters)
                {
                    if (defenseSkater.Info.Position == "D")
                    {
                        EditLinePositionPanelPrefab panelObj = Instantiate(_panelPrefab, _container);
                        panelObj.SetPositionDetails(defenseSkater, null);

                        panelObj.ViewCardButton.SetListener(() =>
                        {
                            _cardPanel.gameObject.SetActive(true);
                            _cardPanel.InitializeEditLinesCardPanel(defenseSkater, null);

                            _cardPanel.SelectButton.SetListener(() => { ClosePanel(() => { SetSkaterPosition(defenseSkater); }); });
                        });

                        panelObj.SelectButton.SetListener(() => { ClosePanel(() => { SetSkaterPosition(defenseSkater); }); });
                    }
                }

                break;
            case 2:
                foreach (Goalie goalie in teamGoalies)
                {
                    EditLinePositionPanelPrefab panelObj = Instantiate(_panelPrefab, _container);
                    panelObj.SetPositionDetails(null, goalie);

                    panelObj.ViewCardButton.SetListener(() =>
                    {
                        _cardPanel.gameObject.SetActive(true);
                        _cardPanel.InitializeEditLinesCardPanel(null, goalie);

                        _cardPanel.SelectButton.SetListener(() => { ClosePanel(() => { SetGoaliePosition(goalie); }); });
                    });

                    panelObj.SelectButton.SetListener(() => { ClosePanel(() => { SetGoaliePosition(goalie); }); });
                }

                break;
            case 0:
            default:
                foreach (Skater forwardSkater in teamSkaters)
                {
                    if (forwardSkater.Info.Position == "F")
                    {
                        EditLinePositionPanelPrefab panelObj = Instantiate(_panelPrefab, _container);
                        panelObj.SetPositionDetails(forwardSkater, null);

                        panelObj.ViewCardButton.SetListener(() =>
                        {
                            _cardPanel.gameObject.SetActive(true);
                            _cardPanel.InitializeEditLinesCardPanel(forwardSkater, null);

                            _cardPanel.SelectButton.SetListener(() => { ClosePanel(() => { SetSkaterPosition(forwardSkater); }); });
                        });

                        panelObj.SelectButton.SetListener(() => { ClosePanel(() => { SetSkaterPosition(forwardSkater); }); });
                    }
                }

                break;
        }
    }

    private void SetSkaterPosition(Skater skater)
    {
        if (!string.IsNullOrEmpty(SelectedPosition))
        {
            GameplayController.Inst.GameData.HomeTeam.SkaterLineup[SelectedPosition] = skater;
            RefreshAction?.Invoke();
        }
    }

    private void SetGoaliePosition(Goalie goalie)
    {
        if (!string.IsNullOrEmpty(SelectedPosition) && SelectedPosition == "G")
        {
            GameplayController.Inst.GameData.HomeTeam.GoalieLineup[SelectedPosition] = goalie;
            RefreshAction?.Invoke();
        }
    }
#endregion
}}
