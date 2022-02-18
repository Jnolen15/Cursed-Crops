using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CropSO : ScriptableObject
{
    public Transform prefab;
    public Sprite preview;
    [Header("title, description, stats, and price")]
    public string cropName;
    [TextArea(2, 5)]
    public string desc;
    [TextArea(2, 5)]
    public string stats;
    public string price; // This isn't needed. But its hooked up to the UI. Delete later
    [Header("Bounty values")]
    public int bountyWorth;
    public int numBeforeFallOff; // Number of crops you can plant before they start falling off
    public int bountyFallOffAmmount;
}
