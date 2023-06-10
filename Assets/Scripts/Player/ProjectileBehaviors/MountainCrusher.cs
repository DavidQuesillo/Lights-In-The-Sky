using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MountainCrusher : PlayerBullet
{
    [Header("Unique Behavior")]
    [SerializeField] private GameObject modelScaler;
    [SerializeField] private SphereCollider col;
    [SerializeField] private float rotatingSpeed = 1f; //the speed at which the boulder rotates as it flies. Purely aesthetic
    [SerializeField] private float baseHealth = 100f;
    private float health;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        rb.AddForce(transform.forward * projSpeed, ForceMode.VelocityChange);
        rb.angularVelocity = Vector3.right * rotatingSpeed;
        health = baseHealth;
    }

    public void ChangeSize(float newSize)
    {
        modelScaler.transform.localScale = Vector3.one * newSize;
        col.radius = newSize;
    }

    private void BreakBoulder(Collider other)
    {        
        GameObject breakParticle = hitFxPool.RequestPoolObject();
        breakParticle.transform.SetPositionAndRotation(other.ClosestPoint(rb.position), Quaternion.LookRotation(-transform.forward));
        //breakParticle.transform.localScale = modelScaler.transform.localScale * 2.5f;
        //killParticle.GetComponent<VisualEffect>()?.Play();
        //killParticle.GetComponent<VisualEffect>().Stop()
        //breakParticle.GetComponent<AudioSource>()?.Play(); //no aus yet
        Debug.Log("broken on: " + other.name);
        gameObject.SetActive(false);
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        //in this case, killFX will be used for the projectile killing and enemy, and hit will be used for it breaking
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyBase>().GetHP() >= damage)
            {
                //GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();



                /*GameObject breakParticle = hitFxPool.RequestPoolObject();
                breakParticle.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                //killParticle.GetComponent<VisualEffect>()?.Play();
                //killParticle.GetComponent<VisualEffect>().Stop()
                //breakParticle.GetComponent<AudioSource>()?.Play(); //no aus yet
                Debug.Log("broken on: " + other.name);
                gameObject.SetActive(false);
                return;*/
                BreakBoulder(other);
            }
            else
            {
                GameObject killParticle = killFxPool.RequestPoolObject();
                killParticle.transform.SetPositionAndRotation(other.ClosestPoint(rb.position), Quaternion.identity);
                //killParticle.GetComponent<VisualEffect>()?.Play();
                //killParticle.GetComponent<VisualEffect>().Stop()
                //killParticle.GetComponent<AudioSource>()?.Play(); //no aus yet
                Debug.Log("crushed " + other.name);
                //gameObject.SetActive(false);
                return;
            }
        }
        else if (other.CompareTag("Boss"))
        {
            /*GameObject breakParticle = hitFxPool.RequestPoolObject();
            breakParticle.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            //killParticle.GetComponent<VisualEffect>()?.Play();
            //killParticle.GetComponent<VisualEffect>().Stop()
            //breakParticle.GetComponent<AudioSource>()?.Play(); //no aus yet
            Debug.Log("broken on: " + other.name);
            gameObject.SetActive(false);
            return;*/
            BreakBoulder(other);
        }
        /*if (!other.CompareTag("EnemyShot"))
        {
            BreakBoulder(other);
        }
        else*/
        else if (other.CompareTag("EnemyShot"))
        {
            health -= other.GetComponent<EnemyBullets>().GetDmg();
            other.gameObject.SetActive(false);
            print("enemy bullet destroyed by boulder");
            if (health <= 0f)
            {
                BreakBoulder(other);
            }
        }
        //if (other.gameObject.layer == LayerMask.NameToLayer("Stage"))

        //former full else: ////////////////////////////////////////////////
        //should probably turn this into a function in this case. This instance of the code will be continuously updated so it should be picked up for it
        //done so
        //BreakBoulder();
        //shouldnt use it here tho

        /*GameObject breakParticle = hitFxPool.RequestPoolObject();
        breakParticle.transform.SetPositionAndRotation(other.ClosestPoint(rb.position), Quaternion.LookRotation(-transform.forward));
        //breakParticle.transform.localScale = modelScaler.transform.localScale * 2.5f;
        //killParticle.GetComponent<VisualEffect>()?.Play();
        //killParticle.GetComponent<VisualEffect>().Stop()
        //breakParticle.GetComponent<AudioSource>()?.Play(); //no aus yet
        Debug.Log("broken on: " + other.name);
        gameObject.SetActive(false);
        return;*/

        //BEYOND OUTDATED but commented just in case
        /*GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();
        particle.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-transform.forward));
        particle.GetComponent<AudioSource>()?.Play();
        //Debug.Log(other.name);
        gameObject.SetActive(false);
        //Destroy(gameObject, 0.02f);*/
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("EnemyShot"))
        {
            BreakBoulder(collision.collider);
        }
        else
        {
            health -= collision.gameObject.GetComponent<EnemyBullets>().GetDmg();
            collision.gameObject.SetActive(false);
            print("enemy bullet destroyed by boulder");
            if (health <= 0f)
            {
                BreakBoulder(collision.collider);
            }
        }
    }
}
