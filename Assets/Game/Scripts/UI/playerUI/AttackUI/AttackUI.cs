using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackUI : BaseUI
{


    [SerializeField] PlayerAim player;          //�÷��̾� attacktime�������� ����
    public TMP_Text playerAttackCoolTime;       // attacktime
    [SerializeField] GameObject mouseattackcolltimeUI;
    [SerializeField] public Image attackUIImage;
    protected override void Awake()
    {
        base.Awake();


        attackUIImage.sprite = GameManager.Data.CurCharacter.attackUIImage;

        playerAttackCoolTime = texts["playerAttackCoolTime"];           //�ؽ�Ʈ �ް�

        playerAttackCoolTime.text = mouseattackcolltimeUI.GetComponent<Image>().fillAmount.ToString("F1");

        mouseattackcolltimeUI.SetActive(false);             //��Ÿ�ӵ��� ��Ӱ� �ð����ư��� ���ӿ�����Ʈ

        mouseattackcolltimeUI.GetComponent<Image>().fillAmount = player.attackCoolTime;        //�÷��̾� ���� ���ư��°� �ð�ȭ;
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
            StartCoroutine(cooltime(player.attackCoolTime));
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
            mouseattackcolltimeUI.GetComponent<Image>().fillAmount = player.attackCoolTime;
            mouseattackcolltimeUI.SetActive(false);
        }

    }
 
}
