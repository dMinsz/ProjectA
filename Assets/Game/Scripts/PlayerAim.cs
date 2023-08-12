using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviourPun, IPunInstantiateMagicCallback
{

    //���ù���,�浹�� ���̾�, ���� �̴���
    [SerializeField] public float attacksize;
    [SerializeField] LayerMask layerMask;
    [SerializeField] public float attackpower;
    [SerializeField] public float attackCoolTime;
    [SerializeField] public bool isattack = false;

    [SerializeField] private UnityEvent Attacksound;  //���߿� ���� ����
    private Animator playeranim;
    [HideInInspector] public Vector3 mousepos;

    public GameObject puck;
    private float circleReuslt;

    public Vector3 attackdir;

    private void Awake()
    {
        circleReuslt = Mathf.Pow(attacksize, 2);
        playeranim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetMousePos();
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

        attackdir = mousepos;

        return mousepos;
    }
    /*
    public void Attack() 
    {
        photonView.RPC("PuckAttack", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void PuckAttack() 
    {
        Vector3 dir = attackdir.normalized;
        Vector3 newVelocity = dir * attackpower;

        puck.GetComponent<Puck>().SetPos(newVelocity, puck.transform.position);
    }

    
    private void OnAttack(InputValue Value)
    {
        photonView.RPC("ResultAttack", RpcTarget.AllViaServer, SetMousePos(), transform.position, puck.transform.position);
    }

    [PunRPC]
    private void RequestAttack(Vector3 mousePos, Vector3 playerPos, Vector3 puckPos)
    {
        photonView.RPC("ResultAttack", RpcTarget.AllViaServer, mousePos, playerPos, puckPos);
    }

    [PunRPC]
    private void ResultAttack(Vector3 mousePos, Vector3 playerPos, Vector3 puckPos, PhotonMessageInfo info)
    {
        StartCoroutine(AttackTimeing(mousePos, playerPos, puckPos));
    }
    */


    // ���� ���� ����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attacksize);
    }

    //����Ÿ�̹� ����
    [PunRPC]
    IEnumerator AttackTimeing(Vector3 mousePos, Vector3 playerPos, Vector3 puckPos)//, PhotonMessageInfo info)
    {

        if (circleReuslt >= Mathf.Pow(playerPos.x - puckPos.x, 2) + Mathf.Pow(playerPos.z - puckPos.z, 2)) // ���� �����ȿ� ��ǥ���ִ��� Ȯ�� 
        {

            Vector3 dir = (mousePos - playerPos).normalized;
            Vector3 newVelocity = dir * attackpower;

            puck.GetComponent<Puck>().SetPos(newVelocity, puckPos);//, info);

            isattack = true;
            yield return new WaitForSeconds(attackCoolTime);
            isattack = false;
        }
    }


    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        int puckViewID = (int)instantiationData[0];

        var puckview = PhotonView.Find(puckViewID);

        puck = puckview.gameObject;

    }


}




