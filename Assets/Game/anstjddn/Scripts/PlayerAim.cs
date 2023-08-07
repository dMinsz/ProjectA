using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
namespace anstjddn
{
    public class PlayerAim : MonoBehaviour
    {

      


        //���ù���,�浹�� ���̾�, ���� �̴���
        [SerializeField] public float attacksize;
     //   [SerializeField] LayerMask ball;
        [SerializeField] public float attackpower;
        [SerializeField] public float attacktime;
        [SerializeField]public bool isattack;

        private Animator playeranim;
      


       [SerializeField] private UnityEvent Attacksound;  //���߿� ���� ����

        public Vector3 mousepos;
        private void Awake()
        {
            playeranim = GetComponent<Animator>();  
        }
        private void Start()
        {
            isattack = false;                 
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                mousepos = hit.point;
                mousepos.y = 0;
            }
         
       
        }
        private void OnAttack(InputValue Value)
        {
              Attack();

        }

        public void Attack()
        {
            if (!isattack)
            {
                StartCoroutine(AttackTimeing(attacktime));
            }

         
        }


        // ���� ���� ����
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attacksize);
        }

        //����Ÿ�̹� ����
        IEnumerator AttackTimeing(float attacktime)
        {
          
                isattack = true;
             playeranim.SetTrigger("attack");
                Collider[] colliders = Physics.OverlapSphere(transform.position, attacksize);
                foreach (Collider collider in colliders)
                {
                    if (isattack == true && collider.gameObject.layer == 7)                //���̾� 7���� ball�� ����
                    {

                        // isattack = true;
                        Vector3 dir = (mousepos - transform.position).normalized;

                        collider.GetComponent<Rigidbody>().velocity = dir * attackpower;
                        //  Attacksound?.Invoke();
                      
                    }
                }
            yield return new WaitForSeconds(attacktime);
            isattack = false;
        }
 
        }
    }

