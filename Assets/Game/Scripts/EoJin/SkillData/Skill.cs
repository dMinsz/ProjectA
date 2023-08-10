using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData")]
public class Skill : ScriptableObject
{
    public enum Key
    {
        Primary,
        Secondary,
        Special
    }

    public enum RangeStyle
    {
        Arc,
        Circle,
        Square
        //,Object //-> 두부의 스킬(통나무가 굴러감) 여유있을 때 구현
    }

    [SerializeField] public string skillName;
    [SerializeField] public Sprite image;
    [SerializeField] public string description;

    [SerializeField] public Key key;
    [SerializeField] public RangeStyle rangeStyle;
    [SerializeField] public float angle; //실제 원하는 각도의 * 0.5로 설정바람
    //angle의 경우 Style이 Circle일 경우 자동으로 180으로 설정되고, Square일 경우 쓰이지 않음
    [SerializeField] public float range;
    [SerializeField] public float additionalRange; //Style이 Squre일 경우 가로범위로 사용 됨
    [SerializeField] public bool isDash;
    [SerializeField] public float duration;
    [SerializeField] public float coolTime;
    [SerializeField] public int playerKnockback;
    [SerializeField] public int coreKnockback;

    public void Init(Skill skill)
    {
        skill.skillName = skillName;

        skill.key = key;
        skill.range = range;
        skill.angle = angle;
        skill.isDash = isDash;
        skill.coolTime = coolTime;

        skill.playerKnockback = playerKnockback;
        skill.coreKnockback = coreKnockback;
    }

    public void Use(Skill skill)
    {

    }


}