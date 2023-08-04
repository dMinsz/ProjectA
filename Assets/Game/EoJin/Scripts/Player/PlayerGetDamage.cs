using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGetDamage : MonoBehaviour
{
    PlayerAttacker attacker;

    public void Awake()
    {
    }

    public void OnEnable()
    {
        if (attacker != null)
            attacker.OnPlayerAttack += GetDamaged;
    }

    public void OnDisable()
    {
        if (attacker != null)
            attacker.OnPlayerAttack -= GetDamaged;
    }

    public void GetDamaged(GameObject who)
    {
        attacker = who.GetComponent<PlayerAttacker>();
        Debug.Log($"{gameObject.name}이 {who}에게 {attacker.skill.skillName}을 받음");
    }
}
