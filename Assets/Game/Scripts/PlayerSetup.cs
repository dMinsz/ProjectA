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

    private int team;

    private PlayerInput input;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        team = PhotonNetwork.LocalPlayer.GetTeamColor();
      
        SetPlayerColor();

        if (!photonView.IsMine)
        {
            floorMark.gameObject.SetActive(false);
            AttackRangeMark.gameObject.SetActive(false);
            Destroy(input);
        }
    }


    private void SetPlayerColor()
    {
        int playerNumber = photonView.Owner.GetPlayerNumber();

        if (playerColor == null || playerColor.Count <= playerNumber)
            return;

        surface.material.color = playerColor[team-1];
        floorMarkImg.color = playerColor[team-1];
       
    }

}


