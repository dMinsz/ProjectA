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
        [SerializeField] private float movespeed;


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

            playerrb.velocity = new Vector3(-movedir.x * movespeed, 0, -movedir.z * movespeed);

        }

        private void OnMove(InputValue Value)
        {
            movedir.x = Value.Get<Vector2>().x;
            movedir.z = Value.Get<Vector2>().y;
        }
       
    }
}
