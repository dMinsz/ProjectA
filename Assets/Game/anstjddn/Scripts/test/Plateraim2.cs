using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Plateraim2 : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    [SerializeField] GameObject mouse;
    //어택범위,충돌할 레이어, 공을 미는힘
    [SerializeField] public float attacksize;
    [SerializeField] LayerMask layerMask;
    [SerializeField] public float attackpower;
    [SerializeField] public float attackCoolTime;
    [SerializeField] public bool isattack;

    [SerializeField] private UnityEvent Attacksound;  //나중에 어택 사운드



    //추가
    private Animator playeranim;



    private Vector3 mousepos;

    public GameObject puck;
    private float circleReuslt;

    private void Awake()
    {
        circleReuslt = Mathf.Pow(attacksize, 2);

        //추가
        playeranim = GetComponent<Animator>();
    }

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



    // 어택 범위 설정
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attacksize);
    }

    //어택타이밍 구현
    [PunRPC]
    IEnumerator AttackTimeing(Vector3 mousePos, Vector3 playerPos, Vector3 puckPos)//, PhotonMessageInfo info)
    {

        if (circleReuslt >= Mathf.Pow(playerPos.x - puckPos.x, 2) + Mathf.Pow(playerPos.z - puckPos.z, 2)) // 원의 범위안에 좌표가있는지 확인 
        {

            Vector3 dir = (mousePos - playerPos).normalized;
            Vector3 newVelocity = dir * attackpower;

           // puck.GetComponent<Puck>().SetPos(newVelocity, puckPos);//, info);

            isattack = true;
            //추가
            playeranim.SetTrigger("attack");
            Vector3 anim = mousePos.normalized;
            transform.LookAt(anim);


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