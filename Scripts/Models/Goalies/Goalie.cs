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
    public string Id;

    public GoalieInfo Info;
    public GoalieCard Card;
    public GoalieGame Game;
    public GoalieSeason Season;
    public GoaliePlayoff Playoff;

    public List<GoalieStats> Stats = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
