using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : EnemyBase
{
    [SerializeField] private Animator anim;
    [SerializeField] private SkinnedMeshRenderer mr, mr2;

    // Start is called before the first frame update
    void Start()
    {
        if (rb.position.x > 0f)
        {
            moveDir = Vector3.left;
        }
        else
        {
            moveDir = Vector3.right;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inPosition)
        {
            rb.MovePosition(rb.position + moveDir * speed * Time.deltaTime);
            //Debug.Log((rb.position + moveDir * speed * Time.deltaTime).ToString());
            rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);
        }                
    }

    public void ShootProjectile()
    {
        GameObject shot = Instantiate(shotPrefab, shootPoint.position, Quaternion.identity, null);
        shot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - shootPoint.position).normalized * projectileSpeed, ForceMode.VelocityChange);
    }
    protected override void Attack()
    {
        //base.Attack();

        anim.SetTrigger("Attack");
        attacking = true;
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
        if (other.CompareTag("EnemyActivate"))
        {
            //BecomeActive();
            StartCoroutine(ActiveDelay());
            Debug.Log("Activating");
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
            gameObject.SetActive(false);
        }
    }

    private IEnumerator ActiveDelay()
    {
        yield return new WaitForSeconds(moveVarRange);
        BecomeActive();
    }
    private IEnumerator RepeatAttack()
    {
        while (true)
        {
            if (!attacking)
            {
                yield return new WaitForSeconds(moveTime);
                Attack();
                attacking = false;

            }
        }
    }
}
