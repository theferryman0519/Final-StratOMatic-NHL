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
public class BottomPanel {
    
#region -------------------- Public Variables --------------------
    public string Title { get; set; }
    public string Body { get; set; }
    public string ButtonA { get; set; }
    public string ButtonB { get; set; }

    public bool HasCloseButton { get; set; }

    public int ButtonCount { get; set; }
    public int SpriteA { get; set; }
    public int SpriteB { get; set; }

    public Action ActionA { get; set; } = null;
    public Action ActionB { get; set; } = null;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
}}
