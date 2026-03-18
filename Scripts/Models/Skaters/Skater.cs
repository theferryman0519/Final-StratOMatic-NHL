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
public class Skater {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }

    public SkaterInfo Info { get; set; }
    public SkaterCard Card { get; set; }
    public SkaterGame Game { get; set; }
    public SkaterSeason Season { get; set; }
    public SkaterPlayoff Playoff { get; set; }

    public List<SkaterStats> Stats { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
