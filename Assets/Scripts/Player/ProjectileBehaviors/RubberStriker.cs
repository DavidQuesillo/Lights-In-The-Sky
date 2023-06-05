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
    Vector3 velPreBounce; //velocity previous to a bounce
    [SerializeField] private int bounceLimit = 5;
    [SerializeField] private float bonusDmgPerBounce = 2f; //dmg added with each bounce
    private int remainingBounces;
    //[SerializeField] private Rigidbody rb;
    //private float startSpeed;

    private void OnEnable()
    {
        lifeTimer = lifetime;
        remainingBounces = bounceLimit;
        rb.AddForce(transform.forward * projSpeed, ForceMode.VelocityChange);
        velPreBounce = transform.forward * projSpeed;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
        rb.rotation = Quaternion.LookRotation(rb.velocity);

        //float distance = Vector3.Distance(GameManager.instance.player.transform.position, transform.position);
        //hlOutline.localScale = startOutlineSize * distance*0.03f;
    }

    private void OnCollisionEnter(Collision collision)
    {        
        //Vector3 velBeforeBounce = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.velocity = Vector3.Reflect(velPreBounce, collision.GetContact(0).normal).normalized * projSpeed;
        remainingBounces -= 1;
        if (remainingBounces <= 0)
        {
            RubberBreak();
        }
        else
        {
            velPreBounce = rb.velocity;
            damage += bonusDmgPerBounce;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //base.OnTriggerEnter(other);
        //velPreBounce = rb.velocity;
    }

    private void RubberBreak() //Im so sorry for this name
    {
        //placeholder code
        print("Ball broken");
        gameObject.SetActive(false);
    }
}
