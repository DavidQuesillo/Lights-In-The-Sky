using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Weapon
{
    [SerializeField] private pool projPool;
    //[SerializeField] protected Transform projPoolParent;
    [Header("Projectile Exclusives")]
    [SerializeField] private Transform rotationRef;
    [SerializeField] private Transform shootPoint;
    
    /*[SerializeField] private pool hitFxPool;
    [SerializeField] private pool killFxPool;*/

    private void Awake()
    {
        //projPool.assignedParent = projPoolParent;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ContinuousFire());
    }
    private void OnEnable()
    {
        //StopCoroutine(ContinuousFire());
        //StartCoroutine(ContinuousFire());

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
    // Update is called once per frame
    void Update()
    {
        /*if (canAttack && GameManager.instance.currentState == EGameStates.Gameplay)
        {
            Fire();
        }*/
    }

    protected virtual void CreateBullet()
    {
        //there has been changes since initial copypaste from the original in Fire
        GameObject shot = projPool.RequestPoolObject();
        shot.transform.rotation = rotationRef.rotation;
        shot.transform.position = shootPoint.position;
        shot.GetComponent<PlayerBullet>().SetDamage(damage);
        shot.GetComponent<PlayerBullet>().SetHitFxPool(hitFxPool);
        shot.GetComponent<PlayerBullet>().SetKillFxPool(killFxPool);
        shot.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //trying a flexible method where weapon tells speed and proj uses it in its own way  /////      //////
        shot.GetComponent<PlayerBullet>().SetSpeed(projectileSpeed);
        shot.SetActive(true);
        //shot.GetComponent<Rigidbody>().AddForce(cam.forward * projectileSpeed, ForceMode.VelocityChange);
        
    }

    #region old fire
    protected override void Fire()
    {
        CreateBullet();

        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //GameObject shot = Instantiate(projectile, transform.position + Vector3.down * 0.6f, GameManager.instance.player.transform.rotation);
            /*GameObject shot = IceDaggerPool.Instance.RequestPoolObject();
            shot.transform.rotation = GameManager.instance.player.transform.rotation;
            shot.transform.position = transform.position + Vector3.down * 0.6f;
            shot.GetComponent<PlayerBullet>().SetDamage(damage);
            shot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            shot.SetActive(true);
            shot.GetComponent<Rigidbody>().AddForce(cam.forward * projectileSpeed, ForceMode.VelocityChange);*/
        /*
            CreateBullet();

            //Debug.Log("Click Shot");
        }*/
       /* if (Input.GetKey(KeyCode.Mouse0))
        {
            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;
                //Debug.Log("cooling down");
            }
            else
            {
                //GameObject shot = Instantiate(projectile, transform.position + Vector3.down*0.6f , GameManager.instance.player.transform.rotation);
                /*GameObject shot = IceDaggerPool.Instance.RequestPoolObject();
                shot.transform.rotation = GameManager.instance.player.transform.rotation;
                shot.transform.position = transform.position + Vector3.down * 0.6f;
                shot.GetComponent<PlayerBullet>().SetDamage(damage);
                shot.GetComponent<Rigidbody>().velocity = Vector3.zero;
                shot.SetActive(true);
                shot.GetComponent<Rigidbody>().AddForce(cam.forward * projectileSpeed, ForceMode.VelocityChange);
                currentCooldown = fireRate;*/
       /*
                CreateBullet();
                currentCooldown = fireRate;

                //Debug.Log("Held shot");
            }
        }*/
    }
    #endregion
}
