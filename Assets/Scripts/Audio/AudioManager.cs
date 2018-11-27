using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour {

    public Slider musicSlider;
    public Slider sfxSlider;

    bool IsInPlaymode = false;

    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        musicSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

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