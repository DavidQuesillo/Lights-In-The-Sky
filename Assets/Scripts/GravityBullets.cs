using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBullets : MonoBehaviour
{
    [SerializeField] private float lifetimeAfterTouch = 0.1f;
    [SerializeField] private float lifetimeVariance = 0.05f;
    [SerializeField] private float timer;
    [SerializeField] private bool touched = false;

    private void Start()
    {
        //StartCoroutine(VanishCoroutine());
    }

    private void OnEnable()
    {
        StopCoroutine(VanishCoroutine());
        touched = false;
        timer = lifetimeAfterTouch + Random.Range(-lifetimeVariance, lifetimeVariance);
        transform.localScale = Vector3.one;
        StartCoroutine(VanishCoroutine());
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("bonked smth");
        touched = true;
        Debug.Log("fireball touched");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
    /*private void OnCollisionExit(Collision collision)
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        
    }*/

    private IEnumerator VanishCoroutine()
    {
        while (true)
        {
            while (touched)
            {
                timer -= Time.deltaTime;
                transform.localScale = Vector3.one * timer;
                if (timer <= 0f)
                {
                    gameObject.SetActive(false);
                    break;
                }
                else
                {
                    yield return null;
                }
                //gameObject.SetActive(false);
            }            
            yield return null;
        }
        Debug.Log("Exited coroutine");
    }
}
