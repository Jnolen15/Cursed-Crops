using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAdder : MonoBehaviour
{

    private SpriteLeaner SL;

    // Start is called before the first frame update
    void Start()
    {
        SL = GameObject.Find("SpriteLeanerManager").GetComponent<SpriteLeaner>();
        SL.leanedSprites.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        // checking to see if SL has been destroyed on scene end
        if (SL != null)
        {
            // Debug.Log(this.gameObject.transform.parent.transform.parent.name);
            SL.leanedSprites.Remove(this.gameObject);
            
        }
    }
}
