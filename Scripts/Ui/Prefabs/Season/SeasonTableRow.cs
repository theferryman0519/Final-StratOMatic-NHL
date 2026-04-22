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
public class SeasonTableTow : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    [Header("Text Elements")]
    [SerializeField] private TMP_Text _columnAText;
    [SerializeField] private TMP_Text _columnBText;
    [SerializeField] private TMP_Text _columnCText;
    [SerializeField] private TMP_Text _columnDText;
    [SerializeField] private TMP_Text _columnEText;
    [SerializeField] private TMP_Text _columnFText;

    [Header("UI Elements")]
    [SerializeField] private Image _background;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private Color colorA;
    private Color colorB;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        colorA = new Color(1f, 1f, 1f, 1f); // #ffffff
        colorB = new Color(0.925f, 0.925f, 0.925f, 1f); // #ececec
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void SetColumnA(string info)
    {
        _columnAText.text = info;
    }

    public void SetColumnB(string info)
    {
        _columnBText.text = info;
    }

    public void SetColumnC(string info)
    {
        _columnCText.text = info;
    }

    public void SetColumnD(string info)
    {
        _columnDText.text = info;
    }

    public void SetColumnE(string info)
    {
        _columnEText.text = info;
    }

    public void SetColumnF(string info)
    {
        _columnFText.text = info;
    }

    public void Setbackground(bool isAlt)
    {
        _background.color = isAlt ? colorB : colorA;
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
