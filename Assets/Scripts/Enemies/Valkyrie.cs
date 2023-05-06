using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class Valkyrie : EnemyBase
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject vSlash;
    [SerializeField] private GameObject hSlash;
    //[SerializeField] private float distanceFromCenter;
    [SerializeField] private Animator anim;
    private Vector3 startPoint = Vector3.zero;
    [SerializeField] private Vector3 worldBoundariesMin;
    [SerializeField] private Vector3 worldBoundariesMax;
    [SerializeField] private AnimationCurve speedCurve;
    private float startX;
    private float startY;


    private void Start()
    {
        DOTween.Init();
    }
    private void OnEnable()
    {
        GameObject spawnVfx = ValkSpawnVfxPool.Instance.RequestPoolObject();
        spawnVfx.transform.position = transform.position;
        spawnVfx.GetComponent<VisualEffect>().Play();

        
        startX = transform.position.x;
        startY = transform.position.y;
        wanderTimer = 0.4f;
        startPoint = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
        FindNewPosition();

        //debug
        canTakeDamage = true;
        health = baseHealth;
    }

    private void FindNewPosition()
    {
        moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
    }

    // Update is called once per frame
    void Update()
    {
        rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);
        if (inPosition)
        {            
            Relocate();
        }
    }

    protected override void Relocate()
    {
        //base.Relocate();

        if (wanderTimer <= 0f)
        {
            if (transform.position.z >= GameManager.instance.player.transform.position.z)
            {
                if (Mathf.Abs(transform.position.z - GameManager.instance.player.transform.position.z) > Mathf.Abs(transform.position.x - GameManager.instance.player.transform.position.x))
                {
                    whereFrom = sideComingFrom.Forward;
                }
                else
                {
                    if (transform.position.x > GameManager.instance.player.transform.position.x)
                    {
                        whereFrom = sideComingFrom.Right;
                    }
                    else
                    {
                        whereFrom = sideComingFrom.Left;
                    }                    
                }
            }
            else
            {
                if (Mathf.Abs(transform.position.z - GameManager.instance.player.transform.position.z) > Mathf.Abs(transform.position.x - GameManager.instance.player.transform.position.x))
                {
                    whereFrom = sideComingFrom.Behind;
                }
                else
                {
                    if (transform.position.x > GameManager.instance.player.transform.position.x)
                    {
                        whereFrom = sideComingFrom.Right;
                    }
                    else
                    {
                        whereFrom = sideComingFrom.Left;
                    }
                }
            }
            Attack();
            wanderTimer = Random.Range(moveTime + -moveVarRange, moveTime + moveVarRange); //debug for loop behavior
            FindNewPosition();
            rb.DOMove(moveDir, speed).SetEase(speedCurve);
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
        //base.Attack();

        if (Random.Range(-1f, 1f) < 0f)
        {
            //GameObject hShot = Instantiate(hSlash, shootPoint.position, Quaternion.LookRotation(GameManager.instance.player.transform.position - transform.position));
            GameObject hShot = ValhSlashPool.Instance.RequestPoolObject();
            hShot.transform.position = shootPoint.position;
            hShot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            anim.SetTrigger("hSlash");
            /*if (whereFrom == sideComingFrom.Forward)
            {
                hShot.GetComponent<Rigidbody>().AddForce(Vector3.back * projectileSpeed, ForceMode.VelocityChange);
            }*/
            //hShot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - transform.position).normalized * projectileSpeed, ForceMode.VelocityChange);

            switch (whereFrom)
            {
                case sideComingFrom.Forward:
                    hShot.transform.rotation = Quaternion.LookRotation(Vector3.back);
                    hShot.GetComponent<Rigidbody>().AddForce(Vector3.back * projectileSpeed, ForceMode.VelocityChange);
                    break;
                case sideComingFrom.Left:
                    hShot.transform.rotation = Quaternion.LookRotation(Vector3.right);
                    hShot.GetComponent<Rigidbody>().AddForce(Vector3.right * projectileSpeed, ForceMode.VelocityChange);
                    break;
                case sideComingFrom.Behind:
                    hShot.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                    hShot.GetComponent<Rigidbody>().AddForce(Vector3.forward * projectileSpeed, ForceMode.VelocityChange);
                    break;
                case sideComingFrom.Right:
                    hShot.transform.rotation = Quaternion.LookRotation(Vector3.left);
                    hShot.GetComponent<Rigidbody>().AddForce(Vector3.left * projectileSpeed, ForceMode.VelocityChange);
                    break;
                default:
                    hShot.SetActive(false);
                    break;
            }
        }
        else
        {
            
            //also old
            //GameObject vShot = Instantiate(vSlash, shootPoint.position, Quaternion.LookRotation(GameManager.instance.player.transform.position - transform.position));
            
            GameObject vShot = ValvSlashPool.Instance.RequestPoolObject();
            vShot.transform.position = shootPoint.position;
            vShot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            anim.SetTrigger("vSlash");
           
            
            //previously discarded code
            //vShot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - transform.position).normalized * projectileSpeed, ForceMode.VelocityChange);
            
            //old mechanism
            /*if (whereFrom == sideComingFrom.Forward)
            {
                vShot.GetComponent<Rigidbody>().AddForce(Vector3.back * projectileSpeed, ForceMode.VelocityChange);
            }*/

            //new
            switch (whereFrom)
            {
                case sideComingFrom.Forward:
                    vShot.transform.rotation = Quaternion.LookRotation(Vector3.back);
                    vShot.GetComponent<Rigidbody>().AddForce(Vector3.back * projectileSpeed, ForceMode.VelocityChange);
                    break;
                case sideComingFrom.Left:
                    vShot.transform.rotation = Quaternion.LookRotation(Vector3.right);
                    vShot.GetComponent<Rigidbody>().AddForce(Vector3.right * projectileSpeed, ForceMode.VelocityChange);
                    break;
                case sideComingFrom.Behind:
                    vShot.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                    vShot.GetComponent<Rigidbody>().AddForce(Vector3.forward * projectileSpeed, ForceMode.VelocityChange);
                    break;
                case sideComingFrom.Right:
                    vShot.transform.rotation = Quaternion.LookRotation(Vector3.left);
                    vShot.GetComponent<Rigidbody>().AddForce(Vector3.left * projectileSpeed, ForceMode.VelocityChange);
                    break;
                default:
                    vShot.SetActive(false);
                    break;
            }
        }
    }

    private IEnumerator GetInPosition()
    {
        rb.DOMove(startPoint, speed);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            transform.parent = null;
            //GetComponent<MeshRenderer>().enabled = true;
            //GetComponentInChildren<MeshRenderer>().enabled = true;
            model.SetActive(true);
            canTakeDamage = true;
            StartCoroutine(GetInPosition());
        }
        if (other.CompareTag("PlayerShot"))
        {
            TakeDamage(other.GetComponent<PlayerBullet>().GetDamage());
        }
    }

    public override void TakeDamage(float dmg)
    {
        Debug.Log("hit");
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
        gameObject.SetActive(false);
    }
}
