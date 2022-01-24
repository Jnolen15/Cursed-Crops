using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CropSO : ScriptableObject
{
    public Transform prefab;
    public Sprite preview;
    // title, description, stats, and price
    public string cropName;
    public string desc;
    public string stats;
    public string price;
}
