using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] LayerMask ball;
        [SerializeField] public float attackpower;
        [SerializeField] public float attacktime;
        [SerializeField]public bool isattack;


      


       [SerializeField] private UnityEvent Attacksound;  //���߿� ���� ����

        private Vector3 mousepos;

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
            Debug.Log(mousepos);
       
        }
        private void OnAttack(InputValue Value)
        {

            Attack();
        }

        private void Attack()
        {
            StartCoroutine(AttackTimeing(attacktime));
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
            Collider[] colliders = Physics.OverlapSphere(transform.position, attacksize, ball);
            foreach (Collider collider in colliders)
            {
                if (isattack == false && collider.gameObject.layer == 7)                //���̾� 7���� ball�� ����
                {

                    isattack = true;
                    Vector3 dir = (mousepos - transform.position).normalized;
                   
                    collider.GetComponent<Rigidbody>().velocity = dir * attackpower;
                  //  Attacksound?.Invoke();
                    yield return new WaitForSeconds(attacktime);
                    isattack = false;
                }
                //�÷��̾� ���� ���� �ʿ�
               
            }
        }
    }
}
