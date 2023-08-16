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
        Debug.Log($"{gameObject.name}이 {who}에게 {attacker.skill.skillName}을 받음");

        //object[] damagedata = new object[] { damage };
        photonView.RPC("Damaged",RpcTarget.AllViaServer, damage);
        //playerState.playercurhp -= damage;


        //Invoke("SetNotDamagedState", damageTime + 0.1f); //0.1f하지 않으면 skill duration과 시간이 딱 맞아 떨어져 duration의 시작과 끝에 한 번씩 총 두 번 damage됨
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
