using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAdder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteLeaner SL = GameObject.Find("SpriteLeanerManager").GetComponent<SpriteLeaner>();
        SL.leanedSprites.Add(this.gameObject);
    }
}
