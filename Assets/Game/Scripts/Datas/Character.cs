using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Object/CharacterData")]
public class Character : ScriptableObject
{
    [SerializeField] public string characterName; //dataManager���� �� �̸����� ĳ���� ��ȯ
    [SerializeField] public Sprite Image; //dataManager���� �� �̸����� ĳ���� ��ȯ
    [SerializeField] public Avatar avatar; //���� �̹� �÷��̾ ���� �ְ� �����Ƿ� �ƹ�Ÿ�� ��������
    [SerializeField] public RuntimeAnimatorController runtimeAnimator;
    [SerializeField] public Effect skillEffect;
    [SerializeField] public bool hasFemaleVoice;
    [SerializeField] public GameObject modeling;//For Looby
    [SerializeField] public Stat stat;
    [SerializeField] public Sprite attackUIImage;
    [SerializeField] public Skill primarySkill; //���콺 ��Ŭ��
    [SerializeField] public Skill secondarySkill; //�����̽���
    [SerializeField] public Skill specialSkill; //R

}
