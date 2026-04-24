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
	public const int Loading_StartUp = 18;

    // Titles
	public const string Game_Title = "Strat-O-Matic Hockey";
	public const string Game_Studio = "Ferryman Studios";
	public const string Game_Email = "ferrymanstudios@gmail.com";
	
	// URL Links
	public const string URL_Firebase = "https://hockey-strat-o-matic-default-rtdb.firebaseio.com/";
	public const string URL_FirebaseAuth = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=";
	public const string URL_FirebaseAuthRemoval = "https://identitytoolkit.googleapis.com/v1/accounts:delete?key=";

    // Player Prefs
	public const string Pref_MusicVolume = "SoM_MusicVolume";
    public const string Pref_EffectsVolume = "SoM_EffectsVolume";
    public const string Pref_Email = "SoM_Email";
	public const string Pref_Password = "SoM_Password";
    public const string Pref_ExhibitionOptions = "SoM_ExhibitionOptions";
    public const string Pref_DefaultExhibitionLeague = "SoM_DefaultExhibitionLeague";
    public const string Pref_DefaultExhibitionTeam = "SoM_DefaultExhibitionTeam";
    public const string Pref_DefaultExhibitionLineup = "SoM_DefaultExhibitionLineup";
    public const string Pref_SeasonOptions = "SoM_SeasonOptions";
    public const string Pref_DefaultSeasonLeague = "SoM_DefaultSeasonLeague";
    public const string Pref_DefaultSeasonTeam = "SoM_DefaultSeasonTeam";
    public const string Pref_DefaultSeasonLineup = "SoM_DefaultSeasonLineup";

    // Resources Load
	public const string ResourceBanners = "Banners/";
    public const string ResourceIcons = "Icons/";
    public const string ResourceLogos = "Logos/";
    public const string ResourceMarkers = "Markers/";

    // Screen Dimensions
	public float Screen_Height;
	public float Screen_Width;
	
	// Team Counts
	public const int NhlTeamCount = 32;
	public const int PwhlTeamCount = 8;
	public const int NhlFranchiseTeamCount = 37;
	public const int PwhlFranchiseTeamCount = 8;

	// Audio Volumes
	public const float Audio_Volume_Music = 1f;
	public const float Audio_Volume_Effects = 1f;

    // Multipliers
	public const float Fading_Multiplier = 0.35f;
	public const float Shrinking_Multiplier = 0.05f;
	public const float Waiting_Multiplier = 0.5f;
	public const float Sliding_Multiplier = 0.2f;

    // Lists & Dictionaries
	public Dictionary<string, Sprite> BannerSprites = new();
    public Dictionary<string, Sprite> IconSprites = new();
    public Dictionary<string, Sprite> LogoSprites = new();
    public Dictionary<string, Sprite> MarkerSprites = new();

    public List<string> PenaltyTypes = new();

    // Enums
    public enum LeagueType
    {
        None,
        NHL,
        NHLFranchise,
        PWHL,
        PWHLFranchise,
    };

    public enum GameType
    {
        None,
        Exhibition,
        Multiplayer,
        Season,
        Playoff,
    };

    public enum ShotType
    {
        Outside,
        Inside,
        RebBreak,
    };

    public enum GoalType
    {
        EvenStrength,
        Powerplay,
        Shorthanded,
        EmptyNet,
        GameWinner,
    };

    public enum PanelType
    {
        // Main
        OpeningInternetError,
        LoadingError,
        LoadingOutdatedVersion,
        LoginInvalidCredentials,
        SignUpInvalidCredentials,
        FirebaseCannotIntialize,

        // Firebase - Get
        FirebaseCannotGetVersions,
        FirebaseCannotGetUser,
        FirebaseCannotGetUserGame,
        FirebaseCannotGetUserSeason,
        FirebaseCannotGetUserPlayoff,
        FirebaseCannotGetTeam,
        FirebaseCannotGetTeams,
        FirebaseCannotGetTeamSeason,
        FirebaseCannotGetTeamPlayoff,
        FirebaseCannotGetSkater,
        FirebaseCannotGetSkaters,
        FirebaseCannotGetSkaterSeason,
        FirebaseCannotGetSkaterPlayoff,
        FirebaseCannotGetGoalie,
        FirebaseCannotGetGoalies,
        FirebaseCannotGetGoalieSeason,
        FirebaseCannotGetGoaliePlayoff,

        // Firebase - Put
        FirebaseCannotPutVersions,
        FirebaseCannotPutUser,
        FirebaseCannotPutUserGame,
        FirebaseCannotPutUserSeason,
        FirebaseCannotPutUserPlayoff,
        FirebaseCannotPutTeam,
        FirebaseCannotPutTeams,
        FirebaseCannotPutTeamSeason,
        FirebaseCannotPutTeamPlayoff,
        FirebaseCannotPutSkater,
        FirebaseCannotPutSkaters,
        FirebaseCannotPutSkaterSeason,
        FirebaseCannotPutSkaterPlayoff,
        FirebaseCannotPutGoalie,
        FirebaseCannotPutGoalies,
        FirebaseCannotPutGoalieSeason,
        FirebaseCannotPutGoaliePlayoff,

        // Firebase - Delete
        FirebaseCannotDeleteVersions,
        FirebaseCannotDeleteUser,
        FirebaseCannotDeleteUserGame,
        FirebaseCannotDeleteUserSeason,
        FirebaseCannotDeleteUserPlayoff,
        FirebaseCannotDeleteTeamSeason,
        FirebaseCannotDeleteTeamPlayoff,
        FirebaseCannotDeleteSkaterSeason,
        FirebaseCannotDeleteSkaterPlayoff,
        FirebaseCannotDeleteGoalieSeason,
        FirebaseCannotDeleteGoaliePlayoff,

        // Settings
        SettingsResetAccount,
        SettingsDeleteAccount,

        // Multiplayer
        MultiplayerOpponentLeft,

        // Season
        SeasonDeleteSeason,
    };
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
	void Update()
	{
		Screen_Height = Screen.height;
		Screen_Width = Screen.width;
	}
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");
        
        LoadBannerSprites();
        LoadIconSprites();
        LoadLogoSprites();
        LoadMarkerSprites();
        SetPenaltyTypes();

		CoreController.Inst.LoadingStepCompleted();
    }
