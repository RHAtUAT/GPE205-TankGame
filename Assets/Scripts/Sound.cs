using UnityEngine;


[System.Serializable]
public class Sound
{

    public SoundType type;
    public string name;
    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;

    [Range(1.0f, 3.0f)]
    public float pitch = 1.0f;

    public bool loop;
    public bool playOnAwake;

    [HideInInspector] public AudioSource source;
}

public enum SoundType { SoundEffect, Music };
