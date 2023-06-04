using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberStriker : PlayerBullet
{
    /*private float damage;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] private GameObject particles;
    [SerializeField] private float lifetime = 2f;
    private float timer;*/

    [SerializeField] private int bounceLimit = 5;
    private int remainingBounces;
    [SerializeField] private Rigidbody rb;

    private void OnEnable()
    {
        lifeTimer = lifetime;
        remainingBounces = bounceLimit;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {        
        rb.velocity = Vector3.Reflect( rb.velocity, collision.GetContact(0).normal);
        remainingBounces -= 1;
        if (remainingBounces <= 0)
        {
            RubberBreak();
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        //base.OnTriggerEnter(other);
    }

    private void RubberBreak() //Im so sorry for this name
    {
        //placeholder code

    }
}