#endregion
#region -------------------- Private Methods --------------------
    private void LoadBannerSprites()
	{
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the banner sprites.");

		BannerSprites.Clear();
		
		Sprite[] spriteArray = Resources.LoadAll<Sprite>(ResourceBanners);

		foreach (Sprite sprite in spriteArray)
		{
			BannerSprites.Add(sprite.name, sprite);
		}
	}

    private void LoadIconSprites()
	{
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the icon sprites.");

		IconSprites.Clear();
		
		Sprite[] spriteArray = Resources.LoadAll<Sprite>(ResourceIcons);

		foreach (Sprite sprite in spriteArray)
		{
			IconSprites.Add(sprite.name, sprite);
		}
	}

    private void LoadLogoSprites()
	{
        CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the logo sprites.");

		LogoSprites.Clear();
		
		Sprite[] spriteArray = Resources.LoadAll<Sprite>(ResourceLogos);

		foreach (Sprite sprite in spriteArray)
		{
			LogoSprites.Add(sprite.name, sprite);
		}
	}
    
	private void LoadMarkerSprites()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Loading the marker sprites.");

		MarkerSprites.Clear();
		
		Sprite[] spriteArray = Resources.LoadAll<Sprite>(ResourceMarkers);

		foreach (Sprite sprite in spriteArray)
		{
			MarkerSprites.Add(sprite.name, sprite);
		}
	}

    private void SetPenaltyTypes()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the penalty types.");

        PenaltyTypes.Clear();

        PenaltyTypes.Add("boarding");
        PenaltyTypes.Add("butt-ending");
        PenaltyTypes.Add("charging");
        PenaltyTypes.Add("checking from behind");
        PenaltyTypes.Add("cross-checking");
        PenaltyTypes.Add("delay of game");
        PenaltyTypes.Add("elbowing");
        PenaltyTypes.Add("embellishment");
        PenaltyTypes.Add("fighting");
        PenaltyTypes.Add("goaltender interference");
        PenaltyTypes.Add("high-sticking");
        PenaltyTypes.Add("holding");
        PenaltyTypes.Add("hooking");
        PenaltyTypes.Add("interference");
        PenaltyTypes.Add("kneeing");
        PenaltyTypes.Add("roughing");
        PenaltyTypes.Add("slashing");
        PenaltyTypes.Add("spearing");
        PenaltyTypes.Add("too many men");
        PenaltyTypes.Add("tripping");
        PenaltyTypes.Add("unsportsmanlike conduct");
    }
#endregion
}}
