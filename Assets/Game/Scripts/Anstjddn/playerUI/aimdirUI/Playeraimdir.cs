
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playeraimdir : MonoBehaviour
{
    [SerializeField] public PlayerAimTest playeraim;   //���������� playeraim ���� �ٲٽø� �˴ϴ� 

    private void Update()
    {
        float dirx = (playeraim.mousepos.x - transform.position.x);
        float dirz = (playeraim.mousepos.z - transform.position.z);

        Vector3 dirui = new Vector3(dirx,0,dirz);
        transform.right = dirui;
        
    }
}
