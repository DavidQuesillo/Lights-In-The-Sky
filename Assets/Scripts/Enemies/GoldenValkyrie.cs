using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GoldenValkyrie : EnemyBase
{
    [Header("References")]
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject vSlash;
    [SerializeField] private GameObject hSlash;
    //[SerializeField] private float distanceFromCenter;
    [SerializeField] private Animator anim;

    [Header("Wandering")]
    private float flyTimer;
    [SerializeField] private float sameDirTimer = 1f; 
    [SerializeField] private bool canSeePlayer;
    [SerializeField] private LayerMask boundaryMask;

    [Header("Barrage")]
    [SerializeField] private int barrageAmount = 3;
    //[SerializeField] private float barrageSpread = 0.4f;
    [SerializeField] private float timeBetweenSlashs = 0.5f;
    private int barrageProg;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RepeatAttack());
        StartCoroutine(UpdateWanderVector());
        canTakeDamage = true;
    }

    private void OnEnable()
    {
        GameObject spawnVfx = GoldValkSpawnVfxPool.Instance.RequestPoolObject();
        spawnVfx.transform.position = transform.position;
        spawnVfx.GetComponent<VisualEffect>().Play();
        aus.PlayOneShot(spawnSoundFX);

        health = baseHealth;
    }

    private void FixedUpdate()
    {
        rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);

        RaycastHit hit;
        if (Physics.Linecast(transform.position, GameManager.instance.player.transform.position, out hit))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                canSeePlayer = false;
                rb.velocity = moveDir * speed;
                //Relocate();
            }
            else
            {
                ChasePlayer();
                canSeePlayer = true;                
            }
        }        
    }

    private void ChasePlayer()
    {
        Debug.Log("Chasing player");
        rb.velocity = (GameManager.instance.player.transform.position - rb.position) * speed;
    }
    protected override void Relocate()
    {
        //base.Relocate();
        //rb.velocity = 

    }

    protected override void Attack()
    {
        //base.Attack();

        if (Random.Range(-1f, 1f) < 0f)
        {
            //GameObject hShot = Instantiate(hSlash, shootPoint.position, Quaternion.LookRotation(GameManager.instance.player.transform.position - transform.position));
            GameObject hShot = ValhSlashPool.Instance.RequestPoolObject();
            hShot.transform.position = shootPoint.position;
            hShot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            anim.SetTrigger("hSlash");
            
            hShot.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position);
            hShot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - transform.position).normalized * projectileSpeed, ForceMode.VelocityChange);            
        }
        else
        {

            //also old
            //GameObject vShot = Instantiate(vSlash, shootPoint.position, Quaternion.LookRotation(GameManager.instance.player.transform.position - transform.position));

            GameObject vShot = ValvSlashPool.Instance.RequestPoolObject();
            vShot.transform.position = shootPoint.position;
            vShot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            anim.SetTrigger("vSlash");

            vShot.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position);
            vShot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - transform.position).normalized * projectileSpeed, ForceMode.VelocityChange);            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShot"))
        {
            TakeDamage(other.GetComponent<PlayerBullet>().GetDamage());
        }
    }

    public override void TakeDamage(float dmg)
    {
        if (canTakeDamage)
        {
            base.TakeDamage(dmg);
            if (health <= 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator RepeatAttack()
    {
        while (true)
        {
            if (attacking && canSeePlayer)
            {
                barrageProg = barrageAmount;
                while (barrageProg > 0)
                {
                    Attack();
                    barrageProg -= 1;
                    yield return new WaitForSeconds(timeBetweenSlashs);
                }
                //Attack();
                attacking = false;

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

    private IEnumerator UpdateWanderVector()
    {
        while (true)
        {
            while (!canSeePlayer)
            {
                flyTimer = sameDirTimer;
                while (!Physics.Raycast(transform.position, moveDir.normalized, 5f, boundaryMask))
                {
                    flyTimer -= Time.deltaTime;
                    Debug.Log("moving successfully");
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
            
            yield return null;
        }
    }
}
