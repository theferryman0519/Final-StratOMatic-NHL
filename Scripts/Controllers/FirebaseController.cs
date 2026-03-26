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

	public string CurrentGameVersion;
	public string CurrentRostersVersion;
	public string CurrentPhotonVersion;

	public FirebaseUser OnlineUser;

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

			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotIntialize);
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
#region ---------- Versions ----------
	public async Task GetCurrentVersions(Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the current versions from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Versions",
			Method = "Versions",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the current versions.");

			Dictionary<string, string> versionsDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

			versionsDict.TryGetValue("Game", out CurrentGameVersion);
			versionsDict.TryGetValue("Rosters", out CurrentRostersVersion);
			versionsDict.TryGetValue("Photon", out CurrentPhotonVersion);

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the current versions.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetVersions);
		};

		await RestGet(newRest);
	}
#endregion
#region ---------- Users ----------
	public async Task PutUser(UserDatabase user, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a user to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Users/{id}",
			Method = "Users",
			Json = JsonConvert.SerializeObject(user),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the user account.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the user account.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutUser);
		};

		await RestPut(newRest);
	}

	public async Task GetUser(string id, Action<UserDatabase> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a user from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Users/{id}",
			Method = "Users",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the user account.");

			UserDatabase newUser = JsonConvert.DeserializeObject<UserDatabase>(responseText);

			continueAction?.Invoke(newUser);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the user account.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetUser);
		};

		await RestGet(newRest);
	}

	public async Task DeleteUser(Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a user from Firebase.");

		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Users/{id}",
			Method = "Users",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the user account.");

			DeleteUserAuthentication(continueAction);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the user account.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteUser);
		};

		await RestDelete(newRest);
	}

	public async Task DeleteUserAuthentication(Action continueAction)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting the user authentication.");
		
		FirebaseUser user = auth.CurrentUser;
		
		if (user != null)
		{
			user.DeleteAsync().ContinueWith(task =>
			{
				CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the user authentication.");
				
				continueAction?.Invoke();
			});
		}
	}
#endregion
#region ---------- Current Games ----------
	public async Task PutCurrentGame(GameDatabase game, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a current game to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Games/{id}",
			Method = "Games",
			Json = JsonConvert.SerializeObject(game),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the user current game.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the user current game.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutUserGame);
		};

		await RestPut(newRest);
	}

	public async Task GetCurrentGame(string id, Action<GameDatabase> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a current game from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Games/{id}",
			Method = "Games",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the user current game.");

			GameDatabase newCurrentGame = JsonConvert.DeserializeObject<GameDatabase>(responseText);

			continueAction?.Invoke(newCurrentGame);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the user current game.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetUserGame);
		};

		await RestGet(newRest);
	}

	public async Task DeleteCurrentGame(Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a current game from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Games/{id}",
			Method = "Games",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the user current game.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the user current game.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteUserGame);
		};

		await RestDelete(newRest);
	}
#endregion
#region ---------- Current Seasons ----------
	public async Task PutSeason(SeasonDatabase season, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a season to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Seasons/{id}",
			Method = "Season",
			Json = JsonConvert.SerializeObject(season),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the user current season.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the user current season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutUserSeason);
		};

		await RestPut(newRest);
	}

	public async Task GetSeason(string id, Action<SeasonDatabase> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a season from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Seasons/{id}",
			Method = "Season",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the user current season.");

			SeasonDatabase newCurrentSeason = JsonConvert.DeserializeObject<SeasonDatabase>(responseText);

			continueAction?.Invoke(newCurrentSeason);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the user current season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetUserSeason);
		};

		await RestGet(newRest);
	}

	public async Task DeleteSeason(Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a season from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Seasons/{id}",
			Method = "Season",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the user current season.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the user current season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteUserSeason);
		};

		await RestDelete(newRest);
	}
