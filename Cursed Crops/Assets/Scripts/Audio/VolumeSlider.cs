using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private VolumeSlider _slider;

    void Start()
    {
        _slider.onValueChanged.AddListener(val => AudioPlayer.Instance.ChangeMasterVolume(val));
    }
}