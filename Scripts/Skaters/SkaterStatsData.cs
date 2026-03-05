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
    
    public float GoalsPer60 { get; set; }
    public float AssistsPer60 { get; set; }
    public float PointsPer60 { get; set; }
    public float PenaltyMinutesPer60 { get; set; }
    public float PlusMinusPer60 { get; set; }
    public float PowerplayGoalsPer60 { get; set; }
    public float ShorthandedGoalsPer60 { get; set; }
    public float ShotsPer60 { get; set; }

    public List<SkaterYearStatsData> YearStats { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
