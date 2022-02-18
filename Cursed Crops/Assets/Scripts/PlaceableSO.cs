using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlaceableSO : ScriptableObject
{
    public Transform prefab;
    public Sprite preview;
    [Header("title, description, stats, and price")]
    public string placeableName;
    [TextArea(2, 5)]
    public string desc;
    [TextArea(2, 5)]
    public string stats;
    public string price;
    [Header("Cost")]
    public int cost;

}
