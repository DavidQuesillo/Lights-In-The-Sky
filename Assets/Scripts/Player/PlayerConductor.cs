using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConductor : Weapon
{
    
    [SerializeField] LayerMask ignoreMask;
    //[SerializeField] private Animator anim;
    [SerializeField] private List<LineRenderer> lr; //intending to make it have several rays goin thru the targets
    [SerializeField] private Transform shootPoint;
    [SerializeField] private AudioSource aus;    
    [SerializeField] private float lightningEntropy = 0.2f;
    private bool raysVisible = false;
    [SerializeField] private float reconductRange; //the radius of the checking sphere
    [SerializeField] private List<Collider> enemylist;
    [SerializeField] private List<Transform> alreadyHitEnemies; //enemies conducted to are added here, and are removed from the sphere overlap list if they are here so there are no loops

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = 0f;
        //StopCoroutine(VanishRays());
        StartCoroutine(VanishRays());
        //StopCoroutine(ContinuousFire());
        StartCoroutine(ContinuousFire());
        
        //EnemyBase[] enemyArray = FindObjectsByType<EnemyBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        //enemylist = new List<EnemyBase>(enemyArray);
    }
    private void OnEnable()
    {
        if (GetIfCooldownPassedWhileSwapped())
        {
            currentCooldown = 0f;
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < lr.Count; i++)
        {
            lr[i].enabled = false;
        }

        timeOfSwap = Time.time;
        fireHeld = false;
    }

    protected override void Fire()
    {
        base.Fire();
        RaycastHit hit;
        raysVisible = true;
        anim.SetBool("Shooting", true);
        if (!GameManager.instance.paused)
        {
            //lr.enabled = true;
            aus.Play();
        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange, ignoreMask.value))
        {
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                /*for (int i = 0; i < lr.Count; i++)
                {
                    lr[i].SetPosition(lr[i].positionCount + -1, hit.point);
                }*/
                //ray.SetPosition(ray.positionCount + -1, hit.point);

                //GameObject sparks = ShotgunSparksPool.Instance.RequestPoolObject();
                GameObject sparks = hitFxPool.RequestPoolObject();
                sparks.transform.position = hit.point;
                sparks.transform.rotation.SetLookRotation(sparks.transform.position - transform.position);
                //lr[0].SetPosition(0, shootPoint.position);
                foreach (var line in lr)
                {
                    line.SetPosition(0, shootPoint.position);
                }
                /*projectile.SetActive(true);
                projectile.transform.position = hit.point;
                projectile.transform.rotation.SetLookRotation(projectile.transform.position - transform.position);*/
            }
            else
            {
                /*for (int i = 0; i < lr.Count; i++)
                {
                    lr[i].SetPosition(lr[i].positionCount + -1, Camera.main.transform.forward * 100f);
                }*/
                //ray.SetPosition(ray.positionCount + -1, Camera.main.transform.forward * 100f);
                //projectile.SetActive(false);
            }
            alreadyHitEnemies.Clear();
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
                ConductToNextEnemy(hit.collider.transform, 1);
            }
            else if (hit.collider.gameObject.CompareTag("Boss"))
            {
                hit.collider.gameObject.GetComponent<BossBase>().TakeDamage(damage);
            }
            else
            {
                foreach (var line in lr)
                {
                    line.positionCount = 2;
                    line.SetPosition(1, hit.point + new Vector3(Random.Range(-lightningEntropy, lightningEntropy),
                                                                                    Random.Range(-lightningEntropy, lightningEntropy),
                                                                                    Random.Range(-lightningEntropy, lightningEntropy)));
                    line.enabled = true;
                    raysVisible = true;
                }
            }
        }
    }

    private void ConductToNextEnemy(Transform enemyHit, int consecutiveHit)
    {        
        if (!Physics.CheckSphere(enemyHit.position, reconductRange, ignoreMask))
        {
            return;
        }        
        alreadyHitEnemies.Add(enemyHit);
        foreach (var line in lr)
        {
            line.positionCount = consecutiveHit + 1;
            line.SetPosition(consecutiveHit, enemyHit.position + new Vector3(Random.Range(-lightningEntropy, lightningEntropy), 
                                                                            Random.Range(-lightningEntropy, lightningEntropy), 
                                                                            Random.Range(-lightningEntropy, lightningEntropy)));
        }
        //lr[0].positionCount = consecutiveHit + 1;
        //lr[0].SetPosition(consecutiveHit, enemyHit.position);
        //lr[0].SetPosition(consecutiveHit, nextConducted.transform.position);
        float shortestDistance = 9999f;        
        enemylist = new List<Collider>(Physics.OverlapSphere(enemyHit.position, reconductRange, ignoreMask));
        List<Collider> toRemoveList = new List<Collider>();
        foreach (var item in enemylist)
        {
            if (alreadyHitEnemies.Contains(item.transform))
            {
                //enemylist.Remove(item);
                toRemoveList.Add(item);
            }
        }
        foreach (var item in toRemoveList)
        {
            enemylist.Remove(item);
        }
        Collider nextConducted = new Collider();
        Collider changeCheck = nextConducted;
        foreach (var enemy in enemylist)
        {
            if (enemy.GetComponent<EnemyBase>() != null)
            {
                if (Vector3.Distance(enemyHit.position, enemy.transform.position) < shortestDistance)
                {
                    nextConducted = enemy;
                    shortestDistance = Vector3.Distance(enemyHit.position, enemy.transform.position);
                }
            }            
        }
        if (nextConducted == changeCheck)
        {
            foreach (var line in lr)
            {
                line.enabled = true;
            }
            //lr[0].enabled = true;
            raysVisible = true;
            return;
        }
        else
        {
            nextConducted.GetComponent<EnemyBase>().TakeDamage(damage);
            GameObject sparks = hitFxPool.RequestPoolObject();
            sparks.transform.position = nextConducted.ClosestPoint(enemyHit.position);
            sparks.transform.rotation.SetLookRotation(sparks.transform.position - transform.position);
            alreadyHitEnemies.Add(nextConducted.transform);
            ConductToNextEnemy(nextConducted.transform, consecutiveHit + 1);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator VanishRays()
    {
        while (true)
        {
            if (raysVisible)
            {
                yield return new WaitForSeconds(0.05f);
                foreach (var item in lr)
                {
                    item.enabled = false;
                }
                raysVisible = false;
                anim.SetBool("Shooting", false);
            }
            yield return null;
        }
    }
}
