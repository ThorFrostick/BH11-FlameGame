using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayJump(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }

    public void PlayWalk(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }
}
