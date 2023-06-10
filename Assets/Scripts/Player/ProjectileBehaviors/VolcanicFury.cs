using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VolcanicFury : PlayerBullet
{
    //[SerializeField] private pool explPool; //the pool from which the explosion gameObject is drawn from
    //
    [SerializeField] private Collider projCol; //the collider of this same obj, to deactivate it so the child's can take over
    [SerializeField] private Collider projCol2; //the solid colllider that the obj was given to ensure surface explosion
    [SerializeField] private Renderer projRend; //the renderer for this projectile, so it is hidden when the explosion happens
    //[SerializeField] private Ease explosionEase; //mostly for testing, should hard-code in once its settled which to use
    [SerializeField] private GameObject explosion; //the obj will have a child object that will be used when this collides
    [SerializeField] private float explosionDamage = 1f;
    [SerializeField] private float explosionSize = 2f;
    [SerializeField] private float explosionDuration = 1f;
    [SerializeField] private AnimationCurve explosionCurve; //the curve with which the explosion expands

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        explosion.SetActive(false);
        explosion.GetComponent<PlayerBullet>().SetDamage(explosionDamage);
        projCol.enabled = true;
        projCol2.enabled = true;
        projRend.enabled = true;
        rb.AddForce(transform.forward * projSpeed, ForceMode.VelocityChange);
    }

    private void Detonate()
    {
        //GameObject explosion = explPool.RequestPoolObject();        
        rb.velocity = Vector3.zero;
        explosion.transform.position = transform.position;
        //damage = explosionDamage;
        explosion.SetActive(true);
        //initializing start values
        explosion.transform.localScale = Vector3.one * 0.2f;
        explosion.GetComponent<Renderer>().material.color = Color.white; //Might use tweening if this doesnt work
        //tweening previous values
        //explosion.transform.DOScale(explosionSize, explosionDuration).SetEase(explosionEase);
        explosion.transform.DOScale(explosionSize, explosionDuration).SetEase(explosionCurve);
        explosion.GetComponent<Renderer>().material.DOFade(0f, explosionDuration);
        StartCoroutine(SelfDisableAfterExploding());
    }

    private void OnTriggerEnter(Collider other)
    {        
        //gameObject.SetActive(false);
        projCol.enabled = false;
        projCol2.enabled = false;
        projRend.enabled = false;
        Detonate();
        if (other.CompareTag("Enemy"))
        {
            /*if (other.GetComponent<EnemyBase>().GetHP() <= damage + explosionDamage)
            {
                GameObject killfx = killFxPool.RequestPoolObject();
                killfx.transform.position = other.transform.position;
            }*/ //this isnt ready but gotta set it down
        }


    }

    private IEnumerator SelfDisableAfterExploding()
    {
        yield return new WaitForSeconds(explosionDuration);
        explosion.SetActive(false);
        gameObject.SetActive(false);
    }
}
