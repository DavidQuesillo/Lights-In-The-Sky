using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Imp : EnemyBase
{
    [SerializeField] private Animator anim;
    [SerializeField] private SkinnedMeshRenderer mr, mr2;
    [SerializeField] private LayerMask boundaryMask;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float sameDirTimer = 1f;
    [SerializeField] private int barrageAmount = 7;
    [SerializeField] private float barrageSpread = 0.4f;
    private int barrageProg;
    private float flyTimer;

    // Start is called before the first frame update
    void Start()
    {
        //health = baseHealth;
        /*StartCoroutine(UpdateFlyVector());
        canTakeDamage = true;
        StartCoroutine(RepeatAttack());*/        
        canTakeDamage = true;
    }

    private void OnEnable()
    {
        GameObject spawnVfx = ImpSpawnVfxPool.Instance.RequestPoolObject();
        spawnVfx.transform.position = transform.position;
        spawnVfx.GetComponent<VisualEffect>().Play();
        aus.PlayOneShot(spawnSoundFX);

        if (rb.position.x > 0f)
        {
            moveDir = Vector3.left;
        }
        else
        {
            moveDir = Vector3.right;
        }
        health = baseHealth;
        rb.velocity = Vector3.zero;
        anim.SetBool("Dead", false);
        GetComponent<Collider>().enabled = true;
        canTakeDamage = true;
        StartCoroutine(UpdateFlyVector());
        //canTakeDamage = true;
        StartCoroutine(RepeatAttack());
    }
    // Update is called once per frame
    /*void Update()
    {
        if (inPosition)
        {
            //rb.MovePosition(rb.position + moveDir * speed * Time.deltaTime);
            rb.AddForce(moveDir * speed * Time.deltaTime, ForceMode.Force);
            //Debug.Log((rb.position + moveDir * speed * Time.deltaTime).ToString());
            rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);
        }                
    }*/

    private void FixedUpdate()
    {
        //rb.MovePosition(rb.position + moveDir * speed * Time.deltaTime);
        /*if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(moveDir * speed * Time.deltaTime, ForceMode.Acceleration);
        }*/
        rb.AddForce(moveDir * speed * Time.deltaTime, ForceMode.Acceleration);
        //Debug.Log((moveDir * speed * Time.deltaTime).ToString());
        //Debug.Log(moveDir);
        rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);
    }

    public void ShootProjectile()
    {
        //GameObject shot = Instantiate(shotPrefab, shootPoint.position, Quaternion.identity, null);
        GameObject shot = FireballPool.Instance.RequestPoolObject();
        shot.transform.position = shootPoint.position;
        shot.GetComponent<Rigidbody>().AddForce((new Vector3(Random.Range(-barrageSpread, barrageSpread), Random.Range(-barrageSpread, barrageSpread), 0f) 
                                                    + GameManager.instance.player.transform.position - shootPoint.position).normalized
                                                        * projectileSpeed, ForceMode.VelocityChange);
    }
    protected override void Attack()
    {
        //base.Attack();
        
        
        //anim.ResetTrigger("Attack");
        //anim.SetTrigger("Attack");
        
        
        attacking = true;
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
    /*public void TurnInactive()
    {
        gameObject.SetActive(false);
    }*/

    private void BecomeActive()
    {
        Debug.Log("IMP GO");
        transform.parent = null;
        inPosition = true;
        mr.enabled = true;
        mr2.enabled = true;
        canTakeDamage = true;
        StartCoroutine(RepeatAttack());
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
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            gameObject.SetActive(false);
        }
    }

    /*private IEnumerator ActiveDelay()
    {
        yield return new WaitForSeconds(moveVarRange);
        BecomeActive();
    }*/
    private IEnumerator RepeatAttack()
    {
        while (true)
        {
            if (attacking)
            {
                anim.SetBool("Attacking", true);
                barrageProg = barrageAmount;
                while (barrageProg > 0)
                {
                    ShootProjectile();
                    barrageProg -= 1;
                    yield return new WaitForSeconds(0.05f);
                }
                //Attack();
                attacking = false;
                anim.SetBool("Attacking", false);
            }
            else
            {
                wanderTimer = moveTime;
                while (wanderTimer > 0f)
                {
                    wanderTimer -= Time.deltaTime;
                    yield return null;
                }
                attacking = true;
            }
            yield return null;
        }
    }
    private IEnumerator UpdateFlyVector()
    {
        while (true)
        {
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
}
