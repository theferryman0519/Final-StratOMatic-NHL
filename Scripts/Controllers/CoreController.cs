// Main Dependencies
using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

// Game Dependencies
using SoM.Core;

namespace SoM.Controllers {
public class CoreController : Singleton<CoreController> {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    [Header("Loading Details")]
    public (int progress, int max) LoadingSteps;
	public (int progress, int max) LoadingImages;

    public bool IsLoaded;

    [Header("Scene Names")]
    // Persistent Scenes
    public string Scene_Persistent00 = "Persistent_00_Controllers";
    
    // Main Scenes
    public string Scene_Main00 = "Main_00_Opening";
    public string Scene_Main01 = "Main_01_AutoLogin";
    public string Scene_Main02 = "Main_02_Loading";
    public string Scene_Main03 = "Main_03_Login";
    public string Scene_Main04 = "Main_04_SignUp";
    public string Scene_Main05 = "Main_05_SignUpTeam";
    public string Scene_Main06 = "Main_06_SavedGame";

    // Home Scenes
    public string Scene_Home00 = "Home_00_Main";
    public string Scene_Home01 = "Home_01_Settings";
    public string Scene_Home02 = "Home_02_UpdateProfile";
    public string Scene_Home03 = "Home_03_UpdateTeam";
    public string Scene_Home04 = "Home_04_Statistics";

    // Exhibition Scenes
    public string Scene_Exhibition00 = "Exhibition_00_Team";
    public string Scene_Exhibition01 = "Exhibition_01_Options";
    public string Scene_Exhibition03 = "Exhibition_03_Lines";
    public string Scene_Exhibition04 = "Exhibition_04_Ready";
    public string Scene_Exhibition05 = "Exhibition_05_Loading";
    public string Scene_Exhibition06 = "Exhibition_06_Results";

    // Multiplayer Scenes
    public string Scene_Multiplayer00 = "Multiplayer_00_Main";
    public string Scene_Multiplayer01 = "Multiplayer_01_HostOptions";
    public string Scene_Multiplayer02 = "Multiplayer_02_JoinPasscode";
    public string Scene_Multiplayer03 = "Multiplayer_03_Waiting";
    public string Scene_Multiplayer04 = "Multiplayer_04_Team";
    public string Scene_Multiplayer05 = "Multiplayer_05_Lines";
    public string Scene_Multiplayer06 = "Multiplayer_06_ReadyWaiting";
    public string Scene_Multiplayer07 = "Multiplayer_07_ReadyStart";

    // Season Scenes
    public string Scene_Season00 = "Season_00_Team";
    public string Scene_Season01 = "Season_01_Options";
    public string Scene_Season02 = "Season_02_Main";
    public string Scene_Season03 = "Season_03_Lines";
    public string Scene_Season04 = "Season_04_Ready";
    public string Scene_Season05 = "Season_05_Loading";
    public string Scene_Season06 = "Season_06_Results";
    public string Scene_Season07 = "Season_07_Simulating";

    // Playoff Scenes
    public string Scene_Playoff00 = "Playoff_00_Start";
    public string Scene_Playoff01 = "Playoff_01_Main";
    public string Scene_Playoff02 = "Playoff_02_Lines";
    public string Scene_Playoff03 = "Playoff_03_Ready";
    public string Scene_Playoff04 = "Playoff_04_Loading";
    public string Scene_Playoff05 = "Playoff_05_Results";
    public string Scene_Playoff06 = "Playoff_06_Simulating";
    public string Scene_Playoff07 = "Playoff_07_FullSimulating";
    public string Scene_Playoff08 = "Playoff_08_Champion";

    // Gameplay Scenes
    public string Scene_Gameplay00 = "Gameplay_00_Main";
#endregion
#region -------------------- Private Variables --------------------
    private bool isInitializing = false;

	private string channelId = "playtime_channel";

	private HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(2) };
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
	    // // FOR TESTING
	    // PlayerPrefs.DeleteAll();
	    // PlayerPrefs.Save();
	    // // FOR TESTING
	    
