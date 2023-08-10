
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Object/CharacterData")]
public class Character : ScriptableObject
{
    [SerializeField] public string characterName; //dataManager���� �� �̸����� ĳ���� ��ȯ
    [SerializeField] public Sprite Image; //dataManager���� �� �̸����� ĳ���� ��ȯ
    //[SerializeField] public Avatar avatar; //���� �̹� �÷��̾ ���� �ְ� �����Ƿ� �ƹ�Ÿ�� ��������
    //[SerializeField] public AnimatorController animator;

    //[SerializeField] public Stat stat;
    //[SerializeField] public Skill primarySkill; //���콺 ��Ŭ��
    //[SerializeField] public Skill secondarySkill; //�����̽���
    //[SerializeField] public Skill specialSkill; //R

    public void Init()
    {
        //stat.Init(stat);
        //primarySkill.Init(primarySkill);
        //secondarySkill.Init(secondarySkill);
        //specialSkill.Init(specialSkill);
    }
}