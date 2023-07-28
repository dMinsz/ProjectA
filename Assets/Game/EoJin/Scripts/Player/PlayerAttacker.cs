using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerAttacker : MonoBehaviour
{
    Skill skill;
    DataManager data;

    float range;
    float angle;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
    }

    public void OnPrimarySkill(InputValue value)
    {
        skill = data.CurCharacter.primarySkill;
        ApplyDamage(skill);
    }

    public void OnSecondarySkill (InputValue value)
    {
        skill = data.CurCharacter.secondarySkill;
        ApplyDamage(skill);
    }

    public void OnSpecailSkill(InputValue value)
    {
        skill = data.CurCharacter.specialSkill;
        ApplyDamage(skill);
    }

    public void ApplyDamage(Skill skill)
    {
        float angle;

        if (skill.range == Skill.Range.EveryWhere)
            angle = 180;
        else
            angle = 15;

        Collider[] colliders = Physics.OverlapSphere(transform.position, skill.rangeAmount);

        foreach (Collider collider in colliders)
        {
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;

            if (collider.tag != "Player")
                continue;

            if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * Mathf.Deg2Rad))
                continue;

            Debug.Log(collider.gameObject.name);

        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);
        Debug.DrawRay(transform.position, rightDir * range, Color.blue);
        Debug.DrawRay(transform.position, leftDir * range, Color.blue);
    }
}
