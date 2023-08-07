using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerSkillAttacker : MonoBehaviour
{
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
    [SerializeField] PlayerAim aim;
    [SerializeField] public GameObject mousePosObj;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        aim = gameObject.GetComponent<PlayerAim>();
    }

    public void Update()
    {
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

    public void OnSecondarySkill (InputValue value)
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
        float angle = Vector3.SignedAngle(transform.position, (aim.mousepos - transform.position), Vector3.up) + 153f;
        Vector3 boxSize = new Vector3(additionalRange, 0.1f, range);

        Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, boxSize, Quaternion.Euler(0f, angle, 0f));
        DetectObjectsInCollider(colliders);
    }

    private void MakeSkillRangeSectorForm()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, skill.range);
        DetectObjectsInCollider(colliders);
    }

    private void DetectObjectsInCollider(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            Vector3 playerNMouse = (transform.position - aim.mousepos).normalized;
            Vector3 playerNTarget = (collider.transform.position - transform.position).normalized;

            if (Vector3.Dot(-playerNMouse, playerNTarget) < Mathf.Cos(angle * Mathf.Deg2Rad))
                continue;

            if (collider.isTrigger == true)
                continue;

            if (collider.gameObject.layer == 7)
            {
                Debug.Log($"{collider.gameObject.name}에게 Attack");
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
        if (!debug)
            return;

        if (skill.rangeStyle == Skill.RangeStyle.Square)
            return;

        Handles.color = Color.cyan;

        Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, -angle, range);
        Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, angle, range);
        
    }

    private void OnDrawGizmos()
    {
        //Style Square의 Gizmos의 경우 플레이어의 뒷부분까지 그려지나 실제 Skill 범위는 Gizmos의 반에 플레이어 앞쪽을 향함

        if (!debug)
            return;

        if (skill.rangeStyle != Skill.RangeStyle.Square)
            return;

        Gizmos.color = Color.cyan;

        float angle = Vector3.SignedAngle(transform.position, (aim.mousepos - transform.position), Vector3.up) + 150f;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0f, angle, 0f), new Vector3(1f, 1f, 1f));
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawCube(Vector3.zero, new Vector3(additionalRange, 0.01f, range * 2f));
    }
}
