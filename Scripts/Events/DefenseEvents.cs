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
public class DefenseEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public Skater DefendingSkater;
    public Skater PriorSkater;
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
            ButtonText = (GameplayController.Inst.GameData.PossTeam == "Away") ? "Attempt Check" : "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineIntimidationOutcome;

        yield return null;
    }

    public IEnumerator IntimidationResultSteal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationResultSteal to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to intimidate, the defending player might be able to steal the puck away",
            ActionText = $"{DefendingSkater.Info.LastName} hits {PriorSkater.Info.LastName} with an open-ice check, successfully stealing the puck.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Outside;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(0); };

        DefendingSkater = null;
        PriorSkater = null;

        yield return null;
    }

    public IEnumerator IntimidationResultShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding IntimidationResultShot to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to intimidate, the shooter could skate around the defender to attempt the Inside Shot.",
            ActionText = $"{PriorSkater.Info.LastName} dekes around the check attempt by {DefendingSkater.Info.LastName}, driving in close and looks to shoot.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Inside;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(2); };

        DefendingSkater = null;
        PriorSkater = null;

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
            ButtonText = (GameplayController.Inst.GameData.PossTeam == "Away") ? "Attempt Defend" : "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = DetermineDefendingOutcome;

        yield return null;
    }

    public IEnumerator DefendingResultSteal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultSteal to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the defending player might successfully take the puck away from the opponent.",
            ActionText = $"A stick lift by {DefendingSkater.Info.LastName} allows them to steal the puck away from {PriorSkater.Info.LastName}.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Outside;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(0); };

        DefendingSkater = null;
        PriorSkater = null;

        yield return null;
    }

    public IEnumerator DefendingResultStealShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultStealShot to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the defending player might successfully take the puck away and attempt a shot.",
            ActionText = $"The back check by {DefendingSkater.Info.LastName} allows them to steal the puck, skate down the ice, and is looking to shoot.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Inside;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(2); };

        DefendingSkater = null;
        PriorSkater = null;

        yield return null;
    }

    public IEnumerator DefendingResultShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultShot to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the play might be unsuccessful and allow the offense to attempt a shot.",
            ActionText = $"{PriorSkater.Info.LastName} dekes around the missed stick lift by {DefendingSkater.Info.LastName} and is now looking to shoot.",
            ButtonText = "Continue",
        };
        
        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Inside;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(2); };

        DefendingSkater = null;
        PriorSkater = null;

        yield return null;
    }

    public IEnumerator DefendingResultPenalty()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding DefendingResultPenalty to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting to defend, the defending player might cause a penalty.",
            ActionText = $"{DefendingSkater.Info.FirstName} {DefendingSkater.Info.LastName} pesters the puck carrier, but did they go too far and result in a penalty?",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunPenaltyEvent(0); };

        DefendingSkater = null;
        PriorSkater = null;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public void DetermineIntimidationOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the intimidation outcome.");
        
        int randomNumber = Random.Range(1,16);
        string intimidationRating = DefendingSkater.Card.Intimidation;
        int intimidationNumber = 0;

        if (intimidationRating == "0") { intimidationNumber = 0; }
        else if (intimidationRating == "1") { intimidationNumber = 1; }
        else
        {
            string splitString = intimidationRating.Split('-')[1];
            intimidationNumber = Int32.Parse(splitString);
        }

        bool isSuccess = intimidationNumber >= randomNumber;

        if (isSuccess)
        {
            PriorSkater = GameplayController.Inst.GetPossSkater();
            GameTeam possTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";
            string newPossPos = GameplayController.Inst.GetSkaterPos(DefendingSkater, newPossTeamString);

            GameplayController.Inst.StatsSet.AddGiveaway(PriorSkater, 1);
            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);
            GameplayController.Inst.StatsSet.AddHit(DefendingSkater, 1);
            GameplayController.Inst.StatsSet.AddTakeaway(DefendingSkater, 1);

            string rating = DefendingSkater.Card.Penalty;
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
                EventsController.Inst.GameplayEvents.PenaltyEvents.PenaltySkater = DefendingSkater;
                EventsController.Inst.GameplayEvents.PenaltyEvents.PenaltyGoalie = null;
                EventsController.Inst.RunPenaltyEvent(0);
            }

            else
            {
                EventsController.Inst.RunDefenseEvent(1);
            }
        }

        else
        {
            PriorSkater = GameplayController.Inst.GetPossSkater();

            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Inside;
            EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = PriorSkater;
            EventsController.Inst.RunDefenseEvent(2);
        }
    }

    public void DetermineDefendingOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the defending outcome.");
        
        int randomNumber = Random.Range(1,15);
        string defendingAction = DefendingSkater.Card.DefendingActions[randomNumber - 1];
        int defendingStamina = DefendingSkater.Game.Stamina;
        
        int staminaShift = 0;
        int finalOption = 0;

        if (defendingStamina >= 85) { staminaShift = 0; }
        else if (defendingStamina >= 60) { staminaShift = 1; }
        else if (defendingStamina >= 45) { staminaShift = 2; }
        else if (defendingStamina >= 30) { staminaShift = 3; }
        else if (defendingStamina >= 15) { staminaShift = 4; }
        else { staminaShift = 5; }

        switch (defendingAction)
        {
            case "PENALTY": finalOption = 10; break;
            case "TA-IN": finalOption = 5; break;
            case "TA-OUT": finalOption = 4; break;
            case "TA": finalOption = 3; break;
            case "OUT": finalOption = 2; break;
            case "IN":
            default: finalOption = 1; break;
        }

        if (finalOption < 10) { finalOption -= staminaShift; }
        if (finalOption < 1) { finalOption = 1; }

        string newPos = GetRandomPos();

        int homeLine = GameplayController.Inst.GameData.HomeTeam.CurrentLine;
        int homePair = GameplayController.Inst.GameData.HomeTeam.CurrentPair;
        int awayLine = GameplayController.Inst.GameData.AwayTeam.CurrentLine;
        int awayPair = GameplayController.Inst.GameData.AwayTeam.CurrentPair;

        PriorSkater = GameplayController.Inst.GetPossSkater();
        bool isPossHome = GameplayController.Inst.GameData.PossTeam == "Home";

        if (finalOption == 1) // IN
        {
            bool isSameSkater = IsSameSkater(PriorSkater, isPossHome, newPos);

            if (!isSameSkater)
            {
                string newPossPos = string.Empty;

                if (isPossHome)
                {
                    newPossPos = newPos.Contains("D") ? $"{newPos}{homePair}" : $"{newPos}{homeLine}";
                }

                else
                {
                    newPossPos = newPos.Contains("D") ? $"{newPos}{awayPair}" : $"{newPos}{awayLine}";
                }

                GameplayController.Inst.StatsSet.AddPossPos(newPossPos);
            }

            Skater newPossSkater = GameplayController.Inst.GetPossSkater();

            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Inside;
            EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = newPossSkater;
            EventsController.Inst.RunDefenseEvent(6);
        }

        else if (finalOption == 2) // OUT
        {
            bool isSameSkater = IsSameSkater(PriorSkater, isPossHome, newPos);

            if (!isSameSkater)
            {
                string newPossPos = string.Empty;

                if (isPossHome)
                {
                    newPossPos = newPos.Contains("D") ? $"{newPos}{homePair}" : $"{newPos}{homeLine}";
                }

                else
                {
                    newPossPos = newPos.Contains("D") ? $"{newPos}{awayPair}" : $"{newPos}{awayLine}";
                }

                GameplayController.Inst.StatsSet.AddPossPos(newPossPos);
            }

            Skater newPossSkater = GameplayController.Inst.GetPossSkater();

            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Outside;
            EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = newPossSkater;
            EventsController.Inst.RunDefenseEvent(6);
        }

        else if (finalOption == 3) // TA
        {
            string newPossTeamString = isPossHome ? "Away" : "Home";
            string newPossPos = GameplayController.Inst.GetSkaterPos(DefendingSkater, newPossTeamString);

            GameplayController.Inst.StatsSet.AddGiveaway(PriorSkater, 1);
            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);
            GameplayController.Inst.StatsSet.AddTakeaway(DefendingSkater, 1);

            EventsController.Inst.RunDefenseEvent(4);
        }

        else if (finalOption == 4) // TA-OUT
        {
            string newPossTeamString = isPossHome ? "Away" : "Home";
            string newPossPos = GameplayController.Inst.GetSkaterPos(DefendingSkater, newPossTeamString);

            GameplayController.Inst.StatsSet.AddGiveaway(PriorSkater, 1);
            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);
            GameplayController.Inst.StatsSet.AddTakeaway(DefendingSkater, 1);

            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Outside;
            EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = DefendingSkater;
            EventsController.Inst.RunDefenseEvent(5);
        }

        else if (finalOption == 5) // TA-IN
        {
            string newPossTeamString = isPossHome ? "Away" : "Home";
            string newPossPos = GameplayController.Inst.GetSkaterPos(DefendingSkater, newPossTeamString);

            GameplayController.Inst.StatsSet.AddGiveaway(PriorSkater, 1);
            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);
            GameplayController.Inst.StatsSet.AddTakeaway(DefendingSkater, 1);

            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.Inside;
            EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = DefendingSkater;
            EventsController.Inst.RunDefenseEvent(5);
        }

        else // PENALTY
        {
            string newPossTeamString = isPossHome ? "Away" : "Home";
            string newPossPos = GameplayController.Inst.GetSkaterPos(DefendingSkater, newPossTeamString);

            GameplayController.Inst.StatsSet.AddGiveaway(PriorSkater, 1);
            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);
            GameplayController.Inst.StatsSet.AddTakeaway(DefendingSkater, 1);

            EventsController.Inst.GameplayEvents.PenaltyEvents.PenaltySkater = DefendingSkater;
            EventsController.Inst.RunDefenseEvent(7);
        }
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

    private bool IsSameSkater(Skater skater, bool isHome, string newPos)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Checking if same skater as new possession position.");

        GameTeam possTeam = isHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        string possPos = string.Empty;

        foreach (KeyValuePair<string, Skater> teamSkater in possTeam.SkaterLineup)
        {
            if (skater.Id == teamSkater.Value.Id)
            {
                string skaterPos = teamSkater.Key;
                possPos = skaterPos.Substring(0, skaterPos.Length - 1);
            }
        }

        return possPos == newPos;
    }
#endregion
}}
