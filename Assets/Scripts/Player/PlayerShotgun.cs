using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.VFX;

public class PlayerShotgun : Weapon
{

    [SerializeField] LayerMask ignoreMask;
    //[SerializeField] private Animator anim;
    [SerializeField] private List<LineRenderer> lr;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private AudioSource aus;
    [SerializeField] private int shotAmount = 7; //this isnt applied yet but it may be better to do so, or scrap the var
    [SerializeField] private float spread = 0.2f;
    [SerializeField] private float lightningEntropy = 0.2f;
    private bool raysVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = 0f;
        //StopCoroutine(VanishRays());
        StartCoroutine(VanishRays());
        //StopCoroutine(ContinuousFire());
        StartCoroutine(ContinuousFire());
    }

    private void OnEnable()
    {
        
        if (GetIfCooldownPassedWhileSwapped())
        {
            currentCooldown = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (canAttack)
        {
            //Fire();
            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;
                //Debug.Log("cooling down");
            }
            print("shotgun cd: " + currentCooldown.ToString());
        }*/
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

    private void FireShotgun()
    {
        RaycastHit hit;
        raysVisible = true;
        anim.SetBool("Shooting", true);
        if (!GameManager.instance.paused)
        {
            //lr.enabled = true;
            aus.Play();
        }
        foreach (var ray in lr) //goes thru each ray that gets shot by using the amount of lineRenderers in the list
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward +
                 /*the spread vector*/ new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread)),
                                                                    out hit, shotRange, ignoreMask.value))
            {
                //Debug.DrawLine(transform.position, hit.point, debugColor, 1f);
                /*for (int i = 0; i < lr.Count; i++)
                {
                    lr[i].SetPosition(0, shootPoint.position);
                }*/
                ray.SetPosition(0, shootPoint.position);

                if (hit.collider != null && !hit.collider.isTrigger)
                {
                    /*for (int i = 0; i < lr.Count; i++)
                    {
                        lr[i].SetPosition(lr[i].positionCount + -1, hit.point);
                    }*/
                    ray.SetPosition(ray.positionCount + -1, hit.point);

                    GameObject sparks = ShotgunSparksPool.Instance.RequestPoolObject();
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
                    ray.SetPosition(ray.positionCount + -1, Camera.main.transform.forward * 100f);
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
            else
            {
                /*for (int i = 0; i < lr.Count; i++)
                {
                    lr[i].SetPosition(lr[i].positionCount + -1, Camera.main.transform.forward * 100f);
                }*/
                //ray.SetPosition(ray.positionCount + -1, Camera.main.transform.forward * 100f);

                ray.SetPosition(ray.positionCount + -1, transform.position + Camera.main.transform.forward.normalized * 40f);
                //projectile.SetActive(false);
            }
            for (int i = 1; i < ray.positionCount + -1; i++)
            {
                ray.SetPosition(i, Vector3.Lerp(shootPoint.position, ray.GetPosition(ray.positionCount + -1), 0.15f * i));
                Vector3 spotOfChange = ray.GetPosition(i);
                ray.SetPosition(i, spotOfChange + new Vector3(Random.Range(-lightningEntropy, lightningEntropy), Random.Range(-lightningEntropy, lightningEntropy), Random.Range(-lightningEntropy, lightningEntropy)));
            }
            ray.enabled = true;
            currentCooldown = fireRate;
            //Debug.Log("Click Shot");
        }
    }

    protected override void Fire()
    {
        #region old code
        /*if (GameManager.instance.currentState != EGameStates.Gameplay || !canAttack || GameManager.instance.paused)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange, ignoreMask.value))
            {
                //Debug.DrawLine(transform.position, hit.point, debugColor, 1f);
                for (int i = 0; i < lr.Count; i++)
                {
                    lr[i].SetPosition(0, shootPoint.position);
                }
                
                if (hit.collider != null && !hit.collider.isTrigger)
                {
                    for (int i = 0; i < lr.Count; i++)
                    {
                        lr[i].SetPosition(lr[i].positionCount + -1, hit.point);
                    }
                    
                    projectile.SetActive(true);
                    projectile.transform.position = hit.point;
                    projectile.transform.rotation.SetLookRotation(projectile.transform.position - transform.position);
                }
                else
                {
                    for (int i = 0; i < lr.Count; i++)
                    {
                        lr[i].SetPosition(lr[i].positionCount + -1, Camera.main.transform.forward * 100f);
                    }                    
                    projectile.SetActive(false);
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
            else
            {
                for (int i = 0; i < lr.Count; i++)
                {
                    lr[i].SetPosition(lr[i].positionCount + -1, Camera.main.transform.forward * 100f);
                }                
            }
            //Debug.Log("Click Shot");
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (currentCooldown <= 0f)
            {

            }


            /*for (int i = 0; i < lr.Count; i++)
            {
                lr[i].SetPosition(0, shootPoint.position);
            }            

            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;
                //Debug.Log("cooling down");
            }
            else
            {
                RaycastHit hit;
                if (!GameManager.instance.paused && GameManager.instance.currentState == EGameStates.Gameplay)
                {
                    //lr.enabled = true;
                }
                else
                {
                    //trying foreach on this
                    foreach (var item in lr)
                    {
                        item.enabled = false;
                    }
                    
                }
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange, ignoreMask.value))
                {
                    for (int i = 0; i < lr.Count; i++)
                    {
                        lr[i].SetPosition(lr[i].positionCount + -1, hit.point);
                        lr[i].enabled = true;
                    }
                    
                    projectile.SetActive(true);
                    projectile.transform.position = hit.point;
                    projectile.transform.rotation.SetLookRotation(projectile.transform.position - transform.position);
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        //Debug.Log("remaining hp: " + hit.collider.GetComponent<EnemyBase>().GetHP().ToString());
                        if (hit.collider.gameObject.GetComponent<EnemyBase>()?.GetHP() <= damage)
                        {
                            GameObject killVfx = ElecRayKillParticlesPool.Instance.RequestPoolObject();
                            killVfx.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-transform.forward));
                            killVfx.SetActive(true);
                            killVfx.GetComponent<VisualEffect>()?.Play();
                            killVfx.GetComponent<AudioSource>()?.Play();
                        }
                        hit.collider.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
                    }
                    if (hit.collider.gameObject.CompareTag("Boss"))
                    {
                        hit.collider.gameObject.GetComponent<BossBase>().TakeDamage(damage);
                    }
                }
                else
                {
                    foreach (var item in lr)
                    {
                        item.SetPosition(item.positionCount + -1, transform.position + Camera.main.transform.forward.normalized * 40f);
                    }
                    
                    projectile.SetActive(false);
                }
                foreach (var item in lr)
                {
                    for (int i = 1; i < item.positionCount + -1; i++)
                    {
                        item.SetPosition(i, Vector3.Lerp(shootPoint.position, item.GetPosition(item.positionCount + -1), 0.15f * i));
                        Vector3 spotOfChange = item.GetPosition(i);
                        item.SetPosition(i, spotOfChange + new Vector3(Random.Range(-lightningEntropy, lightningEntropy), Random.Range(-lightningEntropy, lightningEntropy), Random.Range(-lightningEntropy, lightningEntropy)));
                    }
                    item.enabled = true;
                }
                
                /*for (int i = 1; i < lr.positionCount + -1; i++)
                {

                }*/
        /*currentCooldown = fireRate;
        //Debug.Log("Held shot");
    }*/
        /*}
        else
        {
            foreach (var item in lr)
            {
                item.enabled = false;
            }            
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            foreach (var item in lr)
            {
                item.enabled = false;
            }            
            projectile.SetActive(false);
        }*/
        #endregion
        FireShotgun();
        
    }

    public override void StartFiring()
    {
        base.StartFiring();
    }

    //decided to streamline it instead of just having a giant fire function that does everything
    //my bad
    /*protected override void OnFire(CallbackContext callback) //may have to not even override actually
    {                                                //learning on the go is a mess but I swear I had no other chance or choice
        base.OnFire(value);
        if (fireHeld)
        {
            if (currentCooldown <= 0f)
            {
                FireShotgun();
            }
        }
    ////////////
        print("firing input");
        if (fireHeld)
        {
            
            if (currentCooldown <= 0f)
            {
                FireShotgun();
            }
        }
    ///////////////////////// comented off
    }*/

    /*private IEnumerator ContinuousFire()
    {
        while (true)
        {
            if (GameManager.instance.currentState == EGameStates.Gameplay && !GameManager.instance.paused)
            {
                currentCooldown -= Time.deltaTime;
                if (fireHeld && canAttack)
                {
                    if (currentCooldown <= 0f)
                    {
                        FireShotgun();
                    }
                }
            }            
            yield return null;
        }
    }*/

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
