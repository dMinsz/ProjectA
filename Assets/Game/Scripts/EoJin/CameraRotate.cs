using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0, 1, 0), 0.01f);
    }
}
