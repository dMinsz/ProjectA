using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Room, SignUp }

    [SerializeField] StatePanel statePanel;

    [SerializeField] LoginPanel loginPanel;
    [SerializeField] SignUpPanel signUpPanel;
    [SerializeField] MenuPanel menuPanel;
    [SerializeField] RoomPanel roomPanel;

    private LobbySoundSelector musicSelector;

    private void Awake()
    {
        musicSelector = GetComponent<LobbySoundSelector>();
    }

    public void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject?.SetActive(panel == Panel.Login);
        menuPanel.gameObject?.SetActive(panel == Panel.Menu);
        roomPanel.gameObject?.SetActive(panel == Panel.Room);
        signUpPanel.gameObject?.SetActive(panel == Panel.SignUp);
    }

    public void Start()
    {
        if (PhotonNetwork.IsConnected)
            OnConnectedToMaster();
        else if (PhotonNetwork.InRoom)
            OnJoinedRoom();
        else if (PhotonNetwork.InLobby)
            OnJoinedLobby();
        else
            OnDisconnected(DisconnectCause.None);
    }
    public override void OnConnectedToMaster()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(Panel.Login);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Create room failed with error({returnCode}) : {message}");
        statePanel.AddMessage($"Create room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join room failed with error({returnCode}) : {message}");
        statePanel.AddMessage($"Join room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join random room failed with error({returnCode}) : {message}");
        //statePanel.AddMessage($"Join random room failed with error({returnCode}) : {message}");
        menuPanel.JoinRandomMathingFailed();
    }

    public override void OnJoinedRoom()
    {
        musicSelector.PlayCharacterChooseSound();
        SetActivePanel(Panel.Room);
    }

    public override void OnLeftRoom()
    {
        musicSelector.ResetMusic();
        SetActivePanel(Panel.Menu);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomPanel.PlayerEnterRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomPanel.PlayerLeftRoom(otherPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        roomPanel.PlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomPanel.MasterClientSwitched(newMasterClient);
    }
}
