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
public class UiSettingsMain : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Text Elements")]
	[SerializeField] private TMP_Text _idText;
	[SerializeField] private TMP_Text _versionText;

	[Header("Button Elements")]
	[SerializeField] private SoM_Button _statisticsButton;
	[SerializeField] private SoM_Button _updateButton;
	[SerializeField] private SoM_Button _resetButton;
	[SerializeField] private SoM_Button _deleteButton;
	[SerializeField] private SoM_Button _returnButton;

	[Header("Dropdown Elements")]
	[SerializeField] private SoM_Dropdown _musicDropdown;
	[SerializeField] private SoM_Dropdown _effectsDropdown;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        InitializeUi();
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
        _statisticsButton.SetListener(GoToStatistics);
		_updateButton.SetListener(GoToUpdateProfile);
		_resetButton.SetListener(ShowResetPanel);
		_deleteButton.SetListener(ShowDeletePanel);
		_returnButton.SetListener(GoToHome);

		_musicDropdown.SetListener(ChangeMusicOption);
		_effectsDropdown.SetListener(ChangeEffectsOption);

		SetTexts();

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void GoToStatistics()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to user statistics screen.");

		GoToNewScene(CoreController.Inst.Scene_Settings01);
    }

	private void GoToUpdateProfile()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to update user profile screen.");

		GoToNewScene(CoreController.Inst.Scene_Settings02);
    }

	private void ShowResetPanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the reset account panel.");

		PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.SettingsResetAccount, ResetStatistics);
    }

	private void ResetStatistics()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Resetting the user statistics.");

		UsersController.Inst.UserData.Stats.NhlWins = 0;
		UsersController.Inst.UserData.Stats.NhlLosses = 0;
		UsersController.Inst.UserData.Stats.NhlTies = 0;
		UsersController.Inst.UserData.Stats.NhlOTLs = 0;
		UsersController.Inst.UserData.Stats.PwhlWins = 0;
		UsersController.Inst.UserData.Stats.PwhlLosses = 0;
		UsersController.Inst.UserData.Stats.PwhlTies = 0;
		UsersController.Inst.UserData.Stats.PwhlOTLs = 0;
		UsersController.Inst.UserData.Stats.NhlFranchiseWins = 0;
		UsersController.Inst.UserData.Stats.NhlFranchiseLosses = 0;
		UsersController.Inst.UserData.Stats.NhlFranchiseTies = 0;
		UsersController.Inst.UserData.Stats.NhlFranchiseOTLs = 0;
		UsersController.Inst.UserData.Stats.PwhlFranchiseWins = 0;
		UsersController.Inst.UserData.Stats.PwhlFranchiseLosses = 0;
		UsersController.Inst.UserData.Stats.PwhlFranchiseTies = 0;
		UsersController.Inst.UserData.Stats.PwhlFranchiseOTLs = 0;

		UsersController.Inst.SaveUserData();
	}

	private void ShowDeletePanel()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Showing the delete account panel.");

		PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.SettingsDeleteAccount, DeleteAccount);
    }

	private async void DeleteAccount()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting the user account.");

		string userId = UsersController.Inst.UserData.Id;

		PlayoffDatabase userPlayoffs = null;
		SeasonDatabase userSeason = null;

		await FirebaseController.Inst.GetPlayoffs(userId, playoffs =>
		{
			userPlayoffs = playoffs;

			await FirebaseController.Inst.GetSeason(userId, season =>
			{
				userSeason = season;
			});
		});

		if (userPlayoffs != null)
		{
			await FirebaseController.Inst.DeletePlayoffs(userId);
		}

		if (userSeason != null)
		{
			await FirebaseController.Inst.DeleteSeason(userId);
		}

		await FirebaseController.Inst.DeleteUser(userId, () =>
		{
			PlayerPrefs.DeleteAll();
	    	PlayerPrefs.Save();

			Application.Quit();
		});
	}

	private void GoToHome()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to home screen.");

		GoToNewScene(CoreController.Inst.Scene_Home00);
    }

	private void ChangeMusicOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the music volume option.");

		float currentEffectsVolume = AudioController.Inst.EffectsVolume;

		switch (option)
		{
			case 0:
				AudioController.Inst.SetVolumes(ConstantController.Audio_Volume_Music, currentEffectsVolume);
				break;
			case 1:
			default:
				AudioController.Inst.SetVolumes(0f, currentEffectsVolume);
				break;
		}
    }

	private void ChangeEffectsOption(int option)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Changing the sound effects volume option.");

		float currentMusicVolume = AudioController.Inst.MusicVolume;

		switch (option)
		{
			case 0:
				AudioController.Inst.SetVolumes(currentMusicVolume, ConstantController.Audio_Volume_Effects);
				break;
			case 1:
			default:
				AudioController.Inst.SetVolumes(currentMusicVolume, 0f);
				break;
		}
    }

	private void SetTexts()
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the settings main screen texts.");

		_idText.text = $"User ID: {UsersController.Inst.User.Id}";
		_versionText.text = $"Version: {Application.version}";
	}
#endregion
}}
