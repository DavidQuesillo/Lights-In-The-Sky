using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Keys { Red, Blue, Yellow, Green}
public class ColorKey : MonoBehaviour
{
    [SerializeField] private Keys keyColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeyManager.instance.PickUpKey(keyColor);
            gameObject.SetActive(false);
        }
    }
}
