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
public class Team {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }

    public TeamInfo Info { get; set; }
    public TeamGame Game { get; set; }
    public TeamSeason Season { get; set; }
    public TeamPlayoff Playoff { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
