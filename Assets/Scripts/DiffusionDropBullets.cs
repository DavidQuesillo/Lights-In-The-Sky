using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionDropBullets : MonoBehaviour
{
    [SerializeField] private int amount = 3;
    [SerializeField] private float explodeForce = 1f;
    private Vector3 colNormal; //the normal vector of the collision
    //private List<GameObject> drops;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Explode()
    {
        //Debug.Log("exploded");
        for (int i = 0; i < amount; i++)
        {
            GameObject drop = ImpFlamePool.Instance.RequestPoolObject();
            //drops.Add(ImpFlamePool.Instance.RequestPoolObject());
            //drops[i].transform.position = transform.position + new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f));
            //drops[i].GetComponent<Rigidbody>().AddExplosionForce(explodeForce, transform.position, 1f);
            drop.transform.position = transform.position + colNormal.normalized + new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f))*35;
            drop.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //drop.GetComponent<Rigidbody>().AddExplosionForce(explodeForce, transform.position, 50f);
            drop.GetComponent<Rigidbody>().AddForce(colNormal * explodeForce + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 1f), Random.Range(-0.2f, 0.2f)), ForceMode.Impulse);
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        colNormal = collision.GetContact(0).normal;
        Explode();
        //Debug.Log("collided: " + colNormal.ToString());        
    }

    
}
