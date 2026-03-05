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
public class SkaterData {
    
#region -------------------- Public Variables --------------------
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Id { get; set; }
    public string Team { get; set; }
    public string Position { get; set; }

    public SkaterStatsData Stats { get; set; }
    public SkaterCardData Card { get; set; }
    public SkaterGameData Game { get; set; }
    public SkaterSeasonData Season { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
