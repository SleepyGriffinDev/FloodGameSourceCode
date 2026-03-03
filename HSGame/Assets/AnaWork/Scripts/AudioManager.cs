using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{   
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    [SerializeField]
    AudioClip background;
    [SerializeField]
    AudioClip checkpoint;
    [SerializeField]
    AudioClip death;
    [SerializeField]
    AudioClip footsteps;

    //Dictionary to add functionality to Play(string)
    Dictionary<string, AudioClip> SFXClips = new Dictionary<string, AudioClip>();

    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);


    //}
    private void Start()
    {
        /*Add Dictionary's Key-Value Pairs*/
        SFXClips.Add("Background", background);
        SFXClips.Add("Checkpoint", checkpoint);
        SFXClips.Add("Death", death);
        SFXClips.Add("Footsteps", footsteps);

        musicSource.clip = background;
        musicSource.Play();
    }

    public bool IsPlayingSFX()
    {
        return SFXSource.isPlaying;
    }
    public bool IsPlayingMusic()
    {
        return musicSource.isPlaying;
    }

    public void Play(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void Play(string clipKey)
    {
        SFXSource.PlayOneShot(SFXClips[clipKey]);
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }


}
