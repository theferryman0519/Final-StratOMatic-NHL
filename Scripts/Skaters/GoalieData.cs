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
public class GoalieData {
    
#region -------------------- Public Variables --------------------
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Id { get; set; }
    public string Team { get; set; }
    public string Position { get; set; }

    public GoalieStatsData Stats { get; set; }
    public GoalieCardData Card { get; set; }
    public GoalieGameData Game { get; set; }
    public GoalieSeasonData Season { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
