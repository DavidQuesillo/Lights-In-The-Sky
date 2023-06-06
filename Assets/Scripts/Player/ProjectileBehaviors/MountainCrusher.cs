using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MountainCrusher : PlayerBullet
{
    [SerializeField] private GameObject modelScaler;
    [SerializeField] private SphereCollider col;
    [SerializeField] private float rotatingSpeed = 1f; //the speed at which the boulder rotates as it flies. Purely aesthetic
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        rb.AddForce(transform.forward * projSpeed, ForceMode.VelocityChange);
        rb.angularVelocity = Vector3.right * rotatingSpeed;
    }

    public void ChangeSize(float newSize)
    {
        modelScaler.transform.localScale = Vector3.one * newSize;
        col.radius = newSize;
    }


}
