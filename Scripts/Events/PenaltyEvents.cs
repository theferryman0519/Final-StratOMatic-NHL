// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// Game Dependencies
using SoM.Controllers;
using SoM.Models;

namespace SoM.Events {
public class PenaltyEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public Skater PenaltySkater;
    public Skater ShootingSkater;

    public Goalie PenaltyGoalie;

    public string PenaltyCall;

    public int PenaltyTime;

    public bool IsShorthandedShot;
    public bool IsMajorPenalty;

    public ConstantController.ShotType ShotType;

    public List<string> PenaltyShots = new();
    public List<string> ShorthandedShots = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator PenaltyCheck()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyCheck to the queue.");

        PenaltyCall = string.Empty;
        PenaltyTime = 0;
        IsShorthandedShot = false;
        IsMajorPenalty = false;
        ShotType = ConstantController.ShotType.Outside;
        PenaltyShots.Clear();
        ShorthandedShots.Clear();

        string penaltyPlayer = PenaltyGoalie == null ? PenaltySkater.Info.LastName : PenaltyGoalie.Info.LastName;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"When a player selects a penalty action, their Penalty rating determines if they are getting a penalty or not.",
            ActionText = $"The referees have their arm up as {penaltyPlayer} might get charged with a penalty.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DeterminePenaltyOutcome;

