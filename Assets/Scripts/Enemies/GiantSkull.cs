using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GiantSkull : EnemyBase
{
    [SerializeField] private SkinnedMeshRenderer mr1, mr2, mr3;
    [SerializeField] private Animator animControl;
    private Vector3 startPoint = Vector3.zero;
    [SerializeField] private float zPlacement;
    /*[SerializeField] private float maxX;
    [SerializeField] private float maxY;*/
    [SerializeField] private Vector3 worldBoundariesMin;
    [SerializeField] private Vector3 worldBoundariesMax;

    private void OnEnable()
    {
        DOTween.Init();
        zPlacement = transform.position.z + Random.Range(-1.5f, 1f);
        wanderTimer = 0.4f;
        startPoint = new Vector3(transform.position.x, transform.position.y, zPlacement);
        moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), zPlacement);
        wanderTimer = moveTime;
        canTakeDamage = true;
        health = baseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (inPosition)
        {
            Relocate();

            if (!attacking)
            {
                if (wanderTimer > 0f)
                {
                    wanderTimer -= Time.deltaTime;
                }
                else
                {
                    Attack();
                    wanderTimer = moveTime + Random.Range(-moveVarRange, moveVarRange);
                }
            }
        }
    }

    protected override void Relocate()
    {
        //base.Relocate();

        if (transform.position != moveDir)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveDir, speed * Time.deltaTime);
        }
        else
        {
            switch (whereFrom)
            {
                case sideComingFrom.Forward:
                    moveDir = new Vector3(zPlacement, Random.Range(worldBoundariesMax.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.x, worldBoundariesMax.x));
                    break;
                case sideComingFrom.Left:
                    break;
                case sideComingFrom.Behind:
                    moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMax.y, worldBoundariesMax.y), zPlacement);
                    break;
                case sideComingFrom.Right:
                    moveDir = new Vector3(zPlacement, Random.Range(worldBoundariesMax.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.x, worldBoundariesMax.x));
                    break;
                default:
                    break;
            }
            moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMax.y, worldBoundariesMax.y), zPlacement);
        }
    }

    protected override void Attack()
    {
        //base.Attack();
        animControl.SetTrigger("Attack");
        attacking = true;
        Invoke("InstantiateProjectile", 0.3f);
    }
    public void InstantiateProjectile()
    {
        //GameObject shot = Instantiate(shotPrefab, shootPoint.position, Quaternion.identity, null);
        GameObject shot = GiantSkullShotPool.Instance.RequestPoolObject();
        shot.transform.position = shootPoint.position;
        
        switch (whereFrom)
        {
            case sideComingFrom.Forward:
                shot.GetComponent<Rigidbody>().AddForce(Vector3.back * projectileSpeed, ForceMode.VelocityChange);
                break;
            case sideComingFrom.Left:
                shot.GetComponent<Rigidbody>().AddForce(Vector3.right * projectileSpeed, ForceMode.VelocityChange);
                break;
            case sideComingFrom.Behind:
                shot.GetComponent<Rigidbody>().AddForce(Vector3.forward * projectileSpeed, ForceMode.VelocityChange);
                break;
            case sideComingFrom.Right:
                shot.GetComponent<Rigidbody>().AddForce(Vector3.left * projectileSpeed, ForceMode.VelocityChange);
                break;
            default:
                break;
        }
        attacking = false;
    }

    private IEnumerator GetInPosition()
    {
        rb.DOMove(startPoint, 0.5f);
        while (!inPosition)
        {
            //rb.position = Vector3.MoveTowards(transform.position, startPoint, speed);

            if (transform.position == startPoint)
            {
                inPosition = true;
                yield break;
            }
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
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            canTakeDamage = true;
            mr1.enabled = true;
            mr2.enabled = true;
            mr3.enabled = true;
            transform.parent = null;
            GetComponent<AudioSource>().Play();
            StartCoroutine(GetInPosition());
        }
        if (other.CompareTag("PlayerShot"))
        {
            TakeDamage(other.GetComponent<PlayerBullet>().GetDamage());
        }
    }
}
