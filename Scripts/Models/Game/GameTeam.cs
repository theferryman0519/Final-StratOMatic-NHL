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

namespace SoM.Models {
public class GameTeam {
    
#region -------------------- Public Variables --------------------
    public Dictionary<string, Skater> SkaterLineup { get; set; } = new();
    public Dictionary<string, Goalie> GoalieLineup { get; set; } = new();

    public int CurrentLine { get; set; }
    public int CurrentPair { get; set; }
    public int CurrentStrategy { get; set; }

    public bool IsGoaliePulled { get; set; }

    public TeamInfo Team { get; set; }
    public TeamGame Stats { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
