using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            GameObject shot = Instantiate(projectile, transform.position, GameManager.instance.player.transform.rotation);
            shot.GetComponent<PlayerBullet>().SetDamage(damage);
            shot.GetComponent<Rigidbody>().AddForce(cam.forward * projectileSpeed, ForceMode.VelocityChange);
            Debug.Log("Click Shot");
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;
                Debug.Log("cooling down");
            }
            else
            {
                GameObject shot = Instantiate(projectile, transform.position + Vector3.down*0.2f , GameManager.instance.player.transform.rotation);
                shot.GetComponent<PlayerBullet>().SetDamage(damage);
                shot.GetComponent<Rigidbody>().AddForce(cam.forward * projectileSpeed, ForceMode.VelocityChange);
                currentCooldown = fireRate;
                Debug.Log("Held shot");
            }
        }
    }
}
