using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
namespace anstjddn
{
    public class playercontroll : MonoBehaviour
    {

        //�÷��̾� ���� ����
        private Rigidbody playerrb;
        private Vector3 movedir;
        [SerializeField] private float movespeed;

        private CharacterController contol;

        private Animator anim;

        private void Awake()
        {
            contol = GetComponent<CharacterController>();
            playerrb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            //   transform.rotation = Quaternion.Euler(0, -90, 0);
        }

        private void Update()
        {
            Move();

            if (playerrb.velocity == new Vector3(0, 0, 0))
            {
                anim.SetBool("move", false);
            }
            else
            {
                anim.SetBool("move", true);
            }
            if (movedir.magnitude == 0)
            {
                return;
            }

        }
        private void Move()
        {
            playerrb.velocity = new Vector3(movedir.z * movespeed, 0, -movedir.x * movespeed);
            if (movedir.magnitude == 0)
                return;

            Vector3 look = new Vector3(movedir.z, 0, -movedir.x);
            Quaternion lookrotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(look), 0.1f);
            transform.rotation = lookrotation;

        }

        private void OnMove(InputValue Value)
        {
            movedir.x = Value.Get<Vector2>().x;
            movedir.z = Value.Get<Vector2>().y;
        }

    }
}
