using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] Image allPlayerReadyImage;

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
        dataManager = GameManager.Data;
        playerDictionary = new Dictionary<int, PlayerEntry>();
        blueTeamPlayerDic = new Dictionary<Player, Character>();            // TeamManager ���� �ȸ���� TeamManager�� �Լ� ���� ��
        redTeamPlayerDic = new Dictionary<Player, Character>();
        blueTeamPlayerNameList = new List<string>();                        // CurrnetRoom �� CustomProperty �� ����ȭ �� �� ���� ��
        redTeamPlayerNameList = new List<string>();
        compliteSpotCharacterList = new List<GameObject>();
    }

    private void OnEnable()
    {
        blueTeamPlayerNameList.AddRange(PhotonNetwork.CurrentRoom.GetBlueTeamPlayerList());     // �����ϸ� �� ���� �÷��̾� ���� ������
        redTeamPlayerNameList.AddRange(PhotonNetwork.CurrentRoom.GetRedTeamPlayerList());

        foreach (string playerName in blueTeamPlayerNameList)                                   // �� ���� ������ ���߾� PlayerEntry ����
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

        PhotonNetwork.LocalPlayer.SetCharacterName("None");             // LocalPlayer�� CustomProperty�� �⺻������ �ʱ�ȭ
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

    private void OnDisable()    // CurrentRoom�� ���õ� ���׵� �ʱ�ȭ
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
        blueTeamPlayerNameList.Clear();
        redTeamPlayerDic.Clear();
        redTeamPlayerNameList.Clear();
        compliteSpotCharacterList.Clear();
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.GetBlueTeamsCount() > PhotonNetwork.CurrentRoom.GetRedTeamsCount())
            newPlayer.SetTeamColor((int)PlayerEntry.TeamColor.Red);

        PlayerEntry entry;

        if (newPlayer.GetTeamColor() == (int)PlayerEntry.TeamColor.Blue)
            entry = Instantiate(playerEntryPrefab, blueTeamPlayerContent);
        else
            entry = Instantiate(playerEntryPrefab, redTeamPlayerContent);

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
        AllPlayerTeamCheck();
        AllPlayerReadyCheck();
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
        foreach (Player player in PhotonNetwork.PlayerList)     // ready ���� üũ
        {
            if (player.GetReady())
                readyCount++;
        }

        if (readyCount == PhotonNetwork.PlayerList.Length && StartCheck())  // ready ������ maxPlayer ���� ���ٸ�
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);

        if (!PhotonNetwork.IsMasterClient)              // ������ Ŭ���̾�Ʈ�� �ƴϸ� Start(false)
        {
            allPlayerReadyImage.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);

            return;
        }

        allPlayerReadyImage.gameObject.SetActive(false);
    }

    private bool StartCheck()
    {
        if (PhotonNetwork.CurrentRoom.GetBlueTeamsCount() == PhotonNetwork.CurrentRoom.GetRedTeamsCount()
            && PhotonNetwork.CurrentRoom.GetBlueTeamsCount() + PhotonNetwork.CurrentRoom.GetRedTeamsCount() == PhotonNetwork.CurrentRoom.MaxPlayers)
            return true;
        else
            return false;
    }

    private void AllPlayerTeamCheck()     // ���� Player ��
    {
        blueTeamCount = 0;
        redTeamCount = 0;
        blueTeamPlayerDic.Clear();
        redTeamPlayerDic.Clear();
        blueTeamPlayerNameList.Clear();
        redTeamPlayerNameList.Clear();
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

    private void RenewalPlayerEntry()      // PlayerEntry ����
    {
        blueTeamCount = 0;
        redTeamCount = 0;

        foreach (PlayerEntry playerEntry in playerDictionary.Values)    // PlayerEntry �ʱ�ȭ �� �����
        {
            Destroy(playerEntry.gameObject);
        }

        playerDictionary.Clear();
        blueTeamPlayerDic.Clear();
        redTeamPlayerDic.Clear();
        blueTeamPlayerNameList.Clear();
        redTeamPlayerNameList.Clear();

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

    private void SetCharactorAtSpot()   // TODO : ���� �ʿ�, Instantiate�� Destroy�� ����ϴ°� �ƴ� PoolManager, SetActive �Ǵ� �ٸ� ������ ������ ����ϴ°� �����Ͱ���
    {                                   // �ϴ��� �Ѿ�� �ٸ� ��ɵ� ���� �����ϱ�
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
            StatePanel.Instance.AddMessage("\r\nCannot change team because the team is full");
            return;
        }

        player.SetTeamColor((int)PlayerEntry.TeamColor.Blue);
        RenewalPlayerEntry();
        AllPlayerTeamCheck();
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
            StatePanel.Instance.AddMessage("\r\nCannot change team because the team is full");
            return;
        }

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
