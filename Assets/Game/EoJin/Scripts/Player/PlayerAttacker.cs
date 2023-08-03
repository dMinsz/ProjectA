using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerAttacker : MonoBehaviour
{
    Skill skill;
    DataManager data;
    [SerializeField] bool debug;
    [SerializeField] float control; //플레이어 위치에서 얼마나 떨어진 거리에서 어택 범위 발동할 지
    float rangeAmount;
    float angle;
    bool isAttack = false;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
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

        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, skill.rangeAmount);

        foreach (Collider collider in colliders)
        {
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;

            if (!(collider.tag == "Player"))
                continue;

            if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * Mathf.Deg2Rad))
                continue;

            if (collider.isTrigger == true)
                continue;

            Debug.Log($"{collider.gameObject.name}에게 {skill.skillName}");

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Handles.color = Color.cyan;
        Handles.DrawSolidArc(transform.position - transform.forward * control, transform.up, transform.forward, -angle, rangeAmount);
        Handles.DrawSolidArc(transform.position - transform.forward * control, transform.up, transform.forward, angle, rangeAmount);
    }
}
