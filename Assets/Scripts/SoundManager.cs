using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] _audio;

    int _channel;

    void Start()
    {
        for(int i=0; i<_audio.Length; i++)
            _audio[i].GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audio)
    {
        _channel = _channel % 3;
        _audio[_channel].clip = audio;
        _audio[_channel].Play();
        _channel++;
    }
}

