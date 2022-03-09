using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    void Start()
    {
        AudioPlayer.Instance.PlaySound(_clip);

    }
}