using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour {

    public Slider volumeSlider;

    bool IsInPlaymode = false;

    //public AudioSource buildLevel;
    //public AudioSource playLevel;

    //public AudioSource winAudio;
    //public AudioSource failAudio;

    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        //Debug.Log(volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
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
        FindObjectOfType<AudioManagerBR>().Stop("build");

        FindObjectOfType<AudioManagerBR>().Play("win");
    }

    public void PlayFailAudio()
    {
        //FindObjectOfType<AudioManagerBR>().Stop("play");
        FindObjectOfType<AudioManagerBR>().Stop("build");

        FindObjectOfType<AudioManagerBR>().Play("fail");
    }
}