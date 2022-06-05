using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPlayerDamage : MonoBehaviour
{
    // ================= Public variables =================
    public int playerHealth = 10;
    public int reviveHealth = 10;
    public int damage = 1;
    public float reviveTime = 10f;
    public float reviveTimer = 0f;
    public bool playerDown = false;
    public bool inIFrames = false;
    public bool playerIsStun = false;
    public bool invulnerable = false;
    public AudioClip damageSound;
    public AudioClip downSound;

    // ================= Private variables =================
    private delegate void Callback();
    private PlayerControler pc;
    private PlayerResourceManager prm;
    private SpriteRenderer playerSprite;
    private ParticleSystem psHeal;
    private GameObject mainObjective;
    private Animator animator;
    private Coroutine damageBuffCo;
    private float timeSinceLastHit;
    // Healing Buff Stuff
    private Coroutine healingCo;
    public bool healing = false;
    private int healingAmmount = 1;
    private float healingTickSpeed = 2f;
    private float healingTimer = 2f;
    // Damage Bugg
    public bool damageBuffed = false;

    // Start is called before the first frame update
    void Start()
    {
        pc = this.GetComponent<PlayerControler>();
        prm = this.GetComponent<PlayerResourceManager>();
        playerSprite = this.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
        animator = this.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>();

        psHeal = Instantiate(Resources.Load<GameObject>("Effects/HealParticle"), transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        psHeal.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        // Passive health Regen Timer
        timeSinceLastHit += Time.deltaTime;
        if (timeSinceLastHit > 10.0f)
        {
            // Healing effect. Heals damage every healingTickSpeed seconds
            if (healingTimer <= 0)
            {
                Heal(healingAmmount);
                healingTimer = healingTickSpeed;
            }
            else healingTimer -= Time.deltaTime;
        }

        // Player Getting Downed
        if (playerHealth <= 0 && !playerDown)
        {
            //alphaChekcer = true;
            prm.setCrops(0);
            playerDown = true;
            StartCoroutine(downed());
            Debug.Log(playerHealth);
            //gameOver();
        }

        // Pulse if below 20% health
        if (playerHealth <= (reviveHealth * 0.2))
        {
            playerSprite.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
        }
        else if (playerSprite.color != Color.white && !inIFrames)
        {
            playerSprite.color = Color.white;
        }

        // function for tracking reviveTime
        if (playerDown)
        {
            reviveTimer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!invulnerable)
        {
            /*
            if (other.gameObject.GetComponent<WindUpAttackMelee>() != null && other.gameObject.GetComponent<GrabbageAI>() == null)
            {
                if (other.gameObject.tag == "Enemy" && !isItHit && !hitAgain && other.gameObject.GetComponent<WindUpAttackMelee>().attacking)
                {
                    StartCoroutine(iframes(damage));
                }
            }
            */
            if (other.gameObject.tag == "attackBox" && !inIFrames)
            {
                StopCoroutine(iframes(damage));
                StartCoroutine(iframes(damage));
            }
        }
    }

    public void Heal(int ammount)
    {
        if (playerHealth < reviveHealth)
        {
            psHeal.Emit(6);
            playerHealth += ammount;
        }
    }

    public void Damage(int ammount)
    {
        if(!inIFrames && !invulnerable)
        {
            StopCoroutine(iframes(ammount));
            StartCoroutine(iframes(ammount));
        }
    }

    public IEnumerator iframes(int damages)
    {
        if (playerHealth > 0)
        {
            inIFrames = true;
            gameObject.GetComponent<AudioPlayer>().PlaySound(damageSound);
            playerHealth -= damages;
            timeSinceLastHit = 0;

            // Flash red and play hurt anim
            playerSprite.color = Color.red;
            animator.SetTrigger("Hurt");
            yield return new WaitForSeconds(0.1f);
            playerSprite.color = Color.white;

            Color alpha = playerSprite.color;
            // Player flash
            for (int i = 0; i < 4; i++)
            {
                alpha.a = 0.4f;
                playerSprite.color = alpha;
                yield return new WaitForSeconds(0.2f);
                alpha.a = 1f;
                playerSprite.color = alpha;
                yield return new WaitForSeconds(0.2f);
            }

            // Finish Iframes
            //yield return new WaitForSeconds(0.9f);
            inIFrames = false;
        }
    }

    private IEnumerator downed()
    {
        playerIsStun = true;
        invulnerable = true;
        gameObject.GetComponent<AudioPlayer>().PlaySound(downSound);
        yield return new WaitForSeconds(reviveTime);
        playerDown = false;
        reviveTimer = 0;
        playerHealth = reviveHealth;
        playerIsStun = false;
        invulnerable = false;
    }

    // Type of effects can be seen in the comment below. Length is an optional variable default to 0
    public void ApplyEffect(string type, float length = 0)
    {
        // Types of effects:
        // Buff: DamageBoost
        // Debuff: Restrained?

        switch (type)
        {
            case "DamageBoost":
                // End buff if its being re-applied
                if (damageBuffCo != null && damageBuffed)
                {
                    StopCoroutine(damageBuffCo);
                    pc.damageBoost -= 0.5f;
                }
                // Apply buff
                damageBuffed = true;
                pc.damageBoost += 0.5f;
                damageBuffCo = StartCoroutine(cooldown(() => { damageBuffed = false; pc.damageBoost -= 0.5f; }, length));
                break;
            case "Healing":
                Debug.Log("Applying Healing");
                healing = true;
                if (healingCo != null)
                    StopCoroutine(healingCo);
                healingCo = StartCoroutine(cooldown(() => { Debug.Log("Healing Over"); healing = false; }, length));
                break;
        }
    }

    private void gameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); ;
    }

    IEnumerator cooldown(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }
}
