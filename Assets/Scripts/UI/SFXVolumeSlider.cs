using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeSlider : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.SFXVolume = gameObject.GetComponentInChildren<Slider>();
        AudioManager.instance.SFXVolume.value = PlayerPrefs.GetFloat("SFXVolume", 1);
    }
}
