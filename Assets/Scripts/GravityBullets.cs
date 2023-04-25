using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBullets : MonoBehaviour
{
    [SerializeField] private float lifetimeAfterTouch = 0.1f;
    [SerializeField] private float lifetimeVariance = 0.05f;
    private float timer;
    [SerializeField] private bool touched = false;

    private void Start()
    {
        StartCoroutine(VanishCoroutine());
    }

    private void OnEnable()
    {
        timer = lifetimeAfterTouch + Random.Range(-lifetimeVariance, lifetimeVariance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("bonked smth");
        touched = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator VanishCoroutine()
    {
        while (true)
        {
            while (touched)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    break;
                }
                gameObject.SetActive(false);
                yield return null;
            }            
            yield return null;
        }
    }
}
