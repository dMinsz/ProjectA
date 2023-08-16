using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Object/CharacterData")]
public class Character : ScriptableObject
{
    [SerializeField] public string characterName; //dataManager에서 이 이름으로 캐릭터 전환
    [SerializeField] public Sprite Image; //dataManager에서 이 이름으로 캐릭터 전환
    [SerializeField] public Avatar avatar; //모델은 이미 플레이어가 갖고 있게 했으므로 아바타만 변경해줌
    [SerializeField] public RuntimeAnimatorController runtimeAnimator;
    [SerializeField] public Effect skillEffect;
    [SerializeField] public bool hasFemaleVoice;
    [SerializeField] public GameObject modeling;//For Looby
    [SerializeField] public Stat stat;
    [SerializeField] public Sprite attackUIImage;
    [SerializeField] public Skill primarySkill; //마우스 우클릭
    [SerializeField] public Skill secondarySkill; //스페이스바
    [SerializeField] public Skill specialSkill; //R

}
