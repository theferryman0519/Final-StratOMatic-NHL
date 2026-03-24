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
public class GameDatabase {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    public string Type { get; set; }
    public string League { get; set; }
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public string HomeStatsString { get; set; }
    public string AwayStatsString { get; set; }

    public List<string> HomeSkaterStatsStrings { get; set; } = new();
    public List<string> HomeGoalieStatsStrings { get; set; } = new();
    public List<string> AwaySkaterStatsStrings { get; set; } = new();
    public List<string> AwayGoalieStatsStrings { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
