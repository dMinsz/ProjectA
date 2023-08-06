using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviourPun
{
    [SerializeField] bool isDebug = false;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float maxSpeed;
    Vector3 velocityMaxSpeed;

    private Rigidbody rb;
    private Renderer ren;

    Coroutine mainRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ren = GetComponent<MeshRenderer>();

        velocityMaxSpeed = new Vector3(maxSpeed, 0, maxSpeed);
    }

    private void OnEnable()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
                mainRoutine = StartCoroutine(WallCheckRoutine());
        }
        else
        {
            Debug.Log("Puck has no master server");
            mainRoutine = StartCoroutine(WallCheckRoutine());
        }
    }

    private void OnDisable()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
                if (mainRoutine != null)
                    StopCoroutine(mainRoutine);
        }
        else
        {
            if (mainRoutine != null)
                StopCoroutine(mainRoutine);
        }
    }

    private void WallCheck()
    {
        if (Physics.SphereCast(transform.position, (ren.bounds.extents.x + 0.1f), rb.velocity.normalized, out RaycastHit hit, rb.velocity.magnitude * Time.fixedDeltaTime, layerMask))
        {
            rb.velocity = Vector3.Reflect(rb.velocity, hit.normal);
        }
    }

    IEnumerator WallCheckRoutine()
    {
        Debug.Log("StartCoroutine");
        while (true)
        {
            yield return new WaitForFixedUpdate();
            WallCheck();
            LimitSpeed();
        }
    }

    private void LimitSpeed()
    {
        if (rb.velocity.x > velocityMaxSpeed.x)
        {
            rb.velocity = new Vector3(velocityMaxSpeed.x, rb.velocity.y, rb.velocity.z);
        }
        if (rb.velocity.x < (velocityMaxSpeed.x * -1))
        {
            rb.velocity = new Vector3((velocityMaxSpeed.x * -1), rb.velocity.y, rb.velocity.z);
        }

        if (rb.velocity.z > velocityMaxSpeed.z)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, velocityMaxSpeed.z);
        }
        if (rb.velocity.z < (velocityMaxSpeed.z * -1))
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, (velocityMaxSpeed.z * -1));
        }
    }

    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            if (ren == null)
            {
                ren = GetComponent<MeshRenderer>();
            }
            Gizmos.DrawWireSphere(transform.position, (ren.bounds.extents.x + 0.1f));
        }
    }


    public void SetPos(Vector3 newvelocity,Vector3 puckPos) //, PhotonMessageInfo info)
    {
        //transform.position = puckPos;
        rb.velocity = newvelocity; 
    }

}
