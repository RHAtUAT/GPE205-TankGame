using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider SFXVolume;
    public Sound[] sounds;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;


            if (sound.type == SoundType.Music)
                sound.volume *= PlayerPrefs.GetFloat("masterVolume", 1) * PlayerPrefs.GetFloat("musicVolume", 1);

            else if (sound.type == SoundType.SoundEffect)
                sound.volume *= PlayerPrefs.GetFloat("masterVolume", 1) * PlayerPrefs.GetFloat("SFXVolume", 1);

            else
                sound.volume *= masterVolume.value;
            Debug.Log("Volume: " + sound.volume);
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;

            if (sound.playOnAwake)
                Play(sound.name);
        }
    }

    public void Play(string name)
    {
        Sound _sound = Array.Find(sounds, sound => sound.name == name);
        if (_sound == null)
        {
            Debug.LogWarning("AudioFile: " + name + " not found");
            return;
        }
        _sound.source.Play();
    }

    public void Stop(string name)
    {
        Sound _sound = Array.Find(sounds, sound => sound.name == name);
        if (_sound == null)
        {
            Debug.LogWarning("AudioFile: " + name + " not found");
            return;
        }
        _sound.source.Stop();
    }
}