#endregion
#region ---------- Current Playoffs ----------
	public async Task PutPlayoffs(PlayoffDatabase playoff, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a playoffs to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Playoffs/{id}",
			Method = "Playoff",
			Json = JsonConvert.SerializeObject(playoff),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the user current playoffs.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the user current playoffs.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutUserPlayoff);
		};

		await RestPut(newRest);
	}

	public async Task GetPlayoffs(string id, Action<PlayoffDatabase> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a playoff from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Playoffs/{id}",
			Method = "Playoff",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the user current playoffs.");

			PlayoffDatabase newCurrentPlayoff = JsonConvert.DeserializeObject<PlayoffDatabase>(responseText);

			continueAction?.Invoke(newCurrentPlayoff);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the user current playoffs.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetUserPlayoff);
		};

		await RestGet(newRest);
	}

	public async Task DeletePlayoffs(Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a playoffs from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Playoffs/{id}",
			Method = "Playoff",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the user current playoffs.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the user current playoffs.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteUserPlayoff);
		};

		await RestDelete(newRest);
	}
#endregion
#region ---------- Teams ----------
	public async Task GetAllTeams(Action<List<TeamDatabase>> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting all teams from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Teams",
			Method = "Team",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got all teams.");

			Dictionary<string, TeamDatabase> newTeamsDict = JsonConvert.DeserializeObject<Dictionary<string, TeamDatabase>>(responseText);
			List<TeamDatabase> newTeams = newTeamsDict.Values.ToList();

			continueAction?.Invoke(newTeams);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get all teams.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetTeams);
		};

		await RestGet(newRest);
	}

	public async Task GetTeam(string teamId, Action<TeamDatabase> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a team from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Teams/{teamId}",
			Method = "Team",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the single team.");

			TeamDatabase newTeam = JsonConvert.DeserializeObject<TeamDatabase>(responseText);

			continueAction?.Invoke(newTeam);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the single team.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetTeam);
		};

		await RestGet(newRest);
	}

	public async Task PutTeamSeason(string teamId, string id, string seasonString, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a team season to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Teams/{teamId}/SeasonStrings/{id}",
			Method = "Team",
			Json = JsonConvert.SerializeObject(seasonString),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the team season.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the team season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutTeamSeason);
		};

		await RestPut(newRest);
	}

	public async Task GetTeamSeason(string teamId, string id, Action<string> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a team season from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Teams/{teamId}/SeasonStrings/{id}",
			Method = "Team",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the team season.");

			string newTeamSeason = JsonConvert.DeserializeObject<string>(responseText);

			continueAction?.Invoke(newTeamSeason);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the team season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetTeamSeason);
		};

		await RestGet(newRest);
	}

	public async Task DeleteTeamSeason(string teamId, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a team season from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Teams/{teamId}/SeasonStrings/{id}",
			Method = "Team",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the team season.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the team season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteTeamSeason);
		};

		await RestDelete(newRest);
	}

	public async Task PutTeamPlayoffs(string teamId, string id, string playoffString, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a team playoffs to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Teams/{teamId}/PlayoffStrings/{id}",
			Method = "Team",
			Json = JsonConvert.SerializeObject(playoffString),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the team playoffs.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the team playoffs.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutTeamPlayoff);
		};

		await RestPut(newRest);
	}

	public async Task GetTeamPlayoffs(string teamId, string id, Action<string> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a team playoffs from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Teams/{teamId}/PlayoffStrings/{id}",
			Method = "Team",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the team playoffs.");

			string newTeamPlayoff = JsonConvert.DeserializeObject<string>(responseText);

			continueAction?.Invoke(newTeamPlayoff);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the team playoffs.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetTeamPlayoff);
		};

		await RestGet(newRest);
	}

	public async Task DeleteTeamPlayoffs(string teamId, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a team playoffs from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Teams/{teamId}/PlayoffStrings/{id}",
			Method = "Team",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the team playoffs.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the team playoffs.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteTeamPlayoff);
		};

		await RestDelete(newRest);
	}
