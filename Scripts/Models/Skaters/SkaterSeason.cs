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
public class SkaterSeason {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }

    public int GamesPlayed { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int Points { get; set; }
    public int PlusMinus { get; set; }
    public int PenaltyMinutes { get; set; }
    public int PowerplayGoals { get; set; }
    public int PowerplayAssists { get; set; }
    public int PowerplayPoints { get; set; }
    public int ShorthandedGoals { get; set; }
    public int ShorthandedAssists { get; set; }
    public int ShorthandedPoints { get; set; }
    public int Shots { get; set; }
    public int Giveaways { get; set; }
    public int Takeaways { get; set; }
    public int FaceoffsWon { get; set; }
    public int FaceoffsLost { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
