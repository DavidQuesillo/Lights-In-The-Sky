using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class KeyInteraction : MonoBehaviour
{
    [SerializeField] private Keys requiredKey;
    [SerializeField] private UnityEvent OnOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool CheckForKey()
    {
        return KeyManager.instance.CheckForKey(requiredKey);
    }

    private void OpenEvent()
    {
        OnOpen?.Invoke();
    }

    public void GenericOpen()
    {
        //placeholder code
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (CheckForKey())
            {
                OpenEvent();
            }
        }
    }
}
