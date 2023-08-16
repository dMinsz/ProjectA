using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class playercontroll : MonoBehaviour
{

    //�÷��̾� ���� ����
    private Rigidbody playerrb;
    private Vector3 movedir;
    [SerializeField] public float movespeed;

    private Animator anim;
    [SerializeField] PlayerAim playerat;


    public Vector2 playerdir;

    public bool playerdashing = false;



    //�뽬 ���� �Ҷ� ���� ���������־��
    public PlayerSkillAttacker dashskill;

    public Vector3 dashdir;

    private void Awake()
    {
        playerrb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();


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
        else
        {
            //�÷��̾� �����Ҷ����� ���̴°� ����
            Vector3 aimpos = new Vector3(playerat.attackdir.x, transform.position.y, playerat.attackdir.z);
            transform.LookAt(aimpos);
            Move();
        }

          if(playerrb.velocity ==new Vector3(0, 0, 0))
            {
                anim.SetBool("move", false);
            }
            else
            {
                anim.SetBool("move", true);
            }
          if(movedir.magnitude == 0)
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
            StartCoroutine(PlayerSkillRangeDash(playerat.mousepos, 0.05f));

        }
        else                    //��ų���� �ٱ��϶�
        {

            float x = (playerat.mousepos.x - transform.position.x);
            float z = (playerat.mousepos.z - transform.position.z);
            Vector3 dir = new Vector3(x, 0, z).normalized;
            Vector3 destination = transform.position;
            destination += new Vector3(dir.x * dashskill.range, 0, dir.z * dashskill.range);
            StartCoroutine(PlayerSkillRangeDash(destination, 1f));
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
            transform.position += dashdirspeed;
            yield return null;
        }
        if (distance < 1f)
        {
            playerdashing = false;
        }
        playerdashing = false;
    }

}