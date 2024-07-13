using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioClip textClip;
    [SerializeField] AudioClip vilageClip;
    [SerializeField] AudioClip swordClip;
    AudioSource audio;

    private string clipName;
    // Start is called before the first frame update
    void Start()    
    {
        instance = this;
        audio = GetComponent<AudioSource>();
    }

    public void E_Sound_OnShot()
    {
        audio.PlayOneShot(textClip);
    }

    public void V_Sound()
    {
        audio.PlayOneShot(vilageClip);
    }

    public void PlaySwordSfx()
    {
        audio.PlayOneShot(swordClip);
    }
}
