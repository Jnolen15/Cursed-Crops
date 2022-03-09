using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    // ================= Public variables =================
    public int health = 10;
    public int maxHealth = 10;
    public float overalldamage = 0;
    public bool takingDamage = false;
    public bool knockbackResist = false;
    public bool stunResist = true;
    public bool stunned = false;
    public string lastDamageType;

    // ================= Private variables =================
    private delegate void Callback();
    private Renderer rend;
    private SpriteRenderer sr;
    private ItemDropper itemDropper;
    private GameObject impact;
    private GameObject damagePS;
    private ParticleSystem ps;
    private Vector3 knocknewPosition;
    private float stunTimer = 1f;
    private bool dying = false;
    // Burning Debuff Stuff
    private Coroutine burningCo;
    private bool burning = false;
    private int burnDamage = 1;
    private float burnTickSpeed = 0.5f;
    private float burnTimer = 0f;
    // Healing Buff Stuff
    private Coroutine healingCo;
    public bool healing = false;
    private int healingAmmount = 1;
    private float healingTickSpeed = 1f;
    private float healingTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        sr = this.transform.GetComponentInChildren<SpriteRenderer>();
        itemDropper = GetComponent<ItemDropper>();

        // Get hit effect stuff
        impact = Instantiate(Resources.Load<GameObject>("Effects/Impact"), transform.position, transform.rotation, transform);
        impact.SetActive(false);
        damagePS = Resources.Load<GameObject>("Effects/EnemyDamageParticle");
        ps = Instantiate(damagePS, transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        ps.Pause();
    }

    private void Update()
    {
        // Burning effect. Does damage every burnTickSpeed seconds
        if (burning)
        {
            if (burnTimer <= 0)
            {
                takeDamage(burnDamage, "Burn");
                burnTimer = burnTickSpeed;
            }
            else burnTimer -= Time.deltaTime;
        }

        // Healing effect. Heals damage every healingTickSpeed seconds
        if (healing)
        {
            if (healingTimer <= 0)
            {
                if(health < maxHealth)
                    health += healingAmmount;
                healingTimer = healingTickSpeed;
            }
            else healingTimer -= Time.deltaTime;
        }
    }

    public void takeDamage(int dmg, string type)
    {
        // Subtract from health
        health -= dmg;
        overalldamage += dmg;
        lastDamageType = type;
        if (ps != null)
            ps.Emit(4);
        // If health is below or equal to 0 die
        if (health <= 0)
        {
            if(gameObject.GetComponent<GrabbageAI>() != null)
            {
                gameObject.GetComponent<GrabbageAI>().boostedHealthActivate = false;
                if (gameObject.GetComponent<GrabbageAI>().trappedPlayer != null)
                {
                    gameObject.GetComponent<GrabbageAI>().trappedPlayer.GetComponent<PlayerControler>().trapped = false;
                }
                gameObject.SetActive(false);
            }
            if (!dying) StartCoroutine(DoDeath());
        }
        else
        {
            // In not dead flash red to show hit
            StartCoroutine(hit(rend));
        }
    }

    IEnumerator hit(Renderer renderer)
    {
        //Debug.Log(lastDamageType);
        if (sr != null)
        {
            takingDamage = true;
            impact.SetActive(true);
            Color prevColor = sr.color;
            sr.color = Color.red;
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            yield return new WaitForSeconds(0.2f);
            takingDamage = false;
            sr.color = prevColor;
            impact.SetActive(false);
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = gameObject.GetComponent<EnemyToPlayer>().originalSpeed;
        }
    }

    public void Knockback(Transform other)
    {
        if (!knockbackResist)
        {
            Vector3 knockattackPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            Vector3 knockenemyPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            this.knocknewPosition = ((knockenemyPosition - knockattackPosition) + (knockenemyPosition - knockattackPosition).normalized) / 2;
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
            this.transform.position = Vector3.Lerp(this.transform.position, knocknewPosition, time / duration);

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
                Debug.Log("Applying Burning");
                burning = true;
                if (burningCo != null)
                    StopCoroutine(burningCo);
                burningCo = StartCoroutine(cooldown(() => { Debug.Log("Burning Over"); burning = false; }, length));
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "HealingAura")
        {


            healing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HealingAura")
        {


            healing = false;
        }
    }


    IEnumerator cooldown(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

}
