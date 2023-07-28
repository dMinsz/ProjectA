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

    public enum Range
    {
        OneDirection,
        EveryWhere
    }

    [SerializeField] public string skillName;
    [SerializeField] public Sprite image;
    [SerializeField] public string description;

    [SerializeField] public Key key;
    [SerializeField] public Range range;
    [SerializeField] public float rangeAmount;
    [SerializeField] public bool isDash;
    [SerializeField] public float coolTime;

    [SerializeField] public int playerKnockback;
    [SerializeField] public int coreKnockback;

    public void Init(Skill skill)
    {
        skill.skillName = skillName;

        skill.key = key;
        skill.range = range;
        skill.rangeAmount = rangeAmount;
        skill.isDash = isDash;
        skill.coolTime = coolTime;

        skill.playerKnockback = playerKnockback;
        skill.coreKnockback = coreKnockback;
    }

    public void Use(Skill skill)
    {
        
    }


}