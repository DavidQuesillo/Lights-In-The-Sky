using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernoImp : EnemyBase
{
    [SerializeField] private Animator anim;
    [SerializeField] private SkinnedMeshRenderer mr, mr2;
    [SerializeField] private LayerMask boundaryMask;
    [Header("Firing")]
    [SerializeField] private int barrageAmount = 7;
    [SerializeField] private float barrageSpread = 0.4f;
    private int barrageProg;
    [Header("Flight")]
    [SerializeField] private float sameDirTimer = 1f;
    [SerializeField] private float maxSpeed;
    private float flyTimer;

    // Start is called before the first frame update
    void Start()
    {
        canTakeDamage = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
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

    #region collisions
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
    #endregion

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

    #region coroutines
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
                    if (canAttack)
                    {
                        wanderTimer -= Time.deltaTime;
                    }
                    //wanderTimer -= Time.deltaTime;
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
    #endregion
}
