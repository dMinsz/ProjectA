using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class playercontroll : MonoBehaviour
{

    //�÷��̾� ���� ����
    private Rigidbody playerrb;
    private Vector3 movedir;
    [SerializeField] public float movespeed;

    private Animator anim;
    [SerializeField] PlayerAim playerat;


    public Vector2 playerdir;




    //�뽬 ���� �Ҷ� ���� ��������־��
    public PlayerSkillAttacker dashskill;

    public Vector3 dashdir;

    private void Awake()
    {
        playerrb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();


    }

    private void Update()
    {
        if (!playerat.isattack && dashskill.isSkillingSpecial)
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

        if (playerrb.velocity == new Vector3(0, 0, 0))
        {
            anim.SetBool("move", false);
        }
        else
        {
            anim.SetBool("move", true);
        }
        if (movedir.magnitude == 0)
        {
            return;
        }

        //��� ��ų�߿� �����̰ų� �ٶ󺸴� ���� �ȹٲ�Բ� ����
        if (dashskill.isSkillingSpecial && !playerat.isattack)
        {
            transform.LookAt(dashdir);

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
        if (!dashskill.isSkillingSpecial)
        {
            movedir.x = Value.Get<Vector2>().x;
            movedir.z = Value.Get<Vector2>().y;
        }
        //   movedir.x = Value.Get<Vector2>().x;
        //   movedir.z = Value.Get<Vector2>().y;
    }
    public void Dash()
    {
        dashdir = playerat.mousepos;
        transform.LookAt(dashdir);
        Vector3 mos = (playerat.mousepos - transform.position); //�Ÿ�
        float dismos = mos.sqrMagnitude; //�Ÿ�����

        if (dismos < dashskill.range * dashskill.range)    //��ų ���� �ȿ� ������ �����̰�
        {                                                         //�ϴ� �÷��̾�y ��ǥ���� ���� ����ǰԲ� ����, y��ǥ 0�̸� y��ǥ�� 0�������
            Vector3 targetpos = new Vector3(playerat.mousepos.x, transform.position.y, playerat.mousepos.z);

            transform.position = new Vector3(playerat.mousepos.x, transform.position.y, playerat.mousepos.z);

        }
        else                    //���콺 ����Ʈ�� ��ų���� �ۿ� �ְ� ��ų�� ����ϸ� ���콺�� ���ϴ� �����̸鼭 ��ų�������� �����Բ�
        {

            float x = (playerat.mousepos.x - transform.position.x);
            float z = playerat.mousepos.z - transform.position.z;
            Vector3 dir = new Vector3(x, 0, z).normalized;
            transform.position += new Vector3(dir.x * dashskill.range, 0, dir.z * dashskill.range);
        }

        // playerat.mousepos //���콺 ��ġ
        // dashskill.range //������
    }
}
