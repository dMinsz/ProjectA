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
    float rangeAmount;
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
        skill = data.CurCharacter.primarySkill;
        aim.attacksize = skill.rangeAmount;
        isSkilling = true;
        ApplyDamageRoutine = StartCoroutine(skillDuration());
    }

    public void OnSecondarySkill (InputValue value)
    {
        skill = data.CurCharacter.secondarySkill;
        aim.attacksize = skill.rangeAmount;
        isSkilling = true;
        ApplyDamageRoutine = StartCoroutine(skillDuration());
    }

    public void OnSpecailSkill(InputValue value)
    {
        skill = data.CurCharacter.specialSkill;
        aim.attacksize = skill.rangeAmount;
        isSkilling = true;
        ApplyDamageRoutine = StartCoroutine(skillDuration());
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

        if (skill.range == Skill.Range.Circle)
            angle = 180;
        else if (skill.range == Skill.Range.OneDirection)
            angle = 15;
        else
            angle = 60;

        rangeAmount = skill.rangeAmount;

        
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, skill.rangeAmount);

        foreach (Collider collider in colliders)
        {
            Vector3 playerNMouse = (transform.position - aim.mousepos).normalized;
            Vector3 playerNTarget = (collider.transform.position - transform.position).normalized;
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;

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
                if (collider.GetComponent<PlayerGetDamage>().damaged == false)
                {
                    collider.GetComponent<PlayerGetDamage>().GetDamaged(this.gameObject, skill.duration);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Handles.color = Color.cyan;
        Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, -angle, rangeAmount);
        Handles.DrawSolidArc(transform.position, Vector3.up, (aim.mousepos - transform.position).normalized, angle, rangeAmount);
    }

}
