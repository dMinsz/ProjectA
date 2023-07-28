using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Object/CharacterData")]
public class Character : ScriptableObject
{
    [SerializeField] public string characterName;
    [SerializeField] public Avatar avatar;

    [SerializeField] public Stat stat;
    [SerializeField] public Skill primarySkill;
    [SerializeField] public Skill secondarySkill;
    [SerializeField] public Skill specialSkill;

    public void Init()
    {
        stat.Init(stat);
        primarySkill.Init(primarySkill);
        secondarySkill.Init(secondarySkill);
        specialSkill.Init(specialSkill);
    }
}
