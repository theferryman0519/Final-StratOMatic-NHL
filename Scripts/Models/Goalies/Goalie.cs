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
public class Goalie {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }

    public GoalieInfo Info { get; set; }
    public GoalieCard Card { get; set; }
    public GoalieGame Game { get; set; }
    public GoalieSeason Season { get; set; }
    public GoaliePlayoff Playoff { get; set; }

    public List<GoalieStats> Stats { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
