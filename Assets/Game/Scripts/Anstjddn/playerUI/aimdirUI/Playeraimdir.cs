
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playeraimdir : MonoBehaviour
{
    [SerializeField] public PlayerAimTest playeraim;   //쓰고있으신 playeraim 으로 바꾸시면 됩니다 

    private void Update()
    {
        float dirx = (playeraim.mousepos.x - transform.position.x);
        float dirz = (playeraim.mousepos.z - transform.position.z);

        Vector3 dirui = new Vector3(dirx,0,dirz);
        transform.right = dirui;
        
    }
}
