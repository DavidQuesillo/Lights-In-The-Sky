using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitscan : Weapon
{
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private AudioSource aus;
    [SerializeField] private float lightningEntropy = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            Fire();
        }
    }

    protected override void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (!GameManager.instance.paused)
            {
                lr.enabled = true;
                aus.Play();
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange, ignoreMask.value))
            {
                //Debug.DrawLine(transform.position, hit.point, debugColor, 1f);
                lr.SetPosition(0, shootPoint.position);
                if (hit.collider != null && !hit.collider.isTrigger)
                {
                    lr.SetPosition(lr.positionCount + -1, hit.point);
                    projectile.SetActive(true);
                    projectile.transform.position = hit.point;
                    projectile.transform.rotation.SetLookRotation(projectile.transform.position - transform.position);
                }
                else
                {
                    lr.SetPosition(lr.positionCount + -1, Camera.main.transform.forward * 100f);
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
                lr.SetPosition(lr.positionCount + -1, Camera.main.transform.forward * 100f);
            }
            //Debug.Log("Click Shot");
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            lr.SetPosition(0, shootPoint.position);
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
                    lr.enabled = true;
                }
                else
                {
                    lr.enabled = false;
                }
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange, ignoreMask.value))
                {
                    lr.SetPosition(lr.positionCount + -1, hit.point);
                    projectile.SetActive(true);
                    projectile.transform.position = hit.point;
                    projectile.transform.rotation.SetLookRotation(projectile.transform.position - transform.position);
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
                    lr.SetPosition(lr.positionCount + -1,  transform.position + Camera.main.transform.forward.normalized * 40f);
                    projectile.SetActive(false);
                }
                for (int i = 1; i < lr.positionCount + -1; i++)
                {
                    lr.SetPosition(i, Vector3.Lerp(shootPoint.position, lr.GetPosition(lr.positionCount + -1), 0.15f * i));
                    Vector3 spotOfChange = lr.GetPosition(i);
                    lr.SetPosition(i, spotOfChange + new Vector3(Random.Range(-lightningEntropy, lightningEntropy), Random.Range(-lightningEntropy, lightningEntropy), Random.Range(-lightningEntropy, lightningEntropy)));
                }
                /*for (int i = 1; i < lr.positionCount + -1; i++)
                {

                }*/
                currentCooldown = fireRate;
                //Debug.Log("Held shot");
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            lr.enabled = false;
            projectile.SetActive(false);
        }
    }
}