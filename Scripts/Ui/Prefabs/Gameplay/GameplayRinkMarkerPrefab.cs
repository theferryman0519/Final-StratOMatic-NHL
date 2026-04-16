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
public class GameplayRinkMarkerPrefab : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Marker Elements")]
    [SerializeField] private Image _jerseyImage;
    [SerializeField] private Button _jerseyButton;

    [SerializeField] private RectTransform _transform;
    [SerializeField] private RectTransform _leftAnchor;
    [SerializeField] private RectTransform _rightAnchor;
    [SerializeField] private RectTransform _centerAnchor;
    [SerializeField] private RectTransform _faceoffAnchor;
#endregion
#region -------------------- Public Variables --------------------
    public string Key = string.Empty;

    public RectTransform Transform => _transform;
    public RectTransform LeftAnchor => _leftAnchor;
    public RectTransform RightAnchor => _rightAnchor;
    public RectTransform CenterAnchor => _centerAnchor;
    public RectTransform FaceoffAnchor => _faceoffAnchor;

    public Button JerseyButton => _jerseyButton;
    
    public Vector2 PuckOffset => puckOffset;
#endregion
#region -------------------- Private Variables --------------------
    private Vector2 motionAmplitude = new(10f, 6f);
    private Vector2 puckOffset = new(18f, -10f);

    private float motionSpeed = 1f;
    private float motionPhase = 0f;
#endregion
#region -------------------- Initial Functions --------------------

#endregion
#region -------------------- Coroutines --------------------

#endregion
#region -------------------- Public Methods --------------------
    public Vector2 GetNewPosition(string rinkPhase, bool isFaceoff)
    {
        RectTransform targetAnchor = null;

        switch (rinkPhase)
        {
            case "Left":
                targetAnchor = _leftAnchor;
                break;
            case "Right":
                targetAnchor = _rightAnchor;
                break;
            default:
                targetAnchor = _centerAnchor;
                break;
        }

        float speed = Mathf.Max(0.01f, motionSpeed);
        float phase = motionPhase;
        float time = Time.time;

        float xOffset = Mathf.Sin((time * speed) + phase) * motionAmplitude.x;
        float yOffset = Mathf.Cos((time * speed * 0.85f) + phase) * motionAmplitude.y;

        Vector2 targetPos = targetAnchor != null ? targetAnchor.anchoredPosition : _transform.anchoredPosition;

        if (!isFaceoff)
        {
            targetPos += new Vector2(xOffset, yOffset);
        }

        return targetPos;
    }
#endregion
#region -------------------- Private Methods --------------------

#endregion
}}
