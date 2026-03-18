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

namespace SoM.Models {
public class FirebaseRest {
    
#region -------------------- Public Variables --------------------
    public string Url { get; set; }
    public string Method { get; set; }
    public string Json { get; set; }

    public Action<string> SuccessAction { get; set; }
    public Action<string> FailAction { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
