// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// Game Dependencies
using SoM.Controllers;
using SoM.Models;

namespace SoM.Ui {
public class GameplayRinkMarkerPrefab : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Marker Elements")]
    [SerializeField] private Image _jerseyImage;
    [SerializeField] private Button _jerseyButton;

    [SerializeField] private RectTransform _transform;
    [SerializeField] private RectTransform _leftSegment;
    [SerializeField] private RectTransform _rightSegment;
    [SerializeField] private RectTransform _centerSegment;
    [SerializeField] private RectTransform _homeGoalSegment;
    [SerializeField] private RectTransform _awayGoalSegment;
    [SerializeField] private RectTransform _faceoffAnchor;
#endregion
#region -------------------- Public Variables --------------------
    public string Key = string.Empty;

    public RectTransform Transform => _transform;

    public Button JerseyButton => _jerseyButton;
    
    public Vector2 PuckOffset => puckOffset;
#endregion
#region -------------------- Private Variables --------------------
    private Vector2 puckOffset = new(18f, -10f);

    private float motionSpeed = 1f;
    private float motionPhase = 0f;
    private float paddingX = 20f;
    private float paddingY = 20f;
#endregion
#region -------------------- Initial Functions --------------------

#endregion
#region -------------------- Coroutines --------------------

#endregion
#region -------------------- Public Methods --------------------
    public void SetJerseyImage(GameTeam gameTeam, bool isHomeTeam)
    {
        string spriteLeague = gameTeam.Team.League.Contains("NHL") ? "NHL" : "PWHL";
        string spriteString = isHomeTeam ? $"{spriteLeague}_{gameTeam.Team.Code}_HOME" : $"{spriteLeague}_{gameTeam.Team.Code}_AWAY";
        
        _jerseyImage.sprite = ConstantController.Inst.MarkerSprites[spriteString];
    }

    public Vector2 GetNewPosition(string rinkPhase, bool isFaceoff)
    {
        if (Key == "AwayG")
        {
            return GetRandomPointInSegment(_awayGoalSegment);
        }
        
        if (Key == "HomeG")
        {
            return GetRandomPointInSegment(_homeGoalSegment);
        }
        
        if (isFaceoff && _faceoffAnchor != null)
        {
            return GetRandomPointInSegment(_faceoffAnchor);
        }

        RectTransform targetSegment = GetTargetSegment(rinkPhase);

        if (targetSegment == null)
        {
            return _transform.anchoredPosition;
        }

        return GetRandomPointInSegment(targetSegment);
    }
#endregion
#region -------------------- Private Methods --------------------
    private RectTransform GetTargetSegment(string rinkPhase)
    {
        switch (rinkPhase)
        {
            case "Left":
                return _leftSegment;
            case "Right":
                return _rightSegment;
            default:
                return _centerSegment;
        }
    }

    private Vector2 GetRandomPointInSegment(RectTransform segment)
    {
        Rect rect = segment.rect;

        float minX = rect.xMin + paddingX;
        float maxX = rect.xMax - paddingX;
        float minY = rect.yMin + paddingY;
        float maxY = rect.yMax - paddingY;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Vector3 worldPoint = segment.TransformPoint(new Vector3(randomX, randomY, 0f));
        Vector2 localPoint = _transform.parent.InverseTransformPoint(worldPoint);

        return localPoint;
    }
#endregion
}}
