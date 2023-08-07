using anstjddn;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class AttackUI : BaseUI
{


    [SerializeField] PlayerAim player;          //플레이어 attacktime받을려고 적음
    public TMP_Text playerAttackCoolTime;       // attacktime
    [SerializeField] GameObject mouseattackcolltimeUI;
    protected override void Awake()
    {
        base.Awake();
   
        playerAttackCoolTime = texts["playerAttackCoolTime"];           //텍스트 받고

        playerAttackCoolTime.text = mouseattackcolltimeUI.GetComponent<Image>().fillAmount.ToString("F1");

        mouseattackcolltimeUI.SetActive(false);             //쿨타임돌때 어둡고 시간돌아가는 게임오브젝트

       mouseattackcolltimeUI.GetComponent<Image>().fillAmount = player.attacktime;        //플레이어 공속 돌아가는거 시각화;
    }

 
    private void Update()
    {
        if (player.isattack)
        {
            startcooltime();
        }
 
    }
    private void startcooltime()
    {
            StartCoroutine(cooltime(player.attacktime));
    }

    IEnumerator cooltime(float time)
    {



        if (mouseattackcolltimeUI.GetComponent<Image>().fillAmount > 0)
        {
            playerAttackCoolTime.text = mouseattackcolltimeUI.GetComponent<Image>().fillAmount.ToString("F1");
            mouseattackcolltimeUI.SetActive(true);
            mouseattackcolltimeUI.GetComponent<Image>().fillAmount -= Time.deltaTime;
        }
        
        yield return new WaitForSeconds(time);
        if (mouseattackcolltimeUI.GetComponent<Image>().fillAmount <= 0)
        {
            mouseattackcolltimeUI.GetComponent<Image>().fillAmount = player.attacktime;
            mouseattackcolltimeUI.SetActive(false);
        }

    }
 
}
