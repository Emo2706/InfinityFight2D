using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string soundName;

    [Range(0f, 1f)]
    public float volume;

    [Range(0f, 1f)]
    public float pitch;

    public bool loop;

    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;



}


