using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionDropBullets : MonoBehaviour
{
    [SerializeField] private int amount = 3;
    [SerializeField] private float explodeForce = 1f;
    //private List<GameObject> drops;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Explode()
    {
        Debug.Log("exploded");
        for (int i = 0; i < amount; i++)
        {
            GameObject drop = ImpFlamePool.Instance.RequestPoolObject();
            //drops.Add(ImpFlamePool.Instance.RequestPoolObject());
            //drops[i].transform.position = transform.position + new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f));
            //drops[i].GetComponent<Rigidbody>().AddExplosionForce(explodeForce, transform.position, 1f);
            drop.transform.position = transform.position + new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f));
            drop.GetComponent<Rigidbody>().AddExplosionForce(explodeForce, transform.position, 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    
}
