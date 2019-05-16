using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.musicVolume = gameObject.GetComponentInChildren<Slider>();
        AudioManager.instance.musicVolume.value = PlayerPrefs.GetFloat("musicVolume", 1);
    }
}