using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private AudioClip blockSound;
    //[SerializeField] private float shieldMeter = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            GetComponent<AudioSource>().PlayOneShot(blockSound, 1f);
            SendMessageUpwards("DamageShield", other.GetComponent<EnemyBullets>().GetDmg());
        }
    }
}
