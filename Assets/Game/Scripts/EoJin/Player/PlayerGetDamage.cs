using UnityEngine;

public class PlayerGetDamage : MonoBehaviour
{
    PlayerSkillAttacker attacker;
    public GameObject whoHurtedYou;
    public bool damaged;

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

    public void GetDamaged(GameObject who, float damageTime)
    {
        damaged = true;

        whoHurtedYou = who;
        attacker = whoHurtedYou.GetComponent<PlayerSkillAttacker>();
        Debug.Log($"{gameObject.name}�� {who}���� {attacker.skill.skillName}�� ����");
        
        Invoke("SetNotDamagedState", damageTime + 0.1f); //0.1f���� ������ skill duration�� �ð��� �� �¾� ������ duration�� ���۰� ���� �� ���� �� �� �� damage��
    }

    public void SetNotDamagedState()
    {
        damaged = false;
    }
}
