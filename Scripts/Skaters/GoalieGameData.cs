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
public class GoalieGameData {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    
    public int Saves { get; set; }
    public int ShotsAgainst { get; set; }
    public int GoalsAgainst { get; set; }
    public int PenaltyMinutes { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
