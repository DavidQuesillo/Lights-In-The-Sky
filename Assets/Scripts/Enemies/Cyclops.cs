using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : EnemyBase
{
    [SerializeField] private SkinnedMeshRenderer mr;
    private void OnEnable()
    {
        wanderTimer = 0f;
        GetFromForwardVector();
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
            
            switch (whereFrom)
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
            }

                moveDir *= speed * Time.fixedDeltaTime;
                moveDir.y = rb.velocity.y;
            }
            else
            {
                //rb.AddForce(moveDir, ForceMode.VelocityChange);
                rb.velocity = moveDir;
                transform.LookAt(new Vector3(moveDir.x + transform.position.x, transform.position.y, moveDir.z + transform.position.z) , Vector3.up);
                wanderTimer -= Time.deltaTime;
            }
    }

    protected override void Attack()
    {
        //base.Attack();

        if (Random.Range(-1f, 5f) < 0f)
        {
            GameObject shot = Instantiate(shotPrefab, shootPoint.position, shootPoint.rotation);
            shot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - shootPoint.position).normalized * projectileSpeed, ForceMode.VelocityChange);
        }
        wanderTimer = Random.Range(moveTime + -moveVarRange, moveTime + moveVarRange);

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

    private void GroundCheck()
    {
        if(!Physics.Raycast(shootPoint.position, Vector3.down, 3f))
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
        if (other.CompareTag("PlayerShot"))
        {
            TakeDamage(other.GetComponent<PlayerBullet>().GetDamage());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            Debug.Log("Exited the zone");
            gameObject.SetActive(false);
        }
    }
}
