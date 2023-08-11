using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playeraimdir : MonoBehaviour
{
    [SerializeField] public PlayerAimTest playeraim;
    
    private void Update()
    {
        Vector3 dirui = (playeraim.mousepos - transform.position).normalized;
        transform.right = dirui;
    //    Vector3 aimpos = new Vector3(dirui.x, transform.position.y, dirui.z);
      //  transform.LookAt(aimpos);
    }
}
