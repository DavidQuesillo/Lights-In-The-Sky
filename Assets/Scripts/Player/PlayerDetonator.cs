using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDetonator : Weapon
{
    [SerializeField] private AudioSource aus;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] private pool explosionPool;
    [SerializeField] private float explosionBaseSize = 3f; //explosions will be the hit collider's scale + this amount;
    [SerializeField] private float explosionStartSize = 0.4f; //the size of the explosion at the start
    [SerializeField] private float explosionDuration;

    // Start is called before the first frame update
    void Start()
    {
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
        
    }

    protected override void Fire()
    {
        base.Fire();
        RaycastHit hit;
        anim.SetBool("Shooting", true);
        if (!GameManager.instance.paused)
        {
            //lr.enabled = true;
            aus.Play();
        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange, ignoreMask.value))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("EnemyShot"))
                {
                    hit.collider.gameObject.SetActive(false);
                    GameObject explosion = explosionPool.RequestPoolObject();
                    explosion.transform.position = hit.transform.position;
                    
                    Renderer hitRend = hit.collider.GetComponent<EnemyBullets>().GetRenderer();
                    //explosion.transform.localScale = Vector3.one * explosionStartSize;
                    //explosion.transform.localScale = hitRend.bounds.extents*2f;
                    explosion.transform.localScale = Vector3.one * Mathf.Min(hitRend.bounds.extents.x, 
                                                         hitRend.bounds.extents.y, hitRend.bounds.extents.z) * 2f;

                    explosion.GetComponent<PlayerBullet>().SetLifetime(explosionDuration);
                    explosion.GetComponent<PlayerBullet>().SetDamage(damage);
                    explosion.GetComponent<Renderer>().material.color = Color.white;
                    
                    //explosion.GetComponent<EnemyBullets>(). //realized the boom doesnt need the script

                    explosion.SetActive(true);
                    #region scale relative to bullet size attempts
                    /*Collider collll;
                    bool yea = TryGetComponent<Collider>(out collll);*/
                    //print("extents of the bullet: " + hit.collider.GetComponent<SphereCollider>().radius.ToString());
                    //explosion.transform.DOScale(Vector3.one * (hit.collider.bounds.extents.x + explosionBaseSize), explosionDuration);
                    /*if (hit.collider.TryGetComponent<SphereCollider>(out SphereCollider sphere))
                    {
                        explosion.transform.DOScale(Vector3.one * (hit.collider.GetComponent<SphereCollider>().radius + explosionBaseSize), explosionDuration);
                    }*/

                    //attempts to scale according to bullet size
                    ////
                    ///
                    //explosion.transform.DOScale(Vector3.one * (hit.collider.GetComponent<SphereCollider>().radius + explosionBaseSize), explosionDuration);

                    /*float scaleFromExtent = Mathf.Max(hit.collider.GetComponent<Renderer>().bounds.extents.x, 
                                                        hit.collider.GetComponent<Renderer>().bounds.extents.y);*/
                    //explosion.transform.DOScale(Vector3.one * (scaleFromExtent + explosionBaseSize), explosionDuration);
                    //explosion.transform.DOScale(Vector3.one * (hit.collider.GetComponent<Renderer>().bounds.extents.x + explosionBaseSize), explosionDuration);



                    //current attempt
                    /*if (hit.collider.TryGetComponent<SphereCollider>(out SphereCollider sphere))
                    {
                        explosion.transform.DOScale(Vector3.one * (sphere.radius + explosionBaseSize), explosionDuration);
                    }
                    else
                    {
                        print("couldnt pop " + hit.collider.name + "because it doesnt have a spherecollider");
                    }*/
                    //we are sticking to sphere colliders for now. Every enemy bullet that can be popped
                    //will be a sphere collider. Others like valSlash will be given lore reasons
                    //such as the fact that its not a magic bullet but a magic projection or something

                    /*else
                    {
                        if (TryGetComponent<Renderer>(out Renderer objRend))
                        {
                            float scaleFromExtent = Mathf.Max(hit.collider.GetComponent<Renderer>().localBounds.extents.x,
                                                        hit.collider.GetComponent<Renderer>().localBounds.extents.y,
                                                        hit.collider.GetComponent<Renderer>().localBounds.extents.z);
                            explosion.transform.DOScale(Vector3.one * (scaleFromExtent / 2f + explosionBaseSize), explosionDuration);
                        }
                        else if (hit.collider.gameObject.child<Renderer>(out Renderer childRend))
                        {

                        }
                    }*/

                    //print("global Bounds: " + hit.collider.GetComponent<Renderer>().bounds.ToString());
                    //print("localBounds: " + hit.collider.GetComponent<Renderer>().localBounds.ToString());
                    #endregion
                    
                    if (hit.collider.TryGetComponent<SphereCollider>(out SphereCollider sphere))
                    {                        
                        /*float scaleFromExtent = Mathf.Max(hit.collider.GetComponent<Renderer>().localBounds.extents.x,
                                                        hit.collider.GetComponent<Renderer>().localBounds.extents.y,
                                                        hit.collider.GetComponent<Renderer>().localBounds.extents.z);*/
                        float scaleFromExtent = Mathf.Max(hitRend.bounds.extents.x*2,
                                                        hitRend.bounds.extents.y*2,
                                                        hitRend.bounds.extents.z*2);
                        explosion.transform.DOScale(Vector3.one * (scaleFromExtent *3 + explosionBaseSize), explosionDuration);
                        //explosion.transform.DOScale(Vector3.one * (sphere.radius + explosionBaseSize), explosionDuration);
                    }
                    else
                    {
                        float scaleFromExtent = Mathf.Max(hitRend.localBounds.extents.x,
                                                        hitRend.localBounds.extents.y,
                                                        hitRend.localBounds.extents.z);
                        explosion.transform.DOScale(Vector3.one * (scaleFromExtent / 2f + explosionBaseSize), explosionDuration);
                        //print("couldnt pop " + hit.collider.name + "because it doesnt have a spherecollider");
                    }
                    print("global Bounds: " + hitRend.bounds.ToString());
                    print("localBounds: " + hitRend.localBounds.ToString());
                    //none of it works. I'm making it a set size, at least for now
                    //explosion.transform.DOScale(Vector3.one * (scaleFromExtent / 2f + explosionBaseSize), explosionDuration);
                    explosion.GetComponent<Renderer>().material.DOFade(0f, explosionDuration);
                }
                /*for (int i = 0; i < lr.Count; i++)
                {
                    lr[i].SetPosition(lr[i].positionCount + -1, hit.point);
                }*/
                //ray.SetPosition(ray.positionCount + -1, hit.point);

                //GameObject sparks = ShotgunSparksPool.Instance.RequestPoolObject();
                /*GameObject sparks = hitFxPool.RequestPoolObject();
                sparks.transform.position = hit.point;
                sparks.transform.rotation.SetLookRotation(sparks.transform.position - transform.position);*/
                //lr[0].SetPosition(0, shootPoint.position);
                /*foreach (var line in lr)
                {
                    line.SetPosition(0, shootPoint.position);
                }*/
                /*projectile.SetActive(true);
                projectile.transform.position = hit.point;
                projectile.transform.rotation.SetLookRotation(projectile.transform.position - transform.position);*/
            }
        }
    }
}
