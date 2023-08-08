using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinkSkillUI : BaseUI
{
    [SerializeField] public Character character;
    public TMP_Text qskilCoolTime;       // attacktime
    [SerializeField] public GameObject qskilcolltimeUI;   //q��ų
    [SerializeField] float curtime;
    private bool isqskill; //���߿� ��ų�ʿ��� �޾�
    protected override void Awake()
    {
        base.Awake();
        qskilCoolTime = texts["QskillCoolTime"];
        qskilCoolTime.text = (qskilcolltimeUI.GetComponent<Image>().fillAmount*character.primarySkill.coolTime).ToString("F1");
        //fillamount �ִ밪�� 1.0�̶� ��Ÿ�� ����
        qskilcolltimeUI.SetActive(false);
        isqskill = false;

    }
    private void Update()
    {
        if (!isqskill &&Input.GetKey(KeyCode.Q))
        {
            isqskill=true;//���߿� ��ų�ʿ��� �޾�
           

        }
        if (isqskill == true)
        {
            skillcool();
        }

    }
    private void skillcool()
    {
        StartCoroutine(SkillCoolTime(character.primarySkill.coolTime));
    }

    IEnumerator SkillCoolTime(float cooltime)
    {

        if (qskilcolltimeUI.GetComponent<Image>().fillAmount > 0)
        {
            qskilcolltimeUI.SetActive(true);
            qskilCoolTime.text = (qskilcolltimeUI.GetComponent<Image>().fillAmount * character.primarySkill.coolTime).ToString("F1"); 
           qskilcolltimeUI.GetComponent<Image>().fillAmount -= Time.deltaTime/ character.primarySkill.coolTime;

        }

        yield return new WaitForSeconds(cooltime);
        if (qskilcolltimeUI.GetComponent<Image>().fillAmount <= 0)
        {
            qskilcolltimeUI.GetComponent<Image>().fillAmount = 1f;
            qskilcolltimeUI.SetActive(false);
            isqskill = false;   //���߿� ��ų�ʿ��� �޾�
        }
            
    }

}
