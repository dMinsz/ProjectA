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
        //,Object //-> �κ��� ��ų(�볪���� ������) �������� �� ����
    }

    [SerializeField] public string skillName;
    [SerializeField] public Sprite image;
    [SerializeField] public string description;

    [SerializeField] public Key key;
    [SerializeField] public RangeStyle rangeStyle;
    [SerializeField] public float angle; //���� ���ϴ� ������ * 0.5�� �����ٶ�
    //angle�� ��� Style�� Circle�� ��� �ڵ����� 180���� �����ǰ�, Square�� ��� ������ ����
    [SerializeField] public float range;
    [SerializeField] public float additionalRange; //Style�� Squre�� ��� ���ι����� ��� ��
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