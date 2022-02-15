using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteLeaner : MonoBehaviour
{
    public bool runInEditor = true;
    public float leanAngle = 40f;
    public bool manageSortLayer = true;

    public List<GameObject> leanedSprites = new List<GameObject>();
    void Start()
    {
        // store the tagged sprites ('play' has to be pressed whenever a new sprite is given a tag)
        GameObject[] taggedSprites = GameObject.FindGameObjectsWithTag("LeaningSprite");
        foreach (GameObject sprite in taggedSprites)
        {
            leanedSprites.Add(sprite);
        }

    }

    void Update()
    {
        foreach (GameObject spriteObj in leanedSprites)
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
