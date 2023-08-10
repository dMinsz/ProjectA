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
        Debug.Log($"{gameObject.name}이 {who}에게 {attacker.skill.skillName}을 받음");
        
        Invoke("SetNotDamagedState", damageTime + 0.1f); //0.1f하지 않으면 skill duration과 시간이 딱 맞아 떨어져 duration의 시작과 끝에 한 번씩 총 두 번 damage됨
    }

    public void SetNotDamagedState()
    {
        damaged = false;
    }
}
