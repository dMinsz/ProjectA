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
        //플레이어 좌표를 스크린으로 옮기고
        Vector3 screenPos = Camera.main.WorldToScreenPoint(playerTransform.position + Vector3.up * 2);

        //스크린 좌표로 더하고
        screenPos += Vector3.up * offset;

        //다시 스크린상의 좌표를 월드 좌표로 옮김
        transform.position = Camera.main.ScreenToWorldPoint(screenPos);

       transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    
    }
}
