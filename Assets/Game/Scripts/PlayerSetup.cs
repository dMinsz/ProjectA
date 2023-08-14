using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviourPun
{
    public cameraHpbar hpSlider;
    [SerializeField] Transform floorMark;
    [SerializeField] Image floorMarkImg;
    [SerializeField] List<Color> playerColor;
    //[SerializeField] Renderer surface;
    [SerializeField] Transform AttackRangeMark;

    [HideInInspector]public int playerTeam;

    [HideInInspector] public Vector3 originPos;
    [HideInInspector] public Quaternion originRot;

    //string NickName;
    //public cameraname name;
    // PlayerState.character
    // animator ¼³Á¤ , avatarµµ

    private PlayerInput input;

    public cameraname cameraName;
    public TMP_Text nickName;

    private void Awake()
    {
        nickName = cameraName.playername;
        input = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
        {
            floorMark.gameObject.SetActive(false);
            AttackRangeMark.gameObject.SetActive(false);
            Destroy(input);
        }
    }

    //[PunRPC]
    //public void SentServerColor(PlayerEntry.TeamColor color) 
    //{
    //    photonView.RPC("RequestSetPlayerColor", RpcTarget.MasterClient, (int)color);
    //}

    //[PunRPC]
    //private void RequestSetPlayerColor(int team)
    //{
    //    photonView.RPC("ResultSetPlayerColor", RpcTarget.AllViaServer, team);
    //}

    //[PunRPC]
    //private void ResultSetPlayerColor(int team)
    //{
    //    SetPlayerColor(team);
    //}


    //[PunRPC]
    //public void SetPlayerColor(int team)
    //{

    //    playerTeam = team;
    //    floorMarkImg.color = playerColor[team];
        
    //    originPos = transform.position;
    //    originRot = transform.rotation;

    //    hpSlider.hpbarcolor.color = playerColor[team];
    //}




    [PunRPC]
    public void SentSetUp(PlayerEntry.TeamColor color, string nickName)
    {
        photonView.RPC("RequestSetUp", RpcTarget.MasterClient, (int)color ,nickName);
    }

    [PunRPC]
    private void RequestSetUp(int color ,string nickName)
    {
        photonView.RPC("ResultSetup", RpcTarget.AllViaServer, color ,nickName);
    }

    [PunRPC]
    private void ResultSetup(int color,string nickName)
    {
       SetUp(color,nickName);
    }


    [PunRPC]
    public void SetUp(int color ,string _nickName)
    {
        playerTeam = color;
        floorMarkImg.color = playerColor[color];

        originPos = transform.position;
        originRot = transform.rotation;

        hpSlider.hpbarcolor.color = playerColor[color];

        nickName.text = _nickName;
        nickName.color = playerColor[color];
    }

}


