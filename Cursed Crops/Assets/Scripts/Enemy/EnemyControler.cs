using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public int health = 10;
    public float overalldamage = 0;
    public bool takingDamage = false;
    public string lastDamageType;
    private Renderer rend;
    private SpriteRenderer sr;
    private ItemDropper itemDropper;
    

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        //sr = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        sr = this.transform.GetComponentInChildren<SpriteRenderer>();
        itemDropper = GetComponent<ItemDropper>();
    }

    public void takeDamageMelee(int dmg)
    {
        // Subtract from health
        health -= dmg;
        overalldamage += dmg;
        takingDamage = true;
        lastDamageType = "Melee";
        // If health is below or equal to 0 die
        if (health <= 0)
        {
            death();
        } else
        {
            // In not dead flash red to show hit
            StartCoroutine(hit(rend));
        }
    }

    public void takeDamageRange(int dmg)
    {
        // Subtract from health
        health -= dmg;
        overalldamage += dmg;
        
        lastDamageType = "Range";
        // If health is below or equal to 0 die
        if (health <= 0)
        {
            death();
        }
        else
        {
            // In not dead flash red to show hit
            StartCoroutine(hit(rend));
        }
    }
    IEnumerator hit(Renderer renderer)
    {
        Debug.Log(lastDamageType);
        //renderer.material.SetColor("_Color", Color.red);
        if (sr != null)
        {
            
            Color prevColor = sr.color;
            sr.color = Color.red;
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            yield return new WaitForSeconds(0.2f);
            takingDamage = false;
            sr.color = prevColor;
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = gameObject.GetComponent<EnemyToPlayer>().originalSpeed;
        }
        //renderer.material.SetColor("_Color", Color.white);
    }

    private void death()
    {
        //itemDropper.DropItem(transform.position);
        //Destroy(this.gameObject);
        //gameObject.SetActive(false);
    }

}
