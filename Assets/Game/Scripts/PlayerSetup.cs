using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] Transform floorMark;
    [SerializeField] Image floorMarkImg;
    [SerializeField] List<Color> playerColor;
    [SerializeField] Renderer surface;
    [SerializeField] Transform AttackRangeMark;

    [HideInInspector]public int playerTeam;

    [HideInInspector] public Vector3 originPos;
    [HideInInspector] public Quaternion originRot;


    private PlayerInput input;

    private void Awake()
    {
    
        input = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
        {
            floorMark.gameObject.SetActive(false);
            AttackRangeMark.gameObject.SetActive(false);
            Destroy(input);
        }
    }

    [PunRPC]
    public void SentServerColor() 
    {
        photonView.RPC("RequestSetPlayerColor", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.GetTeamColor());
    }

    [PunRPC]
    private void RequestSetPlayerColor(int team)
    {
        photonView.RPC("ResultSetPlayerColor", RpcTarget.AllViaServer, team);
    }

    [PunRPC]
    private void ResultSetPlayerColor(int team)
    {
        SetPlayerColor(team);
    }


    [PunRPC]
    public void SetPlayerColor(int team)
    {

        if (team == 0)
        {
            playerTeam = 0;
            surface.material.color = playerColor[0];
            floorMarkImg.color = playerColor[0];
        }
        else
        {
            playerTeam = team;
            surface.material.color = playerColor[team - 1];
            floorMarkImg.color = playerColor[team - 1];
        }

        originPos = transform.position;
        originRot = transform.rotation;
    }
}


