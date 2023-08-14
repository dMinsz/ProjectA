using Photon.Pun;
using UnityEngine;

public class PlayerGetDamage : MonoBehaviourPun
{
    PlayerSkillAttacker attacker;
    GameObject whoHurtedYou;
    public bool damaged;
    private PlayerState playerState;

    public void Awake()
    {
        playerState = GetComponent<PlayerState>();
    }

    //public void OnEnable()
    //{
    //    if (attacker != null)
    //        attacker.OnPlayerAttack += GetDamaged;
    //}

    //public void OnDisable()
    //{
    //    if (attacker != null)
    //        attacker.OnPlayerAttack -= GetDamaged;
    //}

    public void GetDamaged(GameObject who, float damageTime, int damage)
    {
        damaged = false;

        whoHurtedYou = who;
        attacker = whoHurtedYou.GetComponent<PlayerSkillAttacker>();
        Debug.Log($"{gameObject.name}�� {who}���� {attacker.skill.skillName}�� ����");

        //object[] damagedata = new object[] { damage };
        photonView.RPC("Damaged",RpcTarget.AllViaServer, damage);
        //playerState.playercurhp -= damage;


        //Invoke("SetNotDamagedState", damageTime + 0.1f); //0.1f���� ������ skill duration�� �ð��� �� �¾� ������ duration�� ���۰� ���� �� ���� �� �� �� damage��
    }

    public void SetNotDamagedState()
    {
        damaged = false;
    }


    [PunRPC]
    public void Damaged(int damage) 
    {
        playerState.playercurhp -= damage;
    }

}
