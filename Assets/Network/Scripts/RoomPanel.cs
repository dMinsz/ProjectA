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
    private int blueTeamsCount;
    private int redTeamsCount;
    private int maxBlueTeamsCount;
    private int maxRedTeamsCount;

    private Dictionary<int, PlayerEntry> playerDictionary;
    private Dictionary<Player, Character> blueTeamPlayerDic;
    private Dictionary<Player, Character> redTeamPlayerDic;
    private List<GameObject> compliteSpotCharacterList;

    private void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();
        playerDictionary = new Dictionary<int, PlayerEntry>();
        //playerCharacterModeling = new Dictionary<int, GameObject>();      // playerDictionary랑 합치는게 좋을듯
        blueTeamPlayerDic = new Dictionary<Player, Character>();            // TeamManager 쓰면 안만들고 TeamManager의 함수 쓰면 됨
        redTeamPlayerDic = new Dictionary<Player, Character>();
        compliteSpotCharacterList = new List<GameObject>();
    }

    private void OnEnable()
    {
        foreach (Player player in PhotonNetwork.PlayerList)     // 팀별로 구분
        {
            PlayerEntry entry;

            if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            {
                entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
                blueTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            }
            else
            {
                entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
                redTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            }

            entry.SetPlayer(player);
            playerDictionary.Add(player.ActorNumber, entry);
        }

        PhotonNetwork.LocalPlayer.SetCharacterName("None");
        PhotonNetwork.LocalPlayer.SetLoad(false);
        PhotonNetwork.LocalPlayer.SetReady(false);
        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
        RenewalPlayerEntry();
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

        PhotonNetwork.LocalPlayer.SetTeamColor(0);
        playerDictionary.Clear();
        blueTeamPlayerDic.Clear();
        redTeamPlayerDic.Clear();
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
            //blueTeamPlayerDic.Add(newPlayer.NickName, dataManager.SetCharacter(newPlayer.GetCharacterName()));
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
        SetCharactorAtSpot();
    }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            blueTeamPlayerDic.Remove(otherPlayer);
        else
            redTeamPlayerDic.Remove(otherPlayer);

        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
        SetCharactorAtSpot();
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
        blueTeamsCount = 0;
        redTeamsCount = 0;
        blueTeamPlayerDic.Clear();    // 싹 초기화하고 다시 만드는것보다는 TeamCheck 밖에서 각 항목을 삭제, 추가하는게 좋을것같음, 근데 어짜피 Renewal에서 기존 dic을 초기화 시켜주는데 걍 통일시키자
        redTeamPlayerDic.Clear();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            {
                ++blueTeamsCount;
                blueTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            }
            else
            {
                ++redTeamsCount;
                redTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            }
        }

        dataManager.BlueTeamsPlayer = blueTeamPlayerDic;
        dataManager.RedTeamsPlayer = redTeamPlayerDic;

        SetCharactorAtSpot();
    }

    private void RenewalPlayerEntry()      // PlayerEntry 갱신
    {
        blueTeamsCount = 0;
        redTeamsCount = 0;

        foreach (PlayerEntry playerEntry in playerDictionary.Values)    // PlayerEntry 초기화 후 재생성
        {
            Destroy(playerEntry.gameObject);
        }

        playerDictionary.Clear();
        blueTeamPlayerDic.Clear();
        redTeamPlayerDic.Clear();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry entry;

            if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            {
                ++blueTeamsCount;
                entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
                blueTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            }
            else
            {
                ++redTeamsCount;
                entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);
                redTeamPlayerDic.Add(player, dataManager.GetCharacter(player.GetCharacterName()));
            }

            entry.SetPlayer(player);
            playerDictionary.Add(player.ActorNumber, entry);
        }

        PhotonNetwork.CurrentRoom.SetBlueTeamsCount(blueTeamsCount);
        PhotonNetwork.CurrentRoom.SetRedTeamsCount(redTeamsCount);

        SetCharactorAtSpot();
    }

    private void SetCharactorAtSpot()   // TODO : 보수 필요, Instantiate와 Destroy를 사용하는게 아닌 PoolManager, SetActive 또는 다른 디자인 패턴을 사용하는게 좋을것같음
    {                                   // 일단은 넘어가고 다른 기능들 먼저 구현하기
        int compliteSpotCount = 0;

        foreach (GameObject CharactorModeling in compliteSpotCharacterList)
        {
            if (CharactorModeling.name == dataManager.GetCharacter("None").characterName)
                continue;

            Destroy(CharactorModeling.gameObject);
        }

        compliteSpotCharacterList.Clear();

        foreach (KeyValuePair<Player, Character> player in blueTeamPlayerDic)
        {
            if (dataManager.GetCharacter("None").characterName == player.Key.GetCharacterName())
                continue;

            compliteSpotCharacterList.Add(Instantiate(player.Value.modeling, blueTeamCharacterSpot[compliteSpotCount++].transform));
        }

        foreach (KeyValuePair<Player, Character> player in redTeamPlayerDic)
        {
            if (dataManager.GetCharacter("None").characterName == player.Key.GetCharacterName())
                continue;

            compliteSpotCharacterList.Add(Instantiate(player.Value.modeling, redTeamCharacterSpot[compliteSpotCount++].transform));
        }
    }

    private void SwitchLocalPlayerBlueTeam(Player player)
    {
        if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue || player.GetReady() || blueTeamsCount >= maxBlueTeamsCount)
            return;

        player.SetTeamColor((int)PlayerEntry.TeamColor.Blue);
        RenewalPlayerEntry();
        AllPlayerTeamCheck();
    }

    private void SwitchLocalPlayerRedTeam(Player player)
    {
        if (player.GetTeamColor() == (int)PlayerEntry.TeamColor.Red || player.GetReady() || redTeamsCount >= maxRedTeamsCount)
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
