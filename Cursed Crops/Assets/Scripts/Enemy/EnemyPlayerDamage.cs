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
    private GameObject mainObjective;
    private Animator animator;
    private Coroutine damageBuffCo;

    // Start is called before the first frame update
    void Start()
    {
        pc = this.GetComponent<PlayerControler>();
        prm = this.GetComponent<PlayerResourceManager>();
        playerSprite = this.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
        animator = this.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // This is a temp fix. Player should not be able to take damage when downed
        //if(playerIsStun)
        //    playerHealth = reviveHealth;

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
            playerHealth += ammount;
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
                Debug.Log("Applying DamageBoost");
                pc.damageBoost += 0.5f;
                if (damageBuffCo != null)
                    StopCoroutine(damageBuffCo);
                damageBuffCo = StartCoroutine(cooldown(() => { Debug.Log("DamageBoost Over"); pc.damageBoost -= 0.5f; }, length));
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
