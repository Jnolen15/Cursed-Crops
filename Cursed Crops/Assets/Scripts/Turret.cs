using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // ================= Public variables =================
    public GameObject bullet;
    public bool onCooldown = false;
    public bool sabotaged = false;
    public float cooldown = 2f;
    public int cost = 0;
    public AudioClip manureSound;
    public AudioClip pitchSound;
    public AudioClip rifleSound;
    public AudioClip vineBoom;
    public LayerMask maskToIgnore;

    // ================= Private variables =================
    private Vector3 direction;
    private Vector3 flipDirection;
    private Transform enemyPosition;
    private Transform firePosition;
    private GameObject targetedEnemy;
    private GameObject turretChild;
    private SpriteRenderer turretSprite;
    private TurretAnimator tAnimator;
    private bool flipped = false;
    private int count = 0;
    private List<GameObject> enemies = new List<GameObject>();
    private GameObject vines;



    void Start()
    {
        turretSprite = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        tAnimator = this.GetComponentInChildren<TurretAnimator>();
        firePosition = this.transform.GetChild(2).transform;
        turretChild = this.transform.GetChild(1).gameObject;
        //this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TurretAnimator>();

        // When turret is placed start a basic cooldown.
        // This is so player's dont abuse turrets by destroying and replacing them
        StartCoroutine(StartCooldown());
    }

    void Update()
    {
        if (!sabotaged)
        {
            // Enemy targeting and shooting
            if (targetedEnemy != null && !onCooldown)
            {
                if (!targetedEnemy.activeSelf)
                {
                    //Debug.Log("Enemy inactive, next target");
                    count++;
                }
                else
                {
                    enemyInRange(targetedEnemy.transform);

                    // Raycast to target to see if it can be hit
                    RaycastHit hit;
                    Debug.DrawRay(firePosition.transform.position, direction, Color.red);
                    if (Physics.Raycast(firePosition.transform.position, direction, out hit, Mathf.Infinity, ~maskToIgnore))
                    {
                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                            StartCoroutine(shoot());
                        }
                        else
                        {
                            //Debug.Log("Blocked by: " + hit.collider.gameObject.name);
                            count++;
                        }
                    }
                    else
                    {
                        //Debug.Log("Raycast hit nothing, next target");
                        count++;
                    }
                }
            }
        }

        // List cleanup
        foreach (GameObject enemy in enemies)
        {
            if(!enemy.activeSelf)
                enemies.Remove(enemy);
            break;
        }

        // Find target if one isn't assigned
        if (enemies.Count < count + 1)
        {
            if(enemies.Count > 0)
            {
                count = 0;
                targetedEnemy = enemies[count];
            }
            else
            {
                targetedEnemy = null;
            }
        } else
        {
            targetedEnemy = enemies[count];
        }


        // If the sabotager has been killed
        if (sabotaged && turretChild.GetComponent<TurretSabotager>().theSabotager != null && !turretChild.GetComponent<TurretSabotager>().theSabotager.activeInHierarchy)
        {
            Sabotage();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /*if (other.gameObject.tag == "Enemy" && other.gameObject.name != "cornnonBullet")
        {
            if (targetedEnemy == null)
            {
                targetedEnemy = other.gameObject;
            }
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<SaboAI>() == null)
        {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if(enemies.Find(gameObject => other.gameObject) != null)
                enemies.Remove(other.gameObject);
            /*// Targeted enemy leaves range
            if (targetedEnemy == other.gameObject)
            {
                targetedEnemy = null;
            }*/
        }
    }

    private void enemyInRange(Transform enemy)
    {
        direction = new Vector3(enemy.position.x - firePosition.position.x, 0, enemy.position.z - firePosition.position.z);
        enemyPosition = enemy.transform;
    }

    private void SpriteFlip()
    {
        if(enemyPosition != null)
        {
            flipDirection = new Vector3(enemyPosition.position.x - transform.position.x, 0, enemyPosition.position.z - transform.position.z);
            if(flipDirection.x > 0 && flipped)
            {
                flipped = false;
                turretSprite.flipX = false;
                firePosition.localPosition = new Vector3(firePosition.localPosition.x * -1, firePosition.localPosition.y, firePosition.localPosition.z);

            }
            else if(flipDirection.x <= 0 && !flipped)
            {
                flipped = true;
                turretSprite.flipX = true;
                firePosition.localPosition = new Vector3(firePosition.localPosition.x * -1, firePosition.localPosition.y, firePosition.localPosition.z);
            }
        }
    }

    IEnumerator StartCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    IEnumerator shoot()
    {
        //Debug.Log("IN SHOOT");
        
        onCooldown = true;
        if(bullet != null && bullet.name == "FMCBullet")
        {
            gameObject.GetComponent<AudioPlayer>().PlaySound(manureSound);
        }
        if (bullet != null && bullet.name == "PitchforkBallistaBullet")
        {
            gameObject.GetComponent<AudioPlayer>().PlaySound(pitchSound);
        }
        if (bullet != null && bullet.name == "RifleStandBullet")
        {
            gameObject.GetComponent<AudioPlayer>().PlaySound(rifleSound);
        }

        SpriteFlip();
        tAnimator.playShoot();
        //MakeBullet();
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    public void MakeBullet()
    {
        GameObject bul = Instantiate(bullet, firePosition.position, firePosition.rotation);
        if (bul.GetComponent<Bullet>().isPayload)
        {
            bul.GetComponent<Bullet>().destination = enemyPosition.position;
        }
        // Send bullet in correct direction
        bul.GetComponent<Bullet>().movement = direction.normalized;
    }

    public bool Sabotage()
    {
        // If already sabotaged
        if (sabotaged)
        {
            if (vines != null)
                Destroy(vines);

            sabotaged = false;
            onCooldown = false;
        } 
        // If no longer sabotaged
        else
        {
            sabotaged = true;
            StopCoroutine(shoot());
            onCooldown = true;
            gameObject.GetComponent<AudioPlayer>().PlaySound(vineBoom);
            if (vines == null)
                vines = Instantiate(Resources.Load<GameObject>("Effects/Vines"), transform.position, transform.rotation, transform);
        }

        return sabotaged;
    }
}
