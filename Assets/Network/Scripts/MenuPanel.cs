using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] Button oneOnOneButton;
    [SerializeField] Button twoToTwoButton;
    [SerializeField] Button threeToThreeButton;
    [SerializeField] TMP_Text oneOnOneText;
    [SerializeField] TMP_Text twoToTwoText;
    [SerializeField] TMP_Text threeToThreeText;

    private Button choosedButton;
    private ColorBlock choosedButtonCol;
    private ColorBlock curButtonCol;
    private bool choosedGameType;
    private int maxPlayerCount;

    private void OnEnable()
    {
        choosedGameType = false;
        maxPlayerCount = 0;
        createRoomPanel.SetActive(false);
    }

    public void CreateRoomMenu()
    {
        choosedGameType = false;
        maxPlayerCount = 0;
        roomNameInputField.text = "";
        ResetNomalColor();
        createRoomPanel.SetActive(true);
    }

    public void CreateRoomConfirm()
    {
        if (!choosedGameType)
            return;

        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = $"Room {Random.Range(1000, 10000)}";

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayerCount };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    private void ResetNomalColor()
    {
        if (choosedButton != null)
        {
            choosedButtonCol = choosedButton.colors;
            choosedButtonCol.normalColor = Color.white;
            choosedButton.colors = choosedButtonCol;
        }
    }

    private void ChangeNormalColor(Button curButton)
    {
        ResetNomalColor();
        curButtonCol = curButton.colors;
        curButtonCol.normalColor = Color.yellow;
        curButton.colors = curButtonCol;
        choosedButton = curButton;
    }

    public void OnCreateOneOnOneButton()
    {
        ChangeNormalColor(oneOnOneButton);
        choosedGameType = true;
        maxPlayerCount = 2;
    }

    public void OnCreateTwoToTwoButton()
    {
        ChangeNormalColor(twoToTwoButton);
        choosedGameType = true;
        maxPlayerCount = 4;
    }

    public void OnCreateThreeToThreeButton()
    {
        ChangeNormalColor(threeToThreeButton);
        choosedGameType = true;
        maxPlayerCount = 6;
    }

    public void CreateRoomCancel()
    {
        choosedGameType = false;
        maxPlayerCount = 0;
        roomNameInputField.text = "";
        ResetNomalColor();
        createRoomPanel.SetActive(false);
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
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayerCount };  // 팀 변경, 새로 만들면 이전 팀으로 설정됨
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
        PhotonNetwork.Disconnect();
    }

}
