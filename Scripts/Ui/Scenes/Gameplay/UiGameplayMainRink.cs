// Main Dependencies
using DG.Tweening;
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
public class UiGameplayMainRink : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Rink Elements")]
    [SerializeField] private RectTransform _rink;
    [SerializeField] private RectTransform _puck;
    [SerializeField] private RectTransform _puckFaceoffAnchor;

    [Header("Marker Elements")]
    [SerializeField] private List<GameplayRinkMarkerPrefab> _markerPrefabs = new();
#endregion
#region -------------------- Private Variables --------------------
    private UiGameplayMain mainUi;

    private float rinkShift = 350f;
    private float moveDuration = 0.35f;

    private bool isInitialized = false;

    private Ease moveEase = Ease.OutCubic;

    private Dictionary<string, GameplayRinkMarkerPrefab> markerMap = new();

    private Sequence activeMoveSequence;
#endregion
#region -------------------- Initial Functions --------------------
    private void Start()
    {
        InitializeRink();
    }
#endregion
#region -------------------- Public Methods --------------------
    public void UpdateRink(UiGameplayMain ui)
    {
        if (ui == null) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the rink.");

        mainUi = ui;

        InitializeRink();
        UpdateRinkVisuals();
    }
#endregion
#region -------------------- Private Methods --------------------
    private void InitializeRink()
    {
        if (isInitialized) { return; }

        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the rink.");

        markerMap.Clear();

        foreach (GameplayRinkMarkerPrefab marker in _markerPrefabs)
        {
            if (marker == null || string.IsNullOrWhiteSpace(marker.Key)) { continue; }

            if (!markerMap.ContainsKey(marker.Key))
            {
                string markerKey = marker.Key;

                if (marker.JerseyButton != null)
                {
                    marker.JerseyButton.onClick.RemoveAllListeners();
                    marker.JerseyButton.onClick.AddListener(() => ShowMarkerStats(markerKey));
                }

                markerMap.Add(markerKey, marker);
            }
        }

        isInitialized = true;
    }

    private void UpdateRinkVisuals()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the rink visuals.");

        bool isFaceoff = CheckFaceoffState();
        string rinkPhase = GetRinkPhase(isFaceoff);

        MoveObjects(rinkPhase, isFaceoff);
    }

    private bool CheckFaceoffState()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Checking if there is a faceoff.");

        if (EventsController.Inst == null || EventsController.Inst.CurrentEventRun == null)
        {
            return false;
        }

        string currentAction = EventsController.Inst.CurrentEventRun.ActionText ?? string.Empty;

        List<string> faceoffActions = new()
        {
            "The puck is about to drop",
            "Let's see who wins this faceoff",
            "Welcome, ladies and gentlemen",
            "Both teams are getting ready to drop"
        };

        foreach (string action in faceoffActions)
        {
            if (currentAction.Contains(action))
            {
                return true;
            }
        }

        return false;
    }

    private string GetRinkPhase(bool isFaceoff)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the current rink phase.");

        if (isFaceoff) { return "Center"; }

        if (GameplayController.Inst.GameData.PossTeam == "Home") { return "Right"; }
        if (GameplayController.Inst.GameData.PossTeam == "Away") { return "Left"; }

        return "Center";
    }

    private void MoveObjects(string rinkPhase, bool isFaceoff)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Moving all objects.");

        activeMoveSequence?.Kill();
        activeMoveSequence = DOTween.Sequence();

        Tween rinkTween = MoveRink(rinkPhase);

        if (rinkTween != null)
        {
            activeMoveSequence.Join(rinkTween);
        }

        Tween markersTween = MoveMarkers(rinkPhase, isFaceoff);

        if (markersTween != null)
        {
            activeMoveSequence.Join(markersTween);
        }

        Tween puckTween = MovePuck(rinkPhase, isFaceoff);

        if (puckTween != null)
        {
            activeMoveSequence.Join(puckTween);
        }

        activeMoveSequence.Play();
    }

    private Tween MoveRink(string rinkPhase)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Moving the rink.");

        if (_rink == null) { return null; }

        _rink.DOKill();

        float targetX = 0f;

        switch (rinkPhase)
        {
            case "Left":
                targetX = -rinkShift;
                break;
            case "Right":
                targetX = rinkShift;
                break;
            default:
                targetX = 0f;
                break;
        }

        return _rink.DOAnchorPos(new Vector2(targetX, _rink.anchoredPosition.y), moveDuration).SetEase(moveEase);
    }

    private Tween MoveMarkers(string rinkPhase, bool isFaceoff)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Moving the markers.");

        if (_markerPrefabs == null || _markerPrefabs.Count == 0) { return null; }

        Sequence markerSequence = DOTween.Sequence();

        foreach (GameplayRinkMarkerPrefab marker in _markerPrefabs)
        {
            if (marker == null || marker.Transform == null) { continue; }

            marker.Transform.DOKill();

            Vector2 targetPos = GetMarkerTargetPosition(marker, rinkPhase, isFaceoff);

            markerSequence.Join(marker.Transform.DOAnchorPos(targetPos, moveDuration).SetEase(moveEase));
        }

        return markerSequence;
    }

    private Tween MovePuck(string rinkPhase, bool isFaceoff)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Moving the puck.");

        _puck.DOKill();

        Vector2 targetPos = _puck.anchoredPosition;

        if (isFaceoff)
        {
            if (_puckFaceoffAnchor != null)
            {
                targetPos = _puckFaceoffAnchor.anchoredPosition;
            }
        }
        else
        {
            GameplayRinkMarkerPrefab possMarker = GetPossessionMarker();

            if (possMarker != null)
            {
                Vector2 markerTarget = GetMarkerTargetPosition(possMarker, rinkPhase, false);
                targetPos = markerTarget + possMarker.PuckOffset;
            }
        }

        return _puck.DOAnchorPos(targetPos, moveDuration).SetEase(moveEase);
    }

    private Vector2 GetMarkerTargetPosition(GameplayRinkMarkerPrefab marker, string rinkPhase, bool isFaceoff)
    {
        if (marker == null || marker.Transform == null)
        {
            return Vector2.zero;
        }

        if (isFaceoff && marker.FaceoffAnchor != null)
        {
            return marker.FaceoffAnchor.anchoredPosition;
        }

        return marker.GetNewPosition(rinkPhase, isFaceoff);
    }

    private GameplayRinkMarkerPrefab GetPossessionMarker()
    {
        string markerKey = GetPossessionMarkerKey();

        if (string.IsNullOrWhiteSpace(markerKey)) { return null; }
        if (!markerMap.ContainsKey(markerKey)) { return null; }

        return markerMap[markerKey];
    }

    private string GetPossessionMarkerKey()
    {
        Game gameData = GameplayController.Inst.GameData;
        if (gameData == null) { return string.Empty; }

        if (string.IsNullOrWhiteSpace(gameData.PossTeam) || gameData.PossTeam == "None")
        {
            return string.Empty;
        }

        if (gameData.PossPos == null || gameData.PossPos.Count == 0)
        {
            return string.Empty;
        }

        string possPos = gameData.PossPos[gameData.PossPos.Count - 1];
        string role = NormalizePossessionRole(possPos);

        if (string.IsNullOrWhiteSpace(role))
        {
            return string.Empty;
        }

        return $"{gameData.PossTeam}{role}";
    }

    private string NormalizePossessionRole(string possPos)
    {
        if (string.IsNullOrWhiteSpace(possPos)) { return string.Empty; }

        string upper = possPos.ToUpperInvariant();

        if (upper.StartsWith("LW")) { return "LW"; }
        if (upper.StartsWith("RW")) { return "RW"; }
        if (upper.StartsWith("LD")) { return "LD"; }
        if (upper.StartsWith("RD")) { return "RD"; }
        if (upper.StartsWith("C")) { return "C"; }
        if (upper.StartsWith("G")) { return "G"; }

        return string.Empty;
    }

    private void ShowMarkerStats(string markerKey)
    {
        GameTeam team = GameplayController.Inst.GameData.HomeTeam;

        if (markerKey.Contains("Away")) { team = GameplayController.Inst.GameData.AwayTeam; }

        int line = team.CurrentLine;
        int pair = team.CurrentPair;

        string posString = "G";

        if (markerKey.Contains("C")) { posString = "C"; }
        else if (markerKey.Contains("LW")) { posString = "LW"; }
        else if (markerKey.Contains("RW")) { posString = "RW"; }
        else if (markerKey.Contains("LD")) { posString = "LD"; }
        else if (markerKey.Contains("RD")) { posString = "RD"; }

        if (posString == "G")
        {
            Goalie goalie = team.GoalieLineup["G"];

            mainUi.ShowGoalieStatsPanel(goalie);
        }

        else
        {
            string fullPos = $"{posString}{line}";

            if (posString.Contains("D")) { fullPos = $"{posString}{pair}"; }

            Skater skater = team.SkaterLineup[fullPos];

            mainUi.ShowSkaterStatsPanel(skater);
        }
    }
#endregion
}}
