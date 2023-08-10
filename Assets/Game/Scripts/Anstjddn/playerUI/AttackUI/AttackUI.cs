using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackUI : BaseUI
{


    [SerializeField] PlayerAimTest player;          //�÷��̾� attacktime�������� ����
    public TMP_Text playerAttackCoolTime;       // attacktime
    [SerializeField] GameObject mouseattackcolltimeUI;
    protected override void Awake()
    {
        base.Awake();
   
        playerAttackCoolTime = texts["playerAttackCoolTime"];           //�ؽ�Ʈ �ް�

        playerAttackCoolTime.text = mouseattackcolltimeUI.GetComponent<Image>().fillAmount.ToString("F1");

        mouseattackcolltimeUI.SetActive(false);             //��Ÿ�ӵ��� ��Ӱ� �ð����ư��� ���ӿ�����Ʈ

       mouseattackcolltimeUI.GetComponent<Image>().fillAmount = player.attacktime;        //�÷��̾� ���� ���ư��°� �ð�ȭ;
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
