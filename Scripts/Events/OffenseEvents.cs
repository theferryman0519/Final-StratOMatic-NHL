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
            ButtonText = (GameplayController.InstGameData.PossTeam == "Home") ? "Draw Action Card" : "Continue",
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

        EventsController.Inst.MainUi.IsOutsideOptions = GameplayController.Inst.GameData.PossTeam != "Away";

        EventRun newEventRun = new EventRun
        {
            InfoText = $"If a skater has an Outside Shot, they might have options to choose from instead of taking a shot. They could attempt a pass or drive the defense.",
            ActionText = $"{possSkater.Info.FirstName} {possSkater.Info.LastName} is thinking about taking a shot, passing the puck, or driving the defense.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = AiPickOption;

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
            ButtonText = (GameplayController.InstGameData.PossTeam == "Home") ? "Attempt Shot" : "Continue",
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
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(0); };

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
            ActionText = $"The shot by {ShootingSkater.Info.LastName} was saved and given to {possSkater.Info.FirstName} {possSkater.Info.LastName}.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(0); };

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
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(8); };

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
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunGoalEvent(0); };

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
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunGoalEvent(5); };

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
            ButtonText = "Check Rebound",
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
            ButtonText = (GameplayController.InstGameData.PossTeam == "Home") ? "Attempt Pass" : "Continue",
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
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(0); };

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
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(2); };

        PassingSkater = null;

        yield return null;
    }

    public IEnumerator PassingResultShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultShot to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        bool isSameSkater = false;

        if (GameplayController.Inst.GameData.PossPos.Count > 1)
        {
            int passCount = GameplayController.Inst.GameData.PossPos.Count;

            if (GameplayController.Inst.GameData.PossPos[passCount -1] == GameplayController.Inst.GameData.PossPos[passCount - 2])
            {
                isSameSkater = true;
            }
        }

        string passedSkater = isSameSkater ? "themself" : possSkater.Info.LastName;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting a pass, the pass might be successful and a shot attempt taken by a teammate.",
            ActionText = $"{PassingSkater.Info.LastName} passes the puck to {passedSkater}, who appears to be attempting a shot on net.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(2); };

        PassingSkater = null;

        yield return null;
    }

    public IEnumerator PassingResultShotIntimidation()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultShotIntimidation to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        bool isSameSkater = false;

        if (GameplayController.Inst.GameData.PossPos.Count > 1)
        {
            int passCount = GameplayController.Inst.GameData.PossPos.Count;

            if (GameplayController.Inst.GameData.PossPos[passCount -1] == GameplayController.Inst.GameData.PossPos[passCount - 2])
            {
                isSameSkater = true;
            }
        }

        string passedSkater = isSameSkater ? "themself" : possSkater.Info.LastName;

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting a pass, the pass might be successful and a teammate has a chance at an Inside Shot after an opponent attempts to intimidate.",
            ActionText = $"The pass by {PassingSkater.Info.LastName} successfully goes to {passedSkater}, who looks to attempt an Inside Shot.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunDefenseEvent(0); };

        PassingSkater = null;

        yield return null;
    }

    public IEnumerator PassingResultOptions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultOptions to the queue.");

        Skater possSkater = GameplayController.Inst.GetPossSkater();

        bool isSameSkater = false;

        if (GameplayController.Inst.GameData.PossPos.Count > 1)
        {
            int passCount = GameplayController.Inst.GameData.PossPos.Count;

            if (GameplayController.Inst.GameData.PossPos[passCount -1] == GameplayController.Inst.GameData.PossPos[passCount - 2])
            {
                isSameSkater = true;
            }
        }

        string passedSkater = isSameSkater ? $"After grabbing their own pass" : $"After a pass from {PassingSkater.Info.LastName}";

        EventRun newEventRun = new EventRun
        {
            InfoText = $"After attempting a pass, the pass might be successful and a teammate has options on the offense.",
            ActionText = $"{passedSkater}, {possSkater.Info.FirstName} {possSkater.Info.LastName} has the puck and is looking to generate some offense.",
            ButtonText = "Continue",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunOffenseEvent(1); };

        PassingSkater = null;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public async void PickActionCard()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Picking an Action Card.");

        GameDatabase saveGame = SaveController.Inst.GetCurrentGameSaveData();
		
		await FirebaseController.Inst.PutCurrentGame(saveGame, UsersController.Inst.UserData.Id, () =>
		{
			SetCardsDrawn(() =>
            {
                CheckForGoaliePull(() =>
                {
                    CheckForInjuries(() =>
                    {
                        UpdateLinesPairs(() =>
                        {
                            DrawActionCard();
                        });
                    });
                });
            });
		});
    }

    public void DetermineShotOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the shot outcome.");
        
        int skaterFatigue = ShootingSkater.Game.Stamina;
        int randomNumber = Random.Range(0,11);
        int staminaShift = 0;
        int finalAction = 4;

        List<string> shotActions = new();

        if (SelectedShotType == ConstantController.ShotType.Outside) { shotActions = ShootingSkater.Card.OutsideShotActions; }
        else if (SelectedShotType == ConstantController.ShotType.Inside) { shotActions = ShootingSkater.Card.InsideShotActions; }
        else { shotActions = ShootingSkater.Card.ReboundShotActions; }

        string shotAction = shotActions[randomNumber];

        if (skaterFatigue >= 85) { staminaShift = 0; }
        else if (skaterFatigue >= 60) { staminaShift = 1; }
        else if (skaterFatigue >= 45) { staminaShift = 2; }
        else if (skaterFatigue >= 30) { staminaShift = 3; }
        else if (skaterFatigue >= 15) { staminaShift = 4; }
        else { skaterFatigue = 5; }

        switch (shotAction)
        {
            case "REBOUND": finalAction = 10; break;
            case "LOSE": finalAction = 1; break;
            case "SHOT": finalAction = 2; break;
            case "GOALIE RATING": finalAction = 3; break;
            case "GOAL":
            default: finalAction = 4; break;
        }

        if (finalAction < 10) { finalAction -= staminaShift; }
        if (finalAction < 1) { finalAction = 1; }

        if (finalAction == 1) // LOSE
        {
            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameTeam newPossTeam = newPossTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            Skater possSkater = GameplayController.Inst.GetPossSkater();

            GameplayController.Inst.StatsSet.AddBlockedShot(possSkater, 1);

            EventsController.Inst.RunOffenseEvent(3);
        }

        else if (finalAction == 2) // SHOT
        {
            GameplayController.Inst.StatsSet.AddShot(ShootingSkater, 1);

            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameTeam newPossTeam = newPossTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            EventsController.Inst.RunOffenseEvent(4);
        }

        else if (finalAction == 3) // GOALIE RATING
        {
            GameplayController.Inst.StatsSet.AddShot(ShootingSkater, 1);

            EventsController.Inst.RunOffenseEvent(6);
        }

        else if (finalAction == 4) // GOAL
        {
            GameplayController.Inst.StatsSet.AddShot(ShootingSkater, 1);

            if (shotAction == "GOAL") { EventsController.Inst.GameplayEvents.GoalEvents.GoalThreshold = 20; }
            else if (shotAction == "GOAL 1") { EventsController.Inst.GameplayEvents.GoalEvents.GoalThreshold = 1; }
            else
            {
                string[] actionSplit = shotAction.Split("-");
                int actionThreshold = Int32.Parse(actionSplit[1]);

                EventsController.Inst.GameplayEvents.GoalEvents.GoalThreshold = actionThreshold;
            }

            EventsController.Inst.RunOffenseEvent(7);
        }

        else // REBOUND
        {
            GameplayController.Inst.StatsSet.AddShot(ShootingSkater, 1);

            EventsController.Inst.RunOffenseEvent(5);
        }
    }

    public void DetermineReboundOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the rebound outcome.");
        
        string randomPos = GetRandomPos();
        string randomTeamString = Random.Range(0,2) == 0 ? "Home" : "Away";

        GameTeam randomTeam = randomTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        if (randomTeamString == GameplayController.Inst.GameData.PossTeam)
        {
            int teamLine = randomTeam.CurrentLine;
            int teamPair = randomTeam.CurrentPair;

            string teamPos = randomPos.Contains("D") ? $"{randomPos}{teamPair}" : $"{randomPos}{teamLine}";

            GameplayController.Inst.StatsSet.AddPossPos(teamPos);

            ShootingSkater = GameplayController.Inst.GetPossSkater();

            SelectedShotType = ConstantController.ShotType.RebBreak;
            EventsController.Inst.RunOffenseEvent(2);
        }

        else
        {
            int teamLine = randomTeam.CurrentLine;
            int teamPair = randomTeam.CurrentPair;

            string teamPos = randomPos.Contains("D") ? $"{randomPos}{teamPair}" : $"{randomPos}{teamLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(randomTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(teamPos);

            ShootingSkater = null;

            EventsController.Inst.RunOffenseEvent(0);
        }
    }

    public void DeterminePassingOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the pass outcome.");
        
        int skaterFatigue = PassingSkater.Game.Stamina;
        int randomNumber = Random.Range(0,11);
        int staminaShift = 0;
        int finalAction = 1;

        string passAction = PassingSkater.Card.PassingActions[randomNumber];

        if (skaterFatigue >= 85) { staminaShift = 0; }
        else if (skaterFatigue >= 60) { staminaShift = 1; }
        else if (skaterFatigue >= 45) { staminaShift = 2; }
        else if (skaterFatigue >= 30) { staminaShift = 3; }
        else if (skaterFatigue >= 15) { staminaShift = 4; }
        else { skaterFatigue = 5; }

        switch (passAction)
        {
            case "IN": finalAction = 5; break;
            case "OUT": finalAction = 4; break;
            case "LOSE": finalAction = 3; break;
            case "LOSE OUT": finalAction = 2; break;
            case "LOSE IN":
            default: finalAction = 1; break;
        }

        finalAction -= staminaShift;

        if (finalAction < 1) { finalAction = 1; }

        if (finalAction == 1) // LOSE IN
        {
            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameTeam newPossTeam = newPossTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            ShootingSkater = GameplayController.Inst.GetPossSkater();

            SelectedShotType = ConstantController.ShotType.Inside;
            EventsController.Inst.RunOffenseEvent(11);
        }

        else if (finalAction == 2) // LOSE OUT
        {
            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameTeam newPossTeam = newPossTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            ShootingSkater = GameplayController.Inst.GetPossSkater();

            SelectedShotType = ConstantController.ShotType.Outside;
            EventsController.Inst.RunOffenseEvent(11);
        }

        else if (finalAction == 3) // LOSE
        {
            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameTeam newPossTeam = newPossTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            EventsController.Inst.RunOffenseEvent(10);
        }

        else if (finalAction == 4) // OUT
        {
            GameTeam newPossTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            ShootingSkater = GameplayController.Inst.GetPossSkater();

            EventsController.Inst.RunOffenseEvent(14);
        }

        else // IN
        {
            GameTeam newPossTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            ShootingSkater = GameplayController.Inst.GetPossSkater();

            EventsController.Inst.RunOffenseEvent(13);
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private void SetCardsDrawn(Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the cards drawn count.");

        GameplayController.Inst.GameData.CardsDrawn += 1;

        if (GameplayController.Inst.GameData.CardsDrawn > 30)
        {
            EventsController.Inst.RunGameFlowEvent(3);
            return;
        }

        continueAction?.Invoke();
    }

    private void CheckForGoaliePull(Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Checking for potential goalie pull.");

        if (GameplayController.Inst.GameData.AwayUserType == "Ai")
        {
            AiChooseToPullGoalie();
        }

        GameTeam possTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

        if (possTeam.IsGoaliePulled)
        {
            EventsController.Inst.RunPullGoalieEvent(0);
            return;
        }

        continueAction?.Invoke();
    }

    private void AiChooseToPullGoalie()
    {
        GameTeam aiTeam = GameplayController.Inst.GameData.AwayTeam;

        if (GameplayController.Inst.GameData.Period == 3)
        {
            int pullThreshold = AiController.Inst.GetAiPullGoalieNoise();

            if (GameplayController.Inst.GameData.CardsDrawn >= pullThreshold)
            {
                if (aiTeam.Stats.Goals < GameplayController.Inst.GameData.HomeTeam.Stats.Goals)
                {
                    int diff = GameplayController.Inst.GameData.HomeTeam.Stats.Goals - aiTeam.Stats.Goals;

                    if (diff <= 3)
                    {
                        aiTeam.IsGoaliePulled = true;
                    }
                }
            }
        }
    }

    private void CheckForInjuries(Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Checking for potential injuries.");

        if (GameplayController.Inst.GameOptions.InjuriesOn)
        {
            Skater possSkater = GameplayController.Inst.GetPossSkater();
            int skaterFatigue = possSkater.Game.Stamina;
            int randomInjury = Random.Range(0,100);
            int injuryThreshold = 0;

            if (skaterFatigue >= 85) { injuryThreshold = 0; }
            else if (skaterFatigue >= 60) { injuryThreshold = 1; }
            else if (skaterFatigue >= 45) { injuryThreshold = 2; }
            else if (skaterFatigue >= 30) { injuryThreshold = 3; }
            else if (skaterFatigue >= 15) { injuryThreshold = 4; }
            else { injuryThreshold = 5; }

            if (injuryThreshold > randomInjury)
            {
                EventsController.Inst.GameplayEvents.GameFlowEvents.InjuredSkater = possSkater;
                EventsController.Inst.RunGameFlowEvent(2);
                return;
            }
        }

        continueAction?.Invoke();
    }

    private void UpdateLinesPairs(Action continueAction = null)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the lines and pairs.");

        if (GameplayController.Inst.GameOptions.LineChangesOn)
        {
            bool isPossTeamHome = GameplayController.Inst.GameData.PossTeam == "Home";

            if (GameplayController.Inst.GameData.AwayUserType == "Ai")
            {
                AiChooseNewLinesPairs();
            }

            GameplayController.Inst.StatsSet.AddTeamStamina(isPossTeamHome);
            GameplayController.Inst.StatsSet.AddTeamStamina(!isPossTeamHome);

            GameplayController.Inst.GameData.HomeTeam.CurrentLine = GameplayController.Inst.GameData.HomeTeam.NextLine;
            GameplayController.Inst.GameData.HomeTeam.CurrentPair = GameplayController.Inst.GameData.HomeTeam.NextPair;
            GameplayController.Inst.GameData.AwayTeam.CurrentLine = GameplayController.Inst.GameData.AwayTeam.NextLine;
            GameplayController.Inst.GameData.AwayTeam.CurrentPair = GameplayController.Inst.GameData.AwayTeam.NextPair;

            GameTeam possTeam = isPossTeamHome ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = possTeam.CurrentLine;
            int newPossPair = possTeam.CurrentPair;

            string poss = GameplayController.Inst.GameData.PossPos[GameplayController.Inst.GameData.PossPos.Count - 1];
            string possPos = poss.Substring(0, poss.Length - 1);
            string newPossPos = possPos.Contains("D") ? $"{possPos}{newPossPair}" : $"{possPos}{newPossLine}";

            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);
        }

        GameplayController.Inst.GameData.HomeTeam.CurrentStrategy = GameplayController.Inst.GameData.HomeTeam.NextStrategy;
        GameplayController.Inst.GameData.AwayTeam.CurrentStrategy = GameplayController.Inst.GameData.AwayTeam.NextStrategy;

        continueAction?.Invoke();
    }

    private void AiChooseNewLinesPairs()
    {
        GameTeam aiTeam = GameplayController.Inst.GameData.AwayTeam;

        int aiLineThreshold = AiController.Inst.GetAiStaminaNoise();
        int aiPairThreshold = AiController.Inst.GetAiStaminaNoise();

        int aiCurrentLine = aiTeam.CurrentLine;
        int aiCurrentPair = aiTeam.CurrentPair;

        aiTeam.NextLine = aiCurrentLine;
        aiTeam.NextPair = aiCurrentPair;

        if (aiTeam.SkaterLineup[$"LW{aiCurrentLine}"].Game.Stamina <= aiLineThreshold)
        {
            aiTeam.NextLine = AiController.Inst.GetAiNextLine(aiCurrentLine);
        }

        if (aiTeam.SkaterLineup[$"LD{aiCurrentPair}"].Game.Stamina <= aiPairThreshold)
        {
            aiTeam.NextPair = AiController.Inst.GetAiNextPair(aiCurrentPair);
        }
    }

    private void DrawActionCard()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Drawing a new action card.");

        List<string> actionOptions = new() { "LOSE BREAK", "LOSE IN", "LOSE OUT", "LOSE", "DEF", "PASS", "OUT", "IN", "BREAK" };

        GameTeam possTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        Skater possSkater = GameplayController.Inst.GetPossSkater();

        int skaterFatigue = possSkater.Game.Stamina;
        int randomNumber = Random.Range(0,9);
        int teamStrategy = possTeam.CurrentStrategy - 3;
        int strategyShift = Random.Range(-teamStrategy, teamStrategy + 1);
        int staminaShift = 0;
        int finalAction = 0;

        if (skaterFatigue >= 85) { staminaShift = 0; }
        else if (skaterFatigue >= 60) { staminaShift = 1; }
        else if (skaterFatigue >= 45) { staminaShift = 2; }
        else if (skaterFatigue >= 30) { staminaShift = 3; }
        else if (skaterFatigue >= 15) { staminaShift = 4; }
        else { skaterFatigue = 5; }

        finalAction = randomNumber - staminaShift + strategyShift;

        if (finalAction > 8) { finalAction = 8; }
        else if (finalAction < 0) { finalAction = 0; }

        string finalActionString = actionOptions[finalAction];

        if (finalActionString == "LOSE BREAK")
        {
            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameTeam newPossTeam = newPossTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            ShootingSkater = newPossTeam.SkaterLineup[newPossPos];
            SelectedShotType = ConstantController.ShotType.RebBreak;
            EventsController.Inst.RunOffenseEvent(2);
        }

        else if (finalActionString == "LOSE IN")
        {
            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameTeam newPossTeam = newPossTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            SelectedShotType = ConstantController.ShotType.Inside;
            ShootingSkater = newPossTeam.SkaterLineup[newPossPos];
            EventsController.Inst.RunDefenseEvent(0);
        }

        else if (finalActionString == "LOSE OUT")
        {
            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameTeam newPossTeam = newPossTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            ShootingSkater = newPossTeam.SkaterLineup[newPossPos];
            SelectedShotType = ConstantController.ShotType.Outside;
            EventsController.Inst.RunOffenseEvent(1);
        }

        else if (finalActionString == "LOSE")
        {
            string newPossTeamString = GameplayController.Inst.GameData.PossTeam == "Home" ? "Away" : "Home";

            GameTeam newPossTeam = newPossTeamString == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;

            int newPossLine = newPossTeam.CurrentLine;
            int newPossPair = newPossTeam.CurrentPair;

            string newPos = GetRandomPos();
            string newPossPos = newPos.Contains("D") ? $"{newPos}{newPossPair}" : $"{newPos}{newPossLine}";

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam(newPossTeamString);
            GameplayController.Inst.StatsSet.AddPossPos(newPossPos);

            EventsController.Inst.RunOffenseEvent(0);
        }

        else if (finalActionString == "DEF")
        {
            int possLine = possTeam.CurrentLine;
            int possPair = possTeam.CurrentPair;

            string pos = GetRandomPos();
            string possPos = pos.Contains("D") ? $"{pos}{possPair}" : $"{pos}{possLine}";

            GameplayController.Inst.StatsSet.AddPossPos(possPos);
            
            EventsController.Inst.RunDefenseEvent(3);
        }

        else if (finalActionString == "PASS")
        {
            int possLine = possTeam.CurrentLine;
            int possPair = possTeam.CurrentPair;

            string pos = GetRandomPos();
            string possPos = pos.Contains("D") ? $"{pos}{possPair}" : $"{pos}{possLine}";

            GameplayController.Inst.StatsSet.AddPossPos(possPos);

            EventsController.Inst.RunOffenseEvent(9);
        }

        else if (finalActionString == "OUT")
        {
            int possLine = possTeam.CurrentLine;
            int possPair = possTeam.CurrentPair;

            string pos = GetRandomPos();
            string possPos = pos.Contains("D") ? $"{pos}{possPair}" : $"{pos}{possLine}";

            GameplayController.Inst.StatsSet.AddPossPos(possPos);

            ShootingSkater = possTeam.SkaterLineup[possPos];
            SelectedShotType = ConstantController.ShotType.Outside;
            EventsController.Inst.RunOffenseEvent(1);
        }

        else if (finalActionString == "IN")
        {
            int possLine = possTeam.CurrentLine;
            int possPair = possTeam.CurrentPair;

            string pos = GetRandomPos();
            string possPos = pos.Contains("D") ? $"{pos}{possPair}" : $"{pos}{possLine}";

            GameplayController.Inst.StatsSet.AddPossPos(possPos);

            SelectedShotType = ConstantController.ShotType.Inside;
            ShootingSkater = possTeam.SkaterLineup[possPos];
            EventsController.Inst.RunDefenseEvent(0);
        }

        else
        {
            int possLine = possTeam.CurrentLine;
            int possPair = possTeam.CurrentPair;

            string pos = GetRandomPos();
            string possPos = pos.Contains("D") ? $"{pos}{possPair}" : $"{pos}{possLine}";

            GameplayController.Inst.StatsSet.AddPossPos(possPos);

            ShootingSkater = possTeam.SkaterLineup[possPos];
            SelectedShotType = ConstantController.ShotType.RebBreak;
            EventsController.Inst.RunOffenseEvent(2);
        }
    }

    private void AiPickOption()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Ai is selecting outside shot option.");

        int noise = AiController.Inst.GetAiNoise();

        if (noise >= 3)
        {
            SelectedShotType = ConstantController.ShotType.Outside;
            EventsController.Inst.RunOffenseEvent(2);
        }

        else if (noise >= 1)
        {
            EventsController.Inst.RunOffenseEvent(9);
        }

        else {
            SelectedShotType = ConstantController.ShotType.Inside;
            EventsController.Inst.RunDefenseEvent(0);
        }
    }

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
