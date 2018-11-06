using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour {

    public Slider volumeSlider;

    public AudioSource buildLevel;
    public AudioSource playLevel;

    public AudioSource winAudio;
    public AudioSource failAudio;

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
        buildLevel.mute = !buildLevel.mute;
        playLevel.mute = !playLevel.mute;
    }

    public void PlayWinAudio()
    {
        winAudio.Play();

        playLevel.Stop();
        buildLevel.Stop();
    }

    public void PlayFailAudio()
    {
        failAudio.Play();

        playLevel.Stop();
        buildLevel.Stop();
    }
}