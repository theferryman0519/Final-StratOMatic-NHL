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
public class UiMainSignUp : UiSceneBase {

#region -------------------- Serialized Variables --------------------
    [Header("Input Elements")]
    [SerializeField] private GameObject _emailObject;
    [SerializeField] private SoM_Input _emailInput;

    [SerializeField] private GameObject _passwordObject;
    [SerializeField] private SoM_Input _passwordInput;

    [SerializeField] private GameObject _nameObject;
    [SerializeField] private SoM_Input _nameInput;

    [Header("Button Elements")]
    [SerializeField] private SoM_Button _continueButton;
    [SerializeField] private SoM_Button _toLogInButton;
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
        _continueButton.SetListener(AttemptToContinue);
        _toLogInButton.SetListener(GoToLogIn);

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
    private void AttemptToContinue()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Attempting to sign up.");

        string email = _emailInput.GetInput();
        string password = _passwordInput.GetInput();

        FirebaseLogin signUpData = new FirebaseLogin
        {
            Email = email,
            Password = password,
            SuccessAction = () => { UiController.Inst.IsNewUser = true; GoToNewScene(CoreController.Inst.Scene_Main01); },
            FailAction = null, // TODO: Show invalid email/password bottom panel
        };

        StartCoroutine(Firebase_Controller.Inst.CreatingUserInFirebase(signUpData));
    }

    private void GoToLogIn()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Going to the log in screen.");

        GoToNewScene(Core_Controller.Inst.Scene_Main02);
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
