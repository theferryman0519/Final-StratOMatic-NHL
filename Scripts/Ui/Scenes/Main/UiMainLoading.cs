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

namespace SoM.Ui {
public class UiMainLoading : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Canvas Group Elements")]
    [SerializeField] private List<CanvasGroup> _pageElements;

    [Header("Version Text Element")]
    [SerializeField] private TMP_Text _versionText;

    [Header("Loading Elements")]
	[SerializeField] private Slider _loadingBar;
	
	[SerializeField] private TMP_Text _loadingText;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private float loadingTimer = 0f;

    private bool isLoading = false;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        InitializeUi();
    }

    void Update()
	{
		SetLoadingText();
		CheckLoading();
	}
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
        _versionText.text = $"Version: {Application.version}";
        _loadingText.text = RandomizeLoadingText();

        base.InitializeUi(StartLoading);
	}
#endregion
#region -------------------- Private Methods --------------------
    private async void StartLoading()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Starting to load the game.");

		bool hasInternet = await CoreController.Inst.HasInternetConnection();

		if (!hasInternet)
		{
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.OpeningInternetError);
			return;
		}

		loadingTimer = 0f;
		isLoading = true;

		await FirebaseController.Inst.GettingCurrentVersions(() =>
		{
			AiController.Inst.InitializeController();
			AnimationController.Inst.InitializeController();
			EventsController.Inst.InitializeController();
			GameplayController.Inst.InitializeController();
			MultiplayerController.Inst.InitializeController();
			SaveController.Inst.InitializeController();
			TeamsController.Inst.InitializeController();
			SkatersController.Inst.InitializeController();
			GoaliesController.Inst.InitializeController();
			SeasonsController.Inst.InitializeController();
			PlayoffsController.Inst.InitializeController();
		});
	}

    private void CheckLoading()
	{
		if (isLoading)
        {
			loadingTimer += Time.deltaTime;
		}

		if (isLoading && loadingTimer >= ConstantController.Loading_Threshold)
		{
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.LoadingError);
			isLoading = false;
			return;
		}

		float percentage = 0f;
		int max = CoreController.Inst.LoadingSteps.max;
		
		if (max > 0)
		{
			percentage = (float)CoreController.Inst.LoadingSteps.progress / max;
		}
		
		if (isLoading && CoreController.Inst.LoadingSteps.max > 0 && percentage < 1f)
		{
			_loadingBar.value = percentage;
			
			if (!string.IsNullOrEmpty(FirebaseController.Inst.CurrentGameVersion))
			{
				if (!IsVersionCompatible(Application.version, FirebaseController.Inst.CurrentGameVersion))
				{
					PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.LoadingOutdatedVersion);
					isLoading = false;
				}
			}
		}
		
		if (isLoading && percentage >= 1f)
		{
			_loadingBar.value = 1;

            ContinueToGame();
					
			isLoading = false;
		}
	}

    private async void ContinueToGame()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Continuing to the game.");

        _mainContent.Clear();
        _mainContent = _pageElements;

		GameDatabase savedGame = await GetCurrentGame(UsersController.Inst.User.Id);

		if (savedGame == null)
		{
			// TODO: Check if new user
			// TODO: If so, go to tutorial
			// TODO: If not, go to home

			GoToNewScene(CoreController.Inst.Scene_Home00);
		}

		else
		{
			GameplayController.Inst.SavedGame = savedGame;
			GoToNewScene(CoreController.Inst.Scene_Main05);
		}
	}

    private bool IsVersionCompatible(string localVersion, string requiredVersion)
	{
		if (string.IsNullOrEmpty(localVersion) || string.IsNullOrEmpty(requiredVersion))
		{
			return false;
		}

		string[] local = localVersion.Split('.');
		string[] required = requiredVersion.Split('.');

		if (local.Length < 2 || required.Length < 2)
		{
			return false;
		}

		return local[0] == required[0] && local[1] == required[1];
	}

    private void SetLoadingText()
	{
		textTimer += Time.deltaTime;

		if (textTimer >= 1.5f)
		{
			_loadingText.text = RandomizeLoadingText();
			textTimer = 0f;
		}
	}

    private string RandomizeLoadingText()
	{
		int randomInt = UnityEngine.Random.Range(0, 6);

		switch (randomInt)
		{
			case 0: return "Lacing up the skates...";
			case 1: return "Taping the sticks...";
			case 2: return "Freezing the pucks...";
			case 3: return "Cleaning the ice...";
			case 4: return "Driving the Zamboni...";
            case 5:
			default: return "Setting up the goals...";
		}
	}
#endregion
}}
