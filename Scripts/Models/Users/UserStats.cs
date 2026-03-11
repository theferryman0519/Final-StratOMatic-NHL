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
public class UserStats {
    
#region -------------------- Public Variables --------------------
    public string Id { get; set; }
    
    public int NhlWins { get; set; }
    public int NhlLosses { get; set; }
    public int NhlTies { get; set; }
    public int NhlOTLs { get; set; }
    public int PwhlWins { get; set; }
    public int PwhlLosses { get; set; }
    public int PwhlTies { get; set; }
    public int PwhlOTLs { get; set; }
    public int NhlFranchiseWins { get; set; }
    public int NhlFranchiseLosses { get; set; }
    public int NhlFranchiseTies { get; set; }
    public int NhlFranchiseOTLs { get; set; }
    public int PwhlFranchiseWins { get; set; }
    public int PwhlFranchiseLosses { get; set; }
    public int PwhlFranchiseTies { get; set; }
    public int PwhlFranchiseOTLs { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
