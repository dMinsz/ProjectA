using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] RectTransform blueTeamPlayerContent;
    [SerializeField] RectTransform redTeamPlayerContent;
    [SerializeField] GameObject[] blueTeamCharacterSpot;
    [SerializeField] GameObject[] redTeamCharacterSpot;
    [SerializeField] PlayerEntry playerEntryPrefab;
    //[SerializeField] TMP_Text blueTeamsCountText;
    //[SerializeField] TMP_Text redTeamsCountText;
    [SerializeField] TMP_Text gameTypeText;
    [SerializeField] Button startButton;
    [SerializeField] Button readyButton;
    //[SerializeField] TMP_Text[] CharacterName;

    private DataManager dataManager;
    private int blueTeamCount;
    private int redTeamCount;
    private int maxBlueTeamCount;
    private int maxRedTeamCount;

    private Dictionary<int, PlayerEntry> playerDictionary;
    private Dictionary<Player, Character> blueTeamPlayerDic;
    private Dictionary<Player, Character> redTeamPlayerDic;
    private List<string> blueTeamPlayerNameList;
    private List<string> redTeamPlayerNameList;
    private List<GameObject> compliteSpotCharacterList;

    private void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();
        playerDictionary = new Dictionary<int, PlayerEntry>();
        //playerCharacterModeling = new Dictionary<int, GameObject>();      // playerDictionary랑 합치는게 좋을듯
        blueTeamPlayerDic = new Dictionary<Player, Character>();            // TeamManager 쓰면 안만들고 TeamManager의 함수 쓰면 됨
        redTeamPlayerDic = new Dictionary<Player, Character>();
        blueTeamPlayerNameList = new List<string>();                        // CurrnetRoom 의 CustomProperty 와 동기화 할 때 쓰는 놈
        redTeamPlayerNameList = new List<string>();
        compliteSpotCharacterList = new List<GameObject>();
    }

    private void OnEnable()
    {
        blueTeamPlayerNameList.AddRange(PhotonNetwork.CurrentRoom.GetBlueTeamPlayerList());
        redTeamPlayerNameList.AddRange(PhotonNetwork.CurrentRoom.GetRedTeamPlayerList());

        //foreach (Player player in PhotonNetwork.PlayerList)     // 팀별로 구분
        //{
        //    PlayerEntry entry;

        //    if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
        //    {
        //        entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
        //        blueTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
        //        blueTeamPlayerNameList.Add(player.NickName);
        //    }
        //    else
        //    {
        //        entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
        //        redTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
        //        redTeamPlayerNameList.Add(player.NickName);
        //    }

        //    entry.SetPlayer(player);
        //    playerDictionary.Add(player.ActorNumber, entry);
        //}

        foreach (string playerName in blueTeamPlayerNameList)
        {
            Player player = PhotonNetwork.CurrentRoom.Players.Values.FirstOrDefault(player => player.NickName == playerName);
            PlayerEntry entry;

            entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
            entry.SetPlayer(player);
            playerDictionary.Add(player.ActorNumber, entry);
        }

        foreach (string playerName in redTeamPlayerNameList)
        {
            Player player = PhotonNetwork.CurrentRoom.Players.Values.FirstOrDefault(player => player.NickName == playerName);
            PlayerEntry entry;

            entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
            entry.SetPlayer(player);
            playerDictionary.Add(player.ActorNumber, entry);
        }

        PhotonNetwork.LocalPlayer.SetCharacterName("None");
        PhotonNetwork.LocalPlayer.SetLoad(false);
        PhotonNetwork.LocalPlayer.SetReady(false);
        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
        RenewalPlayerEntry();
        maxBlueTeamCount = PhotonNetwork.CurrentRoom.MaxPlayers / 2;
        maxRedTeamCount = PhotonNetwork.CurrentRoom.MaxPlayers / 2;
        GameTypeText();
        PhotonNetwork.CurrentRoom.SetBlueTeamsCount(blueTeamCount);
        PhotonNetwork.CurrentRoom.SetRedTeamsCount(redTeamCount);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnDisable()
    {
        foreach (int actorNumber in playerDictionary.Keys)
        {
            Destroy(playerDictionary[actorNumber].gameObject);
        }

        PhotonNetwork.LocalPlayer.SetTeamColor(0);
        playerDictionary.Clear();
        blueTeamPlayerDic.Clear();
        blueTeamPlayerNameList.Clear();
        redTeamPlayerDic.Clear();
        redTeamPlayerNameList.Clear();

        foreach (GameObject characterModeling in compliteSpotCharacterList)
        {
            Destroy(characterModeling.gameObject);
        }
        compliteSpotCharacterList.Clear();
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.GetBlueTeamsCount() <= PhotonNetwork.CurrentRoom.GetRedTeamsCount())
            newPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Blue);
        else
            newPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Red);

        PlayerEntry entry;

        if (newPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
        {
            entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
            //blueTeamPlayerDic.Add(newPlayer.NickName, dataManager.SetCharacter(newPlayer.GetCharacterName()));    // 아마도 none일거임, 확인필요
        }
        else
        {
            entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
            //redTeamplayerDic.Add(newPlayer.NickName, dataManager.SetCharacter(newPlayer.GetCharacterName()));
        }

        entry.SetPlayer(newPlayer);
        playerDictionary.Add(newPlayer.ActorNumber, entry);
        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
        RenewalPlayerEntry();
    }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
        {
            blueTeamPlayerDic.Remove(otherPlayer);
            blueTeamPlayerNameList.Remove(otherPlayer.NickName);
        }
        else
        {
            redTeamPlayerDic.Remove(otherPlayer);
            redTeamPlayerNameList.Remove(otherPlayer.NickName);
        }

        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
        RenewalPlayerEntry();
    }

    public void PlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        playerDictionary[targetPlayer.ActorNumber].ChangeCustomProperty(changedProps);

        if (changedProps.ContainsKey(CustomProperty.TEAM))
        {
            RenewalPlayerEntry();
            //PhotonNetwork.CurrentRoom.SetBlueTeamPlayerList(blueTeamPlayerNameList);
            //PhotonNetwork.CurrentRoom.SetRedTeamPlayerList(redTeamPlayerNameList);
        }

        if (changedProps.ContainsKey(CustomProperty.CHARACTERNAME))
        {
            playerDictionary[targetPlayer.ActorNumber].SelectCharacter(targetPlayer.GetCharacterName());

            if (targetPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
                blueTeamPlayerDic[targetPlayer] = dataManager.GetCharacter(targetPlayer.GetCharacterName());
            else
                redTeamPlayerDic[targetPlayer] = dataManager.GetCharacter(targetPlayer.GetCharacterName());

            SetCharactorAtSpot();
        }

        //if (changedProps.ContainsKey(CustomProperty.BLUETEAMSPLAYERLIST))
        //    //playerDictionary[targetPlayer.NickName].

        AllPlayerReadyCheck();
        //blueTeamsCountText.text = $"{blueTeamsCount} / {PhotonNetwork.CurrentRoom.MaxPlayers / 2}";
        //redTeamsCountText.text = $"{redTeamsCount} / {PhotonNetwork.CurrentRoom.MaxPlayers / 2}";
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
        blueTeamCount = 0;
        redTeamCount = 0;
        blueTeamPlayerDic.Clear();    // 싹 초기화하고 다시 만드는것보다는 TeamCheck 밖에서 각 항목을 삭제, 추가하는게 좋을것같음, 근데 어짜피 Renewal에서 기존 dic을 초기화 시켜주는데 걍 통일시키자
        redTeamPlayerDic.Clear();
        blueTeamPlayerNameList.Clear();
        redTeamPlayerNameList.Clear();

        //foreach (Player player in PhotonNetwork.PlayerList)
        //{
        //    if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
        //    {
        //        ++blueTeamCount;
        //        blueTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
        //        blueTeamPlayerNameList.Add(player.NickName);
        //    }
        //    else
        //    {
        //        ++redTeamCount;
        //        redTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
        //        redTeamPlayerNameList.Add(player.NickName);
        //    }
        //}

        blueTeamPlayerNameList.AddRange(PhotonNetwork.CurrentRoom.GetBlueTeamPlayerList());
        foreach (string playerName in blueTeamPlayerNameList)
        {
            ++blueTeamCount;
            Player player = PhotonNetwork.CurrentRoom.Players.Values.FirstOrDefault(player => player.NickName == playerName);
            blueTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
        }

        redTeamPlayerNameList.AddRange(PhotonNetwork.CurrentRoom.GetRedTeamPlayerList());
        foreach (string playerName in redTeamPlayerNameList)
        {
            ++redTeamCount;
            Player player = PhotonNetwork.CurrentRoom.Players.Values.FirstOrDefault(player => player.NickName == playerName);
            redTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
        }

        dataManager.BlueTeamsPlayer = blueTeamPlayerDic;
        dataManager.RedTeamsPlayer = redTeamPlayerDic;

        SetCharactorAtSpot();
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
        blueTeamPlayerNameList.Clear();
        redTeamPlayerNameList.Clear();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry entry;

            if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            {
                ++blueTeamCount;
                entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
                blueTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            }
            else
            {
                ++redTeamCount;
                entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
                redTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            }

            entry.SetPlayer(player);
            playerDictionary.Add(player.ActorNumber, entry);
        }

        blueTeamPlayerNameList.AddRange(PhotonNetwork.CurrentRoom.GetBlueTeamPlayerList());
        foreach (string playerName in blueTeamPlayerNameList)
        {
            ++blueTeamCount;
            Player player = PhotonNetwork.CurrentRoom.Players.Values.FirstOrDefault(player => player.NickName == playerName);
            PlayerEntry entry;
            entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
            entry.SetPlayer(player);
            blueTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            playerDictionary.Add(player.ActorNumber, entry);
        }

        redTeamPlayerNameList.AddRange(PhotonNetwork.CurrentRoom.GetRedTeamPlayerList());
        foreach (string playerName in redTeamPlayerNameList)
        {
            ++redTeamCount;
            Player player = PhotonNetwork.CurrentRoom.Players.Values.FirstOrDefault(player => player.NickName == playerName);
            PlayerEntry entry;
            entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
            entry.SetPlayer(player);
            redTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            playerDictionary.Add(player.ActorNumber, entry);
        }

        PhotonNetwork.CurrentRoom.SetBlueTeamsCount(blueTeamCount);
        PhotonNetwork.CurrentRoom.SetRedTeamsCount(redTeamCount);
        PhotonNetwork.CurrentRoom.SetBlueTeamPlayerList(blueTeamPlayerNameList);
        PhotonNetwork.CurrentRoom.SetRedTeamPlayerList(redTeamPlayerNameList);

        SetCharactorAtSpot();
    }

    private void SetCharactorAtSpot()   // TODO : 보수 필요, Instantiate와 Destroy를 사용하는게 아닌 PoolManager, SetActive 또는 다른 디자인 패턴을 사용하는게 좋을것같음
    {                                   // 일단은 넘어가고 다른 기능들 먼저 구현하기
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
        if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue || player.GetReady() || blueTeamCount >= maxBlueTeamCount)
            return;

        player.SetTeamColor((int)PlayerEntry.TeamColor.Blue);
        RenewalPlayerEntry();
        AllPlayerTeamCheck();
    }

    private void SwitchLocalPlayerRedTeam(Player player)
    {
        if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Red || player.GetReady() || redTeamCount >= maxRedTeamCount)
            return;

        player.SetTeamColor((int)PlayerEntry.TeamColor.Red);
        RenewalPlayerEntry();
        AllPlayerTeamCheck();
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
            return;

        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        PhotonNetwork.LocalPlayer.SetCharacterName((clickButton.GetComponentInChildren<TMP_Text>().text));
    }

    public void OnReadyButton()
    {
        if (PhotonNetwork.LocalPlayer.GetCharacterName() == "None")
            return;

        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        ready = !ready;
        PhotonNetwork.LocalPlayer.SetReady(ready);
    }
}
