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

namespace SoM.Events {
public class GameFlowEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public Skater InjuredSkater;

    public bool IsOvertimeGame;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator StartOfGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding StartOfGame to the queue.");

        TeamInfo homeInfo = GameplayController.Inst.GameData.HomeTeam.Team;
        TeamInfo awayInfo = GameplayController.Inst.GameData.AwayTeam.Team;

        string homeTeamName = $"{homeInfo.CityName} {homeInfo.NickName}";
        string awayTeamName = $"{awayInfo.CityName} {awayInfo.NickName}";

        EventRun newEventRun = new EventRun
        {
            InfoText = $"This Strat-O-Matic game is about to begin as the two teams are getting ready for the opening faceoff.",
            ActionText = $"Welcome, ladies and gentlemen, to a sprited game between the {awayTeamName} and the {homeTeamName}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        IsOvertimeGame = false;
        InjuredSkater = null;

        yield return null;
    }

    public IEnumerator StartOfPeriod()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding StartOfPeriod to the queue.");

        string period = string.Empty;

        if (GameplayController.Inst.GameData.Period == 1) { period = "first period"; }
        else if (GameplayController.Inst.GameData.Period == 2) { period = "second period"; }
        else if (GameplayController.Inst.GameData.Period == 3) { period = "third period"; }
        else if (GameplayController.Inst.GameData.Period == 4) { period = "overtime"; }
        else if (GameplayController.Inst.GameData.Period == 5) { period = "second overtime"; }
        else { period = "next overtime"; }

        EventRun newEventRun = new EventRun
        {
            InfoText = $"Both teams are getting ready for the faceoff that starts the period.",
            ActionText = $"Both teams are getting ready to drop the puck on the {period}. We're ready to get underway!",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator Injury()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding Injury to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"There is an injury on the ice. When an injury occurs, that player is no longer available for the remainder of the game. Another skater will take their place.",
            ActionText = $"Oh no! It looks like {InjuredSkater.Info.LastName} was injured on that play. Their night looks to be over.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator EndOfPeriod()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding EndOfPeriod to the queue.");

        string period = string.Empty;

        if (GameplayController.Inst.GameData.Period == 1) { period = "one period"; }
        else if (GameplayController.Inst.GameData.Period == 2) { period = "two periods"; }
        else if (GameplayController.Inst.GameData.Period == 2) { period = "regulation"; }
        else { period = "extra time"; }

        string homeTeamName = GameplayController.Inst.GameData.HomeTeam.Team.NickName;
        string homeTeamGoals = GameplayController.Inst.GameData.HomeTeam.Stats.Goals.ToString();
        string awayTeamName = GameplayController.Inst.GameData.AwayTeam.Team.NickName;
        string awayTeamGoals = GameplayController.Inst.GameData.AwayTeam.Stats.Goals.ToString();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"The period has finished because 30 Action Cards have been drawn. Both teams will reset for the next period, unless this was the final period of the game.",
            ActionText = $"That horn sounds the end of the period. After {period}, the score stands as the {homeTeamName} {homeTeamGoals}, the {awayTeamName} {awayTeamGoals}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator OvertimeStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding OvertimeStart to the queue.");

        string teamGoals = GameplayController.Inst.GameData.HomeTeam.Stats.Goals.ToString();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"If after three periods of play the score is still tied, both teams will go into overtime. If still tied, the game ends in a tie unless this is a playoff game.",
            ActionText = $"We're about to start extra time as both teams are currently tied at {teamGoals}. The next goal will be the winner!",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        IsOvertimeGame = true;

        yield return null;
    }

    public IEnumerator EndOfGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding EndOfGame to the queue.");

        string winner = string.Empty;

        int homeTeamGoals = GameplayController.Inst.GameData.HomeTeam.Stats.Goals;
        int awayTeamGoals = GameplayController.Inst.GameData.AwayTeam.Stats.Goals;

        string homeTeamName = GameplayController.Inst.GameData.HomeTeam.Team.NickName;
        string awayTeamName = GameplayController.Inst.GameData.AwayTeam.Team.NickName;

        if (homeTeamGoals == awayTeamGoals) { winner = "Both teams walk away from the game tied"; }
        else if (homeTeamGoals > awayTeamGoals) { winner = $"The {homeTeamName} come away victorious over the {awayTeamName}"; }
        else { winner = $"The {awayTeamName} hand the {homeTeamName} a defeat"; }

        EventRun newEventRun = new EventRun
        {
            InfoText = $"The game has completed.",
            ActionText = $"The final horn sounds for the game. {winner} with a final score of {homeTeamGoals.ToString()} to {awayTeamGoals.ToString()}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator CompleteGame()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding CompleteGame to the queue.");

        // TODO: Add stats as needed

        EventRun newEventRun = new EventRun
        {
            InfoText = string.Empty,
            ActionText = string.Empty,
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
