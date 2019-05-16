using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.masterVolume = gameObject.GetComponentInChildren<Slider>();
        AudioManager.instance.masterVolume.value = PlayerPrefs.GetFloat("masterVolume", 1);
    }
}
