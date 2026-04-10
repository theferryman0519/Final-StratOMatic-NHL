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
public class PlayoffDatabase {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    public string League { get; set; }
    public string Team { get; set; }

    public int Round { get; set; }
    public int GameNumber { get; set; }

    public List<string> SkaterLineup { get; set; }
    public List<string> GoalieLineup { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
