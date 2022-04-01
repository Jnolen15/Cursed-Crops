using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public GameObject bulSprite;
    public Sprite fullBullet;
    public Sprite emptyBullet;

    public int maxBullets = 3;
    public int curBullets = 3;

    private int prevMax = 0;
    private int prevCur = 0;
    [SerializeField]
    private float hbCoolDown = 1f;
    private float hbTime = 0;
    private bool showBullets;

    // Start is called before the first frame update
    void Start()
    {
        prevMax = maxBullets;

        sortBullets();
    }

    // Update is called once per frame
    void Update()
    {
        if (hbTime < hbCoolDown) hbTime += Time.deltaTime; // Face aim period
        else if (showBullets) hideBullets(true);

        if (prevCur != curBullets)
        {
            prevCur = curBullets;
            updateBullets();
        }

        if (prevMax != maxBullets)
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            
            prevMax = maxBullets;
            sortBullets();
        }
    }

    private void sortBullets()
    {
        hbTime = 0;
        if(!showBullets) hideBullets(false);

        // Get an adjustded distance float to offset the bullets so they are center allinged
        float totDist = 0.6f * maxBullets;
        float halfDist = totDist / 2;
        float adjDist = halfDist + 0.3f;

        for (int i = 0; i < maxBullets; i++)
        {
            float offset = (i * 0.6f) - halfDist + 0.3f;
            Vector3 pos = new Vector3(this.transform.localPosition.x + offset, this.transform.localPosition.y, this.transform.localPosition.z);
            //Debug.Log(offset);
            //Debug.Log(pos);
            Instantiate(bulSprite, pos, this.transform.localRotation, this.transform);
        }

        updateBullets();
    }

    private void updateBullets()
    {
        hbTime = 0;
        if(!showBullets) hideBullets(false);

        int count = 0;
        foreach (Transform child in gameObject.transform)
        {
            if(count < curBullets)
            {
                count++;
                child.GetComponent<SpriteRenderer>().sprite = fullBullet;
            } else
            {
                child.GetComponent<SpriteRenderer>().sprite = emptyBullet;
            }
        }
    }

    private void hideBullets(bool hide)
    {
        if (!hide)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.GetComponent<SpriteRenderer>().enabled = true;
            }
            showBullets = true;
        } else
        {
            foreach (Transform child in gameObject.transform)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
            showBullets = false;
        }
    }
}
