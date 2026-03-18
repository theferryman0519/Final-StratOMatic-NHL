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
public class PlayoffRound {
    
#region -------------------- Public Variables --------------------
    public int Round { get; set; }

    public List<Team> Teams { get; set; } = new();
    public List<GameNight> GameNights { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
