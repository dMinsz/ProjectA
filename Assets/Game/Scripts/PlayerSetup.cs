using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
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

    //private int playerTeam;

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
        //int playerNumber = photonView.Owner.GetPlayerNumber();

        //if (playerColor == null || playerColor.Count <= playerNumber)
        //    return;

        if (team == 0)
        {
            surface.material.color = playerColor[0];
            floorMarkImg.color = playerColor[0];
        }
        else
        {
            surface.material.color = playerColor[team - 1];
            floorMarkImg.color = playerColor[team - 1];
        }
        
    }
}


