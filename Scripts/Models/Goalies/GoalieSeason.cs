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
public class GoalieSeason {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    
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
