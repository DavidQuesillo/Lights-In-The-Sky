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
    [SerializeField] private List<EnemyBase> enemylist;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = 0f;
        //StopCoroutine(VanishRays());
        StartCoroutine(VanishRays());
        //StopCoroutine(ContinuousFire());
        StartCoroutine(ContinuousFire());
        
        EnemyBase[] enemyArray = FindObjectsByType<EnemyBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        enemylist = new List<EnemyBase>(enemyArray);
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

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
            }
            if (hit.collider.gameObject.CompareTag("Boss"))
            {
                hit.collider.gameObject.GetComponent<BossBase>().TakeDamage(damage);
            }
        }
    }

    private void ConductToNextEnemy()
    {        
        List<Transform> enemies = FindObjectsOfType<EnemyBase>();
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
