using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class MixerVolumeInitializer : MonoBehaviour
{
    public float InitialVolume = 0.5f;
    public AudioMixer Main;

    // Start is called before the first frame update
    void Start()
    {
        Main.SetFloat("MasterVolume", SliderToMixer(PlayerPrefs.GetFloat("MasterVolume", InitialVolume)));
        Main.SetFloat("MusicVolume", SliderToMixer(PlayerPrefs.GetFloat("MusicVolume", InitialVolume)));
        Main.SetFloat("CharVolume", SliderToMixer(PlayerPrefs.GetFloat("CharVolume", InitialVolume)));
        Main.SetFloat("SFXVolume", SliderToMixer(PlayerPrefs.GetFloat("SFXVolume", InitialVolume)));
    }

    private float SliderToMixer(float x)
    {
        return Mathf.Log10(x) * 30;
    }
}
