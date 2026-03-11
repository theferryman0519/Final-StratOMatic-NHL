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
using SoM.Core;

namespace SoM.Controllers {
public class ConstantController : Singleton<ConstantController> {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    // Loading Sets
	public const int Loading_StartUp = 15;
	public const float Loading_Threshold = 20f;

    // Titles
	public const string Game_Title = "NHL Strat-O-Matic";
	public const string Game_Studio = "Ferryman Studios";
	public const string Game_Email = "ferrymanstudios@gmail.com";

    // Player Prefs
	public const string Pref_Email = "SoM_Email";
	public const string Pref_Password = "SoM_Password";
	public const string Pref_MusicVolume = "SoM_MusicVolume";
    public const string Pref_EffectsVolume = "SoM_EffectsVolume";
    public const string Pref_FavoriteTeam = "SoM_FavoriteTeam";

    // Resources Load
	public const string ResourceBanners = "Banners/";
    public const string ResourceIcons = "Icons/";
    public const string ResourceLogos = "Logos/";

    // Screen Dimensions
	public float Screen_Height;
	public float Screen_Width;

	// Audio Volumes
	public const float Audio_Volume_Music = 1.0f;
	public const float Audio_Volume_Effects = 1.0f;

    // Multipliers
	public const float Fading_Multiplier = 0.35f;
	public const float Shrinking_Multiplier = 0.05f;
	public const float Waiting_Multiplier = 0.5f;
	public const float Sliding_Multiplier = 0.2f;
	public const float Audio_Multiplier = 2f;

    // Timers
	public const float Pausing_Timer = 0.75f;

    // Fatigue Auto Change
    public const int Fatigue_Change = 40;

    // Lists & Dictionaries
	public Dictionary<string, Sprite> BannerSprites = new();
	public Dictionary<string, Sprite> IconSprites = new();
    public Dictionary<string, Sprite> LogoSprites = new();

    public Dictionary<PlayoffRound, string> PlayoffRoundNames = new();
    public Dictionary<LeagueDivision, string> LeagueDivisionNames = new();
    public Dictionary<PenaltyType, int> PenaltyMinuteCounts = new();

    public List<string> GamePenalties = new();

    // Enums
    public enum LeagueDivision
    {
        EAST,
        WEST,
        ATL,
        MET,
        CEN,
        PAC,
    };

    public enum PlayoffRound
    {
        First,
        Second,
        Third,
        Fourth,
    };

    public enum PenaltyType
    {
        Minor,
        Double,
        Major,
        Misconduct,
        Game,
    };
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        SetPlayoffRoundNames();
        SetLeagueDivisionNames();
        SetPenaltyMinuteCounts();
        SetGamePenalties();
        SetBannerSprites();
        SetIconSprites();
        SetLogoSprites();

        CoreController.Inst.LoadingStepCompleted();
    }
#endregion
#region -------------------- Private Methods --------------------
    private void SetPlayoffRoundNames()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the names of the playoff rounds.");

        PlayoffRoundNames.Add(PlayoffRound.First, "First Round");
        PlayoffRoundNames.Add(PlayoffRound.Second, "Divisional Round");
        PlayoffRoundNames.Add(PlayoffRound.Third, "Conference Round");
        PlayoffRoundNames.Add(PlayoffRound.Fourth, "Stanley Cup Finals");
    }

    private void SetLeagueDivisionNames()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the names of the league divisions.");

        LeagueDivisionNames.Add(LeagueDivision.EAST, "Eastern Conference");
        LeagueDivisionNames.Add(LeagueDivision.WEST, "Western Conference");
        LeagueDivisionNames.Add(LeagueDivision.ATL, "Atlantic Division");
        LeagueDivisionNames.Add(LeagueDivision.MET, "Metropolitan Division");
        LeagueDivisionNames.Add(LeagueDivision.CEN, "Central Division");
        LeagueDivisionNames.Add(LeagueDivision.PAC, "Pacific Division");
    }

    private void SetPenaltyMinuteCounts()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the counts for penalty minutes.");

        PenaltyMinuteCounts.Add(PenaltyType.Minor, 2);
        PenaltyMinuteCounts.Add(PenaltyType.Double, 4);
        PenaltyMinuteCounts.Add(PenaltyType.Major, 5);
        PenaltyMinuteCounts.Add(PenaltyType.Misconduct, 10);
        PenaltyMinuteCounts.Add(PenaltyType.Game, 10);
    }

    private void SetGamePenalties()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the penalties of the game.");

        GamePenalties.Add("Boarding");
        GamePenalties.Add("Butt-Ending");
        GamePenalties.Add("Charging");
        GamePenalties.Add("Checking from Behind");
        GamePenalties.Add("Cross-Checking");
        GamePenalties.Add("Delay of Game");
        GamePenalties.Add("Elbowing");
        GamePenalties.Add("Embellishment");
        GamePenalties.Add("Fighting");
        GamePenalties.Add("Goaltender Interference");
        GamePenalties.Add("High-Sticking");
        GamePenalties.Add("Holding");
        GamePenalties.Add("Hooking");
        GamePenalties.Add("Interference");
        GamePenalties.Add("Kneeing");
        GamePenalties.Add("Roughing");
        GamePenalties.Add("Slashing");
        GamePenalties.Add("Spearing");
        GamePenalties.Add("Too Many Men");
        GamePenalties.Add("Tripping");
        GamePenalties.Add("Unsportsmanlike Conduct");
    }

    private void SetBannerSprites()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the banner sprites for each team.");

        BannerSprites.Clear();
		
		Sprite[] spriteArray = Resources.LoadAll<Sprite>(ResourceBanners);

		foreach (Sprite sprite in spriteArray)
		{
			BannerSprites.Add(sprite.name, sprite);
		}
    }

    private void SetIconSprites()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the icon sprites for each team.");

        IconSprites.Clear();
		
		Sprite[] spriteArray = Resources.LoadAll<Sprite>(ResourceIcons);

		foreach (Sprite sprite in spriteArray)
		{
			IconSprites.Add(sprite.name, sprite);
		}
    }

    private void SetLogoSprites()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the logo sprites for each team.");

        LogoSprites.Clear();
		
		Sprite[] spriteArray = Resources.LoadAll<Sprite>(ResourceLogos);

		foreach (Sprite sprite in spriteArray)
		{
			LogoSprites.Add(sprite.name, sprite);
		}
    }
#endregion
}}
