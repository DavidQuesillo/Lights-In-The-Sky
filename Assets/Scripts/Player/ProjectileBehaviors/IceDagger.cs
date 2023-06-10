using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDagger : PlayerBullet
{    
    private void OnEnable()
    {
        rb.AddForce(transform.forward * projSpeed, ForceMode.VelocityChange);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    private void OnTriggerEnter(Collider other)
    {                
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyBase>().GetHP() <= damage)
            {
                //GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();

                //might wanna change this but theres really no need to
                GameObject killParticle = IceDagKillParticlesPool.Instance.RequestPoolObject();
                killParticle.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-transform.forward));
                //killParticle.GetComponent<VisualEffect>()?.Play();
                //killParticle.GetComponent<VisualEffect>().Stop()
                killParticle.GetComponent<AudioSource>()?.Play();
                //Debug.Log(other.name);
                gameObject.SetActive(false);
                return;
            }
        }

        GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();
        particle.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-transform.forward));
        particle.GetComponent<AudioSource>()?.Play();
        //Debug.Log(other.name);
        gameObject.SetActive(false);
        //Destroy(gameObject, 0.02f);
    }
}
