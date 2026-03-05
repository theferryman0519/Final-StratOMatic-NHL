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

namespace SoM.Skaters {
[System.Serializable]
public class SkaterCardData {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    public string Intimidation { get; set; }
    public string Passing { get; set; }
    public string Penalty { get; set; }
    public string Fatigue { get; set; }

    public string Offense { get; set; }
    public string Defense { get; set; }
    public string Breakaway { get; set; }
    public string Faceoff { get; set; }

    public List<string> OutsideShotActions { get; set; } = new();
    public List<string> InsideShotActions { get; set; } = new();
    public List<string> ReboundShotActions { get; set; } = new();
    public List<string> PassingActions { get; set; } = new();
    public List<string> DefendingActions { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
