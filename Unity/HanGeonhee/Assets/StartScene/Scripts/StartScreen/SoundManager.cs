using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundClip
{
    [SerializeField] AudioClip[] clips;

    public AudioClip[] Clips
    {
        get
        {
            return clips;
        }
    }
}

public class SoundManager : MonoSingletonManager<SoundManager>
{
    [SerializeField] AudioSource sfxAudio;
    [SerializeField] AudioSource bgmAudio;

    [SerializeField] AudioMixer masterAudio;

    private bool muteSFX;

    public bool MuteSFX
    {
        set { muteSFX = value; }
    }

    public void SFXPlay(AudioClip clip)
    {
        if (muteSFX)
        {
            return;
        }

        sfxAudio.PlayOneShot(clip);
    }

    public void BGMPlay(AudioClip clip, bool loop = true)
    {
        bgmAudio.clip = clip;
        bgmAudio.loop = loop;
        bgmAudio.Play();
    }

    public void StopBGM()
    {
        if (bgmAudio.isPlaying)
        {
            bgmAudio.Stop();
        }
    }

    public void SetVolumeSFX(float volume)
    {
        sfxAudio.volume = volume;

        DataManager.Instance.Data.SFXVolume = volume;
    }

    public void SetVolumeBGM(float volume)
    {
        bgmAudio.volume = volume;

        DataManager.Instance.Data.BGMVolume = volume;
    }

    public void SetVolumeMaster(float volume)
    {
        masterAudio.SetFloat("Master", ((100 * volume) - 60) / 2);

        if (volume <= 0f)
        {
            masterAudio.SetFloat("Master", -80);
        }

        DataManager.Instance.Data.MasterVolume = volume;
    }

    //ButtonSound
    [SerializeField] SoundClip buttonClip;
    public void ButtonSound()
    {
        SFXPlay(buttonClip.Clips[UnityEngine.Random.Range(0, buttonClip.Clips.Length)]);
    }
}