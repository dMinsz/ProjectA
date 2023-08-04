using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
namespace anstjddn
{
    public class PlayerAim : MonoBehaviourPun
    {
        [SerializeField] GameObject mouse;
        //���ù���,�浹�� ���̾�, ���� �̴���
        [SerializeField] public float attacksize;
        [SerializeField] LayerMask ball;
        [SerializeField] public float attackpower;
        [SerializeField] public float attackCoolTime;
        [SerializeField]public bool isattack;

        [SerializeField] private UnityEvent Attacksound;  //���߿� ���� ����

        private Vector3 mousepos;

        private void Start()
        {
            isattack = false;
        }

        private void Update()
        {
        }

        private Vector3 SetMousePos() 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                mousepos = hit.point;
                mousepos.y = 0;
            }

            return mousepos;
        }

        private void OnAttack(InputValue Value)
        {
            photonView.RPC("RequestAttack", RpcTarget.MasterClient, SetMousePos());
        }

        [PunRPC]
        private void RequestAttack(Vector3 mousePos) 
        {
           
            photonView.RPC("ResultAttack", RpcTarget.AllViaServer, transform.position, mousePos);
        }

        [PunRPC]
        private void ResultAttack(Vector3 position,Vector3 mousePos, PhotonMessageInfo info)
        {
            StartCoroutine(AttackTimeing(position,mousePos,  info));
        }


        // ���� ���� ����
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attacksize);
        }

        //����Ÿ�̹� ����
        [PunRPC]
        IEnumerator AttackTimeing(Vector3 position,Vector3 mousePos, PhotonMessageInfo info)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attacksize, ball);
            foreach (Collider collider in colliders)
            {
                if (isattack == false && collider.gameObject.layer == 7)                //���̾� 7���� ball�� ����
                {

                    isattack = true;
                    //Vector3 dir = (mousepos - transform.position).normalized;
                    
                    Vector3 dir = (mousePos - position).normalized;

                    Vector3 newVelocity = dir * attackpower;
                    //collider.GetComponent<Rigidbody>().velocity = dir * attackpower;

                    collider.GetComponent<Puck>().SetPos(position, newVelocity, info);


                  //  Attacksound?.Invoke();
                    yield return new WaitForSeconds(attackCoolTime);
                    isattack = false;
                }
                //�÷��̾� ���� ���� �ʿ�
               
            }
        }
    }
}
