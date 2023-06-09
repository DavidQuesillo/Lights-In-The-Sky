using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingBullets : EnemyBullets
{
    [SerializeField] private float maxSize;
    [SerializeField] private float growthSpeed;
    [SerializeField] private float startScale = 1.3f;

    private void OnEnable()
    {
        transform.localScale = Vector3.one * startScale;
        timer = dissipateTimer;
        rb.velocity = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (transform.localScale.magnitude < (Vector3.one * maxSize).magnitude)
        {
            transform.localScale += Vector3.one * (Time.deltaTime * growthSpeed);
        }
        
        if (timer <= 0f)
        {
            timer = dissipateTimer;
            gameObject.SetActive(false);
        }
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
