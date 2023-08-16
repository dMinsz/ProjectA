using Photon.Pun;
using Photon.Realtime;
using System.Collections;
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
    [SerializeField] GameObject[] blueTeamCharacterSpot;
    [SerializeField] GameObject[] redTeamCharacterSpot;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] TMP_Text gameTypeText;
    [SerializeField] Button startButton;
    [SerializeField] Image StartImage;
    [SerializeField] Image allPlayerReadyImage;

    private DataManager dataManager;
    private int blueTeamCount;
    private int redTeamCount;
    private int maxBlueTeamCount;
    private int maxRedTeamCount;

    private Dictionary<int, PlayerEntry> playerDictionary;
    private Dictionary<Player, Character> blueTeamPlayerDic;
    private Dictionary<Player, Character> redTeamPlayerDic;
    private List<GameObject> compliteSpotCharacterList;

    private void Awake()
    {
        dataManager = GameManager.Data;
        playerDictionary = new Dictionary<int, PlayerEntry>();
        blueTeamPlayerDic = new Dictionary<Player, Character>();            // TeamManager 쓰면 안만들고 TeamManager의 함수 쓰면 됨
        redTeamPlayerDic = new Dictionary<Player, Character>();
        compliteSpotCharacterList = new List<GameObject>();
    }

    private void OnEnable()
    {
        if (PhotonNetwork.CurrentRoom.GetBlueTeamsCount() > PhotonNetwork.CurrentRoom.GetRedTeamsCount())
            PhotonNetwork.LocalPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Red);

        PhotonNetwork.LocalPlayer.SetCharacterName("None");             // LocalPlayer의 CustomProperty들 기본값으로 초기화
        PhotonNetwork.LocalPlayer.SetLoad(false);
        PhotonNetwork.LocalPlayer.SetReady(false);
        AllPlayerReadyCheck();
        StartCoroutine(EnterRoomRoutine());
        maxBlueTeamCount = PhotonNetwork.CurrentRoom.MaxPlayers / 2;
        maxRedTeamCount = PhotonNetwork.CurrentRoom.MaxPlayers / 2;
        GameTypeText();
        PhotonNetwork.CurrentRoom.SetBlueTeamsCount(blueTeamCount);
        PhotonNetwork.CurrentRoom.SetRedTeamsCount(redTeamCount);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnDisable()    // CurrentRoom과 관련된 사항들 초기화
    {
        foreach (int actorNumber in playerDictionary.Keys)
            Destroy(playerDictionary[actorNumber].gameObject);

        PhotonNetwork.LocalPlayer.SetTeamColor(0);

        foreach (GameObject characterModeling in compliteSpotCharacterList)
        {
            Destroy(characterModeling.gameObject);
        }

        playerDictionary.Clear();
        blueTeamPlayerDic.Clear();
        redTeamPlayerDic.Clear();
        compliteSpotCharacterList.Clear();
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    IEnumerator EnterRoomRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        RenewalPlayerEntry();

        yield break;
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.GetBlueTeamsCount() > PhotonNetwork.CurrentRoom.GetRedTeamsCount())
            newPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Red);

        AllPlayerReadyCheck();
        StartCoroutine(EnterRoomRoutine());
    }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            blueTeamPlayerDic.Remove(otherPlayer);
        else
            redTeamPlayerDic.Remove(otherPlayer);

        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
        AllPlayerReadyCheck();
        RenewalPlayerEntry();
    }

    public void PlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        playerDictionary[targetPlayer.ActorNumber].ChangeCustomProperty(changedProps);

        if (changedProps.ContainsKey(CustomProperty.TEAM))
            RenewalPlayerEntry();

        if (changedProps.ContainsKey(CustomProperty.CHARACTERNAME))
        {
            playerDictionary[targetPlayer.ActorNumber].SelectCharacter(targetPlayer.GetCharacterName());

            if (targetPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
                blueTeamPlayerDic[targetPlayer] = dataManager.GetCharacter(targetPlayer.GetCharacterName());
            else
                redTeamPlayerDic[targetPlayer] = dataManager.GetCharacter(targetPlayer.GetCharacterName());

            SetCharactorAtSpot();
        }

        AllPlayerReadyCheck();
    }

    public void MasterClientSwitched(Player newMasterClient)
    {
        AllPlayerReadyCheck();
        RenewalPlayerEntry();
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        //goto Game
        PhotonNetwork.LoadLevel("GameScene");
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
        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)     // ready 갯수 체크
        {
            if (player.GetReady())
                readyCount++;
        }

        if (readyCount == PhotonNetwork.PlayerList.Length && StartCheck())  // ready 갯수가 maxPlayer 수와 같다면
        {
            if (PhotonNetwork.IsMasterClient)                              // 마스터 클라이언트가 아니면 Start(false)
            {
                allPlayerReadyImage.gameObject.SetActive(false);
                startButton.gameObject.SetActive(true);
                StartImage.gameObject.SetActive(true);
            }
            else
            {
                allPlayerReadyImage.gameObject.SetActive(true);
                startButton.gameObject.SetActive(false);
                StartImage.gameObject.SetActive(false);
            }
        }
    }

    private bool StartCheck()
    {
        if (PhotonNetwork.CurrentRoom.GetBlueTeamsCount() == PhotonNetwork.CurrentRoom.GetRedTeamsCount()
            && PhotonNetwork.CurrentRoom.GetBlueTeamsCount() + PhotonNetwork.CurrentRoom.GetRedTeamsCount() == PhotonNetwork.CurrentRoom.MaxPlayers)
            return true;
        else
            return false;
    }

    private void RenewalPlayerEntry()      // PlayerEntry 갱신
    {
        blueTeamCount = 0;
        redTeamCount = 0;

        foreach (PlayerEntry playerEntry in playerDictionary.Values)    // PlayerEntry 초기화 후 재생성
        {
            Destroy(playerEntry.gameObject);
        }

        playerDictionary.Clear();
        blueTeamPlayerDic.Clear();
        redTeamPlayerDic.Clear();
        GameManager.Data.BlueTeamPlayerNameList.Clear();
        GameManager.Data.RedTeamPlayerNameList.Clear();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry entry;

            if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            {
                ++blueTeamCount;
                entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
                blueTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
                GameManager.Data.BlueTeamPlayerNameList.Add(player.NickName);
            }
            else
            {
                ++redTeamCount;
                entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
                redTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
                GameManager.Data.RedTeamPlayerNameList.Add(player.NickName);
            }

            entry.SetPlayer(player);
            playerDictionary.Add(player.ActorNumber, entry);
        }

        PhotonNetwork.CurrentRoom.SetBlueTeamsCount(blueTeamCount);
        PhotonNetwork.CurrentRoom.SetRedTeamsCount(redTeamCount);

        SetCharactorAtSpot();
    }

    private void SetCharactorAtSpot()
    {
        int compliteSpotCount = 0;

        foreach (GameObject CharactorModeling in compliteSpotCharacterList)
        {
            if (CharactorModeling.name == dataManager.GetCharacter("None").characterName)
            {
                continue;
            }

            Destroy(CharactorModeling.gameObject);
        }

        compliteSpotCharacterList.Clear();

        foreach (KeyValuePair<Player, Character> player in blueTeamPlayerDic)
        {
            if (dataManager.GetCharacter("None").characterName == player.Key.GetCharacterName())
            {
                compliteSpotCount++;
                continue;
            }

            compliteSpotCharacterList.Add(Instantiate(player.Value.modeling, blueTeamCharacterSpot[compliteSpotCount++].transform));
        }

        compliteSpotCount = 0;

        foreach (KeyValuePair<Player, Character> player in redTeamPlayerDic)
        {
            if (dataManager.GetCharacter("None").characterName == player.Key.GetCharacterName())
            {
                compliteSpotCount++;
                continue;
            }

            compliteSpotCharacterList.Add(Instantiate(player.Value.modeling, redTeamCharacterSpot[compliteSpotCount++].transform));
        }
    }

    private void SwitchLocalPlayerBlueTeam(Player player)
    {
        if (player.GetReady())
        {
            StatePanel.Instance.AddMessage("Can't change team while ready");
            return;
        }

        if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            return;

        if (blueTeamCount >= maxBlueTeamCount)
        {
            StatePanel.Instance.AddMessage("Cannot change team because the team is full");
            return;
        }

        player.SetTeamColor((int)PlayerEntry.TeamColor.Blue);
        RenewalPlayerEntry();
    }

    private void SwitchLocalPlayerRedTeam(Player player)
    {
        if (player.GetReady())
        {
            StatePanel.Instance.AddMessage("Can't change team while ready");
            return;
        }

        if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Red)
            return;

        if (redTeamCount >= maxRedTeamCount)
        {
            StatePanel.Instance.AddMessage("Cannot change team because the team is full");
            return;
        }

        player.SetTeamColor((int)PlayerEntry.TeamColor.Red);
        RenewalPlayerEntry();
    }

    public void OnSwitchBlueTeamButton()
    {
        SwitchLocalPlayerBlueTeam(PhotonNetwork.LocalPlayer);
    }

    public void OnSwitchRedTeamButton()
    {
        SwitchLocalPlayerRedTeam(PhotonNetwork.LocalPlayer);

    }

    public void OnSelectCharacterButton()
    {
        if (PhotonNetwork.LocalPlayer.GetReady())
        {
            StatePanel.Instance.AddMessage("Can't change character while ready");
            
            return;
        }

        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        PhotonNetwork.LocalPlayer.SetCharacterName((clickButton.GetComponentInChildren<TMP_Text>().text));
    }

    public void OnReadyButton()
    {
        if (PhotonNetwork.LocalPlayer.GetCharacterName() == "None")
        {
            StatePanel.Instance.AddMessage("Pleas Select Character");

            return;
        }

        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        ready = !ready;
        PhotonNetwork.LocalPlayer.SetReady(ready);
    }
}
