using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SelfDisableTimer : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private VisualEffect vfx;
    private WaitForSeconds lifeDelay;

    private void Start()
    {
        lifeDelay = new WaitForSeconds(lifetime);        
    }

    private void OnEnable()
    {
        StartCoroutine(SelfDisable());
    }
    private IEnumerator SelfDisable()
    {
        yield return lifeDelay;
        vfx.Stop();
        gameObject.SetActive(false);
    }
    
}
