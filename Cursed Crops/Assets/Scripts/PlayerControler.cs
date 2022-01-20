using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    // FLOATS / INTS ===========
    [SerializeField] private float attackCDTimer = 1;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float rollSpeedMax = 24;
    [SerializeField] private float rollSpeedFallofDelay = 0.3f;
    [SerializeField] private float rollCDTime = 0.3f;
    [SerializeField] private float rangeCDTime = 0.4f;
    [SerializeField] private float faceAimTimer = 3f;
    private int attackChain = 1;
    private float attackcoolDown;
    private float rollSpeed;
    private float rollSpeedDropMultiplier;
    private float faceAimTime = 0;

    public int overAllPlayerDamage = 0;
    public float moveSpeed;
    public int money = 0;
    public int health = 10;
    public int maxHealth = 10;

    // BOOLS ===========
    private bool attackCD = false;
    private bool rangeCD = false;
    private bool rollCD = false;
    public bool faceaim = false;
    private bool startRollFallOff = false;

    public bool flipped = false;
    public bool useControler;               // If using controller changes aiming

    // STRINGS ===========
    // Strings are used for input mapping. This is done to make the creation of multiple players clean and simple.
    //[SerializeField] private string HorizontalMove = "Horizontal_P1";
    //[SerializeField] private string VerticalMove = "Vertical_P1";
    //[SerializeField] private string HorizontalAimController = "CHorizontal_P1";
    //[SerializeField] private string VerticalAimController = "CVertical_P1";
    //[SerializeField] private string AttackButton = "Attack_P1";
    //[SerializeField] private string RangedButton = "Ranged_P1";
    //[SerializeField] private string RollButton = "Roll_P1";

    // OTHER COMPONENTS ===========
    //private PlayerInputActions playerInputActions;  // The player input object script
    private BuildingSystem bs;
    private Rigidbody rb;                  // The player's Rigidbody
    private CapsuleCollider cc;
    private SpriteRenderer playerSprite;
    private Animator animator;
    private GameObject meleeAttackLeft;
    private GameObject meleeAttackRight;
    private Vector2 aimInputVector = Vector2.zero;
    private Vector2 moveInputVector = Vector2.zero;
    private Vector3 movement;
    private Vector3 rollDir;
    private Vector3 direction;
    [SerializeField] private LayerMask groundLayermask;
    private delegate void Callback();
    public enum State
    {
        Normal,
        Building,
        Rolling,
    }
    public State state;

    public GameObject bullet;


    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        bs = GetComponent<BuildingSystem>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();

        // Get the left facing attack hitbox and set it inactive
        meleeAttackLeft = this.transform.GetChild(0).gameObject;
        // Get the Right facing attack hitbox and set it inactive
        meleeAttackRight = this.transform.GetChild(1).gameObject;
        /* Note ^
         * Previously I just had one hitbox and would flip the
         * objects X scale when the player flipped directions.
         * However I got warnings that flipping the object
         * could cause the collider to behave not as intended.
         * So instead I just have one attack for each side that
         * are switched based on duirection faced.
         * Then I just flip the players sprite on the spriterender.
         * 
         * > Could just have it move sides instead of flip.
         */

        // Player Sprite
        playerSprite = this.transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        animator = this.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Animator>();
    }

    private void Awake()
    {
        /*// Get access to the player input actions asset
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Assign each action to a function
        playerInputActions.Player.Attack.performed += Attack_performed;
        playerInputActions.Player.Ranged.performed += Ranged_performed;
        playerInputActions.Player.Roll.performed += Roll_performed;
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.Rolling)
        {
            if (!useControler) FaceMouse();
            else FaceController();
        }

        if (bs.buildmodeActive)
            state = State.Building;

        // Cooldown stuffs
        if (faceAimTime < faceAimTimer) faceAimTime += Time.deltaTime; // Face aim period
        else faceaim = false;

        if (attackcoolDown < attackCDTimer) attackcoolDown += Time.deltaTime; // Attack cooldown

        if (attackcoolDown > attackDuration) animator.SetBool("Melee", false); // Reset attack animation

        // Move into the SpriteLeaner script! Placeholder for now
        playerSprite.sortingOrder = -(int)this.transform.position.z;
    }

    // Called every fixed frame-rate frame. Better for physics stuff
    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                // Can't move while attacking
                if (!animator.GetBool("Ranged") && attackcoolDown > attackDuration)
                {
                    Move();
                }
                rollDir = movement;
                break;
            case State.Building:
                // Exit building state if out of build mode
                if (!bs.buildmodeActive)
                    state = State.Normal;
                // Can't move while attacking
                if (!animator.GetBool("Ranged"))
                {
                    if (attackcoolDown > attackDuration)
                        Move();
                }
                rollDir = movement;
                break;
            case State.Rolling:
                cc.enabled = false;
                DodgeRoll();
                animator.SetBool("Dodging", true);
                // Roll speed is constant at the start, then falls off untill just below move speed
                if (!startRollFallOff)
                {
                    startRollFallOff = true;
                    StartCoroutine(cooldown(() => { rollSpeedDropMultiplier = 4f; }, rollSpeedFallofDelay));
                }
                
                if (rollSpeedDropMultiplier > 0)
                {
                    rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;
                    if (rollSpeed <= 3.5f)
                    {
                        cc.enabled = true;
                        state = State.Normal;
                        animator.SetBool("Dodging", false);
                        startRollFallOff = false;
                        rollSpeedDropMultiplier = 0f;
                    }
                }
                break;
        }
    }

    // Aiming ========================================================

    public void Aim_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context.action.ToString());
        if (context.action.ToString() == "Player/Aim[/Mouse/position]")
        {
            useControler = false;
        }
        else
        {
            useControler = true;
            aimInputVector = context.ReadValue<Vector2>();
        }
    }

    private void FaceMouse() // Aiming with the mouse
    {
        // Cast a ray from the camera to the ground plane where the mouse is.
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, groundLayermask))
        {
            // Get the mouse position relative to the player
            Vector3 mousePosition = raycastHit.point;
            direction = new Vector3(mousePosition.x - transform.position.x, 0, mousePosition.z - transform.position.z);

            // Flip sprite to face the mouse position
            if (faceaim)
            {
                if (direction.x > 0 && flipped)
                {
                    flipped = false;
                    playerSprite.flipX = false;
                }
                else if (direction.x < 0 && !flipped)
                {
                    flipped = true;
                    playerSprite.flipX = true;
                }
            } else
            {
                FaceMove();
            }
        }
    }

    private void FaceController()
    {
        //Vector2 inputVector = playerInputActions.Player.Aim.ReadValue<Vector2>();
        direction = Vector3.right * aimInputVector.x + Vector3.forward * aimInputVector.y;

        // Flip sprite to face the joystick position
        if (direction.x < 0 && !flipped)
        {
            flipped = true;
            playerSprite.flipX = true;
        }
        else if (direction.x > 0 && flipped)
        {
            flipped = false;
            playerSprite.flipX = false;
        }
    }

    private void FaceMove()
    {
        // Flip sprite to face the direction the player is moving
        if (moveInputVector.x < 0 && !flipped)
        {
            flipped = true;
            playerSprite.flipX = true;
        }
        else if (moveInputVector.x > 0 && flipped)
        {
            flipped = false;
            playerSprite.flipX = false;
        }
    }

    // Move =================================
    public void Move_performed(InputAction.CallbackContext context)
    {
        moveInputVector = context.ReadValue<Vector2>();
    }

    private void Move() // Movement code, Uses Vector3, input from Horazontal and Vertical Axis, and the RB to move
    {
        //Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        movement = new Vector3(moveInputVector.x, 0, moveInputVector.y).normalized;
        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);

        //run animation management
        animator.SetFloat("MovementMagnitude", movement.magnitude);

        if (flipped)
        {
            if(moveInputVector.x < 0)
                animator.SetFloat("Direction", 1);
            if (moveInputVector.x > 0)
                animator.SetFloat("Direction", -1);
        } else if (!flipped)
        {
            if (moveInputVector.x < 0)
                animator.SetFloat("Direction", -1);
            if (moveInputVector.x > 0)
                animator.SetFloat("Direction", 1);
        }

    }

    public void Roll_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            if (movement.magnitude == 1 && !rollCD && state == State.Normal) // Only roll if moving, not on CD, and in Normal state
            {
                state = State.Rolling;
                rollSpeed = rollSpeedMax;
                rollCD = true;
                StartCoroutine(cooldown(() => { rollCD = false; }, rollCDTime));
            }
        }
    }

    private void DodgeRoll()
    {
        rb.MovePosition(transform.position + rollDir * rollSpeed * Time.fixedDeltaTime);
    }

    // Attack =================================
    public void Attack_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            if (!attackCD && state == State.Normal)
            {
                if (attackcoolDown > attackCDTimer) // If attack isnt on cooldown
                {
                    attackcoolDown = 0;
                    attackChain = 1;
                    animator.SetBool("Melee", true);
                    DoAttack();
                    //Debug.Log("Attack 1");

                }
                else if (attackcoolDown > attackDuration) // If attack is on cooldown, subsiquent atacks chain
                {
                    if (attackChain == 1)
                    {
                        attackcoolDown = 0;
                        attackChain = 2;
                        animator.SetBool("Melee", true);
                        DoAttack();
                        //Debug.Log("Attack 2");
                    }
                    else if (attackChain == 2)
                    {
                        attackcoolDown = 0;
                        attackChain = 3;
                        animator.SetBool("Melee", true);
                        DoAttack();
                        //Debug.Log("Attack 3");
                    }
                    else if (attackChain == 3)
                    {
                        //Debug.Log("Attack chain over");
                    }
                }
            }
        }
    }

    private void DoAttack()
    {
        // Set face aim period
        faceAimTime = 0;
        faceaim = true;

        if (flipped) // Left attack hit detection
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
            Debug.Log(c.name);
            EnemyControler enemyControler = c.GetComponent<EnemyControler>();

            if (c.gameObject.tag == "Enemy" || c.gameObject.name == "cornnonBullet")
            {
                if (attackChain == 1)
                {
                    enemyControler.takeDamageMelee(2);
                    overAllPlayerDamage += 2;
                }
                else if (attackChain == 2)
                {
                    enemyControler.takeDamageMelee(3);
                    overAllPlayerDamage += 3;
                }
                else if (attackChain == 3)
                {
                    enemyControler.takeDamageMelee(6);
                    overAllPlayerDamage += 6;
                }
            }
        }
    }

    public void Ranged_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            if (!rangeCD && state == State.Normal)
            {
                // Set cooldown true
                rangeCD = true;
                //Play animation
                if(animator != null)
                {
                    animator.SetBool("Ranged", true);
                    StartCoroutine(cooldown(() => { animator.SetBool("Ranged", false); }, 0.4f));
                }
                if (direction != new Vector3(0,0,0))
                {
                    // Create bullet
                    GameObject bul = null;
                    StartCoroutine(cooldown(() => { bul = Instantiate(bullet, transform.position, transform.rotation);
                        bul.GetComponent<Bullet>().movement = direction.normalized; }, 0.15f));
                    // Send bullet in correct direction
                    //Debug.Log(direction);
                    //bul.GetComponent<Bullet>().movement = direction.normalized;
                }
                // Start ranged attack cooldown
                StartCoroutine(cooldown(() => { rangeCD = false; }, rangeCDTime));
                // Set face aim period
                faceAimTime = 0;
                faceaim = true;
            }
            else if (rangeCD && state == State.Normal) // If on cooldown send dubug msg
            {
                //Debug.Log("Range on Cooldown");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if colliding with the farm house, bank items
        if (other.gameObject.tag == "MainObjective")
        {
            GetComponent<PlayerResourceManager>().BankItems();
        }
    }

    // Misc =================================

    IEnumerator cooldown(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }
}
