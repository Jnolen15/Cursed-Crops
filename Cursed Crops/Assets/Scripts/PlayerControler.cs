using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody rb;                  // The player's Rigidbody
    private SpriteRenderer sr;
    private GameObject meleeAttackLeft;
    private GameObject meleeAttackRight;

    private bool flipped = false;

    [SerializeField] private LayerMask layermask;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();

        // Get the left facing attack hitbox and set it inactive
        meleeAttackLeft = this.transform.GetChild(0).gameObject;
        //meleeAttackLeft.SetActive(false);
        // Get the Right facing attack hitbox and set it inactive
        meleeAttackRight = this.transform.GetChild(1).gameObject;
        //meleeAttackRight.SetActive(false);
        /* Note ^
         * Previously I just had one hitbox and would flip the
         * objects X scale when the player flipped directions.
         * However I got warnings that flipping the object
         * could cause the collider to behave not as intended.
         * So instead I just have one attack for each side that
         * are switched based on duirection faced.
         * Then I just flip the players sprite on the spriterender.
         */
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        FaceMouse();
    }

    // Called every fixed frame-rate frame. Better for physics stuff
    private void FixedUpdate()
    {
        Move();
    }

    // Aiming ========================================================

    private void FaceMouse() // Aiming with the mouse
    {
        // Cast a ray from the camera to the ground plane where the mouse is.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layermask))
        {
            // Get the mouse position relative to the player
            Vector3 mousePosition = raycastHit.point;
            Vector3 direction = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            // Flip sprite to face the mouse position
            if (direction.x > 0 && !flipped)
            {
                flipped = true;
                sr.flipX = true;
            }
            else if (direction.x < 0 && flipped)
            {
                flipped = false;
                sr.flipX = false;
            }
        }
    }

    // Move =================================
    private void Move() // Movement code, Uses Vector3, input from Horazontal and Vertical Axis, and the RB to move
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Attack =================================
    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //StartCoroutine(doAttack());
            doAttack();
        }
    }

    private void doAttack()
    {
        if (!flipped)
        {
            Collider[] cols = Physics.OverlapBox(meleeAttackLeft.transform.position, meleeAttackLeft.transform.localScale / 2, 
                                                    meleeAttackLeft.transform.rotation, LayerMask.GetMask("Enemies"));
            foreach (Collider c in cols)
            {
                Debug.Log(c.name);
                if (c.gameObject.tag == "Enemy")
                {

                    EnemyControler enemyControler = c.GetComponent<EnemyControler>();
                    enemyControler.takeDamage(5);
                }
            }
        }
        else
        {
            Collider[] cols = Physics.OverlapBox(meleeAttackRight.transform.position, meleeAttackRight.transform.localScale / 2,
                                                            meleeAttackRight.transform.rotation, LayerMask.GetMask("Enemies"));
            foreach (Collider c in cols)
            {
                Debug.Log(c.name);
                if (c.gameObject.tag == "Enemy")
                {

                    EnemyControler enemyControler = c.GetComponent<EnemyControler>();
                    enemyControler.takeDamage(5);
                }
            }
        }
    }

    IEnumerator doAttackCo()
    {
        if (!flipped)
        {
            meleeAttackLeft.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            meleeAttackLeft.SetActive(false);
        } else
        {
            meleeAttackRight.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            meleeAttackRight.SetActive(false);
        }
    }
}
