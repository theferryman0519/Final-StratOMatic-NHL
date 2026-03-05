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
    [SerializeField] private AudioSource _hornASource;
    [SerializeField] private AudioSource _hornBSource;
    [SerializeField] private AudioSource _periodSource;
    [SerializeField] private AudioSource _whistleSource;
    [SerializeField] private AudioSource _movePuckSource;

    [Header("Sound Effects Clips")]
    [SerializeField] private List<AudioClip> _goalHornClips;
    [SerializeField] private AudioClip _defaultHornClip;
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
        CoreController.Inst.WriteLog(this.GetType().Name, $"Updating the volumes.");
		
        _musicSource.volume = newMusicVolume;
        _hornASource.volume = newEffectsVolume;
        _hornBSource.volume = newEffectsVolume;
        _periodSource.volume = newEffectsVolume;
        _whistleSource.volume = newEffectsVolume;
        _movePuckSource.volume = newEffectsVolume;
		
        musicVolume = newMusicVolume;
        effectsVolume = newEffectsVolume;

        PlayerPrefs.SetFloat(ConstantController.Pref_MusicVolume, newMusicVolume);
        PlayerPrefs.SetFloat(ConstantController.Pref_EffectsVolume, newEffectsVolume);
        PlayerPrefs.Save();
    }

    public void PlayMainMusic()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Playing the main music.");

        if (musicVolume > 0f)
        {
            _musicSource.Stop();
            
            _musicSource.loop = true;
            _musicSource.volume = musicVolume;
            
            _musicSource.Play();
        }
    }

    public void PlayGoalHorn(TeamData team, bool isHomeTeam)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Playing the goal horn.");

        if (effectsVolume > 0f)
        {
            AudioSource hornSource = isHomeTeam ? _hornASource : _hornBSource;

            hornSource.Stop();
            
            hornSource.loop = false;
            hornSource.volume = effectsVolume;

            hornSource.clip = SetTeamClip(team);
            
            hornSource.Play();
        }
    }

    public void PlayPeriodHorn()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Playing the period horn.");

        if (effectsVolume > 0f)
        {
            _periodSource.Stop();
            
            _periodSource.loop = true;
            _periodSource.volume = effectsVolume;
            
            _periodSource.Play();
        }
    }

    public void PlayPenaltyWhistle()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Playing the penalty whistle.");

        if (effectsVolume > 0f)
        {
            _whistleSource.Stop();
            
            _whistleSource.loop = true;
            _whistleSource.volume = effectsVolume;
            
            _whistleSource.Play();
        }
    }

    public void PlayMovingPuck()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Playing the move puck sound.");

        if (effectsVolume > 0f)
        {
            _movePuckSource.Stop();
            
            _movePuckSource.loop = true;
            _movePuckSource.volume = effectsVolume;
            
            _movePuckSource.Play();
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private AudioClip SetTeamClip(TeamData team)
    {
        AudioClip newClip = _defaultHornClip;

        for (int i = 0; i < _goalHornClips.Count; i++)
        {
            int index = i;

            if (_goalHornClips[index].name == team.Code)
            {
                newClip = _goalHornClips[index];
            }
        }

        return newClip;
    }

    private void SetVolumesFromPrefs()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting the volume from the PlayerPrefs.");

        float constMusic = ConstantController.Audio_Volume_Music;
        float constEffects = ConstantController.Audio_Volume_Effects;
	    
        musicVolume = (PlayerPrefs.HasKey(ConstantController.Pref_MusicVolume)) ? 
            PlayerPrefs.GetFloat(ConstantController.Pref_MusicVolume) : constMusic;
        effectsVolume = (PlayerPrefs.HasKey(ConstantController.Pref_EffectsVolume)) ? 
            PlayerPrefs.GetFloat(ConstantController.Pref_EffectsVolume) : constEffects;

        SetVolumes(musicVolume, effectsVolume);
    }
#endregion
}}
