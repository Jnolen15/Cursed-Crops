using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("STAB");

            var renderer = other.GetComponent<Renderer>();

            StartCoroutine(hit(renderer));
        }
    }

    IEnumerator hit(Renderer renderer)
    {
        renderer.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(0.2f);
        renderer.material.SetColor("_Color", Color.white);
    }
}
