using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected LayerMask ignoreMask;
    [SerializeField] protected GameObject particles;
    [SerializeField] protected float lifetime = 2f;
    [SerializeField] protected pool hitFxPool;
    [SerializeField] protected pool killFxPool;
    protected float lifeTimer;

    private void Start()
    {
        //StopCoroutine(lifeTimeDisabler());
        StartCoroutine(lifeTimeDisabler());
    }
    private void OnEnable()
    {
        lifeTimer = lifetime;
        /*StopCoroutine(lifeTimeDisabler());
        StartCoroutine(lifeTimeDisabler());*/
    }
    private void Update()
    {
        /*lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }*/
    }
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetHitFxPool(pool fxPool)
    {
        hitFxPool = fxPool;
    }
    public void SetKillFxPool(pool killPool)
    {
        killFxPool = killPool;
    }
    #region og trigger code
    /*if (other.gameObject.layer != ignoreMask.value)
{
    return;
}

if (other.CompareTag("Enemy"))
{
    Instantiate(particles, transform.position, Quaternion.LookRotation(-transform.forward), null);
    Destroy(gameObject, 0.02f);
}
if (other.CompareTag("Boss"))
{
    Instantiate(particles, transform.position, Quaternion.LookRotation(-transform.forward), null);
    Destroy(gameObject, 0.02f);
}
if (other.gameObject.layer == LayerMask.NameToLayer("Stage"))
{
    Instantiate(particles, transform.position, Quaternion.LookRotation(-transform.forward), null);
    Destroy(gameObject, 0.02f);
}
else if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
{
    Destroy(gameObject);
}
Destroy(gameObject, 0.02f);*/

    //Instantiate(particles, transform.position, Quaternion.LookRotation(-transform.forward), null);


    /*if (LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer)) == ignoreMask)
    {
        GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();
        particle.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-transform.forward));
        Debug.Log(other.name);
        gameObject.SetActive(false);
        //Destroy(gameObject, 0.02f);
    }*/


    //the only code that was used:
    //

    /*if (other.CompareTag("Enemy"))
    {
        if (other.GetComponent<EnemyBase>().GetHP() <= damage)
        {
            //GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();

            GameObject killParticle = IceDagKillParticlesPool.Instance.RequestPoolObject();
            killParticle.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-transform.forward));
            //killParticle.GetComponent<VisualEffect>()?.Play();
            //killParticle.GetComponent<VisualEffect>().Stop()
            killParticle.GetComponent<AudioSource>()?.Play();
            Debug.Log(other.name);
            gameObject.SetActive(false);
            return;
        }
    }

    GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();
    particle.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-transform.forward));
    particle.GetComponent<AudioSource>()?.Play();
    //Debug.Log(other.name);
    gameObject.SetActive(false);
    //Destroy(gameObject, 0.02f);*/
    #endregion

    protected IEnumerator lifeTimeDisabler()
    {
        //lifeTimer -= Time.deltaTime;
        while (true)
        {            
            if (lifeTimer <= 0f)
            {
                gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}
