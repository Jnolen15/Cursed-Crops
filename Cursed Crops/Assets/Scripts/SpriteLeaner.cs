using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteLeaner : MonoBehaviour
{
    public bool runInEditor = true;
    public float leanAngle = 40f;
    public bool manageSortLayer = true;

    public GameObject[] taggedSprites;

    void Start()
    {
        // store the tagged sprites ('play' has to be pressed whenever a new sprite is given a tag)
        taggedSprites = GameObject.FindGameObjectsWithTag("LeaningSprite");
    }

    void Update()
    {
        foreach (GameObject spriteObj in taggedSprites)
        {
            // lean the current sprite by the desired angle
            spriteObj.transform.SetPositionAndRotation(
                spriteObj.transform.position,
                Quaternion.Euler(new Vector3(leanAngle, 0, 0))
            );

            // manage sort layer
            if (manageSortLayer)
            {
                // get access to the current sprite renderer & sprite transform
                SpriteRenderer spRend = spriteObj.GetComponentInChildren<SpriteRenderer>();
                Transform spTrans = spriteObj.GetComponent<Transform>();

                // change sprite renderer's sorting order based on z position
                spRend.sortingOrder = -(int)spTrans.position.z;
            }
        }
    }
}
