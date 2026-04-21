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
    public string Type { get; set; } // Exhibition, Season, Playoff, Multiplayer
    public string HomeUserType { get; set; } // User, Ai
    public string AwayUserType { get; set; } // User, Ai
    public string PowerplayTeam { get; set; } // None, Home, Away
    public string PullGoalieTeam { get; set; } // None, Home, Away
    public string PossTeam { get; set; } // None, Home, Away

    public int CardsDrawn { get; set; }
    public int Period { get; set; }

    public List<string> PossPos { get; set; } = new();

    public GameTeam HomeTeam { get; set; }
    public GameTeam AwayTeam { get; set; }

    public List<GameLog> Logs { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
