using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CropSO : ScriptableObject
{
    public Transform prefab;
    public Sprite preview;
    public string cropName;
    public string desc;
}
