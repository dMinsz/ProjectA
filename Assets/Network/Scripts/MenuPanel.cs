using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    //[SerializeField] TMP_InputField maxPlayerInputField;
    
    private bool hasMaxPlayer;
    private int maxPlayerCount;


    private void OnEnable()
    {
        hasMaxPlayer = false;
        maxPlayerCount = 0;
        createRoomPanel.SetActive(false);
    }

    public void CreateRoomMenu()
    {
        createRoomPanel.SetActive(true);
    }

    public void CreateRoomConfirm()
    {
        if (!hasMaxPlayer)
            return;
        //string roomName = roomNameInputField.text;
        //if (roomName == "")
        //    roomName = $"Room {Random.Range(1000, 10000)}";

        //int maxPlayer = maxPlayerInputField.text == "" ? 8 : int.Parse(maxPlayerInputField.text);
        //maxPlayer = Mathf.Clamp(maxPlayer, 1, 8);

        //RoomOptions options = new RoomOptions { MaxPlayers = maxPlayer };
        //PhotonNetwork.CreateRoom(roomName, options);

        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = $"Room {Random.Range(1000, 10000)}";

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayerCount };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void OnOneOnOneButton()
    {
        hasMaxPlayer = true;
        maxPlayerCount = 2;
    }

    public void OnTwoToTwoButton()
    {
        hasMaxPlayer = true;
        maxPlayerCount = 4;
    }

    public void OnThreeToThreeButton()
    {
        hasMaxPlayer = true;
        maxPlayerCount = 6;
    }

    public void CreateRoomCancel()
    {
        createRoomPanel.SetActive(false);
    }

    public void RandomMatching()
    {
        string name = $"Room {Random.Range(1000, 10000)}";
        RoomOptions options = new RoomOptions { MaxPlayers = 8 };
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: name, roomOptions: options);
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
