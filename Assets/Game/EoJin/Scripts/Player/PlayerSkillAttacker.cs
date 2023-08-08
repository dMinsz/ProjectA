using System.Collections;
using UnityEditor;
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
    public bool isSkilling = false;
    bool isPlayingSkillAnim = false;
    public UnityAction OnPlaySkillAnim;
    public UnityAction OnSkillStart;
    public UnityAction OnSkillEnd;
    public UnityAction<GameObject, float> OnPlayerAttack;
    [SerializeField] anstjddn.PlayerAim aim;
    [SerializeField] public GameObject mousePosObj;
    [SerializeField] public GameObject cubeForLookAt;
    Quaternion lookAtMouse;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        aim = gameObject.GetComponent<anstjddn.PlayerAim>();
    }

    public void Update()
    {
        cubeForLookAt.transform.LookAt(aim.mousepos);
        lookAtMouse = cubeForLookAt.transform.rotation;

        if (isSkilling)
            ApplyDamage();

        mousePosObj.transform.position = aim.mousepos;
    }

    Coroutine ApplyDamageRoutine;

    public void OnPrimarySkill(InputValue value)
    {
        if (!isSkilling) //한 skill이 발동되는 동안 다른 skill을 못 쓰게 막음
        {
            skill = data.CurCharacter.primarySkill;
            aim.attacksize = skill.range;
            isSkilling = true;
            ApplyDamageRoutine = StartCoroutine(skillDuration());
        }

    }

    public void OnSecondarySkill(InputValue value)
    {
        if (!isSkilling)
        {
            skill = data.CurCharacter.secondarySkill;
            aim.attacksize = skill.range;
            isSkilling = true;
            ApplyDamageRoutine = StartCoroutine(skillDuration());
        }

    }

    public void OnSpecailSkill(InputValue value)
    {
        if (!isSkilling)
        {
            skill = data.CurCharacter.specialSkill;
            aim.attacksize = skill.range;
            isSkilling = true;
            ApplyDamageRoutine = StartCoroutine(skillDuration());
        }
    }

    IEnumerator skillDuration()
    {
        yield return new WaitForSeconds(skill.duration);
        isSkilling = false;
        isPlayingSkillAnim = false;
        OnSkillEnd?.Invoke();
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
        Vector3 boxSize = new Vector3(additionalRange, 0.1f, range);

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

            if (collider.gameObject.layer == 7)
            {
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

    private void OnDrawGizmosSelected()
    {
        if (skill == null || !debug || skill.rangeStyle == Skill.RangeStyle.Square)
            return;

        Handles.color = Color.cyan;

        Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, -angle, range);
        Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, angle, range);

    }

    private void OnDrawGizmos()
    {
        //Style Square의 Gizmos의 경우 플레이어의 뒷부분까지 그려지나 실제 Skill 범위는 Gizmos의 반에 플레이어 앞쪽을 향함

        if (skill == null || !debug || skill.rangeStyle != Skill.RangeStyle.Square || skill.rangeStyle != Skill.RangeStyle.Square)
            return;

        Gizmos.color = Color.cyan;
        
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, lookAtMouse, new Vector3(1f, 1f, 1f));
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawCube(Vector3.zero, new Vector3(additionalRange, 0.01f, range * 2f));
    }
}