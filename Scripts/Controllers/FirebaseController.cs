// Main Dependencies
using Firebase;
using Firebase.Auth;
using Newtonsoft.Json;
using Proyecto26;
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
using SoM.Models;

namespace SoM.Controllers {
public class FirebaseController : Singleton<FirebaseController> {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    public bool IsNewUser = false;

    public User NewUser;
#endregion
#region -------------------- Private Variables --------------------
    private FirebaseAuth auth;
	
	private string firebaseToken = string.Empty;

	private Regex emailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]{2,}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
#endregion
#region -------------------- Methods --------------------
#region ---------- Base ----------
	public IEnumerator InitializingFirebase(Action continueAction = null)
	{
		FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(ConstantController.URL_Firebase);
		var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
		yield return new WaitUntil(() => dependencyTask.IsCompleted);

		if (dependencyTask.Result == DependencyStatus.Available)
		{
			auth = FirebaseAuth.DefaultInstance;
			
			CoreController.Inst.WriteLog(this.GetType().Name, "Firebase initialized successfully.");
			continueAction?.Invoke();
		}

		else
		{
			CoreController.Inst.WriteError(this.GetType().Name, "Firebase initialization failed.");

			string error = "The loading process has failed. Please contact support and try running the game again shortly.";
			Ui_Controller.Inst.ShowErrorPanel(error);
		}
	}

	public async Task GetFirebaseToken(Action continueAction)
	{
		firebaseToken = await OnlineUser.TokenAsync(true);
		
		CoreController.Inst.WriteLog(this.GetType().Name, $"Retrieving the user's Firebase token: {firebaseToken}.");
		
		continueAction?.Invoke();
	}
#endregion
#region ---------- Get / Put / Delete ----------
	private async Task RestGet(FirebaseRest firebaseRest)
	{
		firebaseToken = await OnlineUser.TokenAsync(true);

		if (string.IsNullOrEmpty(firebaseToken))
		{
			CoreController.Inst.WriteError(GetType().Name, $"Cannot retrieve Firebase token for Get method.");

			firebaseRest.FailAction?.Invoke(string.Empty);
			return;
		}

		TaskCompletionSource<bool> tcs = new();

		RestClient.Get(new RequestHelper
		{
			Uri = $"{ConstantController.URL_Firebase}{firebaseRest.Url}.json?auth={firebaseToken}",
			Timeout = 100
		}).Then(response =>
		{
			if (response.StatusCode != 200 || string.IsNullOrEmpty(response.Text.ToString()) || response.Text.Trim() == "null")
			{
				CoreController.Inst.WriteError(GetType().Name, $"Get method failed for {firebaseRest.Method}.");

				firebaseRest.FailAction?.Invoke(string.Empty);
				tcs.TrySetResult(false);
				return;
			}

			CoreController.Inst.WriteLog(GetType().Name, $"Get method successful for {firebaseRest.Method}.");

			firebaseRest.SuccessAction?.Invoke(response.Text);
			tcs.TrySetResult(true);
		}).Catch(err =>
		{
			CoreController.Inst.WriteError(GetType().Name, $"Get method failed for {firebaseRest.Method}. Error: {err}");

			firebaseRest.FailAction?.Invoke(string.Empty);
			tcs.TrySetResult(false);
		});

		await tcs.Task;
	}

	private async Task RestPut(FirebaseRest firebaseRest)
	{
		firebaseToken = await OnlineUser.TokenAsync(true);

		if (string.IsNullOrEmpty(firebaseToken))
		{
			CoreController.Inst.WriteError(GetType().Name, $"Cannot retrieve Firebase token for Put method.");

			firebaseRest.FailAction?.Invoke(string.Empty);
			return;
		}

		TaskCompletionSource<bool> tcs = new();

		RestClient.Put(new RequestHelper
		{
			Uri = $"{ConstantController.URL_Firebase}{firebaseRest.Url}.json?auth={firebaseToken}",
			Timeout = 100,
			BodyString = firebaseRest.Json,
			Headers = new Dictionary<string, string> {{ "Content-Type", "application/json" }}
		})
		.Then(response =>
		{
			if (response.StatusCode != 200 || string.IsNullOrEmpty(response.Text.ToString()) || response.Text.Trim() == "null")
			{
				CoreController.Inst.WriteError(GetType().Name, $"Put method failed for {firebaseRest.Method}.");

				firebaseRest.FailAction?.Invoke(string.Empty);
				tcs.TrySetResult(false);
				return;
			}

			CoreController.Inst.WriteLog(GetType().Name, $"Put method successful for {firebaseRest.Method}.");

			firebaseRest.SuccessAction?.Invoke(response.Text);
			tcs.TrySetResult(true);
		})
		.Catch(err =>
		{
			CoreController.Inst.WriteLog(GetType().Name, $"Put method failed for {firebaseRest.Method}. Error: {err}");

			firebaseRest.FailAction?.Invoke(string.Empty);
			tcs.TrySetResult(false);
		});

		await tcs.Task;
	}

