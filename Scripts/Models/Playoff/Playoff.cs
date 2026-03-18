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
public class Playoff {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    public string League { get; set; }

    public Team Team { get; set; }

    public List<PlayoffRound> Rounds { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
