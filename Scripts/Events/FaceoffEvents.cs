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
using Random = UnityEngine.Random;

namespace SoM.Events {
public class FaceoffEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator PuckDrop()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PuckDrop to the queue.");

        Skater homeSkaterMain = GetFaceoffCenter(true);
        Skater awaySkaterMain = GetFaceoffCenter(false);

        string homeSkater = $"{homeSkaterMain.Info.FirstName} {homeSkaterMain.Info.LastName}";
        string awaySkater = $"{awaySkaterMain.Info.FirstName} {awaySkaterMain.Info.LastName}";

        EventRun newEventRun = new EventRun
        {
            InfoText = $"Each time a new segment of play occurs, that segment in initiated by a faceoff. The puck is dropped between two centers to start possession.",
            ActionText = $"The puck is about to drop between {homeSkater} and {awaySkater}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunFaceoffEvent(1); };

        yield return null;
    }

    public IEnumerator FaceoffStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding FaceoffStart to the queue.");

        Skater homeSkaterMain = GetFaceoffCenter(true);
        Skater awaySkaterMain = GetFaceoffCenter(false);

        EventRun newEventRun = new EventRun
        {
            InfoText = $"Both centers line up for the faceoff. Each player has a Faceoff rating between 0 and 3. The higher the rating, the more often that center will win the faceoff.",
            ActionText = $"Let's see who wins this faceoff as {homeSkaterMain.Info.LastName} has a Faceoff rating of +{homeSkaterMain.Card.Faceoff} while {awaySkaterMain.Info.LastName} has a Faceoff rating of +{awaySkaterMain.Card.Faceoff}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunFaceoffEvent(2); };

        GameplayController.Inst.StatsSet.ClearPossPos();
        GameplayController.Inst.StatsSet.SetPossTeam("None");

        yield return null;
    }

    public IEnumerator FaceoffResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding FaceoffResult to the queue.");

        Skater homeSkaterMain = GetFaceoffCenter(true);
        Skater awaySkaterMain = GetFaceoffCenter(false);

        string faceoffWinner = DetermineFaceoff(homeSkaterMain, awaySkaterMain);
        string faceoffPos = GetRandomPos();

        Skater skaterPoss = GetSkaterPossession(faceoffPos, faceoffWinner == "Home");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After winning a faceoff, the puck is moved to a member of the winning center's team. They now control play to start this play segment.",
            ActionText = $"After winning the faceoff, {skaterPoss.Info.FirstName} {skaterPoss.Info.LastName} starts with the puck.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(0); };

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    
#endregion
#region -------------------- Private Methods --------------------
    private Skater GetFaceoffCenter(bool isHome)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the team faceoff center.");

        GameTeam team = isHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        int currentLine = team.CurrentLine;
        string currentCenter = $"C{currentLine.ToString()}";

        return team.SkaterLineup[currentCenter];
    }

    private string DetermineFaceoff(Skater homeCenter, Skater awayCenter)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the faceoff winner.");

        int homeFaceoff = homeCenter.Card.Faceoff;
        int awayFaceoff = awayCenter.Card.Faceoff;
        int faceoffMid = 3;
        int faceoffIndex = Random.Range(0,7);
        int faceoffDiff = homeFaceoff - awayFaceoff;
        int total = faceoffMid + faceoffDiff;

        if (faceoffIndex > total) { return "Away"; }
        else { return "Home"; }
    }

    private string GetRandomPos()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a random position.");

        int index = Random.Range(0,4);

        switch (index)
        {
            case 0: return "LW";
            case 1: return "RW";
            case 2: return "LD";
            case 3:
            default: return "RD";
        }
    }

    private Skater GetSkaterPossession(string pos, bool isHome)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the skater possession.");

        GameTeam team = isHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        int currentLine = team.CurrentLine;
        int currentPair = team.CurrentPair;

        string currentSkater = pos.Contains("D") ? $"{pos}{currentPair.ToString()}" : $"{pos}{currentLine.ToString()}";

        GameplayController.Inst.StatsSet.AddPossPos($"C{currentLine.ToString()}");
        GameplayController.Inst.StatsSet.AddPossPos(currentSkater);
        GameplayController.Inst.StatsSet.SetPossTeam(isHome ? "Home" : "Away");
        GameplayController.Inst.StatsSet.AddFaceoffWon(team.SkaterLineup[currentSkater], 1);
        GameplayController.Inst.StatsSet.AddFaceoffLost(team.SkaterLineup[currentSkater], 1);

        return team.SkaterLineup[currentSkater];
    }
#endregion
}}
