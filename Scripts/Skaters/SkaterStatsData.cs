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
public class SkaterStatsData {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }

    public int TotalGames { get; set; }
    
    public float GoalsPer { get; set; }
    public float AssistsPer { get; set; }
    public float PointsPer { get; set; }
    public float PenaltyMinutesPer { get; set; }
    public float PlusMinusPer { get; set; }
    public float PowerplayGoalsPer { get; set; }
    public float ShorthandedGoalsPer { get; set; }
    public float ShotsPer { get; set; }
    public float AvgTimeOnIce { get; set; } // In seconds

    public List<SkaterYearStatsData> YearStats { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
