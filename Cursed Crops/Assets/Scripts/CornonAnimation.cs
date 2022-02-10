using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornonAnimation : MonoBehaviour
{
    // ================= Public variables =================


    // ================= Private variables =================
    private Animator animator;
    private CornnonAI cAI;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        cAI = this.gameObject.GetComponent<CornnonAI>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", cAI.enemySpeed);
        
    }
}
