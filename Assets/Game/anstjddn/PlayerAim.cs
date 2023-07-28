using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] GameObject aimobj;


    private Vector3 aimpos;
    private void Update()
    {
        aimobj.transform.position = new Vector3(aimpos.x,0,aimpos.z);
    }


    private void OnPoint(InputValue Value)
    {
        aimpos.x = Value.Get<Vector2>().x;
        aimpos.z = Value.Get<Vector2>().y;
        aimpos = Camera.main.ScreenToWorldPoint(new Vector3(aimpos.x, 0, aimpos.z));
    }
}
