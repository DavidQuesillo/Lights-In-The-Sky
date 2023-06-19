using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpaler : Weapon
{
    [SerializeField] LayerMask ignoreMask;
    //[SerializeField] private Transform shootPoint;
    //[SerializeField] private AudioSource aus;
    [SerializeField] private pool projPool;

    // Start is called before the first frame update
    void Start()
    {
        //projPool = FindObjectOfType<EarthPillarPool>(); //should replace with something that creates it,
                                                        //probably with a new poolManager script or something
                                                        //thot of smth its almost implemented rn - its done
        currentCooldown = 0f;                                                
        StartCoroutine(ContinuousFire()); //must be commented for debug flow
    }

    // Update is called once per frame
    void Update()
    {
        //StopCoroutine(ContinuousFire());        
    }

    private void OnEnable()
    {
        //this only exist for debugging where the player's weapons list can change during gameplay.
        //while it works identically, its preferable to stick with the initial flow as it doesn't
        //require performing many processes this does. Besides, its just for debugging after all
        /*if (firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(ContinuousFire());
        }
        else
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = StartCoroutine(ContinuousFire());
        }*/

        if (GetIfCooldownPassedWhileSwapped())
        {
            currentCooldown = 0f;
        }
    }
    private void OnDisable()
    {
        timeOfSwap = Time.time;
        fireHeld = false;
    }

    /*private void OnFire()
    {
        if (GameManager.instance.currentState != EGameStates.Gameplay || GameManager.instance.paused)
        { return;}

        RaycastHit hit;
        if (!GameManager.instance.paused)
        {
            //lr.enabled = true;
            aus.Play();
        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange, ignoreMask.value))
        {
            
        }
            
    }*/

    /*private void ErectPillar() //this just has the same thing as Fire() I moved it all there
    {        
        RaycastHit hit;
        if (!GameManager.instance.paused)
        {
            //lr.enabled = true;
            aus.Play();
        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange, ignoreMask.value))
        {
            GameObject pillar = projPool.RequestPoolObject();
            pillar.transform.position = hit.point;
            pillar.transform.rotation = Quaternion.LookRotation(-hit.normal);
            pillar.GetComponent<PlayerBullet>().SetDamage(damage);
            pillar.gameObject.SetActive(true);            
        }
        currentCooldown = fireRate;
    }*/

    public override void StartFiring()
    {
        base.StartFiring();
        anim.SetBool("Shooting", true);
    }
    protected override void Fire()
    {
        //base.Fire();
        anim.SetBool("Shooting", true);
        anim.SetTrigger("FireOnce");
        RaycastHit hit;
        if (!GameManager.instance.paused)
        {
            //lr.enabled = true;
            //aus.Play();
        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange, ignoreMask.value))
        {
            GameObject pillar = projPool.RequestPoolObject();
            pillar.transform.position = hit.point;
            pillar.transform.rotation = Quaternion.LookRotation(-hit.normal);
            pillar.GetComponent<PlayerBullet>().SetDamage(damage);
            pillar.gameObject.SetActive(true);            
        }
        currentCooldown = fireRate;
        //anim.SetBool("Shooting", false);
    }
    public override void StopFiring()
    {
        base.StopFiring();
        anim.SetBool("Shooting", false);
    }

    /*private IEnumerator ContinuousFire()
    {
        while (true)
        {
            if (GameManager.instance.currentState == EGameStates.Gameplay && !GameManager.instance.paused)
            {
                currentCooldown -= Time.deltaTime;
                //Debug.Log("pillar cd: " + currentCooldown.ToString());
                if (fireHeld && canAttack)
                {
                    if (currentCooldown <= 0f)
                    {
                        ErectPillar();
                    }
                }
            }
            yield return null;
        }
    }*/
}
