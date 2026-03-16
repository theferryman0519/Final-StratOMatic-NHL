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
public class UiSettingsUpdate : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Input Elements")]
    [SerializeField] private GameObject _emailObject;
    [SerializeField] private SoM_Input _emailInput;

    [SerializeField] private GameObject _passwordObject;
    [SerializeField] private SoM_Input _passwordInput;

    [SerializeField] private GameObject _nameObject;
    [SerializeField] private SoM_Input _nameInput;

    [Header("Button Elements")]
    [SerializeField] private SoM_Button _saveButton;
    [SerializeField] private SoM_Button _updateTeamButton;
	[SerializeField] private SoM_Button _returnButton;
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private TMP_InputField activeInputField;
	private Coroutine keyboardWatchRoutine;
	private TouchScreenKeyboard keyboard;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        InitializeUi();
    }
#endregion
#region -------------------- Coroutines --------------------
    private IEnumerator WatchKeyboardAndRestore(TMP_InputField field)
	{
		yield return null;

		while (field != null && activeInputField == field && IsKeyboardVisible())
		{
			yield return null;
		}

		if (activeInputField == field && !IsKeyboardVisible())
		{
			activeInputField = null;
			RestoreAllInputObjects();
		}

		keyboardWatchRoutine = null;
	}
#endregion
#region -------------------- Public Methods --------------------
    protected override void InitializeUi()
	{
        _saveButton.SetListener(SaveChanges);
		_updateTeamButton.SetListener(GoToUpdateTeam);
		_returnButton.SetListener(GoToSettings);

        _emailInput.SetListeners(
            _ => OnInputSelected(_emailInput.Input, 0),
            _ => OnInputDeselected(_emailInput.Input),
            _ => OnInputEnded(_emailInput.Input)
        );
        _passwordInput.SetListeners(
            _ => OnInputSelected(_passwordInput.Input, 1),
            _ => OnInputDeselected(_passwordInput.Input),
            _ => OnInputEnded(_passwordInput.Input)
        );
        _nameInput.SetListeners(
            _ => OnInputSelected(_nameInput.Input, 2),
            _ => OnInputDeselected(_nameInput.Input),
            _ => OnInputEnded(_nameInput.Input)
        );

        base.InitializeUi();
	}
#endregion
#region -------------------- Private Methods --------------------
    private void SaveChanges()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Attempting to save changes to the user profile.");

		// TODO

		GoToNewScene(CoreController.Inst.Scene_Settings02);
    }

	private void GoToUpdateTeam()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the update user favorite team screen.");

        GoToNewScene(CoreController.Inst.Scene_Settings03);
    }

	private void GoToSettings()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the settings screen.");

        GoToNewScene(CoreController.Inst.Scene_Settings00);
    }

    private void OpenKeyboardFor(TMP_InputField field)
	{
#if UNITY_IOS || UNITY_ANDROID
		if (keyboard != null && TouchScreenKeyboard.visible)
		{
            return;
        }

		var isPassword = field.contentType == TMP_InputField.ContentType.Password || field.contentType == TMP_InputField.ContentType.Pin;

		keyboard = TouchScreenKeyboard.Open(
			field.text,
			TouchScreenKeyboardType.Default,
			autocorrection: !isPassword,
			multiline: field.lineType != TMP_InputField.LineType.SingleLine,
			secure: isPassword,
			alert: false
		);
#endif
	}

	private void OnInputSelected(TMP_InputField field, int selection)
	{
		activeInputField = field;
		OpenKeyboardFor(field);
		AccountForKeyboard(selection);

		StartKeyboardWatch(field);
	}

	private void OnInputDeselected(TMP_InputField field)
	{
		if (activeInputField != null && activeInputField != field)
		{
			return;
		}

		activeInputField = null;
		StopKeyboardWatch();
		RestoreAllInputObjects();
		keyboard = null;
	}

	private void OnInputEnded(TMP_InputField field)
	{
		if (activeInputField != null && activeInputField != field)
		{
			return;
		}

		activeInputField = null;
		StopKeyboardWatch();
		RestoreAllInputObjects();
		keyboard = null;
	}

	private void StartKeyboardWatch(TMP_InputField field)
	{
		StopKeyboardWatch();
		keyboardWatchRoutine = StartCoroutine(WatchKeyboardAndRestore(field));
	}

	private void StopKeyboardWatch()
	{
		if (keyboardWatchRoutine != null)
		{
			StopCoroutine(_keyboardWatchRoutine);
			keyboardWatchRoutine = null;
		}
	}

	private void RestoreAllInputObjects()
	{
		_emailObject.SetActive(true);
		_passwordObject.SetActive(true);
	}

	private void AccountForKeyboard(int selection)
	{
		Core_Controller.Inst.WriteLog(this.GetType().Name, $"Accounting for the mobile keyboard.");

		_emailObject.SetActive(selection == 0);
		_passwordObject.SetActive(selection == 1);
	}

	private bool IsKeyboardVisible()
	{
		if (keyboard == null)
		{
			return false;
		}

		if (!TouchScreenKeyboard.visible)
		{
			return false;
		}

		var status = keyboard.status;
		return status == TouchScreenKeyboard.Status.Visible || status == TouchScreenKeyboard.Status.Done || status == TouchScreenKeyboard.Status.LostFocus;
	}
#endregion
}}
