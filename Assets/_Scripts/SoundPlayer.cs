using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip sound, float pitch, float volume)
    {
        source.clip = sound;
        source.pitch = pitch;
        source.volume = volume;
        source.Play();
        //Destroy(gameObject, sound.length);
    }
}
