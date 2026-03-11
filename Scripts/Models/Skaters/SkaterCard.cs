// Main Dependencies
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Game Dependencies

namespace SoM.Models {
[System.Serializable]
public class SkaterCard {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    public string Penalty { get; set; }
    public string Intimidation { get; set; }
    public string Passing { get; set; }
    public string Fatigue { get; set; }

    public int Offense { get; set; }
    public int Defense { get; set; }
    public int Breakaway { get; set; }
    public int Faceoff { get; set; }

    public List<string> OutsideShotActions = new();
    public List<string> InsideShotActions = new();
    public List<string> ReboundShotActions = new();
    public List<string> PassingActions = new();
    public List<string> DefendingActions = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
