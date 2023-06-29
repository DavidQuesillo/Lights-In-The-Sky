using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
public class SmallSkull : EnemyBase
{
    [SerializeField] private Vector3 worldBoundariesMin;
    [SerializeField] private Vector3 worldBoundariesMax;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
    }

    private void OnEnable()
    {
        //GameObject spawnVfx = ValkSpawnVfxPool.Instance.RequestPoolObject();
        //spawnVfx.transform.position = transform.position;
        //spawnVfx.GetComponent<VisualEffect>().Play();
        aus.PlayOneShot(spawnSoundFX);


        /*startX = transform.position.x;
        startY = transform.position.y;*/
        wanderTimer = 0.4f;
        //startPoint = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
        FindNewPosition();

        //debug
        canTakeDamage = true;
        canAttack = true;
        health = baseHealth;
        anim.SetBool("Dead", false);
        GetComponent<Collider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);
    }

    private void FindNewPosition()
    {
        moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
    }

    public void ShootProjectile()
    {
        //GameObject shot = Instantiate(shotPrefab, shootPoint.position, Quaternion.identity, null);
        GameObject shot = sSkullShotPool.Instance.RequestPoolObject();
        shot.transform.position = shootPoint.position;
        shot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - shootPoint.position).normalized * projectileSpeed, ForceMode.VelocityChange);

    }

    protected override void Relocate()
    {
        //base.Relocate();

        if (wanderTimer <= 0f)
        {           
            Attack();
            //wanderTimer = Random.Range(moveTime + -moveVarRange, moveTime + moveVarRange); //debug for loop behavior
            //FindNewPosition();
            //rb.DOMove(moveDir, speed).SetEase(speedCurve);
        }
        else
        {
            //rb.position = Vector3.MoveTowards(transform.position, moveDir, speed);

            //transform.LookAt(new Vector3(moveDir.x + transform.position.x, transform.position.y, moveDir.z + transform.position.z), Vector3.up);
            wanderTimer -= Time.deltaTime;
        }
    }

    protected override void Attack()
    {
        base.Attack();
        anim.SetTrigger("Shoot");
    }

    public void MoveToNextPos()
    {
        FindNewPosition();
        rb.DOMove(moveDir, speed).SetEase(speedCurve);
        //wanderTimer = Random.Range(moveTime + -moveVarRange, moveTime + moveVarRange); //debug for loop behavior
        wanderTimer = moveTime;
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        if (health <= 0f)
        {
            //gameObject.SetActive(false);
            Death();
        }
    }
    protected override void Death()
    {
        base.Death();
        anim.SetBool("Death", true);
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
