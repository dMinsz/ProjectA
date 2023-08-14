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

    //플레이어 무브 관련
    private Rigidbody playerrb;
    private Vector3 movedir;
    [SerializeField] public float movespeed;

    private Animator anim;
    [SerializeField] PlayerAim playerat;


    public Vector2 playerdir;




    //대쉬 어택 할때 조건 받을려고넣어둠
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
            //플레이어 어택할때마다 숙이는거 수정
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

        //대시 스킬중에 움직이거나 바라보는 방향 안바뀌게끔 조정
        if (dashskill.isSkillingSpecial && !playerat.isattack)
        {
            transform.LookAt(dashdir);

        }

    }
    private void Move()
    {

        playerrb.velocity = new Vector3(movedir.z * movespeed, 0, -movedir.x * movespeed);

    }
    private void Look()                     //기존에 무비에 던 바라보는 방향 수정
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
        Vector3 mos = (playerat.mousepos - transform.position); //거리
        float dismos = mos.sqrMagnitude; //거리제곱

        if (dismos < dashskill.range * dashskill.range)    //스킬 범위 안에 있을때 움직이게
        {                                                         //일단 플레이어y 좌표값에 따라 적용되게끔 적음, y좌표 0이면 y좌표에 0넣으면됨
            Vector3 targetpos = new Vector3(playerat.mousepos.x, transform.position.y, playerat.mousepos.z);

            transform.position = new Vector3(playerat.mousepos.x, transform.position.y, playerat.mousepos.z);

        }
        else                    //마우스 포인트가 스킬범위 밖에 있고 스킬을 사용하면 마우스가 향하는 방향이면서 스킬끝범위로 나가게끔
        {

            float x = (playerat.mousepos.x - transform.position.x);
            float z = playerat.mousepos.z - transform.position.z;
            Vector3 dir = new Vector3(x, 0, z).normalized;
            transform.position += new Vector3(dir.x * dashskill.range, 0, dir.z * dashskill.range);
        }

        // playerat.mousepos //마우스 위치
        // dashskill.range //레인지
    }
}
