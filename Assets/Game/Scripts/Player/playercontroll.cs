using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using static SoundManager;

public class playercontroll : MonoBehaviourPun
{

    public LayerMask layerMask;
    private Collider coll;
    //�÷��̾� ���� ����
    private Rigidbody playerrb;
    private Vector3 movedir;
    [SerializeField] public float movespeed;

    private Animator anim;
    [SerializeField] PlayerAim playerat;


    public Vector2 playerdir;

    public bool playerdashing = false;



    //�뽬 ���� �Ҷ� ���� ��������־��
    public PlayerSkillAttacker dashskill;

    public Vector3 dashdir;

    public Coroutine mainRoutine;

    private NetWorkedAnimation nAnim;

    private void Awake()
    {
        coll = GetComponent<Collider>();
        playerrb = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();
        nAnim = GetComponent<NetWorkedAnimation>();

        if (GameManager.Data.CurCharacter == null)//debug Mode
        {
            movespeed = 30;
        }
        else 
        {
            movespeed = GameManager.Data.CurCharacter.stat.speed;
        }

    }

    private void Update()
    {
        if (playerdashing)
        {
            return;
        }

        if (!playerat.isattack)
        {
            Move();
            Look();
        }
        else // attack ing
        {
            //�÷��̾� �����Ҷ����� ���̴°� ����
            Vector3 aimpos = new Vector3(playerat.attackdir.x, transform.position.y, playerat.attackdir.z);
            transform.LookAt(aimpos);
            Move();
        }

        if (playerrb.velocity == new Vector3(0, 0, 0))
        {
            nAnim.SendPlayAnimationEvent(photonView.ViewID,"move", "Bool", false);
            //photonView.RPC("animSetBool",RpcTarget.All,"move",false);
            //anim.SetBool("move", false);
        }
        else
        {
            nAnim.SendPlayAnimationEvent(photonView.ViewID, "move", "Bool", true);
            //photonView.RPC("animSetBool", RpcTarget.All, "move", true);
            //anim.SetBool("move", true);
        }
        if (movedir.magnitude == 0)
        {
            return;
        }

        if (dashskill.isskilling)
        {
            transform.LookAt(dashskill.skilldir);
            Move();
        }

    }
    private void Move()
    {

        playerrb.velocity = new Vector3(movedir.z * movespeed, 0, -movedir.x * movespeed);

    }
    private void Look()                     //������ ���� �� �ٶ󺸴� ���� ����
    {
        if (movedir.magnitude == 0)
            return;
        Vector3 viewVector = new Vector3(movedir.z, 0, -movedir.x);
        Quaternion lookrotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(viewVector), 0.1f);
        transform.rotation = lookrotation;
    }

    private void OnMove(InputValue Value)
    {
        if (!playerdashing)
        {
            movedir.x = Value.Get<Vector2>().x;
            movedir.z = Value.Get<Vector2>().y;
        }

    }
    public void Dash()
    {
        playerrb.velocity = Vector3.zero;
        dashdir = playerat.mousepos;
        transform.LookAt(dashdir);

        Vector3 dashDistance = (playerat.mousepos - transform.position); //�Ÿ�
        float dashDistanceSquare = dashDistance.sqrMagnitude; //�Ÿ�����

        if (dashDistanceSquare < dashskill.range * dashskill.range)    //��ų ���� �ȿ� ������ �����̰�
        {
            mainRoutine = StartCoroutine(PlayerSkillRangeDash(playerat.mousepos,1f));

        }
        else                    //��ų���� �ٱ��϶�
        {

            float x = (playerat.mousepos.x - transform.position.x);
            float z = (playerat.mousepos.z - transform.position.z);
            Vector3 dir = new Vector3(x, 0, z).normalized;
            Vector3 destination = transform.position;
            destination += new Vector3(dir.x * dashskill.range, 0, dir.z * dashskill.range);
            mainRoutine = StartCoroutine(PlayerSkillRangeDash(destination, 1f));
        }
    }

    IEnumerator PlayerSkillRangeDash(Vector3 destination, float dashspeed)
    {
        playerdashing = true;
        float distance = Mathf.Abs(Vector3.Distance(transform.position, destination));
        while (distance > 1f)
        {
            distance = Mathf.Abs(Vector3.Distance(transform.position, destination));
            float xspeed = destination.x - transform.position.x;
            float zspeed = destination.z - transform.position.z;
            Vector3 dashdirspeed = new Vector3(xspeed, 0, zspeed).normalized * dashspeed;

            var temp = transform.position + dashdirspeed;

            if (WallCheck(temp) == false)
            {
                transform.position += dashdirspeed;
            }
            else 
            {
                break;
            }
            yield return null;
        }
        if (distance < 1f)
        {
            playerdashing = false;
        }
        playerdashing = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Blocker"))
        {
            playerdashing = false;
            if (mainRoutine != null)
            {
                StopCoroutine(mainRoutine);
            }
        }
        
    }
    private bool WallCheck(Vector3 pos)
    {
        if (Physics.SphereCast(pos, (coll.bounds.extents.x + 0.1f), playerrb.velocity.normalized, out RaycastHit hit, playerrb.velocity.magnitude * Time.fixedDeltaTime, layerMask))
        {
            return true;
        }
        return false;
    }

    [PunRPC]
    public void animSetBool(string name, bool b) 
    {
        anim = GetComponent<Animator>();
        anim.SetBool(name, b);
    }
}
