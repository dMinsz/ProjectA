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
        [SerializeField] LayerMask layerMask;
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
           
            photonView.RPC("ResultAttack", RpcTarget.AllViaServer, mousePos , transform.position);
        }

        [PunRPC]
        private void ResultAttack(Vector3 mousePos,Vector3 playerPos , PhotonMessageInfo info)
        {
            StartCoroutine(AttackTimeing(mousePos, playerPos,  info));
        }


        // ���� ���� ����
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attacksize);
        }

        //����Ÿ�̹� ����
        [PunRPC]
        IEnumerator AttackTimeing(Vector3 mousePos, Vector3 playerPos, PhotonMessageInfo info)
        {
            Collider[] colliders = Physics.OverlapSphere(playerPos, attacksize, layerMask);
            foreach (Collider collider in colliders)
            {
                if (isattack == false && collider.gameObject.layer == 7)                //���̾� 7���� ball�� ����
                {

                    isattack = true;
                    //Vector3 dir = (mousepos - transform.position).normalized;
                    
                    Vector3 dir = (mousePos - playerPos).normalized;

                    Vector3 newVelocity = dir * attackpower;
                    //collider.GetComponent<Rigidbody>().velocity = dir * attackpower;

                    collider.GetComponent<Puck>().SetPos(newVelocity, info);


                  //  Attacksound?.Invoke();
                    yield return new WaitForSeconds(attackCoolTime);
                    isattack = false;
                }
                //�÷��̾� ���� ���� �ʿ�
               
            }
        }
    }


}
