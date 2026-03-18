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
public class FirebaseLogin {
    
#region -------------------- Public Variables --------------------
    public string Email { get; set; }
    public string Password { get; set; }
    
    public Action SuccessAction { get; set; }
    public Action FailAction { get; set; }
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
