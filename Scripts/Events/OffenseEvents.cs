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

namespace SoM.Events {
public class OffenseEvents : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public ConstantController.ShotType SelectedShotType;

    public Skater ShootingSkater;
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

        GameplayController.Inst.StatsSet.AddBlockedShot(GameplayController.Inst.GameData.PossTeam == "Home", 1);

        ShootingSkater = null;

        yield return null;
    }

    public IEnumerator ShotResultSave()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotResultSave to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator ShotResultRebound()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotResultRebound to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator ShotResultGoalieRating()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotResultGoalieRating to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator ShotResultGoal()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ShotResultGoal to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator ReboundCheck()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding ReboundCheck to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PassingStart()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingStart to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PassingResultLose()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultLose to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PassingResultLoseShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultLoseShot to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PassingResultShot()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultShot to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }

    public IEnumerator PassingResultOptions()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Adding PassingResultOptions to the queue.");

        // TODO

        EventRun newEventRun = new EventRun
        {
            InfoText = $"",
            ActionText = $"",
        };

        EventsController.Inst.CurrentEventRun = newEventRun;

        yield return null;
    }
#endregion
#region -------------------- Public Methods --------------------
    
#endregion
#region -------------------- Private Methods --------------------
    
#endregion
}}
