using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Cyclops : EnemyBase
{
    [SerializeField] private SkinnedMeshRenderer mr;
    [SerializeField] private Animator anim;
    private bool playerInRange;

    private void OnEnable()
    {
        //wanderTimer = 0f;
        wanderTimer = Random.Range(moveTime + -moveVarRange, moveTime + moveVarRange);
        GetFromForwardVector();

        GameObject spawnVfx = CyclopsSpawnVfxPool.Instance.RequestPoolObject();
        spawnVfx.transform.position = transform.position;
        spawnVfx.GetComponent<VisualEffect>().Play();
        aus.PlayOneShot(spawnSoundFX);

        health = baseHealth;
        rb.velocity = Vector3.zero;
        anim.SetBool("Dead", false);
        playerInRange = false;
        canTakeDamage = true;
        canAttack = true;
        GetComponent<Collider>().enabled = true;
    }

    private void GetFromForwardVector()
    {
        moveDir = new Vector3(Random.Range(-0.5f, 0.5f) * speed, rb.velocity.y, Random.Range(-1f, 0f) * speed).normalized;
    }
    private void GetFromLeftVector()
    {
        moveDir = new Vector3(Random.Range(0f, 1f) * speed, rb.velocity.y, Random.Range(0.5f, 0.5f) * speed).normalized;
    }
    private void GetFromRightVector()
    {
        moveDir = new Vector3(Random.Range(-1f, 0f) * speed, rb.velocity.y, Random.Range(0.5f, 0.5f) * speed).normalized;
    }
    private void GetFromBehindVector()
    {
        moveDir = new Vector3(Random.Range(-0.5f, 0.5f) * speed, rb.velocity.y, Random.Range(0f, 1f) * speed).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (inPosition)
        {
            GroundCheck();
        }
    }

    private void FixedUpdate()
    {
        if (inPosition)
        {
            Relocate();
        }
        rb.AddForce(Vector3.down * 10, ForceMode.Force);
        //Debug.Log(moveDir.ToString());
        //Debug.Log(wanderTimer);
    }
    protected override void Relocate()
    {
        //base.Relocate();

        if (wanderTimer <= 0f)
        {
            Attack();
        
        /*switch (whereFrom)
        {
            case sideComingFrom.Forward:
                GetFromForwardVector();
                break;
            case sideComingFrom.Left:
                GetFromLeftVector();
                break;
            case sideComingFrom.Right:
                GetFromRightVector();
                break;
            case sideComingFrom.Behind:
                GetFromBehindVector();
                break;
            
            default:
                GetFromForwardVector();
                break;
        }*/
            //moveDir = new Vector3(Random.Range(-0.5f, 0.5f) * speed, rb.velocity.y, Random.Range(-1f, 0f) * speed).normalized;
            moveDir = new Vector3(GameManager.instance.player.transform.position.x - transform.position.x, rb.velocity.y, GameManager.instance.player.transform.position.z - transform.position.z).normalized;

            moveDir *= speed * Time.fixedDeltaTime;
            moveDir.y = rb.velocity.y;
        }
        else
        {
            //rb.AddForce(moveDir, ForceMode.VelocityChange);
            if (playerInRange || canAttack)
            {
                Melee();
            }   
            if (attacking)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                rb.velocity = moveDir;
                transform.LookAt(new Vector3(moveDir.x + transform.position.x, transform.position.y, moveDir.z + transform.position.z), Vector3.up);
                wanderTimer -= Time.deltaTime;
            }                
        }
    }

    protected override void Attack()
    {
        //base.Attack();

        /*if (Random.Range(-1f, 5f) < 0f)
        {
            //GameObject shot = Instantiate(shotPrefab, shootPoint.position, shootPoint.rotation);
            GameObject shot = CyclopsShotPool.Instance.RequestPoolObject();
            shot.transform.position = shootPoint.position;
            Rigidbody shotRb = shot.GetComponent<Rigidbody>();
            shotRb.velocity = Vector3.zero;
            shotRb.AddForce((GameManager.instance.player.transform.position - shootPoint.position).normalized * projectileSpeed, ForceMode.VelocityChange);
        }*/

        //GameObject shot = Instantiate(shotPrefab, shootPoint.position, shootPoint.rotation);
        if (!canAttack)
        {
            wanderTimer = Random.Range(moveTime + -moveVarRange, moveTime + moveVarRange);
            return;
        }
        GameObject shot = CyclopsShotPool.Instance.RequestPoolObject();
        shot.transform.position = shootPoint.position;
        Rigidbody shotRb = shot.GetComponent<Rigidbody>();
        shotRb.velocity = Vector3.zero;
        shotRb.AddForce((GameManager.instance.player.transform.position - shootPoint.position).normalized * projectileSpeed, ForceMode.VelocityChange);

        wanderTimer = Random.Range(moveTime + -moveVarRange, moveTime + moveVarRange);

    }

    public void PlayerInMeleeRange()
    {
        playerInRange = true;
    }
    private void Melee()
    {
        attacking = true;
        anim.SetTrigger("Melee");
        playerInRange = false;
        wanderTimer = Random.Range(moveTime + -moveVarRange, moveTime + moveVarRange);
    }
    public void FinishAttackState()
    {
        attacking = false;
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
        //gameObject.SetActive(false);
        anim.SetBool("Dead", true);        
        GetComponent<Collider>().enabled = false;
    }

    private void GroundCheck()
    {
        if(!Physics.Raycast(shootPoint.position, Vector3.down, 5f))
        {
            moveDir = new Vector3(Random.Range(-1f, 1f), rb.velocity.y, Random.Range(-1f, 1f)).normalized;
            moveDir *= speed * Time.fixedDeltaTime;
            moveDir.y = rb.velocity.y;
            Debug.Log("turning around");
        }
        //Debug.DrawRay(transform.position, transform.forward + -transform.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            canTakeDamage = true;
            transform.parent = null;
            mr.enabled = true;
            inPosition = true;
        }
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
            Debug.Log("Exited the zone");
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Shredder"))
        {
            canAttack = true;
        }
    }
}
