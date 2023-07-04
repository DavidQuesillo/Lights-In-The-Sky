using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : EnemyBase
{
    [SerializeField] private Light dangerLight;
    [SerializeField] private Material fadingStoneEyes; //this material will cover an unlit material that will gradually be uncovered as the attack happens
    [SerializeField] private bool playerInSight;    
    [SerializeField] private float maxLightIntensity; //the intensity that the light reaches when attack is about to happen
    [SerializeField] private float timeBetweenAtks;
    [SerializeField] private float btwnAtksTimer;

    
    void Start()
    {
        
    }

    private void OnEnable()
    {
        wanderTimer = 0f; //in this script, movetime will be used for the timer towards attacking the player        

        attacking = false;
        GetComponent<Collider>().enabled = true;
        StartCoroutine(DetectPlayerVisibility());
        dangerLight.intensity = 0f;
        dangerLight.gameObject.SetActive(true);
        btwnAtksTimer = 0f;
    }

    protected override void Attack()
    {
        base.Attack();
        print("GARGOYLE atk");
        RaycastHit hit;
        if (Physics.Raycast(rb.position, GameManager.instance.player.transform.position - rb.position, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<Player>().TakeDamage();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DetectPlayerVisibility() 
    {
        RaycastHit hit;
        while (true)
        {
            if (GameManager.instance.currentState != EGameStates.Gameplay || GameManager.instance.paused)
            {
                yield return null;
            }
            if (!attacking && btwnAtksTimer >= timeBetweenAtks) //will only begin attack cooldown if theres sight of player
            {
                if (Physics.Raycast(rb.position, GameManager.instance.player.transform.position - rb.position, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        attacking = true;
                        wanderTimer = 0f; //wander timer will count up to movetime due to light intensity programming
                    }
                }
                print("run cycle of not seeing player");
                yield return new WaitForFixedUpdate();
            }
            else if (btwnAtksTimer <= timeBetweenAtks)
            {
                btwnAtksTimer += Time.deltaTime;
            }
            while (attacking)
            {
                if (GameManager.instance.currentState != EGameStates.Gameplay || GameManager.instance.paused)
                {
                    yield return null;
                }
                print("saw player and counting down");
                wanderTimer += Time.deltaTime;
                if (wanderTimer >= moveTime)
                {
                    dangerLight.intensity = 0f;
                    Attack();
                    btwnAtksTimer = 0f;
                    attacking = false;
                    break;
                }
                else
                {
                    dangerLight.intensity = Mathf.Lerp(0f, maxLightIntensity, wanderTimer / moveTime);
                    print((wanderTimer / moveTime).ToString());
                }
                yield return null;
            }
            yield return null;
        }
    }
}
