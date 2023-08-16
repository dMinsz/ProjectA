using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public cameraHpbar hpSlider;
    [SerializeField] Transform floorMark;
    [SerializeField] Image floorMarkImg;
    [SerializeField] List<Color> playerColor;
    [SerializeField] Transform AttackRangeMark;

    [HideInInspector] public int playerTeam;

    [HideInInspector] public Vector3 originPos;
    [HideInInspector] public Quaternion originRot;

    private PlayerAim aim;
    private PlayerInput input;

    public cameraname cameraName;
    public TMP_Text nickName;

    private void Awake()
    {
        aim = GetComponent<PlayerAim>();
        nickName = cameraName.playername;
        input = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
        {
            floorMark.gameObject.SetActive(false);
            AttackRangeMark.gameObject.SetActive(false);
            Destroy(input);
        }

    }

    public void ReSetUp() 
    {
        if (!photonView.IsMine)
        {
            floorMark.gameObject.SetActive(false);
            AttackRangeMark.gameObject.SetActive(false);
        }
    }


    [PunRPC]
    public void SentSetUp(PlayerEntry.TeamColor color, string nickName , string characterName)
    {
        photonView.RPC("RequestSetUp", RpcTarget.MasterClient, (int)color, nickName, characterName);
    }

    [PunRPC]
    private void RequestSetUp(int color, string nickName, string characterName)
    {
        photonView.RPC("ResultSetup", RpcTarget.AllViaServer, color, nickName, characterName);
    }

    [PunRPC]
    private void ResultSetup(int color, string nickName, string characterName)
    {
        SetUp(color, nickName, characterName);
    }


    [PunRPC]
    public void SetUp(int color, string _nickName, string characterName)
    {
        playerTeam = color;
        floorMarkImg.color = playerColor[color];

        originPos = transform.position;
        originRot = transform.rotation;

        hpSlider.hpbarcolor.color = playerColor[color];

        nickName.text = _nickName;
        nickName.color = playerColor[color];

        GetComponent<PlayerCharacter>().ChangeCharacter(characterName);
    }


    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        int puckViewID = (int)instantiationData[0];

        var puckview = PhotonView.Find(puckViewID);


        aim.puck = puckview.gameObject;
    }
}


