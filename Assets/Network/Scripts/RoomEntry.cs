using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text currentPlayer;
    [SerializeField] TMP_Text currnetGameType;
    [SerializeField] Button joinRoomButton;

    private RoomInfo info;

    public void SetRoomInfo(RoomInfo info)
    {
        this.info = info;
        roomName.text = info.Name;
        currentPlayer.text = $"{info.PlayerCount} / {info.MaxPlayers}";
        int curGameType = info.MaxPlayers;

        if (curGameType == 6)
            currnetGameType.text = "3 vs 3";
        else if (curGameType == 4)
            currnetGameType.text = "2 vs 2";
        else
            currnetGameType.text = "1 vs 1";

        joinRoomButton.interactable = info.PlayerCount < info.MaxPlayers;
    }

    public void JoinRoom()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(info.Name);
    }
}
