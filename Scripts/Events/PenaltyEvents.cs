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
public class PenaltyEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public Skater PenaltySkater;
    public Skater ShootingSkater;

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

        EventRun newEventRun = new EventRun
        {
            InfoText = $"When a player selects a penalty action, their Penalty rating determines if they are getting a penalty or not.",
            ActionText = $"The referees have their arm up as {PenaltySkater.Info.LastName} might get charged with a penalty.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PenaltyCheckClear()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyCheckClear to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for a penalty, there might be a chance where the penalty does not get called.",
            ActionText = $"After debate, it looks like {PenaltySkater.Info.LastName} was not given a penalty. The centers are getting ready for a faceoff.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PenaltyShotsList()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsList to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for a penalty, there might be a chance where a penalty is called and a powerplay starts.",
            ActionText = $"The call on the ice is, indeed, a penalty. {PenaltySkater.Info.FirstName} {PenaltySkater.Info.LastName} is getting {PenaltyTime} minutes for {PenaltyCall}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PenaltyShotsStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsStart to the queue.");

        Team ppTeam = GameplayController.Inst.GameData.PowerplayTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"At the start of a powerplay, a shot list is generated based on the team's overall Offense ratings.",
            ActionText = $"It looks like the {ppTeam.Team.CityName} {ppTeam.Team.NickName} will be on the powerplay.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

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

        yield return null;
    }

    public IEnumerator PenaltyShotsAttemptResultGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsAttemptResultGoal to the queue.");

        string shotType = IsShorthandedShot ? "shorthanded goal" : "goal on the powerplay";

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After the shooting player takes a shot, if a goal is scored, the powerplay either continues if the goal was shorthanded or it ends.",
            ActionText = $"With that shot by {ShootingSkater.Info.LastName}, the puck appears to get behind the goalie for a {shotType}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        IsShorthandedShot = false;

        yield return null;
    }


    public IEnumerator PenaltyShotsResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PenaltyShotsResult to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"If no goals are scored throughout the duration of the powerplay, the penalized player comes out of the box and the game continues with a faceoff.",
            ActionText = $"That concludes the powerplay. Both teams are now at even strength as we get ready for a faceoff.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public void DeterminePenaltyOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the penalty outcome.");

        // TODO
        // Determine penalty rating for penalty skater
        // Determine random int between 1 and 5 (inclusive) (D = 1, AA = 5)
        // If penalty rating is equal to or greater than int:
            // Reset position possession tracker
            // Determine the penalty (type and time) based on penalty rating
            // Add penalty minutes to penalty skater
            // Add powerplay count to opposing team
            // Set continue action to penalty shots list
        // If penalty rating is less than int:
            // Reset position possession tracker
            // Set continue action to penalty check clear
    }

    public void DeterminePenalty()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the penalty.");

        // TODO
        // Determine the penalty rating for penalty skater
        // Determine random penalty type (ConstantController.Inst.PenaltyTypes)
        // Determine penalty minutes based on penalty rating
        // Set penalty call
        // Set penalty time
    }

    public void GeneratePenaltyShots()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Generating the penalty shot list.");

        // TODO
        // Determine penalty time
        // Determine total offense ratings of 4 on powerplay unit
        // Determine total defense ratings of 5 on shorthanded unit
        // Determine number of penalty shots based on ratings:
            // Total shorthanded shot attempts is number of 5 ratings
            // Total powerplay shot attempts:
                // Determine random int between 0 and 4 (inclusive)
                // If minor penalty: equal to number of 4 ratings minus random int (must have at least one shot)
                // If double-minor penalty: equal to number of 4 ratings
                // If major penalty: equal to number of 4 ratings plus random int
        // For each count of shot attempts, randomize shot type and position
        // Add string of shot to either shorthanded shots list or powerplay shots list
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
                    // Set continue action to penalty shots attempt result goal
                // If not a goal:
                    // Set continue action to penalty shots attempt result next
            // If goal:
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
            // Set continue action to determine next penalty shot
        // If not:
            // Determine if penalty is minor, double-minor, or major
            // If penalty is minor:
                // Remove all shots from powerplay shots list except first indexed
                // Set continue action to determine next penalty shot
            // If penalty is double-minor:
                // Remove last 3 indexed shots from powerplay shots list (or amount to keep first indexed if less than 3 remain)
                // Set continue action to determine next penalty shot
            // If penalty is major:
                // Set continue action to determine next penalty shot
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
