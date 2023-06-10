using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeShot : Weapon
{
    [SerializeField] private pool projPool;
    [SerializeField] private float minDamage; //the dmg this will deal from zero charge, to lerp into the full max dmg thats represented by the damage var
    [SerializeField] private float currentDamage; //the var that keeps track of the damage the projectile will do if launched now, serialized only for testing
    //[SerializeField] protected Transform projPoolParent;
    [Header("Projectile Exclusives")]
    [SerializeField] private Transform rotationRef;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float startProjSize = 0.2f;
    [SerializeField] private float maxProjSize = 1f;
    [SerializeField] private float projSize = 0.2f;
    private float chargeProgress;
    [SerializeField] private float chargeSpeed;
    private bool charging; //used to differenctiate between releasing charge and just not charging for !fireheld
    

    //the way this is gonna work is
    //projectile from pool will be assigned to projectile var,
    //then that projectile will be put through the process of charging the attack,
    //then it is released and launched, and a new obj from pool will become the next projectile var.
    //the animator will use a completely separate dummy boulder which will be disabled and immediately replaced
    //with the real projectile the moment the attack is fired
    void Start()
    {
        //StartCoroutine(ContinuousFire());
        StartCoroutine(ChargeShot());
    }
    private void OnEnable()
    {
        //StopCoroutine(ContinuousFire());
        //StartCoroutine(ContinuousFire());
        if (GetIfCooldownPassedWhileSwapped())
        {
            currentCooldown = 0f;
        }
        projSize = startProjSize;
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
        shot.GetComponent<MountainCrusher>().ChangeSize(projSize);
        shot.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //trying a flexible method where weapon tells speed and proj uses it in its own way  /////      //////
        shot.GetComponent<PlayerBullet>().SetSpeed(projectileSpeed);
        shot.SetActive(true);
        //shot.GetComponent<Rigidbody>().AddForce(cam.forward * projectileSpeed, ForceMode.VelocityChange);
        currentCooldown = fireRate;
    }

    protected override void Fire()
    {
        base.Fire();

        //animator lines here

        //projectile = projPool.RequestPoolObject();
        //CreateBullet();

    }
    //this is old, kept originally for nondestructiveness
    private IEnumerator ChargeShot1()
    {
        while (true)
        {
            if (GameManager.instance.currentState == EGameStates.Gameplay && !GameManager.instance.paused)
            {
                currentCooldown -= Time.deltaTime;
                //Debug.Log("pillar cd: " + currentCooldown.ToString());
                if (currentCooldown <= 0f && canAttack)
                {
                    while (fireHeld)
                    {
                        charging = true;
                        //projectile.GetComponent<MountainCrusher>().
                        projSize = Mathf.Lerp(startProjSize, maxProjSize, chargeProgress);
                        currentDamage = Mathf.Lerp(minDamage, damage, chargeProgress);
                        chargeProgress += Time.deltaTime * chargeSpeed;
                        print("proj size: " + projSize.ToString());
                        print("proj dmg: " + currentDamage.ToString());
                        if (chargeProgress > 1f)
                        {
                            //break;
                            CreateBullet();
                            projSize = startProjSize;
                            chargeProgress = 0f;
                            charging = false;
                            break;
                        }
                        yield return null;
                    }
                    CreateBullet();
                    projSize = startProjSize;
                    chargeProgress = 0f;
                    charging = false;
                    /*if (currentCooldown <= 0f)
                    {
                        Fire();
                        currentCooldown = fireRate;
                    }*/

                    /*charging = true;
                    //projectile.GetComponent<MountainCrusher>().
                    projSize = Mathf.Lerp(startProjSize, maxProjSize, chargeProgress);
                    currentDamage = Mathf.Lerp(minDamage, damage, currentDamage);
                    chargeProgress += Time.deltaTime * chargeSpeed;
                    print("proj size: " + projSize.ToString());
                    print("proj dmg: " + currentDamage.ToString());*/
                }

                /*if (charging && !fireHeld)
                {
                    
                    //launch the attack
                    CreateBullet();
                    projSize = startProjSize;
                    chargeProgress = 0f;
                    charging = false;
                }*/
            }
            yield return null;
        }
    }
    private IEnumerator ChargeShot()
    {
        while (true)
        {
            if (GameManager.instance.currentState != EGameStates.Gameplay || GameManager.instance.paused)
            {
                yield return null;
            }
            currentCooldown -= Time.deltaTime;
            if (currentCooldown > 0f)
            {
                yield return null;
            }
            while (fireHeld)
            {
                if (currentCooldown > 0f)
                {
                    break;
                }
                charging = true;
                //projectile.GetComponent<MountainCrusher>().
                projSize = Mathf.Lerp(startProjSize, maxProjSize, chargeProgress);
                currentDamage = Mathf.Lerp(minDamage, damage, chargeProgress);
                chargeProgress += Time.deltaTime * chargeSpeed;
                //print("proj size: " + projSize.ToString());
                //print("proj dmg: " + currentDamage.ToString());
                //print("chargeprog: " + chargeProgress);
                if (chargeProgress >= 1f)
                {
                    //Fire();
                    CreateBullet();
                    //launch the attack
                    projSize = startProjSize;
                    chargeProgress = 0f;
                    charging = false;
                    break;
                }
                yield return null;
            }
            if (charging && !fireHeld)
            {
                //Fire();
                CreateBullet();
                //launch the attack
                projSize = startProjSize;
                chargeProgress = 0f;
                charging = false;
            }
            yield return null;
        }
    }    
}
