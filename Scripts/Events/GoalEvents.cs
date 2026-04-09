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
public class GoalEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public Skater ShootingSkater;

    public Goalie DefendingGoalie;

    public ConstantController.GoalType GoalType;

    public int GoalThreshold;

    public Action PowerplayGoalAction;
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    public IEnumerator GoalieRatingStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingStart to the queue.");

        GameTeam defTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.AwayTeam : GameplayController.Inst.GameData.HomeTeam;
        DefendingGoalie = defTeam.GoalieLineup["G"];
        ShootingSkater = EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"One of the actions for a player card might be Goalie Rating. These are sets of actions each goalie has to further determine a play.",
            ActionText = $"{DefendingGoalie.Info.LastName} looks to be having trouble with the shot by {ShootingSkater.Info.LastName}.",
            ButtonText = "Check Goalie Rating",
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
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunFaceoffEvent(0); };

        yield return null;
    }

    public IEnumerator GoalieRatingResultBreakaway()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingResultBreakaway to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for the Goalie Rating, one of the outcomes might be a breakaway for a teammate.",
            ActionText = $"{DefendingGoalie.Info.LastName} quickly finds the puck and fires it down the ice for a teammate on a breakaway.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(2); };

        yield return null;
    }

    public IEnumerator GoalieRatingResultPenalty()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingResultPenalty to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for the Goalie Rating, one of the outcomes might be a penalty taken by the goalie.",
            ActionText = $"After chaos in the crease, it looks like the referees might call a penalty on {DefendingGoalie.Info.LastName}.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunPenaltyEvent(0); };

        yield return null;
    }

    public IEnumerator GoalieRatingResultGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalieRatingResultGoal to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After checking for the Goalie Rating, one of the outcomes might be a goal given up.",
            ActionText = $"The referee is signaling the puck is in the back of the net for a goal after the pileup in front of the goalie.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunGoalEvent(7); };

        yield return null;
    }

    public IEnumerator GoalCheck()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding GoalCheck to the queue.");

        ShootingSkater = EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"Some goal actions for skaters have a goal threshold. The higher the number, the more likely a goal will occur.",
            ActionText = $"We are going to step aside while the refs take a look at the video replay to see if the puck crossed the goal line.",
            ButtonText = "Check for Goal",
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
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunFaceoffEvent(0); };

        yield return null;
    }

    public IEnumerator Goal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding Goal to the queue.");

        ShootingSkater = EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater;

        string announcement = DetermineGoalAnnouncement();

        EventRun newEventRun = new EventRun
        {
            InfoText = $"When scoring a goal, the possession tracker also keeps track of assists. All who earned points on the goal will be mentioned.",
            ActionText = $"{announcement}",
            ButtonText = "Continue",
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

        List<string> actionOptions = new() { "GOAL", "PENALTY", "SAVE", "BREAKAWAY" };
        
        int stamina = 100;

        if (GameplayController.Inst.GameData.Type == "Season" && GameplayController.Inst.GameOptions.GoalieFatigueOn)
        {
            stamina = DefendingGoalie.Season.Stamina;
        }
        else if (GameplayController.Inst.GameData.Type == "Playoff" && GameplayController.Inst.GameOptions.GoalieFatigueOn)
        {
            stamina = DefendingGoalie.Playoff.Stamina;
        }

        int randomNumber = Random.Range(2,13);
        string ratingAction = DefendingGoalie.Card.GoalieRatingActions[randomNumber - 1];
        int staminaShift = 0;
        int finalOption = 0;

        if (stamina >= 80) { staminaShift = 0; }
        else if (stamina >= 50) { staminaShift = 1; }
        else { stamina = 2; }

        switch (ratingAction)
        {
            case "GOAL": finalOption = 0; break;
            case "PENALTY": finalOption = 1; break;
            case "SAVE": finalOption = 2; break;
            case "BREAKAWAY":
            default: finalOption = 3; break;
        }

        finalOption -= staminaShift;

        if (finalOption < 0) { finalOption = 0; }

        string finalAction = actionOptions[finalOption];

        if (finalAction == "SAVE")
        {
            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam("None");

            EventsController.Inst.RunGoalEvent(1);
        }

        else if (finalAction == "BREAKAWAY")
        {
            string newPos = GetRandomPos();

            GameTeam defTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.AwayTeam : GameplayController.Inst.GameData.HomeTeam;
            string newTeamPos = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            int lineNum = defTeam.CurrentLine;
            int pairNum = defTeam.CurrentPair;

            string newSkaterPos = newPos.Contains("D") ? $"{newPos}{pairNum}" : $"{newPos}{lineNum}";
            Skater newPossSkater = defTeam.SkaterLineup[newSkaterPos];

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newTeamPos);
            GameplayController.Inst.StatsSet.AddPossPos("G");
            GameplayController.Inst.StatsSet.AddPossPos(newSkaterPos);

            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ConstantController.ShotType.RebBreak;
            EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = newPossSkater;

            EventsController.Inst.RunGoalEvent(2);
        }

        else if (finalAction == "PENALTY")
        {
            GameTeam defTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.AwayTeam : GameplayController.Inst.GameData.HomeTeam;
            Skater inBoxSkater = defTeam.SkaterLineup["LW4"];

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam("None");

            EventsController.Inst.GameplayEvents.PenaltyEvents.PenaltySkater = inBoxSkater;
            EventsController.Inst.GameplayEvents.PenaltyEvents.PenaltyGoalie = DefendingGoalie;

            EventsController.Inst.RunGoalEvent(3);
        }

        else
        {
            GoalThreshold = 20;

            EventsController.Inst.RunGoalEvent(4);
        }
    }

    public void DetermineGoalOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the goal outcome.");
        
        int randomNumber = Random.Range(1,21);
        bool isUnderThreshold = randomNumber <= GoalThreshold;

        if (isUnderThreshold)
        {
            EventsController.Inst.RunGoalEvent(7);
        }

        else
        {
            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam("None");

            EventsController.Inst.RunGoalEvent(6);
        }
    }

    public void DetermineAfterGoalEvent()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the event after a goal is scored.");
        
        int currentPeriod = GameplayController.Inst.GameData.Period;

        GameplayController.Inst.StatsSet.ClearPossPos();
        GameplayController.Inst.StatsSet.SetPossTeam("None");

        if (currentPeriod > 3)
        {
            EventsController.Inst.RunGameFlowEvent(5);
        }

        else
        {
            if (PowerplayGoalAction != null)
            {
                PowerplayGoalAction?.Invoke();
            }

            else
            {
                EventsController.Inst.RunFaceoffEvent(0);
            }
        }
    }

    public string DetermineGoalAnnouncement()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the goal announcement.");
        
        GameplayController.Inst.StatsSet.AddGoal(ShootingSkater, EventsController.Inst.OffenseEvents.SelectedShotType, 1);

        bool isHome = GameplayController.Inst.GameData.PossTeam == "Home";
        GameTeam possTeam = isHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        Skater assistSkaterA = null;
        Skater assistSkaterB = null;

        Goalie assistGoalieA = null;
        Goalie assistGoalieB = null;

        List<string> assistPos = new();

        foreach (string pos in GameplayController.Inst.GameData.PossPos)
        {
            if (!assistPos.Contains(pos)) { assistPos.Add(pos); }
        }

        if (assistPos.Count > 2)
        {
            string assistPosA = assistPos[assistPos.Count - 2];
            string assistPosB = assistPos[assistPos.Count - 3];

            if (assistPosA == "G")
            {
                assistGoalieA = possTeam.GoalieLineup["G"];
                GameplayController.Inst.StatsSet.AddGoalieAssist(assistGoalieA, 1);
            }

            else
            {
                assistSkaterA = possTeam.SkaterLineup[assistPosA];
                GameplayController.Inst.StatsSet.AddAssist(assistSkaterA, EventsController.Inst.OffenseEvents.SelectedShotType, 1);
            }

            if (assistPosB == "G")
            {
                assistGoalieB = possTeam.GoalieLineup["G"];
                GameplayController.Inst.StatsSet.AddGoalieAssist(assistGoalieB, 1);
            }

            else
            {
                assistSkaterB = possTeam.SkaterLineup[assistPosB];
                GameplayController.Inst.StatsSet.AddAssist(assistSkaterB, EventsController.Inst.OffenseEvents.SelectedShotType, 1);
            }
        }

        else if (assistPos.Count == 2)
        {
            string assistPosA = assistPos[0];

            if (assistPosA == "G") { assistGoalieA = possTeam.GoalieLineup["G"]; }
            else { assistSkaterA = possTeam.SkaterLineup[assistPosA]; }
        }

        if (EventsController.Inst.OffenseEvents.SelectedShotType != ConstantController.ShotType.Powerplay && 
            EventsController.Inst.OffenseEvents.SelectedShotType != ConstantController.ShotType.Shorthanded
        )
        {
            GameplayController.Inst.StatsSet.AddTeamPlusMinus(isHome, 1);
            GameplayController.Inst.StatsSet.AddTeamPlusMinus(!isHome, -1);
        }
        
        string goalAnnouncement = string.Empty;
        string goalTypeString = string.Empty;

        if (EventsController.Inst.OffenseEvents.SelectedShotType == ConstantController.ShotType.Powerplay) { goalTypeString = "on the powerplay"; }
        else if (EventsController.Inst.OffenseEvents.SelectedShotType == ConstantController.ShotType.Shorthanded) { goalTypeString = "while shorthanded"; }

        string goalCountString = "With their first goal of the night";

        if (ShootingSkater.Game.Goals == 2) { goalCountString = "Netting their second goal tonight"; }
        else if (ShootingSkater.Game.Goals == 3) { goalCountString = "With that hat trick goal"; }
        else if (ShootingSkater.Game.Goals > 3) { goalCountString = "After many goals scored tonight"; }

        string gameTypeString = string.Empty;

        if (GameplayController.Inst.GameData.Type == "Season")
        {
            int seasonGoals = ShootingSkater.Season.Goals += ShootingSkater.Game.Goals;
            gameTypeString = $" (number {seasonGoals} on the season)";
        }

        else if (GameplayController.Inst.GameData.Type == "Playoff")
        {
            int playoffGoals = ShootingSkater.Playoff.Goals += ShootingSkater.Game.Goals;
            gameTypeString = $" (number {playoffGoals} of the playoffs)";
        }

        goalAnnouncement = $"{goalCountString}, {ShootingSkater.Info.FirstName} {ShootingSkater.Info.LastName} scores {goalTypeString}{gameTypeString}.";

        string addedAssistsString = string.Empty;

        if (assistSkaterA == null)
        {
            if (assistGoalieA != null)
            {
                addedAssistsString = $" The goal is assisted by {assistGoalieA.Info.FirstName} {assistsGoalieA.Info.LastName}";
            }

            else
            {
                addedAssistsString = " The goal was unassisted";
            }
        }

        else
        {
            addedAssistsString = $" The goal is assisted by {assistSkaterA.Info.FirstName} {assistSkaterA.Info.LastName}";
        }

        if (assistSkaterB == null)
        {
            if (assistGoalieB != null)
            {
                addedAssistsString += $" and by {assistGoalieB.Info.FirstName} {assistGoalieB.Info.LastName}.";
            }

            else
            {
                addedAssistsString += ".";
            }
        }

        else
        {
            addedAssistsString += $" and by {assistSkaterB.Info.FirstName} {assistSkaterB.Info.LastName}.";
        }

        goalAnnouncement += addedAssistsString;

        return goalAnnouncement;
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
