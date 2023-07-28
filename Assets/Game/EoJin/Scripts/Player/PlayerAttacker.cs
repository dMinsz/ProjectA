using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    Skill skill;


    public void OnPrimarySkill(InputValue value)
    {
        skill = gameObject.GetComponent<Character>().primarySkill;
        ApplyDamage(skill);
    }

    public void OnSecondarySkill (InputValue value)
    {
        skill = gameObject.GetComponent<Character>().secondarySkill;
        ApplyDamage(skill);
    }

    public void OnSpecailSkill(InputValue value)
    {
        skill = gameObject.GetComponent<Character>().specialSkill;
        ApplyDamage(skill);
    }

    public void ApplyDamage(Skill skill)
    {
        float angle;

        if (skill.range == Skill.Range.EveryWhere)
            angle = 180;
        else
            angle = 15;

        Collider[] colliders = Physics.OverlapSphere(transform.position, angle);

        foreach (Collider collider in colliders)
        {
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;

            if (!(collider.tag == "Player"))
                continue;

            if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * Mathf.Deg2Rad))
                continue;

            Debug.Log(collider.gameObject.name);

        }
    }
}
