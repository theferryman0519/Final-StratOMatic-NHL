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
public class OffenseEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public ConstantController.ShotType SelectedShotType;

    public Skater ShootingSkater;
    public Skater PassingSkater;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator ActionCard()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ActionCard to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"At the start of each play, players must draw an Action Card. This will put the play into motion with a specific puck action.",
            ActionText = $"{possSkater.Info.LastName} has the puck and is getting ready to draw an Action Card to get the play started.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = PickActionCard;

        PassingSkater = null;
        ShootingSkater = null;

        yield return null;
    }

    public IEnumerator OutsideOptions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding OutsideOptions to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"If a skater has an Outside Shot, they might have options to choose from instead of taking a shot. They could attempt a pass or drive the defense.",
            ActionText = $"{possSkater.Info.FirstName} {possSkater.Info.LastName} is thinking about taking a shot, passing the puck, or driving the defense.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator ShotStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotStart to the queue.");

        ShootingSkater = null;

        Skater possSkater = GameplayController.Inst.GetPossSkater();
        string shotType = string.Empty;

        if (SelectedShotType == ConstantController.ShotType.Outside) { shotType = "shot from the perimeter"; }
        else if (SelectedShotType == ConstantController.ShotType.Inside) { shotType = "shot from within the faceoff circles"; }
        else { shotType = "shot from in tight near the crease"; }

        EventRun newEventRun = new EventRun
        {
            InfoText = $"A skater has three different types of shots: Outside, Inside, and Rebound/Breakaway. The closer to the net, the better the shots become.",
            ActionText = $"{possSkater.Info.LastName} is looking to attempt a {shotType}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineShotOutcome;

        ShootingSkater = possSkater;

        yield return null;
    }

    public IEnumerator ShotResultLose()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotResultLose to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After taking a shot, one action that might occur is the shooter losing the puck to an opponent after blocking a shot.",
            ActionText = $"It looks like {possSkater.Info.FirstName} {possSkater.Info.LastName} blocked the shot by {ShootingSkater.Info.LastName}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(0);

        ShootingSkater = null;

        yield return null;
    }

    public IEnumerator ShotResultSave()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotResultSave to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After taking a shot, one action that might occur is the shot being saved by the goalie before being moved to another player.",
            ActionText = $"The shot by {ShootingSkater.Info.LastName} was blocked and passed to {possSkater.Info.FirstName} {possSkater.Info.LastName}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(0);

        ShootingSkater = null;

        yield return null;
    }

    public IEnumerator ShotResultRebound()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotResultRebound to the queue.");

        Goalie opposingGoalie = GameplayController.Inst.GetOpposingGoalie();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After taking a shot, one action that might occur is the shot being saved but causing a rebound.",
            ActionText = $"After the shot by {ShootingSkater.Info.LastName}, the puck bounces off the pads of {opposingGoalie.Info.LastName}, causing a potential rebound.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(8);

        ShootingSkater = null;

        yield return null;
    }

    public IEnumerator ShotResultGoalieRating()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotResultGoalieRating to the queue.");

        Goalie opposingGoalie = GameplayController.Inst.GetOpposingGoalie();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After taking a shot, one action that might occur is the shot causing a Goalie Rating.",
            ActionText = $"{opposingGoalie.Info.LastName} looks to be in trouble from the shot by {ShootingSkater.Info.LastName}. We have a Goalie Rating.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunGoalEvent(0);

        yield return null;
    }

    public IEnumerator ShotResultGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotResultGoal to the queue.");

        Goalie opposingGoalie = GameplayController.Inst.GetOpposingGoalie();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After taking a shot, one action that might occur is the shot goes into the back of the net for a goal.",
            ActionText = $"At first glance, the shot by {ShootingSkater.Info.LastName} appears to get behind {opposingGoalie.Info.LastName}. Let's see if it is a goal.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunGoalEvent(2);

        yield return null;
    }

    public IEnumerator ReboundCheck()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ReboundCheck to the queue.");

        Goalie opposingGoalie = GameplayController.Inst.GetOpposingGoalie();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"When a rebound occurs, there is a battle in front of the net to fight for the puck.",
            ActionText = $"A rebound has occured in front of {opposingGoalie.Info.LastName}. Who will end up with the puck?",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineReboundOutcome;

        yield return null;
    }

    public IEnumerator PassingStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingStart to the queue.");

        PassingSkater = null;

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"One action a skater might take is passing the puck to another player for a better shot at the net.",
            ActionText = $"{possSkater.Info.FirstName} {possSkater.Info.LastName} is attempting to pass the puck.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DeterminePassingOutcome;

        PassingSkater = possSkater;

        yield return null;
    }

    public IEnumerator PassingResultLose()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultLose to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting a pass, the puck might be taken away from an opposing player.",
            ActionText = $"{PassingSkater.Info.LastName} has the puck stripped by {possSkater.Info.FirstName} {possSkater.Info.LastName}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(0);

        PassingSkater = null;

        yield return null;
    }

    public IEnumerator PassingResultLoseShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultLoseShot to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting a pass, the puck might be taken away from an opposing player who now has a shot attempt.",
            ActionText = $"{possSkater.Info.FirstName} {possSkater.Info.LastName} strips the puck away from {PassingSkater.Info.LastName} and looks to shoot the puck.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(2);

        PassingSkater = null;

        yield return null;
    }

    public IEnumerator PassingResultShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultShot to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting a pass, the pass might be successful and a shot attempt taken by a teammate.",
            ActionText = $"{PassingSkater.Info.LastName} passes the puck to {possSkater.Info.LastName}, who appears to be attempting a shot on net.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(2);

        PassingSkater = null;

        yield return null;
    }

    public IEnumerator PassingResultShotIntimidation()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultShotIntimidation to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting a pass, the pass might be successful and a teammate has a chance at an Inside Shot after an opponent attempts to intimidate.",
            ActionText = $"The pass by {PassingSkater.Info.LastName} successfully goes to {possSkater.Info.LastName}, who looks to attempt an Inside Shot.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunDefenseEvent(0);

        PassingSkater = null;

        yield return null;
    }

    public IEnumerator PassingResultOptions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultOptions to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting a pass, the pass might be successful and a teammate has options on the offense.",
            ActionText = $"After grabbing a pass from {PassingSkater.Info.LastName}, {possSkater.Info.FirstName} {possSkater.Info.LastName} has the puck and is looking to generate some offense.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(1);

        PassingSkater = null;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public void PickActionCard()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Picking an Action Card.");

        // TODO
        // Determine different action outcomes
        // Determine current team strategy
        // Set action outcome based on team strategy
        // Add to total cards drawn
        // Update game time
        // Check for injury of possession player
        // Reduce current line stamina for both teams
        // Reduce current pair stamina for both teams
        // Set continue action to outcome
    }

    public void DetermineShotOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the shot outcome.");

        // TODO
        // Determine current skater stamina
        // Determine shot type
        // Determine random 2d6 roll
        // Get action from skater card
        // Set action shift based on stamina
        // If lose puck:
            // Reset position possession tracker
            // Set new team possession
            // Set new player possession
            // Add new player to position possession tracker
            // Add blocked shot for new player
            // Set continue action to shot result lose
        // If save shot:
            // Add shot for shooter
            // Add shot against for opposing goalie
            // Reset position possession tracker
            // Set new team possession
            // Set new player possession
            // Add new player to position possession tracker
            // Set continue action to shot result shot
        // If rebound shot:
            // Add shot for shooter
            // Add shot against for opposing goalie
            // Set continue action to shot result rebound
        // If goalie rating:
            // Add shot for shooter
            // Add shot against for opposing goalie
            // Set continue action to shot result goalie rating
        // If goal:
            // Add shot for shooter
            // Add shot against for opposing goalie
            // Set continue action to shot result goal
    }

    public void DetermineReboundOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the rebound outcome.");

        // TODO
        // Determine random position: both teams
        // Determine new skater possession
        // If same team as shooter:
            // Set new player possession
            // Add new player to position possession tracker
            // Set selected shot type as RebBreak
            // Set continue action to shot start
        // If opposing team as shooter:
            // Reset position possession tracker
            // Set new team possession
            // Set new player possession
            // Add new player to position possession tracker
            // Set continue action to action card
    }

    public void DeterminePassingOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the pass outcome.");

        // TODO
        // Determine current skater stamina
        // Determine random letter from A to L (inclusive)
        // Get action from skater card
        // Set action shift based on stamina
        // If lose:
            // Add giveaway to player
            // Reset position possession tracker
            // Set new team possession
            // Set new player possession
            // Add new player to position possession tracker
            // Add takeaway to new player
            // Set continue action to pass result lose
        // If lose shot:
            // Add giveaway to player
            // Reset position possession tracker
            // Set new team possession
            // Set new player possession
            // Add new player to position possession tracker
            // Add takeaway to new player
            // Set continue action to pass result lose shot
        // If shot:
            // Set new player possession
            // Add new player to position possession tracker
            // Set selected shot type based on card action
            // Set continue action to pass result shot
        // If shot intimidation:
            // Set new player possession
            // Add new player to position possession tracker
            // Set selected shot type to Inside
            // Set continue action to pass result shot intimidation
        // If shot options:
            // Set new player possession
            // Add new player to position possession tracker
            // Set selected shot type to Outside
            // Set continue action to pass result shot options
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
