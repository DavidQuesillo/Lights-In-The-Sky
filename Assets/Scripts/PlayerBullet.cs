using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float damage;
    [SerializeField] private GameObject particles;

    private void Start()
    {
        Destroy(gameObject, 4f);
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Instantiate(particles, transform.position, Quaternion.LookRotation(-transform.forward), null);
            Destroy(gameObject, 0.02f);
        }
    }
}