#endregion
#region ---------- Skaters ----------
	public async Task GetAllSkaters(Action<List<SkaterDatabase>> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting all skaters from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Skaters",
			Method = "Skater",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got all skaters.");

			Dictionary<string, SkaterDatabase> newSkatersDict = JsonConvert.DeserializeObject<Dictionary<string, SkaterDatabase>>(responseText);
			List<SkaterDatabase> newSkaters = newSkatersDict.Values.ToList();

			continueAction?.Invoke(newSkaters);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get all skaters.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetSkaters);
		};

		await RestGet(newRest);
	}

	public async Task GetSkater(string skaterId, Action<SkaterDatabase> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a skater from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Skaters/{skaterId}",
			Method = "Skater",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the single skater.");

			SkaterDatabase newSkater = JsonConvert.DeserializeObject<SkaterDatabase>(responseText);

			continueAction?.Invoke(newSkater);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the single skater.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetSkater);
		};

		await RestGet(newRest);
	}

	public async Task PutSkaterSeason(string skaterId, string id, string seasonString, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a skater season to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Skaters/{skaterId}/SeasonStrings/{id}",
			Method = "Skater",
			Json = JsonConvert.SerializeObject(seasonString),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the skater season.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the skater season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutSkaterSeason);
		};

		await RestPut(newRest);
	}

	public async Task GetSkaterSeason(string skaterId, string id, Action<string> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a skater season from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Skaters/{skaterId}/SeasonStrings/{id}",
			Method = "Skater",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the skater season.");

			string newSkaterSeason = JsonConvert.DeserializeObject<string>(responseText);

			continueAction?.Invoke(newSkaterSeason);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the skater season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetSkaterSeason);
		};

		await RestGet(newRest);
	}

	public async Task DeleteSkaterSeason(string skaterId, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a skater season from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Skaters/{skaterId}/SeasonStrings/{id}",
			Method = "Skater",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the skater season.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the skater season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteSkaterSeason);
		};

		await RestDelete(newRest);
	}

	public async Task PutSkaterPlayoffs(string skaterId, string id, string playoffString, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a skater playoffs to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Skaters/{skaterId}/PlayoffStrings/{id}",
			Method = "Skater",
			Json = JsonConvert.SerializeObject(playoffString),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the skater playoff.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the skater playoff.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutSkaterPlayoff);
		};

		await RestPut(newRest);
	}

	public async Task GetSkaterPlayoffs(string skaterId, string id, Action<string> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a skater playoffs from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Skaters/{skaterId}/PlayoffStrings/{id}",
			Method = "Skater",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the skater playoff.");

			string newSkaterPlayoff = JsonConvert.DeserializeObject<string>(responseText);

			continueAction?.Invoke(newSkaterPlayoff);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the skater playoff.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetSkaterPlayoff);
		};

		await RestGet(newRest);
	}

	public async Task DeleteSkaterPlayoffs(string skaterId, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a skater playoffs from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Skaters/{skaterId}/PlayoffStrings/{id}",
			Method = "Skater",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the skater playoff.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the skater playoff.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteSkaterPlayoff);
		};

		await RestDelete(newRest);
	}
#endregion
#region ---------- Goalies ----------
	public async Task GetAllGoalies(Action<List<GoalieDatabase>> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting all goalies from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Goalies",
			Method = "Goalie",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got all goalies.");

			Dictionary<string, GoalieDatabase> newGoaliesDict = JsonConvert.DeserializeObject<Dictionary<string, GoalieDatabase>>(responseText);
			List<GoalieDatabase> newGoalies = newGoaliesDict.Values.ToList();

			continueAction?.Invoke(newGoalies);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get all goalies.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetGoalies);
		};

		await RestGet(newRest);
	}

	public async Task GetGoalie(string goalieId, Action<GoalieDatabase> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a goalie from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Goalies/{goalieId}",
			Method = "Goalie",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the single goalie.");

			GoalieDatabase newGoalie = JsonConvert.DeserializeObject<GoalieDatabase>(responseText);

			continueAction?.Invoke(newGoalie);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the single goalie.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetGoalie);
		};

		await RestGet(newRest);
	}

	public async Task PutGoalieSeason(string goalieId, string id, string seasonString, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a goalie season to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Goalies/{goalieId}/SeasonStrings/{id}",
			Method = "Goalie",
			Json = JsonConvert.SerializeObject(seasonString),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the goalie season.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the goalie season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutGoalieSeason);
		};

		await RestPut(newRest);
	}

	public async Task GetGoalieSeason(string goalieId, string id, Action<string> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a goalie season from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Goalies/{goalieId}/SeasonStrings/{id}",
			Method = "Goalie",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the goalie season.");

			string newGoalieSeason = JsonConvert.DeserializeObject<string>(responseText);

			continueAction?.Invoke(newGoalieSeason);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the goalie season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetGoalieSeason);
		};

		await RestGet(newRest);
	}

	public async Task DeleteGoalieSeason(string goalieId, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a goalie season from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Goalies/{goalieId}/SeasonStrings/{id}",
			Method = "Goalie",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the goalie season.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the goalie season.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteGoalieSeason);
		};

		await RestDelete(newRest);
	}

	public async Task PutGoaliePlayoffs(string goalieId, string id, string playoffString, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Putting a goalie playoffs to Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Goalies/{goalieId}/PlayoffStrings/{id}",
			Method = "Goalie",
			Json = JsonConvert.SerializeObject(playoffString),
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully put the goalie playoffs.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot put the goalie playoffs.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotPutGoaliePlayoff);
		};

		await RestPut(newRest);
	}

	public async Task GetGoaliePlayoffs(string goalieId, string id, Action<string> continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a goalie playoffs from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Goalies/{goalieId}/PlayoffStrings/{id}",
			Method = "Goalie",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully got the goalie playoffs.");

			string newGoaliePlayoff = JsonConvert.DeserializeObject<string>(responseText);

			continueAction?.Invoke(newGoaliePlayoff);
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot get the goalie playoffs.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotGetGoaliePlayoff);
		};

		await RestGet(newRest);
	}

	public async Task DeleteGoaliePlayoffs(string goalieId, string id, Action continueAction = null)
	{
		CoreController.Inst.WriteLog(this.GetType().Name, $"Deleting a goalie playoffs from Firebase.");

		FirebaseRest newRest = new FirebaseRest
		{
			Url = $"Goalies/{goalieId}/PlayoffStrings/{id}",
			Method = "Goalie",
			Json = string.Empty,
		};

		newRest.SuccessAction = (responseText) =>
		{
			CoreController.Inst.WriteLog(this.GetType().Name, $"Successfully deleted the goalie playoffs.");

			continueAction?.Invoke();
		};

		newRest.FailAction = (errorText) =>
		{
			CoreController.Inst.WriteError(this.GetType().Name, $"Cannot delete the goalie playoffs.");
			PanelController.Inst.ShowBottomPanel(ConstantController.PanelType.FirebaseCannotDeleteGoaliePlayoff);
		};

		await RestDelete(newRest);
	}
