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
    [SerializeField] float sliderValue = 1;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(SliderValueChanged);
        _toggle.onValueChanged.AddListener(ToggleValueChanged);
        sliderValue = PlayerPrefs.GetFloat(_volumeParameter, _slider.value);
        _slider.value = sliderValue;
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
        _mixer.SetFloat(_volumeParameter, SliderToMixer(value));
        _disableToggleEvent = true;
        _toggle.isOn = _slider.value > _slider.minValue;
        _disableToggleEvent = false;
        PlayerPrefs.SetFloat(_volumeParameter, _slider.value);
    }

    private float SliderToMixer(float x)
    {
        return Mathf.Log10(x) * _multiplier;
    }
}
