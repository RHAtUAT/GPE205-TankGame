using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public GameObject splitScreenOptions;
    public TMP_Dropdown screenLayoutDropDown;

    private void Start()
    {
        if (splitScreenOptions.activeSelf == true)
            screenLayoutDropDown.value = PlayerPrefs.GetInt("ScreenLayout", screenLayoutDropDown.value);
    }

    public void SetScreenLayout()
    {
        //Save the chosen ScreenLayout
        PlayerPrefs.SetInt("ScreenLayout", screenLayoutDropDown.value);

        //Set the value of the dropdown to the chosen ScreenLayout
        screenLayoutDropDown.value = PlayerPrefs.GetInt("ScreenLayout", screenLayoutDropDown.value);

        if (GameManager.instance == null) return;
        GameManager.instance.cameraConfig = (CameraConfig)PlayerPrefs.GetInt("ScreenLayout", screenLayoutDropDown.value);

        if (UIManager.instance == null) return;
        UIManager.instance.SetScreenLayout();
    }

    public void Exit()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            Application.Quit();
        else
        {
            AudioManager.instance.Play("buttonPressed");
            SaveManager.instance.SaveScore();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Back()
    {

        AudioManager.instance.Stop("gameMusic");
        AudioManager.instance.Play("buttonPressed");
        gameObject.SetActive(false);
    }

    public void ChangeVolume()
    {
        if (AudioManager.instance.masterVolume == null && AudioManager.instance.masterVolume == null && AudioManager.instance.masterVolume == null) return;

        foreach (Sound sound in AudioManager.instance.sounds)
        {
            if (sound.type == SoundType.Music)
                sound.source.volume = AudioManager.instance.masterVolume.value * AudioManager.instance.musicVolume.value;

            else if (sound.type == SoundType.SoundEffect)
                sound.source.volume = AudioManager.instance.masterVolume.value * AudioManager.instance.SFXVolume.value;

            else
                sound.source.volume = AudioManager.instance.masterVolume.value;
        }

        PlayerPrefs.SetFloat("masterVolume", AudioManager.instance.masterVolume.value);
        PlayerPrefs.SetFloat("musicVolume", AudioManager.instance.musicVolume.value);
        PlayerPrefs.SetFloat("SFXVolume", AudioManager.instance.SFXVolume.value);

    }
}
