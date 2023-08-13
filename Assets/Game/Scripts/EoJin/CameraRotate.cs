using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] Camera thiscamera;

    void Update()
    {
        if (thiscamera.enabled == true)
            transform.RotateAround(transform.position, new Vector3(0, 1, 0), 0.01f);
    }
}
