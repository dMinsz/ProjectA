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

    private Button choosedButton;
    private ColorBlock beforeButtonCol;
    private ColorBlock curButtonCol;
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

        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = $"Room {Random.Range(1000, 10000)}";

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayerCount };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void OnOneOnOneButton()
    {
        if (choosedButton != null)
        {
            beforeButtonCol = choosedButton.colors;
            beforeButtonCol.normalColor = Color.white;
            choosedButton.colors = beforeButtonCol;
        }

        curButtonCol = oneOnOneButton.colors;    // 선택 후 RoomNameText 누르면 색깔 풀리는거 방지
        curButtonCol.normalColor = Color.yellow;
        oneOnOneButton.colors = curButtonCol;
        choosedButton = oneOnOneButton;
        hasMaxPlayer = true;
        maxPlayerCount = 2;
    }

    public void OnTwoToTwoButton()
    {
        if (choosedButton != null)
        {
            beforeButtonCol = choosedButton.colors;
            beforeButtonCol.normalColor = Color.white;
            choosedButton.colors = beforeButtonCol;
        }

        curButtonCol = twoToTwoButton.colors;
        curButtonCol.normalColor = Color.yellow;
        twoToTwoButton.colors = curButtonCol;
        choosedButton = twoToTwoButton;
        hasMaxPlayer = true;
        maxPlayerCount = 4;
    }

    public void OnThreeToThreeButton()
    {
        if (choosedButton != null)
        {
            beforeButtonCol = choosedButton.colors;
            beforeButtonCol.normalColor = Color.white;
            choosedButton.colors = beforeButtonCol;
        }

        curButtonCol = threeToThreeButton.colors;
        curButtonCol.normalColor = Color.yellow;
        threeToThreeButton.colors = curButtonCol;
        choosedButton = threeToThreeButton;
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
