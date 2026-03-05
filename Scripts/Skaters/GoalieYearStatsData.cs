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
public class GoalieYearStatsData {
    
#region -------------------- Public Variables --------------------
    public int StartYear { get; set; }

    public int GamesPlayed { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Shutouts { get; set; }
    public int GoalsAgainst { get; set; }
    public int ShotsAgainst { get; set; }
    public int Assists { get; set; }
    public int PenaltyMinutes { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
