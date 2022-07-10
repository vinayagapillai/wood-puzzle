using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonPersistent<SoundManager>
{
    private AudioSource _source;

    public AudioClip CLICK;
    public AudioClip LEVELUP;
    public AudioClip GAMEOVER;


    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
