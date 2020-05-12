using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource _audioBGM;


    private Dictionary<AudioSource, AudioClip> _audioSounds;
    private float _sfxVolume = 1;


    void Awake()
    {

        Instance = this;
        _audioBGM = gameObject.AddComponent<AudioSource>();
        // _audioBGM.loop = true;
        _audioSounds = new Dictionary<AudioSource, AudioClip>();
        SetBGMVolume(PlayerPrefs.GetFloat("BGM", 1.0f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFX", 1.0f));
    }

    public void SetBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat("BGM", volume);
        _audioBGM.volume = volume;
    }

    public float GetBGMVolume()
    {
        return PlayerPrefs.GetFloat("BGM", 1.0f);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFX", volume);
        _sfxVolume = volume;
        foreach (var v in _audioSounds)
        {
            v.Key.volume = volume;
        }
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFX", 1.0f);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip != null && _audioBGM.clip != clip)
        {
            _audioBGM.clip = clip;
            _audioBGM.Play();
        }
    }

    public void StopBGM()
    {
        _audioBGM.Stop();
    }

    public void PlaySFX(AudioClip clip, int playTimes = 1, Action callBack = null)
    {
        if (clip != null)
        {
            AudioSource source = null;
            foreach (var v in _audioSounds)
            {
                if (v.Value == null)
                    source = v.Key;
            }
            if (source == null)
                source = gameObject.AddComponent<AudioSource>();

            source.volume = _sfxVolume;
            source.clip = clip;
            if (_audioSounds.ContainsKey(source))
                _audioSounds[source] = clip;
            else
                _audioSounds.Add(source, clip);
            StartCoroutine("AudioTimer", new AudioData(source, playTimes, callBack));
        }
    }

    public void StopSFX(AudioClip clip)
    {
        if (clip != null)
        {
            foreach (var v in _audioSounds)
            {
                if (clip == v.Value)
                {
                    v.Key.Stop();
                    _audioSounds[v.Key] = null;
                }
            }
        }
    }

    public void PauseAllListener(bool pause)
    {
        AudioListener.pause = pause;
    }

    private IEnumerator AudioTimer(AudioData audioData)
    {
        audioData.source.Play();
        yield return new WaitForSeconds(audioData.source.clip.length);
        audioData.playTimes--;
        if (audioData.playTimes > 0)
        {
            StartCoroutine("AudioTimer", audioData);
        }
        else
        {
            _audioSounds[audioData.source] = null;
            if (audioData.callback != null)
                audioData.callback();
        }
    }
}

public class AudioData
{
    public AudioSource source;
    public int playTimes;
    public Action callback;

    public AudioData(AudioSource source, int playTimes, Action callback)
    {
        this.source = source;
        this.playTimes = playTimes;
        this.callback = callback;
    }
}