	private async Task RestDelete(FirebaseRest firebaseRest)
	{
		firebaseToken = await OnlineUser.TokenAsync(true);

		if (string.IsNullOrEmpty(firebaseToken))
		{
			CoreController.Inst.WriteError(GetType().Name, $"Cannot retrieve Firebase token for Delete method.");

			firebaseRest.FailAction?.Invoke(string.Empty);
			return;
		}

		TaskCompletionSource<bool> tcs = new();

		RestClient.Delete(new RequestHelper
		{
			Uri = $"{ConstantController.URL_Firebase}{firebaseRest.Url}.json?auth={firebaseToken}",
			Timeout = 100
		})
		.Then(response =>
		{
			if (response.StatusCode != 200)
			{
				CoreController.Inst.WriteError(GetType().Name, $"Delete method failed for {firebaseRest.Method}. StatusCode: {response.StatusCode}");

				firebaseRest.FailAction?.Invoke(string.Empty);
				tcs.TrySetResult(false);
				return;
			}

			CoreController.Inst.WriteLog(GetType().Name, $"Delete method successful for {firebaseRest.Method}.");

			firebaseRest.SuccessAction?.Invoke(response.Text);
			tcs.TrySetResult(true);
		})
		.Catch(err =>
		{
			CoreController.Inst.WriteError(GetType().Name, $"Delete method failed for {firebaseRest.Method}. Error: {err}");

			firebaseRest.FailAction?.Invoke(string.Empty);
			tcs.TrySetResult(false);
		});

		await tcs.Task;
	}
#endregion
#region ---------- Log In / Sign Up ----------
    public IEnumerator SigningInUserToFirebase(FirebaseLogin login)
    {
        if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Log in email or password is empty.");

			login.FailAction?.Invoke();
			yield break;
		}

		if (!emailRegex.IsMatch(login.Email))
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Log in email is not in the proper format.");

			login.FailAction?.Invoke();
			yield break;
		}

		var signInUserTask = auth.SignInWithEmailAndPasswordAsync(login.Email, login.Password);

        yield return new WaitUntil(() => signInUserTask.IsCompleted);

        if (signInUserTask.IsCanceled || signInUserTask.IsFaulted)
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"User is unable to sign in.");
			
			login.FailAction?.Invoke();
            yield break;
        }

        else
        {
            AuthResult user = signInUserTask.Result;
            OnlineUser = user.User;
            
            CoreController.Inst.WriteLog(this.GetType().Name, "User exists and signed in.");

			IsNewUser = false;
			
			PlayerPrefs.SetString(ConstantController.Pref_Email, login.Email);
			PlayerPrefs.SetString(ConstantController.Pref_Password, login.Password);
			PlayerPrefs.Save();

			login.SuccessAction?.Invoke();
            yield break;
        }
    }

    public IEnumerator CreatingUserInFirebase(FirebaseLogin creation)
    {
		if (string.IsNullOrEmpty(creation.Email) || string.IsNullOrEmpty(creation.Password))
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Sign up email or password is empty.");

			creation.FailAction?.Invoke();
			yield break;
		}

		if (!emailRegex.IsMatch(creation.Email))
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Sign up email is not in the proper format.");

			creation.FailAction?.Invoke();
			yield break;
		}

		var providersTask = auth.FetchProvidersForEmailAsync(creation.Email);
	    yield return new WaitUntil(() => providersTask.IsCompleted);
	
	    if (providersTask.IsCanceled || providersTask.IsFaulted)
	    {
	        creation.FailAction?.Invoke();
	        yield break;
	    }
	
	    var providers = providersTask.Result;
	    bool emailExists = providers != null && providers.Count() > 0;
	
	    if (emailExists)
	    {
	        creation.FailAction?.Invoke();
	        yield break;
	    }

        var createUserTask = auth.CreateUserWithEmailAndPasswordAsync(creation.Email, creation.Password);

        yield return new WaitUntil(() => createUserTask.IsCompleted);

        if (createUserTask.IsCanceled || createUserTask.IsFaulted)
        {
            CoreController.Inst.WriteError(this.GetType().Name, "Creating user account has failed.");

            creation.FailAction?.Invoke();
            yield break;
        }

        else
        {
            AuthResult newUser = createUserTask.Result;
            OnlineUser = newUser.User;
			NewUser.Id = OnlineUser.UserId;
            NewUser.Info.Id = OnlineUser.UserId;
            NewUser.Stats.Id = OnlineUser.UserId;
            NewUser.SeasonStats.Id = OnlineUser.UserId;
            
            CoreController.Inst.WriteLog(this.GetType().Name, "User created successfully.");

			IsNewUser = true;
			
			PlayerPrefs.SetString(ConstantController.Pref_Email, creation.Email);
			PlayerPrefs.SetString(ConstantController.Pref_Password, creation.Password);
			PlayerPrefs.Save();

			creation.SuccessAction?.Invoke();
            yield break;
        }
    }
#endregion
#endregion
}}
