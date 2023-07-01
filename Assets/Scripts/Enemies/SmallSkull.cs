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
    [SerializeField] private float maxDistanceMove = 5f;
    [SerializeField] private LayerMask boundaryMask;
    private Tween movingTween;
    private bool moveVectorReady; //intended to use for waiting for an obstructionless vector to be ready

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
        //FindNewPosition();
        MoveToNextPos();
        moveVectorReady = false;
        StartCoroutine(FindNewLocationGuaranteed());

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
        //Relocate();
    }

    public void ShootProjectile()
    {
        //GameObject shot = Instantiate(shotPrefab, shootPoint.position, Quaternion.identity, null);
        GameObject shot = sSkullShotPool.Instance.RequestPoolObject();
        shot.transform.position = shootPoint.position;
        shot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - shootPoint.position).normalized * projectileSpeed, ForceMode.VelocityChange);

    }
    protected override void Attack()
    {
        base.Attack();
        anim.SetTrigger("Shoot");
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

    private void FindNewPosition()
    {
        moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
        /*if (Physics.Raycast(rb.position, Vector3.MoveTowards(rb.position, moveDir, 1f), Vector3.Distance(rb.position, moveDir), boundaryMask))
        {
            moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
            print("move1 dont work");
        }
        if (Physics.Raycast(rb.position, Vector3.MoveTowards(rb.position, moveDir, 1f), Vector3.Distance(rb.position, moveDir), boundaryMask))
        {
            moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
            print("move2 dont work");
        }
        if (Physics.Raycast(rb.position, Vector3.MoveTowards(rb.position, moveDir, 1f), Vector3.Distance(rb.position, moveDir), boundaryMask))
        {
            moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
            print("went for move 3");
        }*/
        if (Vector3.Distance(moveDir, rb.position) > maxDistanceMove)
        {
            print("move too big");
            //moveDir = (rb.position + (moveDir - rb.position).normalized * maxDistanceMove) / 2f;
            moveDir = Vector3.MoveTowards(rb.position, moveDir, maxDistanceMove);
        }
        if (Physics.Raycast(rb.position, Vector3.MoveTowards(rb.position, moveDir, 1f), Vector3.Distance(rb.position, moveDir) + 1f, boundaryMask))
        {
            moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
            print("move1 dont work");
            //FindNewPosition(false);
        }
    }
    public void MoveToNextPos()
    {
        moveVectorReady = false; //this initiates the coroutine
        //FindNewPosition();
        //movingTween = rb.DOMove(moveDir, moveTime).SetEase(speedCurve).OnComplete(() => Attack());
        
        
        
        
        //wanderTimer = Random.Range(moveTime + -moveVarRange, moveTime + moveVarRange); //debug for loop behavior
        //wanderTimer = moveTime;
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
        anim.SetBool("Dead", true);
        GetComponent<Collider>().enabled = false;      
        if (movingTween != null)
        {
            movingTween.Kill();
        }
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

    private IEnumerator FindNewLocationGuaranteed()
    {
        while (true)
        {
            while (!moveVectorReady)
            {
                if (GameManager.instance.paused || GameManager.instance.currentState != EGameStates.Gameplay)
                {
                    yield return null;
                }
                moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
                /*if (Physics.Raycast(rb.position, Vector3.MoveTowards(rb.position, moveDir, 1f), Vector3.Distance(rb.position, moveDir), boundaryMask))
                {
                    moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
                    print("move1 dont work");
                }
                if (Physics.Raycast(rb.position, Vector3.MoveTowards(rb.position, moveDir, 1f), Vector3.Distance(rb.position, moveDir), boundaryMask))
                {
                    moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
                    print("move2 dont work");
                }
                if (Physics.Raycast(rb.position, Vector3.MoveTowards(rb.position, moveDir, 1f), Vector3.Distance(rb.position, moveDir), boundaryMask))
                {
                    moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
                    print("went for move 3");
                }*/
                moveDir = rb.position - moveDir;
                moveDir = moveDir.normalized * maxDistanceMove;
                /*if (Vector3.Distance(moveDir, rb.position) > maxDistanceMove)
                {
                    print("move too big");
                    //moveDir = (rb.position + (moveDir - rb.position).normalized * maxDistanceMove) / 2f;
                    moveDir = Vector3.MoveTowards(rb.position, moveDir, maxDistanceMove);
                }*/
                //if (Physics.Raycast(rb.position, Vector3.MoveTowards(rb.position, moveDir, 1f), Vector3.Distance(rb.position, moveDir) + 1.5f, boundaryMask))
                if (Physics.Raycast(rb.position, moveDir, moveDir.magnitude + 1.5f, boundaryMask))
                {
                    Debug.DrawLine(rb.position, Vector3.MoveTowards(rb.position, moveDir, 1f));
                    print("move hit something. Recalculating");
                    //yield return null;
                    //moveDir = new Vector3(Random.Range(worldBoundariesMin.x, worldBoundariesMax.x), Random.Range(worldBoundariesMin.y, worldBoundariesMax.y), Random.Range(worldBoundariesMin.z, worldBoundariesMax.z));
                    //print("move1 dont work");
                    //FindNewPosition(false);
                    
                    //print("first vector: " + moveDir);
                    Vector3 oppositeway = -moveDir;
                    //print("opposite vector: " + oppositeway);
                    if (Physics.Raycast(rb.position, oppositeway, oppositeway.magnitude + 1.5f, boundaryMask))
                    {
                        print("opposite direction failed. Restarting calculation");
                        Debug.DrawLine(rb.position, Vector3.MoveTowards(rb.position, moveDir, -1f));
                        yield return null;
                    }
                    else
                    {
                        movingTween = rb.DOMove(rb.position + oppositeway, moveTime).SetEase(speedCurve).OnComplete(() => Attack());
                        moveVectorReady = true;
                        print("opposite vector success");
                        Debug.DrawLine(rb.position, Vector3.MoveTowards(rb.position, moveDir, -1f), Color.green);
                        break;
                    }
                }
                else
                {
                    movingTween = rb.DOMove(rb.position + moveDir, moveTime).SetEase(speedCurve).OnComplete(() => Attack());
                    moveVectorReady = true;
                    print("vector success");
                    break;
                }
            }
            yield return null;
        }
    }

    /*private IEnumerator MovementCycle()
    {
        while (true)
        {
            if (inPosition)
            {
                Attack();
            }
        }
    }*/
}