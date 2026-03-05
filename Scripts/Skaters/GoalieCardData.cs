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
public class GoalieCardData {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    public string Penalty { get; set; }

    public List<string> GoalieRatingAction { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
