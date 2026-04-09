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
            GameplayController.Inst.GameData.PowerplayTeam = "None";
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

        if (randomNum >= thresholdNumB) { PenaltyTime = 5; IsMajorPenalty = true; }
        else if (randomNum >= thresholdNumA) { PenaltyTime = 4; IsMajorPenalty = false; }
        else { PenaltyTime = 2; IsMajorPenalty = false; }
    }

    public void GeneratePenaltyShots()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Generating the penalty shot list.");

        EventsController.Inst.GameplayEvents.GoalEvents.PowerplayGoalAction = null;

        PenaltyShots.Clear();
        ShorthandedShots.Clear();

        GameplayController.Inst.GameData.CardsDrawn -= 3;

        string powerplayTeam = GameplayController.Inst.GameData.PowerplayTeam == "Home" ? "Home" : "Away";
        string penaltyKillTeam = GameplayController.Inst.GameData.PowerplayTeam == "Home" ? "Away" : "Home";

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

        if (ShorthandedShots.Count > 0)
        {
            string firstShot = ShorthandedShots[0];
            string firstPos = GetRandomPos();

            if (firstPos == "C") { firstPos = "LW"; }

            if (firstShot == "OUT") { ShotType = ConstantController.ShotType.Outside; }
            else if (firstShot == "IN") { ShotType = ConstantController.ShotType.Inside; }
            else { ShotType = ConstantController.ShotType.RebBreak; }

            ShootingSkater = pkTeam.SkaterLineup[$"{firstPos}1"];

            GameplayController.Inst.StatsSet.SetPossTeam(penaltyKillTeam);
            GameplayController.Inst.StatsSet.AddPossPos($"{firstPos}1");

            IsShorthandedShot = true;
        }

        else
        {
            string firstShot = PenaltyShots[0];
            string firstPos = GetRandomPos();

            if (firstShot == "OUT") { ShotType = ConstantController.ShotType.Outside; }
            else if (firstShot == "IN") { ShotType = ConstantController.ShotType.Inside; }
            else { ShotType = ConstantController.ShotType.RebBreak; }

            ShootingSkater = ppTeam.SkaterLineup[$"{firstPos}1"];

            GameplayController.Inst.StatsSet.SetPossTeam(powerplayTeam);
            GameplayController.Inst.StatsSet.AddPossPos($"{firstPos}1");

            IsShorthandedShot = false;
        }
        
        EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ShotType;
        EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = ShootingSkater;
        EventsController.Inst.RunPenaltyEvent(3);
    }

    public void DetermineNextPenaltyShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the next penalty shot.");

        string powerplayTeam = GameplayController.Inst.GameData.PowerplayTeam == "Home" ? "Home" : "Away";
        string penaltyKillTeam = GameplayController.Inst.GameData.PowerplayTeam == "Home" ? "Away" : "Home";

        GameTeam ppTeam = powerplayTeam == "Home" ? GameplayController.Inst.GameData.HomeTeam : GameplayController.Inst.GameData.AwayTeam;
        GameTeam pkTeam = powerplayTeam == "Home" ? GameplayController.Inst.GameData.AwayTeam : GameplayController.Inst.GameData.HomeTeam;
        
        if (IsShorthandedShot)
        {
            ShorthandedShots.RemoveAt(0);

            GameplayController.Inst.StatsSet.ClearPossPos();
            GameplayController.Inst.StatsSet.SetPossTeam("None");
        }

        else
        {
            PenaltyShots.RemoveAt(0);
        }

        if (ShorthandedShots.Count > 0) { IsShorthandedShot = true; }
        else { IsShorthandedShot = false; }

        if (ShorthandedShots.Count > 0)
        {
            string firstShot = ShorthandedShots[0];
            string firstPos = GetRandomPos();

            if (firstPos == "C") { firstPos = "LW"; }

            if (firstShot == "OUT") { ShotType = ConstantController.ShotType.Outside; }
            else if (firstShot == "IN") { ShotType = ConstantController.ShotType.Inside; }
            else { ShotType = ConstantController.ShotType.RebBreak; }

            ShootingSkater = pkTeam.SkaterLineup[$"{firstPos}1"];

            GameplayController.Inst.StatsSet.SetPossTeam(penaltyKillTeam);
            GameplayController.Inst.StatsSet.AddPossPos($"{firstPos}1");

            IsShorthandedShot = true;

            EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ShotType;
            EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = ShootingSkater;
            EventsController.Inst.RunPenaltyEvent(4);
        }

        else
        {
            IsShorthandedShot = false;

            if (PenaltyShots.Count > 0)
            {
                string firstShot = PenaltyShots[0];
                string firstPos = GetRandomPos();

                if (firstShot == "OUT") { ShotType = ConstantController.ShotType.Outside; }
                else if (firstShot == "IN") { ShotType = ConstantController.ShotType.Inside; }
                else { ShotType = ConstantController.ShotType.RebBreak; }

                ShootingSkater = ppTeam.SkaterLineup[$"{firstPos}1"];

                GameplayController.Inst.StatsSet.SetPossTeam(powerplayTeam);
                GameplayController.Inst.StatsSet.AddPossPos($"{firstPos}1");

                EventsController.Inst.GameplayEvents.OffenseEvents.SelectedShotType = ShotType;
                EventsController.Inst.GameplayEvents.OffenseEvents.ShootingSkater = ShootingSkater;
                EventsController.Inst.RunPenaltyEvent(4);
            }

            else
            {
                GameplayController.Inst.StatsSet.ClearPossPos();
                GameplayController.Inst.StatsSet.SetPossTeam("None");

                EventsController.Inst.RunPenaltyEvent(7);
            }
        }
    }

    public void DeterminePenaltyShotOutcome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the penalty shot outcome.");
        
        int randomNumber = Random.Range(0,11);

        List<string> shotActions = new();

        if (ShotType == ConstantController.ShotType.Outside) { shotActions = ShootingSkater.Card.OutsideShotActions; }
        else if (ShotType == ConstantController.ShotType.Outside) { shotActions = ShootingSkater.Card.InsideShotActions; }
        else { ShotType = ShootingSkater.Card.ReboundShotActions; }

        string shotAction = shotActions[randomNumber];

        GameplayController.Inst.StatsSet.AddShot(ShootingSkater, 1);

        if (shotAction == "GOAL" || shotAction == "GOALIE RATING")
        {
            EventsController.Inst.RunPenaltyEvent(6);
        }

        else
        {
            EventsController.Inst.GameplayEvents.GoalEvents.PowerplayGoalAction = null;
            EventsController.Inst.RunPenaltyEvent(5);
        }
    }

    public void DetermineNextPenaltyShotAfterGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Determining the next penalty shot after a goal.");
        
        EventsController.Inst.GameplayEvents.GoalEvents.PowerplayGoalAction = () =>
        {
            DetermineNextPenaltyShot();
        };
        
        if (IsShorthandedShot)
        {
            EventsController.Inst.GameplayEvents.GoalEvents.ShootingSkater = ShootingSkater;
            EventsController.Inst.GameplayEvents.GoalEvents.GoalType = ConstantController.GoalType.Shorthanded;
            EventsController.Inst.RunGoalEvent(7);
        }

        else
        {
            if (PenaltyTime == 2)
            {
                PenaltyShots.Clear();
            }

            else if (PenaltyTime == 4)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (PenaltyShots.Count > 0)
                    {
                        PenaltyShots.RemoveAt(0);
                    }
                }
            }

            EventsController.Inst.GameplayEvents.GoalEvents.ShootingSkater = ShootingSkater;
            EventsController.Inst.GameplayEvents.GoalEvents.GoalType = ConstantController.GoalType.Powerplay;
            EventsController.Inst.RunGoalEvent(7);
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
            case 5: return "IN";
            case 6:
            default: return "REB";
        }
    }
#endregion
}}
