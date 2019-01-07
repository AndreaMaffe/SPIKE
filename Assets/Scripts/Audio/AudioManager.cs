using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour {

    public Sounds[] sounds;
    public Sounds[] placings;
    private int count = 0;

    private Slider musicSlider;
    private Slider sfxSlider;

    //public Toggle soundToggle;
    public Toggle musicToggle;

    private SaveManager saveManager;

    bool IsInPlaymode = false;

    public void Awake()
    {
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
        }

        foreach (Sounds p in placings)
        {
            p.source = gameObject.AddComponent<AudioSource>();
            p.source.clip = p.clip;

            p.source.volume = p.volume;
            p.source.pitch = p.pitch;
            p.source.loop = p.loop;
        }
    }

    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) //schermata home
        {
            Play("landing");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1) //SampleSceneRange
        {
            Play("build");
            Play("play");
            Mute("play");
        }

        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");
        //Debug.Log("Volume musica iniziale: " + saveManager.musicVolume);
        FindSoundSlider();
        if (musicToggle)
            musicToggle.isOn = saveManager.musicVolume;
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

    public void OnValueChanged()
    {
        if (!musicToggle.isOn)
        {
            AudioListener.volume = 1f;
            saveManager.musicVolume = true;
            SaveUtility.SaveObject(saveManager, "saveFile");
        }
        else
        {
            AudioListener.volume = 0;
            saveManager.musicVolume = false;
            SaveUtility.SaveObject(saveManager, "saveFile");
        }
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
            FindObjectOfType<AudioManager>().Mute("build");
            FindObjectOfType<AudioManager>().Unmute("play");

            IsInPlaymode = true;
        }
        else {
            FindObjectOfType<AudioManager>().Mute("play");
            FindObjectOfType<AudioManager>().Unmute("build");

            IsInPlaymode = false;
        }
        //playLevel.mute = !playLevel.mute;
    }

    public void PlayWinAudio()
    {
        FindObjectOfType<AudioManager>().Mute("play");
        FindObjectOfType<AudioManager>().Mute("build");

        FindObjectOfType<AudioManager>().Play("win");
    }

    public void PlayFailAudio()
    {
        FindObjectOfType<AudioManager>().Mute("play");
        FindObjectOfType<AudioManager>().Mute("build");

        FindObjectOfType<AudioManager>().Play("fail");
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void Mute(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.mute = true;
    }

    public void Unmute(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.mute = false;
    }

    //public void RandomClip()
    //{
    //    Sounds p = Array.Find(placings, sound => sound.name == name);
    //    if (p == null)
    //    {
    //        Debug.LogWarning("Sound: " + name + " not found!");
    //        return;
    //    }
    //}

    public void ObstacleDraggedIn()
    {
        int i = Random.Range(0, placings.Length);
        placings[i].source.Play();
    }
}