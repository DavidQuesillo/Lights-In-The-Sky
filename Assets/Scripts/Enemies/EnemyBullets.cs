using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullets : MonoBehaviour
{
    [SerializeField] protected float dissipateTimer = 5f;
    protected float timer;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] private float shieldDmg;

    private void OnEnable()
    {
        timer = dissipateTimer;
        rb.velocity = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = dissipateTimer;
            gameObject.SetActive(false);
        }
    }

    public float GetDmg()
    {
        return shieldDmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShield") || other.CompareTag("Player"))
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
