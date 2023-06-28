using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampBeast : EnemyBase
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask boundaryMask;
    [SerializeField] private Collider diveCol; //the collider that takes over during diving
    [Header("Flight")]
    [SerializeField] private float sameDirTimer = 1f;
    [SerializeField] private float maxSpeed;
    private float flyTimer;
    private Vector3 diveDir; //the stored direction to dive. recorded when pre-dive initiates
    [SerializeField] private float diveSpeed;
    //private bool hitSomething; //the var the changes when the lunge hits something and stops
    private bool lunging = false; //the var that keeps track of whether the bat is freeflying or diving at player
    
   
    void Start()
    {
        health = baseHealth;
        canTakeDamage = true;
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        anim.SetBool("Dead", false);
        anim.SetBool("Lunging", false);
        GetComponent<Collider>().enabled = true;
        canTakeDamage = true;
        canAttack = true;
        StartCoroutine(UpdateFlyVector());
        StartCoroutine(RepeatAttack());
    }

    private void FixedUpdate()
    {
        if (!lunging && !attacking)
        {
            rb.AddForce(moveDir * speed * Time.deltaTime, ForceMode.Acceleration);
            //Debug.Log((moveDir * speed * Time.deltaTime).ToString());
            //Debug.Log(moveDir);
            rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);
        }
        else if (!lunging && attacking)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else if (lunging)
        {
            rb.velocity = diveDir * diveSpeed;
            if (Physics.Raycast(rb.position, diveDir, 6f, boundaryMask))
            {
                lunging = false;
                anim.SetBool("Lunging", false);
                GetComponent<Collider>().enabled = true;
                diveCol.enabled = false;
            }
        }
    }

    public void BeginLunge() //function to be called by the end of the preLunge animation
    {
        lunging = true;
        anim.SetBool("Lunging", true);
    }

    private IEnumerator RepeatAttack()
    {
        while (true)
        {
            if (attacking)
            {
                //anim.SetBool("Lunging", true);
                anim.SetTrigger("Attack");
                GetComponent<Collider>().enabled = false;
                diveCol.enabled = true;
                //barrageProg = barrageAmount;
                yield return new WaitUntil(() => lunging);
                while (lunging)
                {
                    
                    yield return new WaitForFixedUpdate();
                }
                //Attack();
                attacking = false;
                anim.SetBool("Lunging", false);
            }
            else
            {
                wanderTimer = moveTime;
                while (wanderTimer > 0f)
                {
                    if (canAttack)
                    {
                        wanderTimer -= Time.deltaTime;
                    }
                    //wanderTimer -= Time.deltaTime;
                    yield return null;
                }
                //diveDir = (transform.position - GameManager.instance.player.transform.position).normalized;
                diveDir = (GameManager.instance.player.transform.position - transform.position).normalized;
                attacking = true;
            }
            yield return null;
        }
    }    

    private IEnumerator UpdateFlyVector()
    {
        while (true)
        {
            /*while (GameManager.instance.currentState != EGameStates.Gameplay || GameManager.instance.paused)
            {
                yield return new WaitForEndOfFrame();
            }*/
            flyTimer = sameDirTimer;
            while (!Physics.Raycast(transform.position, moveDir.normalized, 5f, boundaryMask))
            {
                flyTimer -= Time.deltaTime;
                //Debug.Log("moving successfully");
                if (flyTimer <= 0f)
                {
                    break;
                }
                yield return null;
            }
            if (flyTimer > 0)
            {
                rb.velocity = Vector3.zero;
            }
            moveDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            yield return null;
        }
    }


    public override void TakeDamage(float dmg)
    {
        if (canTakeDamage)
        {
            base.TakeDamage(dmg);
            if (health <= 0f)
            {
                //gameObject.SetActive(false);
                Death();
            }
        }
    }
    protected override void Death()
    {
        base.Death();
        anim.SetBool("Dead", true);
        GetComponent<Collider>().enabled = false;
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("EnemyActivate"))
        {
            //BecomeActive();
            StartCoroutine(ActiveDelay());
            Debug.Log("Activating");
        }*/
        if (other.CompareTag("PlayerShot") || other.CompareTag("Explosion"))
        {
            TakeDamage(other.GetComponent<PlayerBullet>().GetDamage());
        }
        if (other.CompareTag("Shredder"))
        {
            //LockAttack();
            canAttack = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Shredder"))
        {
            //LockAttack();
            canAttack = true;
        }
    }
}
