using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float damage;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] private GameObject particles;
    [SerializeField] private float lifetime = 2f;
    private float timer;

    private void OnEnable()
    {
        timer = lifetime;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public float GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
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
        GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();
        particle.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-transform.forward));
        gameObject.SetActive(false);
        //Destroy(gameObject, 0.02f);
    }
}