#endregion
#region ---------- Contact Support ----------
	public void ContactSupport()
	{
	    CoreController.Inst.WriteLog(this.GetType().Name, "Opening support email.");
	
	    string toEmail = ConstantController.Game_Email;
	    string subject = Uri.EscapeDataString("Contact Support");
	    string mailto = $"mailto:{toEmail}?subject={subject}";
	
	    Application.OpenURL(mailto);
	}
#endregion
#endregion
}}






// Versions
	// Rosters: 0.0.0
	// Game: 0.0.0
	// Photon: 0.0.0
// Users
	// [Id]
		// Id: [string]
		// InfoString: [string]
		// StatsString: [string]
		// SeasonStatsString: [string]
// Games
	// [UserId]
		// Id: [string]
		// Type: [string]
		// League: [string]
		// HomeTeam: [string]
		// AwayTeam: [string]
		// HomeStatsString: [string]
		// AwayStatsString: [string]
		// HomeSkaterStatsStrings
			// [string]
			// [string]
		// HomeGoalieStatsStrings
			// [string]
			// [string]
		// AwaySkaterStatsStrings
			// [string]
			// [string]
		// AwayGoalieStatsStrings
			// [string]
			// [string]
// Goalies
	// [Id]
		// Id: [string]
		// InfoString: [string]
		// StatsString
			// [string]
			// [string]
		// SeasonStrings
			// [UserId]: [string]
			// [UserId]: [string]
		// PlayoffStrings
			// [UserId]: [string]
			// [UserId]: [string]
// Playoffs
	// [UserId]
		// Id: [string]
		// League: [string]
		// Team: [string]
		// Round: [int]
		// GameNumber: [int]
// Seasons
	// [UserId]
		// Id: [string]
		// League: [string]
		// Team: [string]
		// Version: [int]
		// GameNight: [int]
// Skaters
	// [Id]
		// Id: [string]
		// InfoString: [string]
		// StatsString
			// [string]
			// [string]
		// SeasonStrings
			// [UserId]: [string]
			// [UserId]: [string]
		// PlayoffStrings
			// [UserId]: [string]
			// [UserId]: [string]
// Teams
	// [Id]
		// Id: [string]
		// InfoString: [string]
		// SeasonStrings
			// [UserId]: [string]
			// [UserId]: [string]
		// PlayoffStrings
			// [UserId]: [string]
			// [UserId]: [string]
