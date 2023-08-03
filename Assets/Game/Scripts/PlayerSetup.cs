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
    private PlayerInput input;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();

      
        SetPlayerColor();

        if (!photonView.IsMine)
        {
            floorMark.gameObject.SetActive(false);
            Destroy(input);
        }
    }


    private void SetPlayerColor()
    {
        int playerNumber = photonView.Owner.GetPlayerNumber();

        if (playerColor == null || playerColor.Count <= playerNumber)
            return;

        //Renderer render = GameObject.Find("Alpha_Surface").GetComponent<Renderer>();
        surface.material.color = playerColor[playerNumber];
        floorMarkImg.color = playerColor[playerNumber];
       
    }

}

