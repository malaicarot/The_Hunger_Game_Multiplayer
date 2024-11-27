using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundController : Singleton<SoundController>
{

    AudioSource PistolAudio;
    AudioSource RifleAudio;
    AudioSource ShotgunAudio;
    AudioSource SniperAudio;
    AudioSource ExplodeAudio;
    /**************************/
    AudioSource GetItemAudio;
    AudioSource ShootAudio;
    AudioSource WinnerAudio;

    /**************************/

    AudioSource BackgroundMusic;





    private void Start()
    {
        PistolAudio = GetComponents<AudioSource>()[0];
        RifleAudio = GetComponents<AudioSource>()[1];
        ShotgunAudio = GetComponents<AudioSource>()[2];
        SniperAudio = GetComponents<AudioSource>()[3];
        BackgroundMusic = GetComponents<AudioSource>()[4];
        ExplodeAudio = GetComponents<AudioSource>()[5];
        GetItemAudio = GetComponents<AudioSource>()[6];
        WinnerAudio = GetComponents<AudioSource>()[7];

    }

    public void ShootAudioPlay(String type)
    {
        switch (type)
        {
            case "Pistol":
                ShootAudio = PistolAudio;
                break;
            case "Rifle":
                ShootAudio = RifleAudio;
                break;
            case "Shotgun":
                ShootAudio = ShotgunAudio;
                break;
            case "Sniper":
                ShootAudio = SniperAudio;
                break;
        }
        PlayAudio(ShootAudio);
    }

    public void PowerUpAudioPlay()
    {
        if (GetItemAudio != null && GetItemAudio.clip != null)
        {
            GetItemAudio.Stop();
            GetItemAudio.Play();
        }
    }

    public void ExplodeAudioPlay()
    {
        PlayAudio(ExplodeAudio);
    }


    public void PlayBackgroundMusic()
    {
        PlayAudio(BackgroundMusic);

    }

    public void StopBackgroundMusic()
    {
        StopAudio(BackgroundMusic);

    }

    public void GetItemAudioPlay()
    {
        PlayAudio(GetItemAudio);

    }

    public void WinnerAudioPlay()
    {
        PlayAudio(WinnerAudio);

    }

    public void WinnerAudioStop()
    {
        StopAudio(WinnerAudio);

    }
    void PlayAudio(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }

    void StopAudio(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Stop();
        }
    }
}
