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
public class GoalEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public Skater ShootingSkater;

    public Goalie DefendingGoalie;

    public ConstantController.GoalType GoalType;

    public int GoalThreshold;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator GoalieRatingStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingStart to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"One of the actions for a player card might be Goalie Rating. These are sets of actions each goalie has to further determine a play.",
            ActionText = $"{DefendingGoalie.Info.LastName} looks to be having trouble with the shot by {ShootingSkater.Info.LastName}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineGoalieRatingOutcome;

        yield return null;
    }

    public IEnumerator GoalieRatingResultSave()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingResultSave to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for the Goalie Rating, one of the outcomes might be a save, leading to a faceoff.",
            ActionText = $"It looks as if {DefendingGoalie.Info.LastName} was able to find the puck and cover it for a whistle.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunFaceoffEvent(0);

        yield return null;
    }

    public IEnumerator GoalieRatingResultBreakaway()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingResultBreakaway to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for the Goalie Rating, one of the outcomes might be a breakaway for a teammate.",
            ActionText = $"{DefendingGoalie.Info.LastName} quickly finds the puck and fires it down the ice for a teammate on a breakaway.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(2);

        yield return null;
    }

    public IEnumerator GoalieRatingResultPenalty()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingResultPenalty to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for the Goalie Rating, one of the outcomes might be a penalty taken by the goalie.",
            ActionText = $"After chaos in the crease, it looks like the referees might call a penalty on {DefendingGoalie.Info.LastName}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunPenaltyEvent(0);

        yield return null;
    }

    public IEnumerator GoalieRatingResultGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingResultGoal to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for the Goalie Rating, one of the outcomes might be a goal given up.",
            ActionText = $"The referee is signaling the puck is in the back of the net for a goal after the pileup in front of the goalie.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunGoalEvent(7);

        yield return null;
    }

    public IEnumerator GoalCheck()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalCheck to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"Some goal actions for skaters have a goal threshold. The higher the number, the more likely a goal will occur.",
            ActionText = $"We are going to step aside while the refs take a look at the video replay to see if the puck crossed the goal line.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineGoalOutcome;

        yield return null;
    }

    public IEnumerator NoGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding NoGoal to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for a goal, one of the outcomes is a no goal, which will lead to a faceoff.",
            ActionText = $"After video review, the puck does not seem to fully cross the line. Both sides are getting ready for a faceoff.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunFaceoffEvent(0);

        yield return null;
    }

    public IEnumerator Goal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding Goal to the queue.");

        string announcement = DetermineGoalAnnouncment();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"When scoring a goal, the possession tracker also keeps track of assists. All who earned points on the goal will be mentioned.",
            ActionText = $"{announcement}",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineAfterGoalEvent;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public void DetermineGoalieRatingOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the goalie rating outcome.");

        // TODO
        // Determine goalie rating actions of defending goalie
        // Determine random 2d6 result
        // Determine goalie rating action based on dice sum
        // If result is a save:
            // Set possession team to none
            // Reset possession position tracker
            // Set continue action to goalie rating result save
        // If result is a breakaway:
            // Determine random skater position
            // Add to possession position tracker
            // Set shot type as RebBreak
            // Set continue action to goalie rating result breakaway
        // If result is a penalty:
            // Set penalty skater as center from fourth line
            // Add penalty minutes to defending goalie
            // Set continue action to goalie rating result penalty
        // If result is a goal:
            // Set continue action to goalie rating result goal
    }

    public void DetermineGoalOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the goal outcome.");

        // TODO
        // Determine goal threshold number
        // Determine random number between 1 and 20 (inclusive)
        // If number is less than or equal to threshold number:
            // Set continue action to goal
        // If number is greater than threshold number:
            // Set possession team to none
            // Reset possession position tracker
            // Set continue action to no goal
    }

    public void DetermineAfterGoalEvent()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the event after a goal is scored.");

        // TODO
        // Determine current period
        // If period is 4 or higher:
            // Set continue action to end of game
        // If period is 1, 2, or 3:
            // Set continue action ot puck drop
    }

    public string DetermineGoalAnnouncment()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the goal announcement.");

        // TODO
        // Add goal to shooting skater
        // Add goal against for defending goalie
        // Determine up to two prior skaters in possession position tracker
        // Add assists to prior skaters
        // Determine goal type
        // If goal is shorthanded:
            // Add shorthanded goal to shooting skater
            // Add shorthanded assists to prior skaters
        // If goal is powerplay:
            // Add powerplay goal to shooting skater
            // Add powerplay assists to prior skaters
        // Else:
            // Add pluses to all on ice for scoring team
            // Add minuses to all on ice for defending team
        
        string goalAnnoucement = string.Empty;

        // TODO
        // Determine game type for specific goal announcement
        // Determine goal number for specific goal annoucement
        // Add string for goal scorer
        // Add string for assists

        return goalAnnouncement;
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
