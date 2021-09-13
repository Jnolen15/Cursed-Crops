using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public int health = 10;
    private Renderer rend;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        //sr = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        sr = this.transform.GetComponentInChildren<SpriteRenderer>();

    }

    public void takeDamage(int dmg)
    {
        // Subtract from health
        health -= dmg;

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

    IEnumerator hit(Renderer renderer)
    {
        //renderer.material.SetColor("_Color", Color.red);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
        //renderer.material.SetColor("_Color", Color.white);
    }

    private void death()
    {
        Destroy(gameObject);
    }
}
