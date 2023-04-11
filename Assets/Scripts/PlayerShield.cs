using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private AudioClip blockSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            GetComponent<AudioSource>().PlayOneShot(blockSound, 1f);
        }
    }
}
