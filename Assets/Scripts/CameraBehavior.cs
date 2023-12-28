using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraBehavior : MonoBehaviour
{

    public Transform target;
    public GameObject platform;

    float rotationValue;
    float rotationRadians;
    public Quaternion rotation = new Quaternion();
    public Vector3 position = new Vector3(0, 15, 0);
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetAxis("Camera")>0)
        {
            transform.RotateAround(target.transform.position, Vector3.up, -50 * Time.deltaTime);
        }
        else if (Input.GetAxis("Camera")<0)
        {
            transform.RotateAround(target.transform.position, Vector3.up, 50 * Time.deltaTime);
        }
    }
}
