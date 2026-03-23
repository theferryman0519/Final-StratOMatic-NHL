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
public class GoalieDatabase {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    public string InfoString { get; set; }
    public string SeasonString { get; set; }
    public string PlayoffString { get; set; }

    public List<string> StatsStrings { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
