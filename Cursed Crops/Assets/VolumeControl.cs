using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string _volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _slider;
    [SerializeField] float _multiplier = 30f;
    [SerializeField] Toggle _toggle;
    [SerializeField] bool _disableToggleEvent;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(SliderValueChanged);
        _toggle.onValueChanged.AddListener(ToggleValueChanged);
    }

    private void ToggleValueChanged(bool enableSound)
    {
        if (_disableToggleEvent)
        {
            return;
        }
        if (enableSound)
        {
            _slider.value = _slider.maxValue;
        }
        else
        {
            _slider.value = _slider.minValue;
        }
    }

    private void onDisable()
    {
        PlayerPrefs.SetFloat(_volumeParameter, _slider.value);
    }

    private void SliderValueChanged(float value)
    {
        _mixer.SetFloat(_volumeParameter, Mathf.Log10(value) *_multiplier);
        _disableToggleEvent = true;
        _toggle.isOn = _slider.value > _slider.minValue;
        _disableToggleEvent = false;
    }

    //Start is called before the first frame update  
    void Start()
    {
        _slider.value = PlayerPrefs.GetFloat(_volumeParameter, _slider.value);
        // SliderValueChanged(_slider.value);
    }

    // Update is called once per frame
    //void Update()
  //  {
        
  //  }
}
