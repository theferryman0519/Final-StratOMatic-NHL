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
[System.Serializable]
public class GoalieCard {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    public string Penalty { get; set; }
    public string Fatigue { get; set; }

    public List<string> GoalieRatingActions { get; set; } = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
