using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    // ================= Public variables =================
    public float health = 10;
    public float maxHealth = 10;
    public float overalldamage = 0;
    public bool isBasic = false;
    public bool takingDamage = false;
    public bool knockbackResist = false;
    public bool stunResist = true;
    public bool stunned = false;
    public string lastDamageType;
    public GameObject theHealingAura;
    // Audio Variables
    
    public AudioClip hurtSound;
    public AudioClip spawningSound;
    public AudioClip shieldHit;


    // ================= Private variables =================
    private GameRuleManager grm;
    private AudioPlayer daAudio;
    private GameObject DaShield;
    private delegate void Callback();
    private Renderer rend;
    private SpriteRenderer sr;
    private ItemDropper itemDropper;
    private GameObject impact;
    private ParticleSystem ps;
    private ParticleSystem psBurn;
    private ParticleSystem psHeal;
    private Vector3 knocknewPosition;
    private float stunTimer = 1f;
    private bool dying = false;
    private bool hitFence = false;
    // Burning Debuff Stuff
    private Coroutine burningCo;
    private bool burning = false;
    private int burnDamage = 1;
    private float burnTickSpeed = 0.5f;
    private float burnTimer = 0f;
    // Healing Buff Stuff
    private Coroutine healingCo;
    public bool healing = false;
    public bool invincible = false;
    private int healingAmmount = 1;
    private float healingTickSpeed = 1f;
    private float healingTimer = 1f;
    private bool instantiateOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        sr = this.transform.GetComponentInChildren<SpriteRenderer>();
        itemDropper = GetComponent<ItemDropper>();

        // Health increase by difficulty
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();

        // Health scaling with difficulty
        if (!isBasic)
        {
            float hpBuff = (5 * grm.difficulty);
            //Debug.Log("5 * " + grm.difficulty + " = " + hpBuff);
            hpBuff = (hpBuff / 100);
            //Debug.Log("prev/100 = " + hpBuff);
            hpBuff = (maxHealth * hpBuff);
            //Debug.Log(maxHealth + "*prev = " + hpBuff);
            maxHealth += hpBuff;
            health += hpBuff;
            //Debug.Log("Difficulty: " + grm.difficulty + " Health buff: " + hpBuff + " Max Health: " + maxHealth);
        }

        //Audio set up
        daAudio = gameObject.GetComponent<AudioPlayer>();
        if (gameObject.tag != "enemyBullet")
        {
            daAudio.PlaySound(spawningSound);
        }

        // Get hit effect stuff
        impact = Instantiate(Resources.Load<GameObject>("Effects/Impact"), transform.position, transform.rotation, transform);
        impact.SetActive(false);

        // particle effects
        //damagePS = Resources.Load<GameObject>("Effects/EnemyDamageParticle");
        ps = Instantiate(Resources.Load<GameObject>("Effects/EnemyDamageParticle"), transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        psBurn = Instantiate(Resources.Load<GameObject>("Effects/BurnParticle"), transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        psHeal = Instantiate(Resources.Load<GameObject>("Effects/HealParticle"), transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        ps.Pause();
        psBurn.Stop();
        psHeal.Pause();
    }

    private void Update()
    {
        // Burning effect. Does damage every burnTickSpeed seconds
        if (burning)
        {
            psBurn.Play();
            if (burnTimer <= 0)
            {
                takeDamage(burnDamage, "Burn");
                burnTimer = burnTickSpeed;
            }
            else burnTimer -= Time.deltaTime;
        } else
        {
            if(psBurn.isPlaying) psBurn.Stop();
        }
        if(theHealingAura != null && !theHealingAura.activeInHierarchy)
        {
            invincible = false;
            healing = false;
        }
        if (DaShield != null && !invincible)
        {
            instantiateOnce = false;
            Destroy(DaShield);
        }
        if (invincible && !instantiateOnce)
        {

            instantiateOnce = true;
            Vector3 shieldPlacement = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z + 2);
            DaShield = Instantiate(Resources.Load<GameObject>("Effects/Shield"), shieldPlacement, transform.rotation, transform);
            
        }
        // Healing effect. Heals damage every healingTickSpeed seconds
        if (healing)
        {
            if (healingTimer <= 0)
            {
                psHeal.Emit(6);
                if (health < maxHealth)
                    health += healingAmmount;
                healingTimer = healingTickSpeed;
            }
            else healingTimer -= Time.deltaTime;
        }
    }

    public void takeDamage(float dmg, string type)
    {
        // Subtract from health
        if (!invincible)
        {
            
            health -= dmg;
            overalldamage += dmg;
            lastDamageType = type;
            daAudio.PlaySound(hurtSound);
            if (ps != null)
                ps.Emit(4);
            // If health is below or equal to 0 die
            if (health <= 0)
            {
                if (gameObject.GetComponent<GrabbageAI>() != null)
                {
                    gameObject.GetComponent<GrabbageAI>().boostedHealthActivate = false;
                    
                    if (gameObject.GetComponent<GrabbageAI>().trappedPlayer != null)
                    {
                        gameObject.GetComponent<GrabbageAI>().trappedPlayer.GetComponent<PlayerControler>().trapped = false;
                    }
                    gameObject.SetActive(false);
                }
                if(gameObject.GetComponent<GoToEnemy>() != null)
                {
                    invincible = false;
                }
                invincible = false;
                if (!dying && gameObject.activeInHierarchy) StartCoroutine(DoDeath());
            }
            else
            {
                // In not dead flash red to show hit
                StartCoroutine(hit(rend));
            }
        }
        else
        {
            gameObject.GetComponent<AudioPlayer>().PlaySound(shieldHit);
        }
        
        
    }

    IEnumerator hit(Renderer renderer)
    {
        //Debug.Log(lastDamageType);
        if (sr != null)
        {
            takingDamage = true;
            impact.SetActive(true);
            //Color prevColor = sr.color;
            sr.color = Color.red;
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            yield return new WaitForSeconds(0.2f);
            takingDamage = false;
            sr.color = Color.white;
            impact.SetActive(false);
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = gameObject.GetComponent<EnemyToPlayer>().originalSpeed;
        }
    }

    public void Knockback(Transform other, float knockbackReduction)
    {
        if (!knockbackResist)
        {
            Vector3 knockattackPosition = new Vector3(other.transform.position.x, this.transform.position.y, other.transform.position.z);
            Vector3 knockenemyPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            this.knocknewPosition = ((knockenemyPosition - knockattackPosition) + (knockenemyPosition - knockattackPosition).normalized) / knockbackReduction;
            this.knocknewPosition += knockenemyPosition;
            StartCoroutine(DoKnockback());
        }
    }

    IEnumerator DoKnockback()
    {
        float time = 0;
        float duration = 0.1f;
        while (time < duration)
        {
            if (!hitFence)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, knocknewPosition, time / duration);
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void Stun()
    {
        if (!stunResist)
        {
            StopCoroutine(DoStun(stunTimer));
            StartCoroutine(DoStun(stunTimer));
        }
    }
    
    IEnumerator DoStun(float duration)
    {
        stunned = true;
        //Debug.Log("getting stun");
        //sr.color = Color.blue;
        yield return new WaitForSeconds(duration);
        //sr.color = Color.white;
        //takingDamage = false;
        stunned = false;
    }

    public void death()
    {
        if(!dying) StartCoroutine(DoDeath());
    }

    IEnumerator DoDeath()
    {
        stunned = true;
        dying = true;
        ps.Emit(4);
        impact.SetActive(true);
        sr.color = Color.red;
        if (itemDropper != null)
            itemDropper.DropItem(transform.position);
        //StopAllCoroutines();
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    // Type of effects can be seen in the comment below. Length is an optional variable default to 0
    public void ApplyEffect(string type, float length = 0)
    {
        // Types of effects:
        // Buff: Healing
        // Debuff: Burning

        switch (type)
        {
            case "Burning":
                //Debug.Log("Applying Burning");
                burning = true;
                if (burningCo != null)
                    StopCoroutine(burningCo);
                burningCo = StartCoroutine(cooldown(() => { burning = false; }, length));
                break;
            case "Healing":
                //Debug.Log("Applying Healing");
                healing = true;
                if (healingCo != null)
                    StopCoroutine(healingCo);
                healingCo = StartCoroutine(cooldown(() => { healing = false; }, length));
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "HealingAura")
        {
            theHealingAura = other.gameObject;
            if (gameObject.GetComponent<GoToEnemy>() == null)
            {
                invincible = true;
            }
            healing = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Border")
        {
            hitFence = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HealingAura")
        {
            //if (gameObject.GetComponent<GoToEnemy>() == null)
            //{
                invincible = false;
            //}
            healing = false;
        }
        if (other.gameObject.tag == "Border")
        {
            hitFence = false;
        }
    }


    IEnumerator cooldown(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

}
