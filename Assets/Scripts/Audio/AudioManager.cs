using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour {

    private Slider musicSlider;
    private Slider sfxSlider;

    private SaveManager saveManager;

    bool IsInPlaymode = false;

    public void Start()
    {
        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");
        //Debug.Log("Volume musica iniziale: " + saveManager.musicVolume);
        FindSoundSlider();
        musicSlider.value = saveManager.musicVolume;
    }

    //Trova gli slider giusti in scena
    private void FindSoundSlider()
    {
        Slider[] sliders;
        sliders = FindObjectOfType<Canvas>().GetComponentsInChildren<Slider>(true);
        foreach (Slider s in sliders)
        {
            if (s.name == "MusicSlider")
                musicSlider = s;
            if (s.name == "SFXSlider")
                sfxSlider = s;
        }
    }

    public void SaveAudioSettings()
    {
        saveManager.musicVolume = AudioListener.volume;
        //Debug.Log("Volume Musica salvato a: " + saveManager.musicVolume);
        SaveUtility.SaveObject(saveManager, "saveFile");
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        //Debug.Log(musicSlider.value);
        AudioListener.volume = musicSlider.value;
    }

    public void BuildPlaySoundtrack()
    {
        if (IsInPlaymode == false)
        {
            FindObjectOfType<AudioManagerBR>().Mute("build");
            FindObjectOfType<AudioManagerBR>().Unmute("play");

            IsInPlaymode = true;
        }
        else {
            FindObjectOfType<AudioManagerBR>().Mute("play");
            FindObjectOfType<AudioManagerBR>().Unmute("build");

            IsInPlaymode = false;
        }
        //playLevel.mute = !playLevel.mute;
    }

    public void PlayWinAudio()
    {
        FindObjectOfType<AudioManagerBR>().Stop("play");
        FindObjectOfType<AudioManagerBR>().Unmute("build");

        FindObjectOfType<AudioManagerBR>().Play("win");
    }

    public void PlayFailAudio()
    {
        FindObjectOfType<AudioManagerBR>().Mute("play");
        FindObjectOfType<AudioManagerBR>().Mute("build");

        FindObjectOfType<AudioManagerBR>().Play("fail");
    }
}