using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagerBR : MonoBehaviour {

    public Sounds[] sounds;
    //public Slider volumeSlider;

    //public static AudioManagerBR instance;

    void Awake () {

        //non sono sicuro mi servano
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

		foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
        }
    }

    public void Play (string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s==null)
        {
            Debug.LogWarning("Soun: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}

//      FindObjectOfType<AudioManagerBR>().Play("nome dell'audio");