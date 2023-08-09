using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] RectTransform blueTeamPlayerContent;
    [SerializeField] RectTransform redTeamPlayerContent;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] TMP_Text blueTeamsCountText;
    [SerializeField] TMP_Text redTeamsCountText;
    [SerializeField] TMP_Text gameTypeText;
    [SerializeField] Button startButton;
    [SerializeField] TMP_Text ACaracterText;
    [SerializeField] TMP_Text BCaracterText;
    [SerializeField] TMP_Text CCaracterText;
    [SerializeField] TMP_Text DCaracterText;

    private int blueTeamsCount;
    private int redTeamsCount;
    private int maxBlueTeamsCount;
    private int maxRedTeamsCount;

    private Dictionary<int, PlayerEntry> playerDictionary;

    private void Awake()
    {
        playerDictionary = new Dictionary<int, PlayerEntry>();
    }

    private void OnEnable()
    {
        foreach (Player player in PhotonNetwork.PlayerList)     // 팀별로 구분
        {
            if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            {
                PlayerEntry entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
                entry.SetPlayer(player);
                playerDictionary.Add(player.ActorNumber, entry);
                PhotonNetwork.LocalPlayer.SetReady(false);
                PhotonNetwork.LocalPlayer.SetLoad(false);
            }
            else if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Red)
            {
                PlayerEntry entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
                entry.SetPlayer(player);
                playerDictionary.Add(player.ActorNumber, entry);
                PhotonNetwork.LocalPlayer.SetReady(false);
                PhotonNetwork.LocalPlayer.SetLoad(false);
            }
        }

        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
        maxBlueTeamsCount = PhotonNetwork.CurrentRoom.MaxPlayers / 2;
        maxRedTeamsCount = PhotonNetwork.CurrentRoom.MaxPlayers / 2;
        GameTypeText();
        PhotonNetwork.CurrentRoom.SetBlueTeamsCount(blueTeamsCount);
        PhotonNetwork.CurrentRoom.SetRedTeamsCount(redTeamsCount);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnDisable()
    {
        foreach (int actorNumber in playerDictionary.Keys)
        {
            Destroy(playerDictionary[actorNumber].gameObject);
        }

        playerDictionary.Clear();

        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.GetBlueTeamsCount() <= PhotonNetwork.CurrentRoom.GetRedTeamsCount())
            newPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Blue);
        else
            newPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Red);

        if (newPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
        {
            PlayerEntry entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
            entry.SetPlayer(newPlayer);
            playerDictionary.Add(newPlayer.ActorNumber, entry);
        }
        else if (newPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Red)
        {
            PlayerEntry entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
            entry.SetPlayer(newPlayer);
            playerDictionary.Add(newPlayer.ActorNumber, entry);
        }

        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
    }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
    }

    public void PlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        playerDictionary[targetPlayer.ActorNumber].ChangeCustomProperty(changedProps);

        if (changedProps.ContainsKey(CustomProperty.TEAM))
            RenewalPlayerEntry();

        AllPlayerReadyCheck();
        blueTeamsCountText.text = $"{blueTeamsCount} / {PhotonNetwork.CurrentRoom.MaxPlayers / 2}";
        redTeamsCountText.text = $"{redTeamsCount} / {PhotonNetwork.CurrentRoom.MaxPlayers / 2}";
    }

    public void MasterClientSwitched(Player newMasterClient)
    {
        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        //goto Game
        PhotonNetwork.LoadLevel("PuckTestZone");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void GameTypeText()
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers == 2)
            gameTypeText.text = "1 vs 1";
        else if (PhotonNetwork.CurrentRoom.MaxPlayers == 4)
            gameTypeText.text = "2 vs 2";
        else
            gameTypeText.text = "3 vs 3";
    }

    private void AllPlayerReadyCheck()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(false);
            return;
        }

        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady())
                readyCount++;
        }

        if (readyCount == PhotonNetwork.PlayerList.Length && StartCheck())
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);
    }

    private bool StartCheck()
    {
        if (PhotonNetwork.CurrentRoom.GetBlueTeamsCount() == PhotonNetwork.CurrentRoom.GetRedTeamsCount()
            && PhotonNetwork.CurrentRoom.GetBlueTeamsCount() + PhotonNetwork.CurrentRoom.GetRedTeamsCount() == PhotonNetwork.CurrentRoom.MaxPlayers)
            return true;
        else
            return false;
    }

    private void AllPlayerTeamCheck()     // 팀별 Player 수
    {
        blueTeamsCount = 0;
        redTeamsCount = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
                ++blueTeamsCount;
            else if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Red)
                ++redTeamsCount;
        }
    }

    private void RenewalPlayerEntry()      // PlayerEntry 갱신
    {
        blueTeamsCount = 0;
        redTeamsCount = 0;

        foreach (PlayerEntry playerEntry in playerDictionary.Values)    // PlayerEntry 초기화
        {
            Destroy(playerEntry.gameObject);
        }

        playerDictionary.Clear();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            {
                ++blueTeamsCount;
                PlayerEntry entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
                entry.SetPlayer(player);
                playerDictionary.Add(player.ActorNumber, entry);
            }
            else if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Red)
            {
                ++redTeamsCount;
                PlayerEntry entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
                entry.SetPlayer(player);
                playerDictionary.Add(player.ActorNumber, entry);
            }
        }

        PhotonNetwork.CurrentRoom.SetBlueTeamsCount(blueTeamsCount);
        PhotonNetwork.CurrentRoom.SetRedTeamsCount(redTeamsCount);
    }

    private void SwitchLocalPlayerBlueTeam()
    {
        if (PhotonNetwork.LocalPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue || PhotonNetwork.LocalPlayer.GetReady() || blueTeamsCount >= maxBlueTeamsCount)
            return;

        PhotonNetwork.LocalPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Blue);
        RenewalPlayerEntry();
        AllPlayerTeamCheck();
    }

    private void SwitchLocalPlayerRedTeam()
    {
        if (PhotonNetwork.LocalPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Red || PhotonNetwork.LocalPlayer.GetReady() || redTeamsCount >= maxRedTeamsCount)
            return;

        Destroy(playerDictionary[PhotonNetwork.LocalPlayer.ActorNumber].gameObject);
        PhotonNetwork.LocalPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Red);
        RenewalPlayerEntry();
        AllPlayerTeamCheck();
    }

    public void OnSwitchBlueTeamButton()
    {
        SwitchLocalPlayerBlueTeam();
        Debug.Log(PhotonNetwork.LocalPlayer.GetTeamColor());
    }

    public void OnSwitchRedTeamButton()
    {
        SwitchLocalPlayerRedTeam();
    }

    public void OnSelectCharacterButton()
    {
        // TODO : 각 버튼이 가지고 있는 CharacterName 넣기

        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        Debug.Log(clickButton.GetComponentInChildren<TMP_Text>().text);
        //PhotonNetwork.LocalPlayer.SetCharacter((clickButton.GetComponentInChildren<TMP_Text>().text));
    }
}
