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
public class UserSeasonStats {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }

    public bool IsInSeason { get; set; }
    
    public int CurrentWins { get; set; }
    public int CurrentLosses { get; set; }
    public int CurrentTies { get; set; }
    public int CurrentOTLs { get; set; }
    public int TotalCups { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
