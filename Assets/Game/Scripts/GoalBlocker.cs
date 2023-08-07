using Photon.Pun;
using System.Collections;
using UnityEngine;

public class GoalBlocker : MonoBehaviourPun
{
    [SerializeField] bool isDebug;
    [SerializeField] GameObject blocker1;
    [SerializeField] GameObject blocker2;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveStopTime;

    private Coroutine moveRoutine;
    private bool movePossible = true;
    private Collider coll;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }
    private void Start()    // Debug용
    {
        if (!isDebug)
            return;

        //moveRoutine = StartCoroutine(MoveRoutine());
    }

    private void OnCollisionEnter(Collision collision)  // Puck 닿았으면 루틴 시작
    {
        if (!movePossible)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            photonView.RPC("MoveBlockerToServer", RpcTarget.AllViaServer);
        }
    }


    [PunRPC]
    public void MoveBlockerToServer() 
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        moveRoutine = StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        movePossible = false;
        float moveTime = 0;

        //yield return new WaitForSeconds(1f);

        while (moveTime < moveStopTime)
        {
            MoveBlocker();
            moveTime += Time.deltaTime;

            yield return null;
        }

        //Destroy(gameObject);
        PhotonNetwork.Destroy(gameObject);

        yield break;
    }

    private void MoveBlocker()
    {
        if (blocker1.transform.position.x > 0)
        {
            blocker1.transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            blocker2.transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
        }
        else
        {
            blocker1.transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            blocker2.transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
        }
    }
}
