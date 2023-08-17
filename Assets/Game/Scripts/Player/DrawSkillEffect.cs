using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.TextCore.Text;

public class DrawSkillEffect : MonoBehaviourPun
{
    PlayerSkillAttacker skillAttacker;
    List<GameObject> effects = new List<GameObject>();
    //[SerializeField] GameObject cubeForLookAt;
    Vector3 startPos;
    Vector3 destination;
    PlayerAim aim;

    private void Awake()
    {
        skillAttacker = GetComponent<PlayerSkillAttacker>();
        aim = GetComponent<PlayerAim>();
    }

    private void FixedUpdate()
    {
        if (effects.Count > 0)
        {
            Vector3 playerNInst = startPos - effects[10].transform.position;
            Vector3 instVecButYIsZero = new Vector3(playerNInst.x, 0f, playerNInst.z);

            //2.5f더한 이유는 조금 오바되는 게 시각적으로 좋을 것 같아서 이펙트 속도 0.3기준
            //if (Mathf.Abs(instVecButYIsZero.x) > Mathf.Abs(destination.x) + 4f || Mathf.Abs(instVecButYIsZero.z) > Mathf.Abs(destination.z) + 4f)

            float additionalTime = 0f;

            if (PhotonNetwork.LocalPlayer.GetCharacterName() == "None") // for debug
            {
                additionalTime = GameManager.Data.GetCharacter("Mario").skillEffect.additionalTime;
            }
            else 
            {
                additionalTime = GameManager.Data.GetCharacter(PhotonNetwork.LocalPlayer.GetCharacterName()).skillEffect.additionalTime;
            }
            if (Mathf.Abs(instVecButYIsZero.x) > Mathf.Abs(destination.x) + additionalTime || Mathf.Abs(instVecButYIsZero.z) > Mathf.Abs(destination.z) + additionalTime)
                DestroyAllEffects();
        }
    }

    private void DestroyAllEffects()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            Destroy(effects[i]);
        }
        effects.Clear();
    }

    //public void OnEnable()
    //{
    //    skillAttacker.OnSkillStart += EffectStart;
    //}

    //public void OnDisable()
    //{
    //    skillAttacker.OnSkillStart -= EffectStart;
    //}
    public void EffectStart(int skillnum, Vector3 mousePos,string CharacterName) // 0 == primary, 1 == secondory, 2 == special skill
    {
        //object[] skilldata = new object[] { skill.skillName };
        photonView.RPC("RequestEffectStart", RpcTarget.AllViaServer, skillnum, mousePos, CharacterName);
    }

    [PunRPC]
    private void RequestEffectStart(int skillnum, Vector3 mousePos, string CharacterName)
    {
        Skill skill;
        Character character;
        if (CharacterName == "None")
        {//forDebug
            character = GameManager.Data.GetCharacter("Mario");
        }
        else 
        {
            character = GameManager.Data.GetCharacter(CharacterName);
        }
        skill = character.primarySkill;
        switch (skillnum)
        {
            case 0:
                skill = character.primarySkill;
                break;
            case 1:
                skill = character.secondarySkill;
                break;
            case 2:
                skill = character.specialSkill;
                break;
            default:
                break;
        }


        startPos = transform.position;
        Vector3 playerNmouse = (startPos - mousePos).normalized;

        destination = playerNmouse * skill.range;

        Vector3 dir = (transform.position - mousePos).normalized;
        var effectRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));

        for (int i = -10; i <= 10; i++)
        {
            float angle = skill.angle * (0.1f * i);


            //GameManager.Data.CurCharacter.skillEffect.effectPrefab
            GameObject instance = Instantiate(character.skillEffect.effectPrefab, transform.position, effectRotation * Quaternion.Euler(0f, angle, 0f));

            effects.Add(instance);
        }
    }

}