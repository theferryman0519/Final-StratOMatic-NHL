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

namespace SoM.Users {
[System.Serializable]
public class UserData {
    
#region -------------------- Public Variables --------------------
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Id { get; set; }
    public string Team { get; set; }
    public string SeasonTeam { get; set; }

    public int CareerWins { get; set; }
    public int CareerLosses { get; set; }
    public int CareerTies { get; set; }
    public int CareerOTLs { get; set; }

    public int SeasonWins { get; set; }
    public int SeasonLosses { get; set; }
    public int SeasonTies { get; set; }
    public int SeasonOTLs { get; set; }
    public int SeasonNight { get; set; }
    public int SeasonYear { get; set; }

    public int Championships { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