        yield return null;
    }

    public IEnumerator PenaltyCheckClear()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyCheckClear to the queue.");

        string penaltyPlayer = PenaltyGoalie == null ? PenaltySkater.Info.LastName : PenaltyGoalie.Info.LastName;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for a penalty, there might be a chance where the penalty does not get called.",
            ActionText = $"After debate, it looks like {penaltyPlayer} was not given a penalty. The centers are getting ready for a faceoff.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunFaceoffEvent(0); };

        PenaltySkater = null;
        PenaltyGoalie = null;

        yield return null;
    }

    public IEnumerator PenaltyShotsList()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsList to the queue.");

        string penaltyPlayerFirst = PenaltyGoalie == null ? PenaltySkater.Info.FirstName : PenaltyGoalie.Info.FirstName;
        string penaltyPlayerLast = PenaltyGoalie == null ? PenaltySkater.Info.LastName : PenaltyGoalie.Info.LastName;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for a penalty, there might be a chance where a penalty is called and a powerplay starts.",
            ActionText = $"The call on the ice is, indeed, a penalty. {penaltyPlayerFirst} {penaltyPlayerLast} is getting {PenaltyTime} minutes for {PenaltyCall}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = GeneratePenaltyShots;

        yield return null;
    }

    public IEnumerator PenaltyShotsStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsStart to the queue.");

        GameTeam ppTeam = GameplayController.Inst.GameData.PowerplayTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"At the start of a powerplay, a shot list is generated based on the team's overall Offense ratings.",
            ActionText = $"It looks like the {ppTeam.Team.CityName} {ppTeam.Team.NickName} will be on the powerplay.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunPenaltyEvent(4); };

        yield return null;
    }

    public IEnumerator PenaltyShotsAttemptStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsAttemptStart to the queue.");

        string shotType = IsShorthandedShot ? "shorthanded shot" : "shot on the powerplay";
        string shot = string.Empty;

        if (ShotType == ConstantController.ShotType.Outside) { shot = "from near the blue line"; }
        else if (ShotType == ConstantController.ShotType.Inside) { shot = "after driving toward the slot"; }
        else { shot = "while near the goal crease"; }

        EventRun newEventRun = new EventRun
        {
            InfoText = $"During the powerplay, each shot attempt taken by a player will be either an Outside, Inside, or Rebound shot.",
            ActionText = $"{ShootingSkater.Info.LastName} is looking to get a {shotType} {shot}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DeterminePenaltyShotOutcome;

        yield return null;
    }

    public IEnumerator PenaltyShotsAttemptResultNext()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsAttemptResultNext to the queue.");

        string actionText = (PenaltyShots.Count > 1) ? 
            "The shot was saved by the goalie as there is still time left on this powerplay." : 
            "And with no time left on the powerplay, the goalie covers up for the whistle.";

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After the shooting player takes a shot, if a goal is not scored, the powerplay either continues or ends.",
            ActionText = $"{actionText}",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineNextPenaltyShot;

        yield return null;
    }

    public IEnumerator PenaltyShotsAttemptResultGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsAttemptResultGoal to the queue.");

        string shotType = IsShorthandedShot ? "shorthanded goal" : "goal on the powerplay";

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After the shooting player takes a shot, if a goal is scored, the powerplay either continues if the goal was shorthanded or it ends.",
            ActionText = $"With that shot by {ShootingSkater.Info.LastName}, the puck appears to get behind the goalie for a {shotType}!",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineNextPenaltyShotAfterGoal;

        yield return null;
    }


    public IEnumerator PenaltyShotsResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsResult to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After the duration of the penalty, the penalized player comes out of the box and the game continues with a faceoff.",
            ActionText = $"That concludes the powerplay. Both teams are now at even strength as we get ready for a faceoff.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunFaceoffEvent(0); };

        PenaltySkater = null;
        PenaltyGoalie = null;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public void DeterminePenaltyOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the penalty outcome.");
        
        string rating = string.Empty;

        if (PenaltyGoalie != null) { rating = PenaltyGoalie.Card.Penalty; }
        else { rating = PenaltySkater.Card.Penalty; }
        
        int randomNum = Random.Range(1,21);
        int thresholdNum = 0;

        switch (rating)
        {
            case "AA": thresholdNum = 5; break;
            case "A": thresholdNum = 8; break;
            case "B": thresholdNum = 11; break;
            case "C": thresholdNum = 14; break;
            case "D":
            default: thresholdNum = 17; break;
        }

        bool isPenalty = thresholdNum <= randomNum;

        if (isPenalty)
        {
            string powerplayTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(powerplayTeam);
            GameplayController.Inst.GameData.PowerplayTeam = powerplayTeam;

            GameplayController.Inst.GameData.HomeTeam.CurrentLine = 1;
            GameplayController.Inst.GameData.HomeTeam.CurrentPair = 1;
            GameplayController.Inst.GameData.AwayTeam.CurrentLine = 1;
            GameplayController.Inst.GameData.AwayTeam.CurrentPair = 1;

            GameplayController.Inst.StatsSet.ResetFullTeamStamina(true);
            GameplayController.Inst.StatsSet.ResetFullTeamStamina(false);

            DeterminePenalty();

            if (PenaltyGoalie != null) { GameplayController.Inst.StatsSet.AddGoaliePenaltyMinute(PenaltyGoalie, PenaltyTime); }
            else { GameplayController.Inst.StatsSet.AddPenaltyMinute(PenaltySkater, PenaltyTime); }

            GameTeam ppTeam = powerplayTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            ppTeam.Powerplays += 1;

            EventsController.Inst.RunPenaltyEvent(2);
        }

        else
        {
            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam("None");

            EventsController.Inst.RunPenaltyEvent(1);
        }
    }

    public void DeterminePenalty()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the penalty.");

        int randomCall = Random.Range(0, ConstantController.Inst.PenaltyTypes.Count);

        PenaltyCall = ConstantController.Inst.PenaltyTypes[randomCall];

        string rating = string.Empty;

        if (PenaltyGoalie != null) { rating = PenaltyGoalie.Card.Penalty; }
        else { rating = PenaltySkater.Card.Penalty; }

        int randomNum = Random.Range(1,101);
        int thresholdNumA = 0;
        int thresholdNumB = 0;

        switch (rating)
        {
            case "AA": thresholdNumA = 40; thresholdNumB = 65; break;
            case "A": thresholdNumA = 45; thresholdNumB = 70; break;
            case "B": thresholdNumA = 50; thresholdNumB = 75; break;
            case "C": thresholdNumA = 55; thresholdNumB = 80; break;
            case "D":
            default: thresholdNumA = 60; thresholdNumB = 85; break;
        }

        if (randomNum >= thresholdNumB) { PenaltyTime = 5; }
        else if (randomNum >= thresholdNumA) { PenaltyTime = 4; }
        else { PenaltyTime = 2; }
    }

    public void GeneratePenaltyShots()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Generating the penalty shot list.");

        PenaltyShots.Clear();
        ShorthandedShots.Clear();

        string powerplayTeam = GameplayController.Inst.GameData.PowerplayTeam == "Home" ? "Home" : "Away";

        GameTeam ppTeam = powerplayTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        GameTeam pkTeam = powerplayTeam == "Home" ? GameplayController.Inst.GameData.AwayTeam : GameplayController.Inst.GameData.HomeTeam;

        int ppOffense = 0;
        int pkDefense = 0;

        if (ppTeam.SkaterLineup["C1"].Card.Offense == 4) { ppOffense += 1; }
        if (ppTeam.SkaterLineup["LW1"].Card.Offense == 4) { ppOffense += 1; }
        if (ppTeam.SkaterLineup["RW1"].Card.Offense == 4) { ppOffense += 1; }
        if (ppTeam.SkaterLineup["LD1"].Card.Offense == 4) { ppOffense += 1; }
        if (ppTeam.SkaterLineup["RD1"].Card.Offense == 4) { ppOffense += 1; }

        if (pkTeam.SkaterLineup["LW1"].Card.Defense == 4) { pkDefense += 1; }
        if (pkTeam.SkaterLineup["RW1"].Card.Defense == 4) { pkDefense += 1; }
        if (pkTeam.SkaterLineup["LD1"].Card.Defense == 4) { pkDefense += 1; }
        if (pkTeam.SkaterLineup["RD1"].Card.Defense == 4) { pkDefense += 1; }

        for (int pk = 0; pk < pkDefense; pk++)
        {
            ShorthandedShots.Add(GetRandomShot());
        }

        int ppTimeShift = Random.Range(0,5);
        int ppShift = ppOffense;

        if (PenaltyTime == 2) { ppShift - ppTimeShift; }
        else if (PenaltyTime == 5) { ppShift + ppTimeShift; }

        if (ppShift < 1) { ppShift = 1; }

        for (int pp = 0; pp < ppShift; pp++)
        {
            PenaltyShots.Add(GetRandomShot());
        }

        // TODO
        // Check if ShorthandedShots has a count
        // If so:
            // Get first index and set shot type
            // Set random pos
            // Get skater from pos
            // Add to PossPos list
            // Set offense events shot type
            // Set offense events shooter
            // Set this shooter
        // If not:
            // Get first index and set shot type
            // Set random pos
            // Get skater from pos
            // Add to PossPos list
            // Set offense events shot type
            // Set offense events shooter
            // Set this shooter

        EventsController.Inst.RunPenaltyEvent(3);
    }

    public void DetermineNextPenaltyShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the next penalty shot.");

        // TODO
        // Determine if current shot was shorthanded
        // If so:
            // Remove first indexed shot from shorthanded shots list
            // Clear possession position tracker
        // If not:
            // Remove first indexed shot from powerplay shots list
        // Determine count of shots in shorthanded shots list
        // If count is greater than zero:
            // Set shorthanded shot boolean to true
        // If count is zero:
            // Set shorthanded shot boolean to false
        // Determine count of shots in powerplay shots list
        // If count is greater than zero:
            // Set continue action to penalty shots attempt start
        // If count is zero:
            // Set continue action to penalty shots result
    }

    public void DeterminePenaltyShotOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the penalty shot outcome.");

        // TODO
        // Add to possession position tracker
        // Determine the shot type
        // Determine the action card of the shooter
        // Determine random 2d6 result
        // Determine shot action based on dice sum and shot type
        // If action is a goalie rating or a goal:
            // Add shot for shooter
            // Add shot against for opposing goalie
            // If goalie rating:
                // Determine random 2d6 result
                // Determine goalie rating action based on dice sum
                // If goal:
                    // Add goal for shooter
                    // Add powerplay or shorthanded goal for shooter
                    // Add goal against for opposing goalie
                    // Set continue action to penalty shots attempt result goal
                // If not a goal:
                    // Set continue action to penalty shots attempt result next
            // If goal:
                // Add goal for shooter
                // Add powerplay or shorthanded goal for shooter
                // Add goal against for opposing goalie
                // Set continue action to penalty shots attempt result goal
        // If action is not:
            // Add shot for shooter
            // Add shot against for opposing goalie
            // Set continue action to penalty shots attempt result next
    }

    public void DetermineNextPenaltyShotAfterGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the next penalty shot after a goal.");

        // TODO
        // Determine if current shot was shorthanded
        // If so:
            // Set after goal action to determine next penalty shot
            // Set continue action to goal
        // If not:
            // Determine if penalty is minor, double-minor, or major
            // If penalty is minor:
                // Remove all shots from powerplay shots list except first indexed
                // Set after goal action to determine next penalty shot
                // Set continue action to goal
            // If penalty is double-minor:
                // Remove last 3 indexed shots from powerplay shots list (or amount to keep first indexed if less than 3 remain)
                // Set after goal action to determine next penalty shot
                // Set continue action to goal
            // If penalty is major:
                // Set after goal action to determine next penalty shot
                // Set continue action to goal
    }
#endregion
#region -------------------- Private Methods --------------------
    private string GetRandomPos()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a random position.");

        int index = Random.Range(0,5);

        switch (index)
        {
            case 0: return "C";
            case 1: return "LW";
            case 2: return "RW";
            case 3: return "LD";
            case 4:
            default: return "RD";
        }
    }

    private string GetRandomShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a random shot.");

        int index = Random.Range(0,6);

        switch (index)
        {
            case 0:
            case 1:
            case 2: return "OUT";
            case 3:
            case 5: return "IN";
            case 6:
            default: return "REB";
        }
    }
#endregion
}}
