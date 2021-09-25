using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // FLOATS / INTS ===========
    [SerializeField] private float attackCDTimer = 1;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float coolDownTimer;
    [SerializeField] private float rollSpeedMax = 100;
    private float rollSpeed;
    private int attackChain = 1;
    public int overAllPlayerDamage = 0;

    public float moveSpeed;

    // BOOLS ===========
    private bool flipped = false;
    private bool attackCD = false;
    private bool rangeCD = false;

    public bool useControler;               // If using controller changes aiming

    // STRINGS ===========
    // Strings are used for input mapping. This is done to make the creation of multiple players clean and simple.
    [SerializeField] private string HorizontalMove = "Horizontal_P1";
    [SerializeField] private string VerticalMove = "Vertical_P1";
    [SerializeField] private string HorizontalAimController = "CHorizontal_P1";
    [SerializeField] private string VerticalAimController = "CVertical_P1";
    [SerializeField] private string AttackButton = "Attack_P1";
    [SerializeField] private string RangedButton = "Ranged_P1";
    [SerializeField] private string RollButton = "Roll_P1";

    // OTHER COMPONENTS ===========
    private Rigidbody rb;                  // The player's Rigidbody
    //private SpriteRenderer sr;
    private SpriteRenderer playerSprite;
    private Animator animator;
    private GameObject meleeAttackLeft;
    private GameObject meleeAttackRight;
    private Vector3 movement;
    private Vector3 rollDir;
    [SerializeField] private LayerMask layermask;
    private delegate void Callback();
    private enum State
    {
        Normal,
        Rolling,
    }
    private State state;

    public GameObject bullet;


    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        rb = GetComponent<Rigidbody>();
        //sr = GetComponent<SpriteRenderer>();

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

        // Player Sprite
        playerSprite = this.transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        animator = this.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Normal) // Not in rolling state
        {
            Attack();
            RangeAttack();
            if (!useControler) FaceMouse();
            else FaceController();
        }

        if (coolDownTimer < attackCDTimer) coolDownTimer += Time.deltaTime; // Attack cooldown

        if (coolDownTimer > attackDuration) animator.SetBool("Melee", false); // Reset attack animation

        if (Input.GetButtonDown(RollButton)) //Dodge Roll
        {
            Debug.Log(RollButton);
            state = State.Rolling;
            rollSpeed = rollSpeedMax;
        }
    }

    // Called every fixed frame-rate frame. Better for physics stuff
    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                //if(coolDownTimer > attackDuration) Move(); //Do it this way if movement should be locked while attacking
                Move();
                rollDir = movement;
                break;
            case State.Rolling:
                DodgeRoll();

                float rollSpeedDropMultiplier = 3f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                if (rollSpeed < moveSpeed) state = State.Normal;

                break;
        }
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
                playerSprite.flipX = true;
            }
            else if (direction.x < 0 && flipped)
            {
                flipped = false;
                playerSprite.flipX = false;
            }
        }
    }

    private void FaceController()
    {
        Vector3 direction = Vector3.right * Input.GetAxisRaw(HorizontalAimController) + Vector3.up * -Input.GetAxisRaw(VerticalAimController);

        // Flip sprite to face the mouse position
        if (direction.x > 0 && !flipped)
        {
            flipped = true;
            playerSprite.flipX = true;
        }
        else if (direction.x < 0 && flipped)
        {
            flipped = false;
            playerSprite.flipX = false;
        }
    }

    // Move =================================
    private void Move() // Movement code, Uses Vector3, input from Horazontal and Vertical Axis, and the RB to move
    {
        movement = new Vector3(Input.GetAxisRaw(HorizontalMove), 0, Input.GetAxisRaw(VerticalMove)).normalized;

        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void DodgeRoll()
    {
        rb.MovePosition(transform.position + rollDir * rollSpeed * Time.fixedDeltaTime);
    }

    // Attack =================================
    private void Attack()
    {
        if (Input.GetButtonDown(AttackButton) && !attackCD)
        {
            Debug.Log(AttackButton);
            if (coolDownTimer > attackCDTimer) // If attack isnt on cooldown
            {
                //attackCD = true;
                //StartCoroutine(cooldown(delegate() {attackCD = false;}, 0.2f));

                coolDownTimer = 0;
                attackChain = 1;
                animator.SetBool("Melee", true);
                DoAttack();
                Debug.Log("Attack 1");

            } else if (coolDownTimer > attackDuration) // If attack is on cooldown, subsiquent atacks chain
            {
                if (attackChain == 1)
                {
                    coolDownTimer = 0;
                    attackChain = 2;
                    animator.SetBool("Melee", true);
                    DoAttack();
                    Debug.Log("Attack 2");
                }
                else if (attackChain == 2)
                {
                    coolDownTimer = 0;
                    attackChain = 3;
                    animator.SetBool("Melee", true);
                    DoAttack();
                    Debug.Log("Attack 3");
                }
                else if (attackChain == 3)
                {
                    Debug.Log("Attack chain over");
                }
            }
        }
    }

    private void DoAttack()
    {
        if (!flipped) // Left attack hit detection
        {
            Collider[] cols = Physics.OverlapBox(meleeAttackLeft.transform.position, meleeAttackLeft.transform.localScale / 2, 
                                                    meleeAttackLeft.transform.rotation, LayerMask.GetMask("Enemies"));
            DamageEnemies(cols);
        }
        else  // Right attack hit detection
        {
            Collider[] cols = Physics.OverlapBox(meleeAttackRight.transform.position, meleeAttackRight.transform.localScale / 2,
                                                            meleeAttackRight.transform.rotation, LayerMask.GetMask("Enemies"));
            DamageEnemies(cols);
        }
    }

    private void DamageEnemies(Collider[] cols)
    {
        foreach (Collider c in cols)
        {
            //Debug.Log(c.name);
            EnemyControler enemyControler = c.GetComponent<EnemyControler>();

            if (c.gameObject.tag == "Enemy")
            {
                if (attackChain == 1)
                {
                    enemyControler.takeDamage(2);
                    overAllPlayerDamage += 2;
                }
                else if (attackChain == 2)
                {
                    enemyControler.takeDamage(3);
                    overAllPlayerDamage += 3;
                }
                else if (attackChain == 3)
                {
                    enemyControler.takeDamage(6);
                    overAllPlayerDamage += 6;
                }
            }
        }
    }

    private void RangeAttack()
    {
        if (Input.GetButtonDown(RangedButton) && !rangeCD)
        {
            // Set cooldown true
            rangeCD = true;
            //Play animation
            animator.SetBool("Ranged", true);
            StartCoroutine(cooldown(() => { animator.SetBool("Ranged", false); }, 0.2f));
            // Create bullet
            GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
            // Send bullet in correct direction
            if (flipped) bul.GetComponent<Bullet>().movement = new Vector3(1, 0, 0);
            else bul.GetComponent<Bullet>().movement = new Vector3(-1, 0, 0);
            // Start ranged attack cooldown
            StartCoroutine(cooldown(()=> { rangeCD = false; }, 4f));

        } else if (Input.GetButtonDown(RangedButton) && rangeCD) // If on cooldown send dubug msg
        {
            Debug.Log("Range on Cooldown");
        }
    }

    // Misc =================================

    IEnumerator cooldown(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        if (callback != null)
        {
            callback();
        }
    }
}
