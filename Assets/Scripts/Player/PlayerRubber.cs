using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRubber : Weapon
{
    [SerializeReference] private Transform rotationRef;
    [SerializeReference] private Transform shootPoint;
    [SerializeField] private pool projPool; //thinking of implementing a system where weapons create their own bulletPools

    // Start is called before the first frame update
    void Start()
    {
        projPool = FindObjectOfType<RubberStrikerPool>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        //StopCoroutine(ContinuousFire());
        //StartCoroutine(ContinuousFire());
        if (GetIfCooldownPassedWhileSwapped())
        {
            currentCooldown = 0f;
        }
    }

    private void ShootStriker()
    {
        //GameObject shot = IceDaggerPool.Instance.RequestPoolObject();
        GameObject shot = projPool.RequestPoolObject();
        shot.transform.rotation = rotationRef.rotation;
        shot.transform.position = shootPoint.position;
        shot.GetComponent<PlayerBullet>().SetDamage(damage);
        shot.GetComponent<Rigidbody>().velocity = Vector3.zero;
        shot.SetActive(true);
        shot.GetComponent<Rigidbody>().AddForce(cam.forward * projectileSpeed, ForceMode.VelocityChange);
        currentCooldown = fireRate;
    }

    protected override void Fire()
    {
        //base.Fire();
        ShootStriker();
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
                        ShootStriker();
                    }
                }
            }
            yield return null;
        }
    }*/
}
