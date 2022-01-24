using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class popUpUISO : ScriptableObject
{
    public Transform Prefab;
    public string UIname;
    //public string[] textA = new string[4];
    //public string[] textB = new string[4];
    //public string[] textC = new string[4];
    //public string[] textD = new string[4];

    public PlaceableSO[] buildables = new PlaceableSO[4];
    public CropSO[] plantables = new CropSO[4];

    //public string[] text = new string[]{ buildables[0].placeableName, "two", "three", "four" };
}
