using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinkSkillUI : BaseUI
{
    [SerializeField] public Character character;
    public TMP_Text qskilCoolTime;       // qcooltime
    [SerializeField] public GameObject qskilcolltimeUI;   //q스킬
    [SerializeField] float curtime;
    [SerializeField] public PlayerSkillAttacker playerskill;

    public TMP_Text eskilCoolTime;       // ecooltime
    [SerializeField] public GameObject eskilcolltimeUI;   //e스킬

    public TMP_Text rskilCoolTime;       // rcooltime
    [SerializeField] public GameObject rskilcolltimeUI;   //r스킬



    protected override void Awake()
    {
        base.Awake();
        qskilCoolTime = texts["QskillCoolTime"];
        qskilCoolTime.text = (qskilcolltimeUI.GetComponent<Image>().fillAmount*character.primarySkill.coolTime).ToString("F1");
        //fillamount 최대값이 1.0이라서 쿨타임 곱함
        qskilcolltimeUI.SetActive(false);

        eskilCoolTime = texts["EskillCoolTime"];
        eskilCoolTime.text = (eskilcolltimeUI.GetComponent<Image>().fillAmount * character.secondarySkill.coolTime).ToString("F1");
        eskilcolltimeUI.SetActive(false);

        rskilCoolTime = texts["RskillCoolTime"];
        rskilCoolTime.text = (rskilcolltimeUI.GetComponent<Image>().fillAmount * character.specialSkill.coolTime).ToString("F1");
        rskilcolltimeUI.SetActive(false);

    }
    private void Update()
    {
        if (playerskill.isSkillingPrimary)
        {
            qskillcool();
        }
        if (playerskill.isSkillingSecondary)
        {
            eskillcool();
        }
        if (playerskill.isSkillingSpecial)
        {
            rskillcool();
        }

    }
    private void qskillcool()
    {
        StartCoroutine(qSkillCoolTime(character.primarySkill.coolTime));
    }
    private void eskillcool()
    {
        StartCoroutine(eSkillCoolTime(character.secondarySkill.coolTime));
    }

    private void rskillcool()
    {
        StartCoroutine(rSkillCoolTime(character.secondarySkill.coolTime));
    }

    IEnumerator qSkillCoolTime(float cooltime)
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

    IEnumerator eSkillCoolTime(float cooltime)
    {
        if (eskilcolltimeUI.GetComponent<Image>().fillAmount > 0)
        {
            eskilcolltimeUI.SetActive(true);
            eskilCoolTime.text = (eskilcolltimeUI.GetComponent<Image>().fillAmount * character.secondarySkill.coolTime).ToString("F1");
            eskilcolltimeUI.GetComponent<Image>().fillAmount -= Time.deltaTime / character.secondarySkill.coolTime;

        }

        yield return new WaitForSeconds(cooltime);
        if (eskilcolltimeUI.GetComponent<Image>().fillAmount <= 0)
        {
            eskilcolltimeUI.GetComponent<Image>().fillAmount = 1f;
            eskilcolltimeUI.SetActive(false);

        }
    }
    IEnumerator rSkillCoolTime(float cooltime)
    {
        if (rskilcolltimeUI.GetComponent<Image>().fillAmount > 0)
        {
            rskilcolltimeUI.SetActive(true);
            rskilCoolTime.text = (rskilcolltimeUI.GetComponent<Image>().fillAmount * character.specialSkill.coolTime).ToString("F1");
            rskilcolltimeUI.GetComponent<Image>().fillAmount -= Time.deltaTime / character.specialSkill.coolTime;

        }

        yield return new WaitForSeconds(cooltime);
        if (rskilcolltimeUI.GetComponent<Image>().fillAmount <= 0)
        {
            rskilcolltimeUI.GetComponent<Image>().fillAmount = 1f;
            rskilcolltimeUI.SetActive(false);

        }
    }
}
