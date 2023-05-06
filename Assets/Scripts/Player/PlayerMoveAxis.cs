using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAxis : MonoBehaviour
{
    private Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, cam.eulerAngles.y, 0);
    }
}
