using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercontroll : MonoBehaviour
{

    //플레이어 무브 관련
    private Rigidbody playerrb;
    private Vector3 movedir;
    [SerializeField] private float movespeed;


    //플레이어 어택관련
    [SerializeField] float attacksize;
    [SerializeField] LayerMask ball;
    [SerializeField] float attackpower;
         private Vector3 aimpos;
    [SerializeField] Camera main;

    private void Awake()
    {
        playerrb = GetComponent<Rigidbody>();  
    }

    private void Update()
    {
        Move();
 
    }
    private void Move()
    {
        // transform.Translate(-movedir.x * movespeed * Time.deltaTime, 0, -movedir.z * movespeed * Time.deltaTime);
        playerrb.velocity = new Vector3(-movedir.x * movespeed, 0, -movedir.z * movespeed);

    }

    private void OnMove(InputValue Value)
    {
        movedir.x = Value.Get<Vector2>().x;
        movedir.z = Value.Get<Vector2>().y;
    }
    private void OnAttack(InputValue Value)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attacksize, ball);
         foreach (Collider collider in colliders)
        {
            
            Vector3 dir = transform.position - collider.transform.position;
            collider.GetComponent<Rigidbody>().velocity = dir * -attackpower;


            //플레이어 공격 구현 필요
        }
        Debug.Log("좌클릭");
    }

    private void OnPoint(InputValue Value)
    {
        aimpos.x = Value.Get<Vector2>().x;
        aimpos.z = Value.Get<Vector2>().y;
      
    }


    // 어택 범위 설정
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attacksize);
    }
}
