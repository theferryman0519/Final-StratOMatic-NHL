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
public class User {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    
    public UserInfo Info { get; set; }
    public UserStats Stats { get; set; }
    public UserSeasonStats SeasonStats { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
