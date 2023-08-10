using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.InputSystem;
namespace anstjddn
{


    public class playercontroll : MonoBehaviour
    {
   
        //플레이어 무브 관련
        private Rigidbody playerrb;
        private Vector3 movedir;
        [SerializeField] public float movespeed;

        private Animator anim;




        [SerializeField] PlayerAimTest playerat;

        private void Awake()
        {
            playerrb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
  
        }

        private void Update()                          
        {
            if (!playerat.isattack)
            {
                Move();
                Look();
            }
            else                                       
            {
                transform.LookAt(playerat.attackdir);
                Move();
            }

          if(playerrb.velocity ==new Vector3(0, 0, 0))
            {
                anim.SetBool("move", false);
            }
            else
            {
                anim.SetBool("move", true);
            }
          if(movedir.magnitude == 0)
            {
                return;
            }
          
        
        }
        private void Move()
        {
          
            playerrb.velocity = new Vector3(movedir.z* movespeed, 0, -movedir.x * movespeed);

        }
        private void Look()                     //기존에 무비에 던 바라보는 방향 수정
        {
            if (movedir.magnitude == 0)
                return;
            Vector3 viewVector = new Vector3(movedir.z, 0, -movedir.x);
            Quaternion lookrotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(viewVector), 0.1f);
            transform.rotation = lookrotation;
        }

        private void OnMove(InputValue Value)
        {
            movedir.x = Value.Get<Vector2>().x;
            movedir.z = Value.Get<Vector2>().y;
        }
       
    }
}
