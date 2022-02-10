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
    [TextArea(2, 5)]
    public string desc;
    [TextArea(2, 5)]
    public string stats;
    public string price;
}
