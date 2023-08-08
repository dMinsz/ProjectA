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

    public void OnOneOnOneButton()
    {
        ChangeNormalColor(oneOnOneButton);
        choosedGameType = true;
        maxPlayerCount = 2;
    }

    public void OnTwoToTwoButton()
    {
        ChangeNormalColor(twoToTwoButton);
        choosedGameType = true;
        maxPlayerCount = 4;
    }

    public void OnThreeToThreeButton()
    {
        ChangeNormalColor(threeToThreeButton);
        choosedGameType = true;
        maxPlayerCount = 6;
    }

    public void CreateRoomCancel()
    {
        choosedGameType = false;
        roomNameInputField.text = "";
        ResetNomalColor();
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
