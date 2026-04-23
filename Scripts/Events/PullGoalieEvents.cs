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

    public GameTeam PulledGoalieTeam;

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

        GameplayController.Inst.GameData.HomeTeam.CurrentLine = 1;
        GameplayController.Inst.GameData.HomeTeam.CurrentPair = 1;
        GameplayController.Inst.GameData.HomeTeam.NextLine = 1;
        GameplayController.Inst.GameData.HomeTeam.NextPair = 1;

        GameplayController.Inst.GameData.AwayTeam.CurrentLine = 1;
        GameplayController.Inst.GameData.AwayTeam.CurrentPair = 1;
        GameplayController.Inst.GameData.AwayTeam.NextLine = 1;
        GameplayController.Inst.GameData.AwayTeam.NextPair = 1;

        GameplayController.Inst.StatsSet.ResetFullTeamStamina(true);
        GameplayController.Inst.StatsSet.ResetFullTeamStamina(false);

        PulledGoalieTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        GameplayController.Inst.GameData.PullGoalieTeam = GameplayController.Inst.GameData.PossTeam == "Home" ? "Home" : "Away";

        EventRun newEventRun = new EventRun
        {
            InfoText = $"When a team pulls their goalie, they will get an extra attacker to help attempt to generate some offense and score.",
            ActionText = $"It looks like the coach for the {PulledGoalieTeam.Team.NickName} is calling over the goalie, trying to get an extra attacker on the ice.",
            ButtonText = "Continue",
        };

        PulledGoalieTeam.IsGoaliePulled = false;
        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = GeneratePullGoalieShots;

        yield return null;
    }

    public IEnumerator PullGoalieShotsStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PullGoalieShotsStart to the queue.");

        EventRun newEventRun = new EventRun
        {
            InfoText = $"At the start after a team pulls their goalie, a shot list is generated based on the team's overall Offense ratings.",
            ActionText = $"With the goalie pulled, the {PulledGoalieTeam.Team.CityName} {PulledGoalieTeam.Team.NickName} look to add some offense to their game.",
            ButtonText = "Continue",
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
            ButtonText = (GameplayController.Inst.GameData.PossTeam == "Home") ? "Attempt Shot" : "Continue",
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
            ButtonText = "Continue",
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
            ButtonText = "Continue",
        };

        EventsController.Inst.GameplayEvents.GoalEvents.ShootingSkater = ShootingSkater;
        EventsController.Inst.GameplayEvents.GoalEvents.GoalType = ConstantController.GoalType.EvenStrength;
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
            ButtonText = "Continue",
        };

        EventsController.Inst.GameplayEvents.GoalEvents.ShootingSkater = ShootingSkater;
        EventsController.Inst.GameplayEvents.GoalEvents.GoalType = ConstantController.GoalType.EmptyNet;
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
            ButtonText = "Continue",
        };

        GameplayController.Inst.GameData.PullGoalieTeam = "None";
        EventsController.Inst.CurrentEventRun = newEventRun;
        EventsController.Inst.ContinueAction = () => { EventsController.Inst.RunFaceoffEvent(0); };

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    public void GeneratePullGoalieShots()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Generating the pull goalie shot list.");

        PullGoalieShots.Clear();
        EmptyNetShots.Clear();

        GameplayController.Inst.GameData.CardsDrawn += 3;

        string extraAttackerTeam = GameplayController.Inst.GameData.PullGoalieTeam == "Home" ? "Home" : "Away";
        string emptyNetTeam = GameplayController.Inst.GameData.PullGoalieTeam == "Home" ? "Away" : "Home";

        GameTeam eaTeam = extraAttackerTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        GameTeam enTeam = extraAttackerTeam == "Home" ? GameplayController.Inst.GameData.AwayTeam : GameplayController.Inst.GameData.HomeTeam;

        int eaOffense = 0;
        int enDefense = 0;

        if (eaTeam.SkaterLineup["C1"].Card.Offense == 4) { eaOffense += 1; }
        if (eaTeam.SkaterLineup["LW1"].Card.Offense == 4) { eaOffense += 1; }
        if (eaTeam.SkaterLineup["RW1"].Card.Offense == 4) { eaOffense += 1; }
        if (eaTeam.SkaterLineup["LD1"].Card.Offense == 4) { eaOffense += 1; }
        if (eaTeam.SkaterLineup["RD1"].Card.Offense == 4) { eaOffense += 1; }

        ExtraSkater = eaTeam.SkaterLineup["C2"].Id == eaTeam.SkaterLineup["C1"].Id ? eaTeam.SkaterLineup["C3"] : eaTeam.SkaterLineup["C2"];

        if (ExtraSkater.Card.Offense == 4) { eaOffense += 1; }

        if (enTeam.SkaterLineup["C1"].Card.Defense == 4) { enDefense += 1; }
        if (enTeam.SkaterLineup["LW1"].Card.Defense == 4) { enDefense += 1; }
        if (enTeam.SkaterLineup["RW1"].Card.Defense == 4) { enDefense += 1; }
        if (enTeam.SkaterLineup["LD1"].Card.Defense == 4) { enDefense += 1; }
        if (enTeam.SkaterLineup["RD1"].Card.Defense == 4) { enDefense += 1; }

        for (int en = 0; en < enDefense; en++)
        {
            EmptyNetShots.Add(GetRandomShot());
        }

        for (int ea = 0; ea < eaOffense; ea++)
        {
            PullGoalieShots.Add(GetRandomShot());
        }

        if (EmptyNetShots.Count > 0)
        {
            string firstShot = EmptyNetShots[0];
            string firstPos = GetRandomPos();

            if (firstShot == "OUT") { ShotType = ConstantController.ShotType.Outside; }
            else if (firstShot == "IN") { ShotType = ConstantController.ShotType.Inside; }
            else { ShotType = ConstantController.ShotType.RebBreak; }

            ShootingSkater = enTeam.SkaterLineup[$"{firstPos}1"];

            GameplayController.Inst.StatsSet.SetPossTeam(emptyNetTeam);
            GameplayController.Inst.StatsSet.AddPossPos($"{firstPos}1");

            IsEmptyNetShot = true;
        }

        else
        {
            string firstShot = PullGoalieShots[0];
            string firstPos = GetRandomPos();
            int firstLine = 1;

            if (firstPos == "C")
            {
                firstLine = Random.Range(0,2) == 0 ? 1 : 2;
            }

            if (firstShot == "OUT") { ShotType = ConstantController.ShotType.Outside; }
            else if (firstShot == "IN") { ShotType = ConstantController.ShotType.Inside; }
            else { ShotType = ConstantController.ShotType.RebBreak; }

            ShootingSkater = eaTeam.SkaterLineup[$"{firstPos}{firstLine}"];

            GameplayController.Inst.StatsSet.SetPossTeam(extraAttackerTeam);
            GameplayController.Inst.StatsSet.AddPossPos($"{firstPos}{firstLine}");

            IsEmptyNetShot = false;
        }

        EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ShotType;
        EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = ShootingSkater;
        EventsController.Inst.RunPullGoalieEvent(1);
    }

    public void DetermineNextPullGoalieShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the next pull goalie shot.");
        
        string extraAttackerTeam = GameplayController.Inst.GameData.PullGoalieTeam == "Home" ? "Home" : "Away";
        string emptyNetTeam = GameplayController.Inst.GameData.PullGoalieTeam == "Home" ? "Away" : "Home";

        GameTeam eaTeam = extraAttackerTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        GameTeam enTeam = extraAttackerTeam == "Home" ? GameplayController.Inst.GameData.AwayTeam : GameplayController.Inst.GameData.HomeTeam;
        
        if (IsEmptyNetShot)
        {
            EmptyNetShots.RemoveAt(0);

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam("None");
        }

        else
        {
            PullGoalieShots.RemoveAt(0);
        }

        if (EmptyNetShots.Count > 0) { IsEmptyNetShot = true; }
        else { IsEmptyNetShot = false; }

        if (EmptyNetShots.Count > 0)
        {
            string firstShot = EmptyNetShots[0];
            string firstPos = GetRandomPos();

            if (firstShot == "OUT") { ShotType = ConstantController.ShotType.Outside; }
            else if (firstShot == "IN") { ShotType = ConstantController.ShotType.Inside; }
            else { ShotType = ConstantController.ShotType.RebBreak; }

            ShootingSkater = enTeam.SkaterLineup[$"{firstPos}1"];

            GameplayController.Inst.StatsSet.SetPossTeam(emptyNetTeam);
            GameplayController.Inst.StatsSet.AddPossPos($"{firstPos}1");

            IsEmptyNetShot = true;

            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ShotType;
            EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = ShootingSkater;
            EventsController.Inst.RunPullGoalieEvent(2);
        }

        else
        {
            IsEmptyNetShot = false;

            if (PullGoalieShots.Count > 0)
            {
                string firstShot = PullGoalieShots[0];
                string firstPos = GetRandomPos();
                int firstLine = 1;

                if (firstPos == "C")
                {
                    firstLine = Random.Range(0,2) == 0 ? 1 : 2;
                }

                if (firstShot == "OUT") { ShotType = ConstantController.ShotType.Outside; }
                else if (firstShot == "IN") { ShotType = ConstantController.ShotType.Inside; }
                else { ShotType = ConstantController.ShotType.RebBreak; }

                ShootingSkater = eaTeam.SkaterLineup[$"{firstPos}{firstLine}"];

                GameplayController.Inst.StatsSet.SetPossTeam(extraAttackerTeam);
                GameplayController.Inst.StatsSet.AddPossPos($"{firstPos}{firstLine}");

                EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ShotType;
                EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = ShootingSkater;
                EventsController.Inst.RunPullGoalieEvent(2);
            }

            else
            {
                GameplayController.Inst.StatsSet.ClearPossPos();
                GameplayController.Inst.StatsSet.SetPossTeam("None");

                EventsController.Inst.RunPullGoalieEvent(6);
            }
        }
    }

    public void DeterminePullGoalieShotOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the pull goalie shot outcome.");
        
        int randomNumber = Random.Range(0,11);

        List<string> shotActions = new();

        if (ShotType == ConstantController.ShotType.Outside) { shotActions = ShootingSkater.Card.OutsideShotActions; }
        else if (ShotType == ConstantController.ShotType.Inside) { shotActions = ShootingSkater.Card.InsideShotActions; }
        else { shotActions = ShootingSkater.Card.ReboundShotActions; }

        string shotAction = shotActions[randomNumber];

        GameplayController.Inst.StatsSet.AddShot(ShootingSkater, 1);

        if (shotAction == "GOAL" || shotAction == "GOALIE RATING")
        {
            if (IsEmptyNetShot) { EventsController.Inst.RunPullGoalieEvent(5); }
            else { EventsController.Inst.RunPullGoalieEvent(4); }
        }

        else
        {
            EventsController.Inst.RunPullGoalieEvent(3);
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
            case 4: return "IN";
            case 5:
            default: return "REB";
        }
    }
#endregion
}}
