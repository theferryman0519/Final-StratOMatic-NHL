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
public class Game {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    public string Type { get; set; }
    public string HomeUserType { get; set; } // User, Ai
    public string AwayUserType { get; set; } // User, Ai

    public GameTeam HomeTeam { get; set; }
    public GameTeam AwayTeam { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
