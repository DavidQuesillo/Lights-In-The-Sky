using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerArea : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private LayerMask checkLayers;
    [SerializeField] private bool moreThanOnce;

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer)) == checkLayers)
        {
            onEnter?.Invoke();
            Debug.Log("Area triggered");
            if (!moreThanOnce)
            {
                gameObject.SetActive(false);
            }
        }        
    }
}
