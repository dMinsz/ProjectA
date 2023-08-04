using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using anstjddn;
using Unity.VisualScripting;

public class PlayerAttacker : MonoBehaviour
{
    public Skill skill;
    DataManager data;
    [SerializeField] bool debug;
    [SerializeField] float control; //플레이어 위치에서 얼마나 떨어진 거리에서 어택 범위 발동할 지
    float rangeAmount;
    float angle;
    bool isAttack = false;
    public UnityAction OnPlaySkillAnim;
    PlayerAim aim;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        aim = gameObject.GetComponent<PlayerAim>();
    }

    public void Update()
    {
        if (isAttack)
            ApplyDamage();
    }

    Coroutine ApplyDamageRoutine;

    public void OnPrimarySkill(InputValue value)
    {
        skill = data.CurCharacter.primarySkill;
        isAttack = true;
        ApplyDamageRoutine = StartCoroutine(skillDuration());
    }

    public void OnSecondarySkill (InputValue value)
    {
        skill = data.CurCharacter.secondarySkill;
        isAttack = true;
        ApplyDamageRoutine = StartCoroutine(skillDuration());
    }

    public void OnSpecailSkill(InputValue value)
    {
        skill = data.CurCharacter.specialSkill;
        isAttack = true;
        ApplyDamageRoutine = StartCoroutine(skillDuration());
    }

    IEnumerator skillDuration()
    {
        OnPlaySkillAnim?.Invoke();
        yield return new WaitForSeconds(skill.duration);
        isAttack = false;
    }

    public void ApplyDamage()
    {
        if (skill == null)
            return;


        if (skill.range == Skill.Range.Circle)
            angle = 180;
        else if (skill.range == Skill.Range.OneDirection)
            angle = 15;
        else
            angle = 60;

        rangeAmount = skill.rangeAmount;

        Vector3 playerNMouse = (transform.position - aim.mousepos).normalized;
        
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, skill.rangeAmount);

        foreach (Collider collider in colliders)
        {
            Vector3 playerNTarget = (collider.transform.position - transform.position).normalized;

            if (!(collider.tag == "Player"))
                continue;


            if (Vector3.Dot(transform.position, playerNTarget) < Mathf.Cos(angle * Mathf.Deg2Rad))
                continue;

            if (collider.isTrigger == true)
                continue;

            if (collider.gameObject.layer == 7)
            {

                aim.Attack();
                Debug.Log($"{collider.gameObject.name}에게 PlayerAim.Attack");
                return;
            }

            Debug.Log($"{collider.gameObject.name}에게 {skill.skillName}");

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Handles.color = Color.cyan;
        Handles.DrawSolidArc(transform.position - transform.forward * control, transform.up, aim.mousepos, -angle, rangeAmount);
        Handles.DrawSolidArc(transform.position - transform.forward * control, transform.up, aim.mousepos, angle, rangeAmount);
    }
}
