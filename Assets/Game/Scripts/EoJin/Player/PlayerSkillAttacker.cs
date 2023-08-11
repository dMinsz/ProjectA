using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerSkillAttacker : MonoBehaviour
{
    /*
     * ������ �ݸ� ����ȭ�� �޽��� ���� ť�긦 �������� �ۼ� �����Ƿ� ������ prefab�� �ִ� ť�꿡 �����ų ��
     */

    public Skill skill;
    DataManager data;
    [SerializeField] bool debug;
    [SerializeField] float control; //�÷��̾� ��ġ���� �󸶳� ������ �Ÿ����� ���� ���� �ߵ��� ��
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
    [SerializeField] PlayerAimTest aim;
    [SerializeField] public GameObject mousePosObj;
    [SerializeField] public GameObject cubeForLookAt;
    public bool makeColliderDetect;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        aim = gameObject.GetComponent<PlayerAimTest>();
    }

    public void Update()
    {
        cubeForLookAt.transform.LookAt(mousePosObj.transform);
        mousePosObj.transform.position = aim.mousepos;
    }

    Coroutine primarySkillCoroutine;
    Coroutine secondarySkillCoroutine;
    Coroutine specialSkillCoroutine;

    public void OnPrimarySkill(InputValue value)
    {
        if (canSkillPrimary) //�� skill�� �ߵ��Ǵ� ���� �ٸ� skill�� �� ���� ����
        {
            canSkillPrimary = false;
            skill = data.CurCharacter.primarySkill;
            aim.attacksize = skill.range;

            ApplyDamage();
            isSkillingPrimary = true;
            primarySkillCoroutine = StartCoroutine(skillDurationPrimary());
        }

    }

    public void OnSecondarySkill(InputValue value)
    {
        if (canSkillSecondary)
        {
            canSkillSecondary = false;
            skill = data.CurCharacter.secondarySkill;
            aim.attacksize = skill.range;

            ApplyDamage();
            isSkillingSecondary = true;
            secondarySkillCoroutine = StartCoroutine(skillDurationSecondary());
        }

    }

    public void OnSpecailSkill(InputValue value)
    {
        if (canSkillSpecial)
        {
            canSkillSpecial = false;
            skill = data.CurCharacter.specialSkill;
            aim.attacksize = skill.range;

            ApplyDamage();
            isSkillingSpecial = true;
            specialSkillCoroutine = StartCoroutine(skillDurationSpecial());
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
        /*
        coolTimeP = 1;
        
        while (skill.coolTime >= coolTimeP)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log($"Primary : {skill.skillName}�� ��Ÿ�� ��� {coolTimeP} / {skill.coolTime}");
            coolTimeP += 1;
        }
        */

        //while�� ���� �� �Ʒ� ���� �ּ� ����� ��
        yield return new WaitForSeconds(skill.coolTime);
        canSkillPrimary = true;
    }

    IEnumerator skillCoolTimeSecondary()
    {
        /*
        coolTimeS = 1;
        
        while (skill.coolTime >= coolTimeS)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log($"Secondary : {skill.skillName}�� ��Ÿ�� ��� {coolTimeS} / {skill.coolTime}");
            coolTimeS += 1;
        }
        */

        //while�� ���� �� �Ʒ� ���� �ּ� ����� ��
        yield return new WaitForSeconds(skill.coolTime);
        canSkillSecondary = true;
    }

    IEnumerator skillCoolTimeSpecial()
    {
        /*
        coolTimeSP = 1;
        
        while (skill.coolTime >= coolTimeSP)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log($"Special : {skill.skillName}�� ��Ÿ�� ��� {coolTimeSP} / {skill.coolTime}");
            coolTimeSP += 1;
        }
        */

        //while�� ���� �� �Ʒ� ���� �ּ� ����� ��
        yield return new WaitForSeconds(skill.coolTime);
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
        Vector3 boxSize = new Vector3(additionalRange * 0.5f, 0.1f, range);
        //OverlapBox���� half boxSize�� ���ϱ� ������ �ݾ� ���� ����)additionalRange, 0.1f, range * 2

        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize, cubeForLookAt.transform.rotation);
        makeColliderDetect = true;
        StopAllCoroutines();
        makeColliderRoutine = StartCoroutine(MakeColliderCoroutine());
        DetectObjectsCollider(colliders);
    }

    private void OnDrawGizmos()
    {
        //Style Square�� Gizmos�� ��� �÷��̾��� �޺κб��� �׷����� ���� Skill ������ Gizmos�� �ݿ� �÷��̾� ������ ����

        if (skill == null || !debug || skill.rangeStyle != Skill.RangeStyle.Square || skill.rangeStyle != Skill.RangeStyle.Square)
            return;

        Gizmos.color = Color.cyan;

        Vector3 boxSize = new Vector3(additionalRange, 0.1f, range * 2);

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, cubeForLookAt.transform.rotation, new Vector3(1f, 1f, 1f));
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawCube(new Vector3(0f, 0f, 0f), boxSize);
    }

    private void MakeSkillRangeSectorForm()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, skill.range);
        makeColliderDetect = true;
        StopAllCoroutines();
        makeColliderRoutine = StartCoroutine(MakeColliderCoroutine());
        DetectObjectsCollider(colliders);
    }

    private void OnDrawGizmosSelected()
    {
        if (skill == null || !debug || skill.rangeStyle == Skill.RangeStyle.Square)
            return;

        Handles.color = Color.cyan;

        Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, -angle, range);
        Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, angle, range);

    }

    Coroutine makeColliderRoutine;
    IEnumerator MakeColliderCoroutine()
    {
        yield return new WaitForSeconds(skill.duration);
        Debug.Log("Collider Detect off");
        makeColliderDetect = false;
    }

    private void DetectObjectsCollider(Collider[] colliders)
    {
        if (makeColliderDetect)
        {
            foreach (Collider collider in colliders)
            {
                Vector3 playerNMouse = (aim.mousepos - transform.position).normalized;
                Vector3 colliderPosButYIsZero = new Vector3(collider.transform.position.x, 0f, collider.transform.position.z);
                Vector3 playerNTarget = (colliderPosButYIsZero - transform.position).normalized;

                if (collider.gameObject.layer == 7)
                {
                    Debug.Log("���� �ݶ��̴� ���� ���� ����");
                    continue;
                }

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


    }


}