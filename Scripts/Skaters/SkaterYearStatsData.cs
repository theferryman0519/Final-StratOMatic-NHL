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
public class SkaterYearStatsData {
    
#region -------------------- Public Variables --------------------
    public int StartYear { get; set; }

    public int GamesPlayed { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int Points { get; set; }
    public int PenaltyMinutes { get; set; }
    public int PlusMinus { get; set; }
    public int PowerplayGoals { get; set; }
    public int ShorthandedGoals { get; set; }
    public int Shots { get; set; }
    public int TimePerGame { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
