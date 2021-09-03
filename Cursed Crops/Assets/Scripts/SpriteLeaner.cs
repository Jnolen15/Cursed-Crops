using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteLeaner : MonoBehaviour
{
    public float leanAngle = 40f;
    public bool manageSortLayer = false;

    public GameObject[] taggedSprites;

    void Start()
    {
        // store the tagged sprites
        taggedSprites = GameObject.FindGameObjectsWithTag("LeaningSprite");

        /* // set a new pivot point for all of the tagged sprites
        foreach (GameObject spriteObj in taggedSprites)
        {
            RectTransform currRect = spriteObj.GetComponent<RectTransform>();
            currRect.pivot = new Vector2(0, currRect.rect.y/2);
        }
        */
    }

    void Update()
    {
        foreach(GameObject spriteObj in taggedSprites)
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
                SpriteRenderer spRend = spriteObj.GetComponent<SpriteRenderer>();
                Transform spTrans = spriteObj.GetComponent<Transform>();

                // change sprite renderer's sorting order based on z position
                spRend.sortingOrder = -(int)spTrans.position.z;
            }
        }
    }
}
