using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject settingPopUpUI;

    private int maxPlayerCount;

    private void OnEnable()
    {
        maxPlayerCount = 0;
    }

    public void OnCreateOneOnOneButton()
    {
        maxPlayerCount = 2;
    }

    public void OnCreateTwoToTwoButton()
    {
        maxPlayerCount = 4;
    }

    public void OnCreateThreeToThreeButton()
    {
        maxPlayerCount = 6;
    }

    private void RandomMathing(int maxPlayerCount)
    {
        this.maxPlayerCount = maxPlayerCount;
        PhotonHashtable roomProperies = new PhotonHashtable();
        PhotonNetwork.JoinRandomRoom(roomProperies, (byte)maxPlayerCount);
    }

    public void JoinRandomMathingFailed()   // 인원 수 지정을 위해 분리구현, OnJoinRandomFailed in LobbyManager 에서 실행
    {
        string name = $"Room {Random.Range(1000, 10000)}";
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayerCount };
        PhotonNetwork.CreateRoom(name, options);
    }

    public void OnMathingOneOnOneButton()
    {
        RandomMathing(2);
    }

    public void OnMathingTwoToTwoButton()
    {
        RandomMathing(4);
    }
    
    public void OnMathingThreeToThreeButton()
    {
        RandomMathing(6);
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void Logout()
    {
        Debug.Log("LogOutClick");
        PhotonNetwork.Disconnect();
    }

    public void OnSettingButton()
    {
        settingPopUpUI.SetActive(true);
    }

}
