using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioPlayer : MonoBehaviour
{
    //Insures that only a single instance of this class will be running at a time
    public static AudioPlayer Instance;

    [SerializeField] private AudioSource _musicSource, _effectsSource;

    void Awake(){
        //Creates an instance of this class if one hasn't already been created
        if(Instance == null){
            Instance = this;
            //Tells Unity not to detroy the instance while switching scenes
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip _clip){
        _effectsSource.PlayOneShot(_clip);
    }

    public void ChangeMasterVolume(float val){
        AudioListener.volume = val;
    }
}