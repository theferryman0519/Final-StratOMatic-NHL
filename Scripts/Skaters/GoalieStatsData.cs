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
public class GoalieStatsData {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }

    public int TotalGames { get; set; }
    
    public float WinPercentage { get; set; }
    public float ShutoutPercentage { get; set; }
    public float GoalsAgainstPer60 { get; set; }
    public float ShotsAgainstPer60 { get; set; }
    public float AssistsPer60 { get; set; }
    public float PenaltyMinutesPer60 { get; set; }

    public List<GoalieYearStatsData> YearStats { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
