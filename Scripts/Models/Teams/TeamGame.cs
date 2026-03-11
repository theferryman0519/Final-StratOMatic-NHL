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
public class TeamGame {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    
    public int Goals { get; set; }
    public int Shots { get; set; }
    public int PowerplayGoals { get; set; }
    public int Powerplays { get; set; }
    public int ShorthandedGoals { get; set; }
    public int FaceoffsWon { get; set; }
    public int FaceoffsLost { get; set; }
    public int Hits { get; set; }
    public int BlockedShots { get; set; }
    public int Giveaways { get; set; }
    public int Takeaways { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
