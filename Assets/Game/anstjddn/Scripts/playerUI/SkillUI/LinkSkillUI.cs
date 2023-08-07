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
    [SerializeField] public GameObject qskilcolltimeUI;   //q스킬
    [SerializeField] float curtime;
    protected override void Awake()
    {
        base.Awake();
        qskilCoolTime = texts["QskillCoolTime"];
        qskilCoolTime.text = (qskilcolltimeUI.GetComponent<Image>().fillAmount*character.primarySkill.coolTime).ToString("F1");
        //fillamount 최대값이 1.0이라서 쿨타임 곱함
        qskilcolltimeUI.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            StartCoroutine(SkillCoolTime(character.primarySkill.coolTime));

        }
       
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
        }
            
    }

}
