using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerShotgun : Weapon
{

    [SerializeField] LayerMask ignoreMask;
    [SerializeField] private List<LineRenderer> lr;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private AudioSource aus;
    [SerializeField] private float lightningEntropy = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = fireRate;
    }

    // Update is called once per frame
    /*void Update()
    {
        if (canAttack)
        {
            Fire();
        }
    }*/

    private void OnDisable()
    {
        for (int i = 0; i < lr.Count; i++)
        {
            lr[i].enabled = false;
        }        
    }

    protected override void Fire()
    {
        if (GameManager.instance.currentState != EGameStates.Gameplay || !canAttack)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (!GameManager.instance.paused)
            {
                //lr.enabled = true;
                aus.Play();
            }
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
            for (int i = 0; i < lr.Count; i++)
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
                currentCooldown = fireRate;
                //Debug.Log("Held shot");
            }
        }
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
        }
    }
}
