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
    public string Id;

    public SkaterInfo Info;
    public SkaterCard Card;
    public SkaterGame Game;
    public SkaterSeason Season;
    public SkaterPlayoff Playoff;

    public List<SkaterStats> Stats = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
