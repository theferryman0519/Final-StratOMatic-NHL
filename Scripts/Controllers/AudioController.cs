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
using SoM.Teams;

namespace SoM.Controllers {
public class AudioController : Singleton<AudioController> {

#region -------------------- Serialized Variables --------------------
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _goalSource;
    [SerializeField] private AudioSource _periodSource;
    [SerializeField] private AudioSource _whistleSource;

    [Header("Sound Effects Clips")]
    [SerializeField] private List<AudioClip> _goalClips = new();
#endregion
#region -------------------- Public Variables --------------------
    public float MusicVolume => musicVolume;
    public float EffectsVolume => effectsVolume;
#endregion
#region -------------------- Private Variables --------------------
    private float musicVolume = -1f;
    private float effectsVolume = -1f;
#endregion
#region -------------------- Initial Functions --------------------
    void Start()
    {
        SetVolumesFromPrefs();
        PlayMainMusic();
    }
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        CoreController.Inst.LoadingStepCompleted();
    }

    public void SetVolumes(float newMusicVolume, float newEffectsVolume)
    {
        Core_Controller.Inst.WriteLog(this.GetType().Name, $"Updating the volumes.");
		
        _musicSource.volume = newMusicVolume;
        _goalSource.volume = newEffectsVolume;
        _periodSource.volume = newEffectsVolume;
        _whistleSource.volume = newEffectsVolume;
		
        musicVolume = newMusicVolume;
        effectsVolume = newEffectsVolume;

        PlayerPrefs.SetFloat(ConstantController.Pref_MusicVolume, musicVolume);
        PlayerPrefs.SetFloat(ConstantController.Pref_EffectsVolume, effectsVolume);
        PlayerPrefs.Save();
    }

    public void PlayMainMusic()
    {
        if (musicVolume > 0f)
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Playing the main music.");

            _musicSource.Stop();
            
            _musicSource.loop = true;
            _musicSource.volume = musicVolume;
            
            _musicSource.Play();
        }
    }

    public void PlayGoalHorn(TeamData team)
    {
        if (effectsVolume > 0f)
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Playing the goal horn.");

            _goalSource.Stop();
            
            _goalSource.loop = false;
            _goalSource.volume = effectsVolume;

            AudioClip hornClip = GetGoalHorn(team);
            
            _goalSource.Play();
        }
    }

    public void PlayPeriodHorn()
    {
        if (effectsVolume > 0f)
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Playing the period horn.");

            _periodSource.Stop();
            
            _periodSource.loop = false;
            _periodSource.volume = effectsVolume;
            
            _periodSource.Play();
        }
    }

    public void PlayPenaltyWhistle()
    {
        if (effectsVolume > 0f)
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Playing the penalty whistle.");

            _whistleSource.Stop();
            
            _whistleSource.loop = false;
            _whistleSource.volume = effectsVolume;
            
            _whistleSource.Play();
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private void SetVolumesFromPrefs()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the volume from the PlayerPrefs.");

        float constMusic = ConstantController.Audio_Volume_Music;
        float constEffects = ConstantController.Audio_Volume_Effects;
	    
        musicVolume = (PlayerPrefs.HasKey(ConstantController.Pref_MusicVolume)) ? PlayerPrefs.GetFloat(ConstantController.Pref_MusicVolume) : constMusic;
        effectsVolume = (PlayerPrefs.HasKey(ConstantController.Pref_EffectsVolume)) ? PlayerPrefs.GetFloat(ConstantController.Pref_EffectsVolume) : constEffects;

        SetVolumes(musicVolume, effectsVolume);
    }
    
    private void GetGoalHorn(TeamData team)
    {
        AudioClip newClip = _goalClips[0];

        foreach (AudioClip clip in _goalClips)
        {
            if (clip.name == team.Code)
            {
                newClip = clip;
            }
        }

        return newClip;
    }
#endregion
}}
