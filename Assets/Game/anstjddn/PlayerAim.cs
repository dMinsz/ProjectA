using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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

        public GameObject puck;

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
            photonView.RPC("RequestAttack", RpcTarget.MasterClient, SetMousePos(), transform.position , puck.transform.position);
        }

        [PunRPC]
        private void RequestAttack(Vector3 mousePos , Vector3 playerPos, Vector3 puckPos) 
        {
           
            photonView.RPC("ResultAttack", RpcTarget.AllViaServer, mousePos , playerPos, puckPos);
        }

        [PunRPC]
        private void ResultAttack(Vector3 mousePos,Vector3 playerPos, Vector3 puckPos, PhotonMessageInfo info)
        {
            StartCoroutine(AttackTimeing(mousePos, playerPos , puckPos,  info));
        }


        // ���� ���� ����
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attacksize);
        }

        //����Ÿ�̹� ����
        [PunRPC]
        IEnumerator AttackTimeing(Vector3 mousePos, Vector3 playerPos, Vector3 puckPos, PhotonMessageInfo info)
        {

            if (Mathf.Pow(attacksize,2) >= Mathf.Pow(playerPos.x - puckPos.x,2) + Mathf.Pow(playerPos.z - puckPos.z,2) ) // ���� �����ȿ� ��ǥ���ִ��� Ȯ�� 
            {
                isattack = true;

                Vector3 dir = (mousePos - playerPos).normalized;
                Vector3 newVelocity = dir * attackpower;
                //collider.GetComponent<Rigidbody>().velocity = dir * attackpower;

                puck.GetComponent<Puck>().SetPos(newVelocity, info);

                yield return new WaitForSeconds(attackCoolTime);
                isattack = false;
            }

            //Collider[] colliders = Physics.OverlapSphere(playerPos, attacksize, layerMask);
            //foreach (Collider collider in colliders)
            //{
            //    if (isattack == false && collider.gameObject.layer == 7)                //���̾� 7���� ball�� ����
            //    {

            //        isattack = true;
            //        //Vector3 dir = (mousepos - transform.position).normalized;
                    
            //        Vector3 dir = (mousePos - playerPos).normalized;

            //        Vector3 newVelocity = dir * attackpower;
            //        //collider.GetComponent<Rigidbody>().velocity = dir * attackpower;

            //        collider.GetComponent<Puck>().SetPos(newVelocity, info);


            //      //  Attacksound?.Invoke();
            //        yield return new WaitForSeconds(attackCoolTime);
            //        isattack = false;
            //    }
            //    //�÷��̾� ���� ���� �ʿ�
               
            //}
        }
    }


}
