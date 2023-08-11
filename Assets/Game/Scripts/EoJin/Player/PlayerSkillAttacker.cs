using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerSkillAttacker : MonoBehaviour
{
    /*
     * 윗면의 반만 투명화된 메쉬를 입힌 큐브를 기준으로 작성 됐으므로 무조건 prefab에 있는 큐브에 적용시킬 것
     */

    public Skill skill;
    DataManager data;
    [SerializeField] bool debug;
    [SerializeField] float control; //플레이어 위치에서 얼마나 떨어진 거리에서 어택 범위 발동할 지
    float range;
    float additionalRange;
    public float angle;
    public bool isSkillingPrimary = false;
    public bool isSkillingSecondary = false;
    public bool isSkillingSpecial = false;
    public bool canSkillPrimary = true;
    public bool canSkillSecondary = true;
    public bool canSkillSpecial = true;
    bool isPlayingSkillAnim = false;
    public UnityAction OnPlaySkillAnim;
    public UnityAction OnSkillStart;
    public UnityAction<GameObject, float> OnPlayerAttack;
    [SerializeField] PlayerAim aim;
    [SerializeField] public GameObject mousePosObj;
    [SerializeField] public GameObject cubeForLookAt;
    Quaternion lookAtMouse;
    float time;
    float coolTimeP;
    float coolTimeS;
    float coolTimeSP;

    private bool draw;

    [SerializeField] DrawSkillRange DrawRange;
    private Animator anim;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        aim = gameObject.GetComponent<PlayerAim>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        //cubeForLookAt.transform.LookAt(aim.mousepos);
        //lookAtMouse = cubeForLookAt.transform.rotation;

        //mousePosObj.transform.position = aim.mousepos;
    }

    Coroutine primarySkillCoroutine;
    Coroutine secondarySkillCoroutine;
    Coroutine specialSkillCoroutine;


    bool isDubleClick = false;

    public void OnPrimarySkill(InputValue value)
    {

        if (canSkillPrimary) //한 skill이 발동되는 동안 다른 skill을 못 쓰게 막음
        {

            skill = data.CurCharacter.primarySkill;
            aim.attacksize = skill.range;


            DrawRange.SetIsDrawingTrue();

            if (isDubleClick)
            {
                anim.SetTrigger("Primary");
                canSkillPrimary = false;
                isSkillingPrimary = true;
                ApplyDamage();

                isDubleClick = false;
                DrawRange.SetIsDrawingFalse();
                primarySkillCoroutine = StartCoroutine(skillCoolTimePrimary());
            }
            else 
            {
                isDubleClick = true;
            }

        }

    }

    public void OnSecondarySkill(InputValue value)
    {

        if (canSkillSecondary) //한 skill이 발동되는 동안 다른 skill을 못 쓰게 막음
        {

            skill = data.CurCharacter.secondarySkill;
            aim.attacksize = skill.range;


            DrawRange.SetIsDrawingTrue();

            if (isDubleClick)
            {
                anim.SetTrigger("Secondary");
                canSkillSecondary = false;
                isSkillingSecondary = true;
                ApplyDamage();

                isDubleClick = false;

                DrawRange.SetIsDrawingFalse();
                secondarySkillCoroutine = StartCoroutine(skillCoolTimeSecondary());
            }
            else
            {
                isDubleClick = true;
            }

        }
    }

    public void OnSpecailSkill(InputValue value)
    {
        if (canSkillSpecial) //한 skill이 발동되는 동안 다른 skill을 못 쓰게 막음
        {

            skill = data.CurCharacter.specialSkill;
            aim.attacksize = skill.range;


            DrawRange.SetIsDrawingTrue();

            if (isDubleClick)
            {

                anim.SetTrigger("Special");
                canSkillSpecial = false;
                isSkillingSpecial = true;
                ApplyDamage();
                isDubleClick = false;

                DrawRange.SetIsDrawingFalse();
                specialSkillCoroutine = StartCoroutine(skillCoolTimeSpecial());
            }
            else
            {
                isDubleClick = true;
            }

        }

    }

    IEnumerator skillDurationPrimary()
    {
        
        yield return new WaitForSeconds(skill.duration);
        isSkillingPrimary = false;
        isPlayingSkillAnim = false;

        primarySkillCoroutine = StartCoroutine(skillCoolTimePrimary());
    }

    IEnumerator skillDurationSecondary()
    {
        yield return new WaitForSeconds(skill.duration);
        isSkillingSecondary = false;
        isPlayingSkillAnim = false;

        secondarySkillCoroutine = StartCoroutine(skillCoolTimeSecondary());
    }

    IEnumerator skillDurationSpecial()
    {
        yield return new WaitForSeconds(skill.duration);
        isSkillingSpecial = false;
        isPlayingSkillAnim = false;

        specialSkillCoroutine = StartCoroutine(skillCoolTimeSpecial());
    }

    IEnumerator skillCoolTimePrimary()
    {
        /*   coolTimeP = 1;
           while (skill.coolTime >= coolTimeP)
           {
               yield return new WaitForSeconds(1f);
               Debug.Log($"Primary : {skill.skillName}의 쿨타임 대기 {coolTimeP} / {skill.coolTime}");
               coolTimeP += 1;
           }*/
        yield return new WaitForSeconds(skill.coolTime);

        //while문 삭제 후 아래 문장 주석 취소할 것
        //yield return new WaitForSeconds(skill.coolTime);
        isSkillingPrimary = false;
        canSkillPrimary = true;
    }

    IEnumerator skillCoolTimeSecondary()
    {
        /*  coolTimeS = 1;
          while (skill.coolTime >= coolTimeS)
          {
              yield return new WaitForSeconds(1f);
              Debug.Log($"Secondary : {skill.skillName}의 쿨타임 대기 {coolTimeS} / {skill.coolTime}");
              coolTimeS += 1;
          }
        */

        yield return new WaitForSeconds(skill.coolTime);
        //while문 삭제 후 아래 문장 주석 취소할 것
        //yield return new WaitForSeconds(skill.coolTime);
        isSkillingSecondary =false;
        canSkillSecondary = true;
    }

    IEnumerator skillCoolTimeSpecial()
    {
        /* coolTimeSP = 1;
         while (skill.coolTime >= coolTimeSP)
         {
             yield return new WaitForSeconds(1f);
             Debug.Log($"Special : {skill.skillName}의 쿨타임 대기 {coolTimeSP} / {skill.coolTime}");
             coolTimeSP += 1;
         }*/
        yield return new WaitForSeconds(skill.coolTime);

        //while문 삭제 후 아래 문장 주석 취소할 것
        //yield return new WaitForSeconds(skill.coolTime);
        isSkillingSpecial =false;
        canSkillSpecial = true;
    }

    public void ApplyDamage()
    {
        OnSkillStart?.Invoke();

        if (skill == null)
            return;

        if (!isPlayingSkillAnim)
        {
            OnPlaySkillAnim?.Invoke();
            isPlayingSkillAnim = true;
        }

        angle = skill.angle;
        range = skill.range;
        additionalRange = skill.additionalRange;

        if (skill.rangeStyle == Skill.RangeStyle.Square)
            MakeSkillRangeSquareForm();
        else
            MakeSkillRangeSectorForm();

    }

    private void MakeSkillRangeSquareForm()
    {
        float angle = Vector3.Angle(transform.position, aim.mousepos);
        //additionalRange에 float를 곱해주며 스킬범위 민감도 설정가능
        Vector3 boxSize = new Vector3(additionalRange * 0.5f, 0.1f, range);

        Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, boxSize, lookAtMouse);
        DetectObjectsCollider(colliders);
    }


    private void MakeSkillRangeSectorForm()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, skill.range);
        DetectObjectsCollider(colliders);
    }

    private void DetectObjectsCollider(Collider[] colliders)
    {
        
        foreach (Collider collider in colliders)
        {
            Vector3 playerNMouse = (aim.mousepos - transform.position).normalized;
            Vector3 colliderPosButYIsZero = new Vector3(collider.transform.position.x, 0f, collider.transform.position.z);
            Vector3 playerNTarget = (colliderPosButYIsZero - transform.position).normalized;

            if (skill.rangeStyle == Skill.RangeStyle.Square)
                angle = 90f;

            if (Vector3.Dot(playerNMouse, playerNTarget) < Mathf.Cos(angle * Mathf.Deg2Rad))
                continue;

            if (collider.gameObject == this.gameObject)
                continue;

            if (collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                Debug.Log("범위내에 있음");
                aim.Attack();
                continue;
            }

            if (collider.gameObject.tag == "Player")
            {
                if (collider.gameObject.GetComponent<PlayerGetDamage>().damaged == false)
                {
                    collider.gameObject.GetComponent<PlayerGetDamage>().GetDamaged(this.gameObject, skill.duration);
                }
            }
        }
        

    }

}