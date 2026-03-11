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
	public const int Loading_StartUp = 17;

    // Titles
	public const string Game_Title = "Strat-O-Matic Hockey";
	public const string Game_Studio = "Ferryman Studios";
	public const string Game_Email = "ferrymanstudios@gmail.com";

    // Player Prefs
	public const string Pref_MusicVolume = "SoM_MusicVolume";
    public const string Pref_EffectsVolume = "SoM_EffectsVolume";
    public const string Pref_Email = "SoM_Email";
	public const string Pref_Password = "SoM_Password";

    // Resources Load
	public const string ResourceBanners = "Banners/";
    public const string ResourceIcons = "Icons/";
    public const string ResourceLogos = "Logos/";

    // Screen Dimensions
	public float Screen_Height;
	public float Screen_Width;

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

        LoadBannerSprites();
        LoadIconSprites();
        LoadLogoSprites();

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
#endregion
}}
