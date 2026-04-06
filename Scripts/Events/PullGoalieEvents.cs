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
public class PullGoalieEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public Skater ExtraSkater;
    public Skater ShootingSkater;

    public bool IsEmptyNetShot;

    public ConstantController.ShotType ShotType;

    public List<string> PullGoalieShots = new();
    public List<string> EmptyNetShots = new();
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator PullGoalieShotsList()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsList to the queue.");

        IsEmptyNetShot = false;
        ShotType = ConstantController.ShotType.Outside;
        PullGoalieShots.Clear();
        EmptyNetShots.Clear();

        GameTeam exTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"When a team pulls their goalie, they will get an extra attacker to help attempt to generate some offense and score.",
            ActionText = $"It looks like the coach for the {exTeam.Team.NickName} is calling over the goalie, trying to get an extra attacker on the ice.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = GeneratePullGoalieShots;

        yield return null;
    }

    public IEnumerator PullGoalieShotsStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsStart to the queue.");

        GameTeam exTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"At the start after a team pulls their goalie, a shot list is generated based on the team's overall Offense ratings.",
            ActionText = $"With the goalie pulled, the {exTeam.Team.CityName} {exTeam.Team.NickName} look to add some offense to their game.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunPullGoalieEvent(2); };

        yield return null;
    }

    public IEnumerator PullGoalieShotsAttemptStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsAttemptStart to the queue.");

        string shot = string.Empty;

        if (ShotType == ConstantController.ShotType.Outside) { shot = "while on the blue line"; }
        else if (ShotType == ConstantController.ShotType.Inside) { shot = "off the top of the slot"; }
        else { shot = "from in tight by the goaltender"; }

        string action = $"With the extra attacker out, {ShootingSkater.Info.LastName} looks to shoot the puck {shot}.";

        if (IsEmptyNetShot)
        {
            action = $"With the net empty, {ShootingSkater.Info.FirstName} {ShootingSkater.Info.LastName} sends the puck down the ice toward the goal.";
        }

        EventRun newEventRun = new EventRun
        {
            InfoText = $"While the goalie is pulled, each shot attempt taken by a player will be either an Outside, Inside, or Rebound shot.",
            ActionText = $"{action}",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DeterminePullGoalieShotOutcome;

        yield return null;
    }

    public IEnumerator PullGoalieShotsAttemptResultNext()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsAttemptResultNext to the queue.");

        string action = $"After the 6-on-5 shot by {ShootingSkater.Info.LastName}, the puck stays out of the net for now.";

        if (IsEmptyNetShot)
        {
            action = $"The full-rink clear by {ShootingSkater.Info.LastName} doesn't make it all the way down the ice as the attackers regain possession.";
        }

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After the shooting player takes a shot, if a goal is not scored, the extra attacker stays on the ice.",
            ActionText = $"{action}",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineNextPullGoalieShot;

        yield return null;
    }

    public IEnumerator PullGoalieShotsAttemptResultGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsAttemptResultGoal to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After the shooting player takes a shot, if a goal is scored, the goalie comes back onto the ice.",
            ActionText = $"The gamble with pulling the goalie pays off as the puck is put into the back of the net by {ShootingSkater.Info.LastName}!",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunGoalEvent(7); };

        yield return null;
    }

    public IEnumerator PullGoalieShotsAttemptResultEmptyGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsAttemptResultEmptyGoal to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After the shooting player takes a shot, if a goal is scored into the empty net, the goalie comes back onto the ice.",
            ActionText = $"The puck sails down the ice and into the yawning net with that shot by {ShootingSkater.Info.LastName}!",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunGoalEvent(7); };

        yield return null;
    }

    public IEnumerator PullGoalieShotsResult()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsResult to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After the duration of the goalie being pulled, the goalie comes back onto the ice and the game continues with a faceoff.",
            ActionText = $"The puck is stopped for a whistle and it looks as if the goalie is coming back onto the ice as we get ready for the faceoff at 5-on-5.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunFaceoffEvent(0); };

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public void GeneratePullGoalieShots()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Generating the pull goalie shot list.");

        // TODO
        // Determine total offense ratings of 4 on extra attacker unit
        // Determine total defense ratings of 5 on defending unit
        // Determine number of pull goalie shots based on ratings:
            // Total empty net shot attempts is number of 5 ratings
            // Total pull goalie shot attempts is number of 4 ratings
        // For each count of shot attempts, randomize shot type and position
        // Add string of shot to either empty net shots list or pull goalie shots list
    }

    public void DetermineNextPullGoalieShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the next pull goalie shot.");

        // TODO
        // Determine if current shot was empty net
        // If so:
            // Remove first indexed shot from empty net shots list
            // Clear possession position tracker
        // If not:
            // Remove first indexed shot from pull goalie shots list
        // Determine count of shots in empty net shots list
        // If count is greater than zero:
            // Set empty net shot boolean to true
        // If count is zero:
            // Set empty net shot boolean to false
        // Determine count of shots in pull goalie shots list
        // If count is greater than zero:
            // Set continue action to pull goalie shots attempt start
        // If count is zero:
            // Set continue action to pull goalie shots result
    }

    public void DeterminePullGoalieShotOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the pull goalie shot outcome.");

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
                    // If pull goalie goal, add goal against for opposing goalie
                    // Set continue action to pull goalie shots attempt result goal
                // If not a goal:
                    // Set continue action to pull goalie shots attempt result next
            // If goal:
                // Add goal for shooter
                // If pull goalie goal, add goal against for opposing goalie
                // Set continue action to pull goalie shots attempt result goal
        // If action is not:
            // Add shot for shooter
            // Add shot against for opposing goalie
            // Set continue action to pull goalie shots attempt result next
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
#endregion
}}
