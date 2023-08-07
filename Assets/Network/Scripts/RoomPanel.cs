using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] RectTransform BlueTeamPlayerContent;
    [SerializeField] RectTransform RedTeamPlayerContent;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] PlayerEntry localPlayerEntry;
    [SerializeField] Button startButton;

    private int blueTeamsCount;
    private int redTeamsCount;

    private Dictionary<int, PlayerEntry> playerDictionary;

    private void Awake()
    {
        playerDictionary = new Dictionary<int, PlayerEntry>();
    }

    private void Update()
    {
        Debug.Log(blueTeamsCount + "blueTeamsCount");
        Debug.Log(PhotonNetwork.CurrentRoom.GetTeamPlayersCount(PlayerEntry.TeamColor.Blue) + "GetBlueTeamPlayersCount");
        Debug.Log(redTeamsCount + "redTeamsCount");
        Debug.Log(PhotonNetwork.CurrentRoom.GetTeamPlayersCount(PlayerEntry.TeamColor.Red) + "GetRedTeamPlayersCount");
    }

    private void OnEnable()
    {
        //foreach (Player player in PhotonNetwork.PlayerList)
        //{
        //if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)   // ∆¿∫∞∑Œ ±∏∫–
        //{
        //    PlayerEntry entry = Instantiate(playerEntryPrefab, BlueTeamplayerContent);
        //    entry.SetPlayer(player);
        //    playerDictionary.Add(player.ActorNumber, entry);
        //}
        //else
        //{
        //    PlayerEntry entry = Instantiate(playerEntryPrefab, RedTeamplayerContent);
        //    entry.SetPlayer(player);
        //    playerDictionary.Add(player.ActorNumber, entry);
        //}
        //}
        TeamDivision();
        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);
        // PhotonNetwork.LocalPlayer.SetTeamColor(0);   // ∆¿ √ ±‚»≠

        AllPlayerReadyCheck();
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

    private void TeamDivision()     // ∆¿∫∞∑Œ ±∏∫–
    {
        blueTeamsCount = 0;
        redTeamsCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            {
                //PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Blue, ++blueTeamsCount);
                ++blueTeamsCount;
                PlayerEntry entry = Instantiate(playerEntryPrefab, BlueTeamPlayerContent);
                entry.SetPlayer(player);
                playerDictionary.Add(player.ActorNumber, entry);

                if (player == PhotonNetwork.LocalPlayer)
                    localPlayerEntry = entry;
            }
            else
            {
                //PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Red, ++redTeamsCount);
                ++redTeamsCount;
                PlayerEntry entry = Instantiate(playerEntryPrefab, RedTeamPlayerContent);
                entry.SetPlayer(player);
                playerDictionary.Add(player.ActorNumber, entry);

                if (player == PhotonNetwork.LocalPlayer)
                    localPlayerEntry = entry;
            }
        }
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        if (blueTeamsCount <= redTeamsCount)
        {
            PhotonNetwork.LocalPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Blue);
            //PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Blue, ++blueTeamsCount);
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Red);
            //PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Red, ++redTeamsCount);
        }

        TeamDivision();

        //PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
        //entry.SetPlayer(newPlayer);
        //playerDictionary.Add(newPlayer.ActorNumber, entry);
        AllPlayerReadyCheck();
    }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        //if (otherPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
        //    PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Blue, --blueTeamsCount);
        //else
        //    PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Red, --redTeamsCount);

        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
        TeamDivision();
        AllPlayerReadyCheck();
    }

    public void PlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        playerDictionary[targetPlayer.ActorNumber].ChangeCustomProperty(changedProps);
        AllPlayerReadyCheck();
    }

    public void RoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(CustomProperty.BLUETEAMSCOUNT))
            PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Blue, blueTeamsCount);
        else if (propertiesThatChanged.ContainsKey(CustomProperty.REDTEAMSCOUNT))
            PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Red, redTeamsCount);
        TeamDivision();
    }

    public void MasterClientSwitched(Player newMasterClient)
    {
        TeamDivision();
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

        if (readyCount == PhotonNetwork.PlayerList.Length)
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);
    }

    public void SwitchLocalPlayerBlueTeam()
    {
        if (PhotonNetwork.LocalPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue || PhotonNetwork.LocalPlayer.GetReady())
            return;

        PhotonNetwork.LocalPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Blue);
        localPlayerEntry = Instantiate(playerEntryPrefab, BlueTeamPlayerContent);

        if (playerDictionary.TryGetValue(PhotonNetwork.LocalPlayer.ActorNumber, out PlayerEntry removeEntry))
            Destroy(removeEntry.gameObject);

        localPlayerEntry.SetPlayer(PhotonNetwork.LocalPlayer);
        playerDictionary[PhotonNetwork.LocalPlayer.ActorNumber] = localPlayerEntry;
        ++blueTeamsCount;
        --redTeamsCount;
        //PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Blue, blueTeamsCount);
    }

    public void SwitchLocalPlayerRedTeam()
    {
        if (PhotonNetwork.LocalPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Red || PhotonNetwork.LocalPlayer.GetReady())
            return;

        PhotonNetwork.LocalPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Red);
        localPlayerEntry = Instantiate(playerEntryPrefab, RedTeamPlayerContent);

        if (playerDictionary.TryGetValue(PhotonNetwork.LocalPlayer.ActorNumber, out PlayerEntry removeEntry))
            Destroy(removeEntry.gameObject);

        localPlayerEntry.SetPlayer(PhotonNetwork.LocalPlayer);
        playerDictionary[PhotonNetwork.LocalPlayer.ActorNumber] = localPlayerEntry;
        ++redTeamsCount;
        --blueTeamsCount;
        //PhotonNetwork.CurrentRoom.SetTeamPlayersCount(PlayerEntry.TeamColor.Red, redTeamsCount);
    }
}