        IsLoaded = false;
        SceneManager.LoadSceneAsync(Scene_Main00, LoadSceneMode.Single);
        StartCoroutine(RequestNotificationPermissions());
    }
#endregion
#region -------------------- Coroutines --------------------
    private IEnumerator RequestNotificationPermissions()
	{
#if UNITY_IOS
	    var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
	
	    using var req = new AuthorizationRequest(authorizationOption, true);
		
	    while (!req.IsFinished)
		{
			yield return null;
		}
	
	    var success = string.IsNullOrEmpty(req.Error) && req.Granted;
	    if (success)
		{
			WriteLog(GetType().Name, "iOS notifications have been granted.");
		}
		
	    else
		{
			WriteError(GetType().Name, "iOS notifications have been denied.");
		}
#endif
#if UNITY_ANDROID
	    if (DeviceInfo.GetApiLevel() >= 33)
	    {
	        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
	        {
	            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");

				yield return new WaitForSeconds(0.25f);
	        }
	    }
#endif
	    yield return null;
	}
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        WriteLog(this.GetType().Name, $"Initializing the controller.");

        isInitializing = true;

		LoadingSteps.max = Constant_Controller.Loading_StartUp;
		LoadingSteps = (0, Constant_Controller.Loading_StartUp);
		
		LoadingStepCompleted();

		isInitializing = false;
    }

    public void LoadingStepCompleted()
	{
		WriteLog(this.GetType().Name, $"Loading step has completed.");
		
		int newProgress = Mathf.Clamp(LoadingSteps.progress + 1, 0, LoadingSteps.max);
		LoadingSteps = (newProgress, LoadingSteps.max);

		if (newProgress == LoadingSteps.max)
		{
			IsLoaded = true;
		}

		else
		{
			IsLoaded = false;
		}
	}

    public async Task<bool> HasInternetConnection()
    {
        try
        {
			string testUrl = string.Empty;
#if UNITY_IOS
			testUrl = "https://www.apple.com/library/test/success.html";
#else
			testUrl = "https://www.google.com";
#endif
            using var response = await client.GetAsync(testUrl, HttpCompletionOption.ResponseHeadersRead);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

	public void WriteLog(string fileName, string content)
	{
#if UNITY_EDITOR
		Debug.Log($"{fileName}: {content}");
#endif
	}

	public void WriteError(string fileName, string content)
	{
#if UNITY_EDITOR
		Debug.LogError($"{fileName}: {content}");
#endif
	}

	public void ChangeScene(string sceneName)
	{
		WriteLog(this.GetType().Name, $"Changing the scene to {sceneName}.");
		
		SceneManager.LoadScene(sceneName);
	}

    public void Schedule24hFromNow()
    {
        CancelScheduled();

#if UNITY_ANDROID
        var notification = new AndroidNotification
        {
            Title = "Lace Up Your Skates!",
            Text = "The puck is about to drop on your next game!",
            FireTime = DateTime.Now.AddHours(24)
        };
        AndroidNotificationCenter.SendNotification(notification, channelId);
#endif
#if UNITY_IOS
        var trigger = new iOSNotificationTimeIntervalTrigger
        {
            TimeInterval = TimeSpan.FromHours(24),
            Repeats = false
        };

        var notification = new iOSNotification
        {
            Identifier = "game-time",
            Title = "Lace Up Your Skates!",
            Body = "The puck is about to drop on your next game!",
            ShowInForeground = false,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Sound,
            Trigger = trigger
        };

        iOSNotificationCenter.ScheduleNotification(notification);
#endif
    }

    public void CancelScheduled()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllScheduledNotifications();
#endif
#if UNITY_IOS
        iOSNotificationCenter.RemoveScheduledNotification("game-time");
#endif
    }
#endregion
#region -------------------- Private Methods --------------------
    private void InitializeNotifications()
	{
#if UNITY_ANDROID
	    var reward = new AndroidNotificationChannel
	    {
	        Id = channelId,
	        Name = "Daily Game",
	        Description = "Notifies you when a daily game is ready.",
	        Importance = Importance.Default
	    };
	    AndroidNotificationCenter.RegisterNotificationChannel(reward);
#endif
    }
#endregion
}}
