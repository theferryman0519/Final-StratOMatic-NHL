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
public class DefenseEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public Skater DefendingSkater;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator IntimidationStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationStart to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();
        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"If a skater has an Inside Shot, there might be an opportunity for the defending player to intimidate to steal the puck.",
            ActionText = $"As {possSkater.Info.LastName} is attempting a shot from in close, {DefendingSkater.Info.FirstName} {DefendingSkater.Info.LastName} is attempting to check and intimidate.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineIntimidationOutcome;

        yield return null;
    }

    public IEnumerator IntimidationResultSteal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationResultSteal to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();
        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to intimidate, the defending player might be able to steal the puck away",
            ActionText = $"{DefendingSkater.Info.LastName} hits {possSkater.Info.LastName} with an open-ice check, successfully stealing the puck.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(0);

        DefendingSkater = null;

        yield return null;
    }

    public IEnumerator IntimidationResultShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationResultShot to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();
        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to intimidate, the shooter could skate around the defender to attempt the Inside Shot.",
            ActionText = $"{possSkater.Info.LastName} dekes around the check attempt by {DefendingSkater.Info.LastName}, driving in close and looks to shoot.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(2);

        DefendingSkater = null;

        yield return null;
    }

    public IEnumerator DefendingStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingStart to the queue.");

        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"One of the actions on an Action Card might be an action from a defender, which includes attempting to steal or taking a penalty.",
            ActionText = $"It looks like the skater is moving near {DefendingSkater.Info.FirstName} {DefendingSkater.Info.LastName}, who is attempting to defend.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineDefendingOutcome;

        yield return null;
    }

    public IEnumerator DefendingResultSteal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultSteal to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();
        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the defending player might successfully take the puck away from the opponent.",
            ActionText = $"A stick lift by {DefendingSkater.Info.LastName} allows them to steal the puck away from {possSkater.Info.LastName}.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(0);

        DefendingSkater = null;

        yield return null;
    }

    public IEnumerator DefendingResultStealShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultStealShot to the queue.");

        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the defending player might successfully take the puck away and attempt a shot.",
            ActionText = $"The back check by {DefendingSkater.Info.LastName} allows them to steal the puck, skate down the ice, and is looking to shoot.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(2);

        DefendingSkater = null;

        yield return null;
    }

    public IEnumerator DefendingResultStealShotIntimidation()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultStealShotIntimidation to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();
        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the defending player might successfull take the puck away and attempt an Inside Shot, which might allow the opponent to attempt to intimidate.",
            ActionText = $"After a miscue by {possSkater.Info.LastName}, {DefendingSkater.Info.FirstName} {DefendingSkater.Info.LastName} skates away with the puck, looking to drive the defense.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunDefenseEvent(0);

        DefendingSkater = null;

        yield return null;
    }

    public IEnumerator DefendingResultShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultShot to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();
        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the play might be unsuccessful and allow the offense to attempt a shot.",
            ActionText = $"{possSkater.Info.LastName} dekes around the missed stick lift by {DefendingSkater.Info.LastName} and is now looking to shoot.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunOffenseEvent(2);

        DefendingSkater = null;

        yield return null;
    }

    public IEnumerator DefendingResultShotIntimidation()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultShotIntimidation to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();
        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the play might be unsuccessful and allow the offense to attempt an Inside Shot, which might allow the opponent to attempt to Intimidate.",
            ActionText = $"The poke attempt by {DefendingSkater.Info.LastName} misses as {possSkater.Info.LastName} drives in for a close shot on net.",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunDefenseEvent(0);

        DefendingSkater = null;

        yield return null;
    }

    public IEnumerator DefendingResultPenalty()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultPenalty to the queue.");

        DefendingSkater = GameplayController.Inst.GetDefendingSkater();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the defending player might cause a penalty.",
            ActionText = $"{DefendingSkater.Info.FirstName} {DefendingSkater.Info.LastName} pesters the puck carrier, but did they go too far and result in a penalty?",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = EventsController.Inst.RunPenaltyEvent(0);

        DefendingSkater = null;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public void DetermineIntimidationOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the intimidation outcome.");

        // TODO
        // Determine intimidation rating for intimidating skater
        // Determine random int between 1 and 15 (inclusive)
        // If int is within intimidation range:
            // Add giveaway for current player
            // Reset position possession tracker
            // Set new team possession
            // Set new player possession
            // Add new player to position possession tracker
            // Add hit for new player
            // Add takeaway for new player
            // Set continue action to intimidation result steal
        // If int is outside intimidation range:
            // Set offense events shot type to Inside shot
            // Set continue action to intimidation result shot
    }

    public void DetermineDefendingOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the defending outcome.");

        // TODO
        // Determine current skater stamina
        // Determine random letter from 1 to 14 (inclusive)
        // Get action from skater card
        // Set action shift based on stamina
        // If steal:
            // Add giveaway for current player
            // Reset position possession tracker
            // Set new team possession
            // Set new player possession
            // Add new player to position possession tracker
            // Add takeaway for new player
            // Set continue action to defending result steal
        // If steal then shoot:
            // Add giveaway for current player
            // Reset position possession tracker
            // Set new team possession
            // Set new player possession
            // Add new player to position possession tracker
            // Add takeaway for new player
            // Set continue action to defending result steal shot
        // If steal then shoot with intimidation:
            // Add giveaway for current player
            // Reset position possession tracker
            // Set new team possession
            // Set new player possession
            // Add new player to position possession tracker
            // Add takeaway for new player
            // Set continue action to defending result steal shot intimidation
        // If shot:
            // Check if new player differs from current one
            // If so:
                // Set new player possession
                // Add new player to position possession tracker
            // Set continue action to defending result shot
        // If shot with intimidation:
            // Check if new player differs from current one
            // If so:
                // Set new player possession
                // Add new player to position possession tracker
            // Set continue action to defending result shot intimidation
        // If penalty:
            // Set continue action to defending result penalty
    }
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
