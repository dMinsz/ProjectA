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

    [SerializeField] public string skillName;
    [SerializeField] public Sprite image;
    [SerializeField] public string description;

    [SerializeField] public Key key;
    [SerializeField] public float angle; //실제 원하는 각도의 * 0.5로 설정바람
    [SerializeField] public float range;
    [SerializeField] public bool isDash;
    [SerializeField] public float duration;
    [SerializeField] public float coolTime;
    [SerializeField] public AnimationClip skillAnimation;
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