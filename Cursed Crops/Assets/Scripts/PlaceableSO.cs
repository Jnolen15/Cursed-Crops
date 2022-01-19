using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlaceableSO : ScriptableObject
{
    public Transform prefab;
    public string placeableName;
    public string desc;
}
