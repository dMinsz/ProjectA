using System;
using TMPro;
using UnityEngine;

public class cameraname : BaseUI
{
    public TMP_Text playername;
    public string nickname;

    [SerializeField] Transform playerTransform;
    [SerializeField] int offset;

    protected override void Awake()
    {
        base.Awake();
        playername = texts["Playername"];
        //if (GameManager.Data.CurCharacter == null)
        //{
        //    playername.text = "Debug";
        //}
        //else 
        //{
        //    playername.text = GameManager.Data.CurCharacter.characterName;
        //}
    }
    
    private void LateUpdate()
    {
        //�÷��̾� ��ǥ�� ��ũ������ �ű��
        Vector3 screenPos = Camera.main.WorldToScreenPoint(playerTransform.position + Vector3.up * 2);

        //��ũ�� ��ǥ�� ���ϰ�
        screenPos += Vector3.up * offset;

        //�ٽ� ��ũ������ ��ǥ�� ���� ��ǥ�� �ű�
        transform.position = Camera.main.ScreenToWorldPoint(screenPos);

       transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    
    }
}
