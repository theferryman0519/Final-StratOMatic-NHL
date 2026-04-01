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
using SoM.Controllers;
using SoM.Models;

namespace SoM.Gameplay {
public class GameplayStatsSet : MonoBehaviour {

#region -------------------- Game Stats Methods --------------------
    public void SetPossTeam(string team)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the possession team.");

        GameplayController.Inst.GameData.PossTeam = team;
    }

    public void ClearPossPos()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Clearing the possession position tracker.");

        GameplayController.Inst.GameData.PossPos.Clear();
    }

    public void AddPossPos(string pos)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to the possession position tracker.");

        GameplayController.Inst.GameData.PossPos.Add(pos);
    }
#endregion
#region -------------------- Team Stats Methods --------------------
    
#endregion
#region -------------------- Skater Stats Methods --------------------
    public void AddFaceoffWon(bool isHome, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to faceoff won.");

        GameTeam team = isHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        int currentLine = team.CurrentLine;
        string center = $"C{currentLine}";

        team.SkaterLineup[center].Game.FaceoffsWon += delta;
    }

    public void AddFaceoffLost(bool isHome, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to faceoff won.");

        GameTeam team = isHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        int currentLine = team.CurrentLine;
        string center = $"C{currentLine}";

        team.SkaterLineup[center].Game.FaceoffsLost += delta;
    }

    public void AddBlockedShot(bool isHome, int delta)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding to blocked shot.");

        GameTeam team = isHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        string possPos = GameplayController.Inst.GameData.PossPos[GameplayController.Inst.GameData.PossPos.Count - 1];

        team.SkaterLineup[possPos].Game.BlockedShots += delta;
    }
#endregion
#region -------------------- Goalie Stats Methods --------------------
    
#endregion
}}
