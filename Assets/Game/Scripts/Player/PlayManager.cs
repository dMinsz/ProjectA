using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
using Unity.VisualScripting;
using System;

public class PlayManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] float startTimer;

    [SerializeField] Transform puckPos;
    [SerializeField] List<Transform> blueSpwans;
    [SerializeField] List<Transform> redSpwans;


    private List<bool> blueTeamChecker = new List<bool>();
    private List<bool> redTeamChecker = new List<bool>();


    [HideInInspector] public GameObject playPuck;

    [HideInInspector] public List<GameObject> pPlayerList = new List<GameObject>();
    [HideInInspector] public List<int> playersViewId = new List<int>();
    private ScoreChecker scoreChecker;


    private List<string> blueTeamPlayerNameList = new List<string>();
    private List<string> redTeamPlayerNameList = new List<string>();


    private void Awake()
    {
        scoreChecker = GetComponent<ScoreChecker>();
    }

    void Start()
    {

        // Normal game mode
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LocalPlayer.SetLoad(true);
        }
        // Debug game mode
        else
        {
            infoText.text = "Debug Mode";
            PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {UnityEngine.Random.Range(1000, 10000)}";
            PhotonNetwork.ConnectUsingSettings();

        }

    }

    private void GameStart()
    {
        blueTeamPlayerNameList.AddRange(GameManager.Data.BlueTeamPlayerNameList);
        for (int num = 0; num < blueTeamPlayerNameList.Count; num++)
        {
            if (PhotonNetwork.LocalPlayer.NickName == blueTeamPlayerNameList[num])
            {

                object[] puckData = new object[] { playPuck.GetComponent<PhotonView>().ViewID };
                var player = PhotonNetwork.Instantiate("Players", blueSpwans[num].position, blueSpwans[num].rotation, 0, puckData);

                player.GetComponent<PlayerDelayCompensation>().SetSyncronize(true);
                //player.GetComponent<PlayerSetup>().SentServerColor(PlayerEntry.TeamColor.Blue);
                player.GetComponent<PlayerSetup>().SentSetUp(PlayerEntry.TeamColor.Blue, blueTeamPlayerNameList[num], PhotonNetwork.LocalPlayer.GetCharacterName());


                pPlayerList.Add(player);

                object[] playerData = new object[] { player.GetComponent<PhotonView>().ViewID };
                photonView.RPC("AddPlayer", RpcTarget.OthersBuffered, playerData);
                //blueTeamChecker[num] = false;
            }
        }

        redTeamPlayerNameList.AddRange(GameManager.Data.RedTeamPlayerNameList);
        for (int num = 0; num < redTeamPlayerNameList.Count; num++)
        {
            if (PhotonNetwork.LocalPlayer.NickName == redTeamPlayerNameList[num]) //&& redTeamChecker[num] == true)
            {
                object[] puckData = new object[] { playPuck.GetComponent<PhotonView>().ViewID };
                var player = PhotonNetwork.Instantiate("Players", redSpwans[num].position, redSpwans[num].rotation, 0, puckData);



                player.GetComponent<PlayerDelayCompensation>().SetSyncronize(true);
                //player.GetComponent<PlayerSetup>().SentServerColor(PlayerEntry.TeamColor.Red);
                player.GetComponent<PlayerSetup>().SentSetUp(PlayerEntry.TeamColor.Red, redTeamPlayerNameList[num], PhotonNetwork.LocalPlayer.GetCharacterName());


                pPlayerList.Add(player);

                object[] playerData = new object[] { player.GetComponent<PhotonView>().ViewID };
                photonView.RPC("AddPlayer", RpcTarget.OthersBuffered, playerData);
                //redTeamChecker[num] = false;
            }
        }
        scoreChecker.StartTimer();
    }


    private void DebugGameStart()
    {
        if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == 0)
        {
            object[] puckData = new object[] { playPuck.GetComponent<PhotonView>().ViewID };
            var player = PhotonNetwork.Instantiate("Players", blueSpwans[0].position, blueSpwans[0].rotation, 0, puckData);

            player.GetComponent<PlayerDelayCompensation>().SetSyncronize(true);
            //test
            player.GetComponent<PlayerSetup>().SentSetUp(PlayerEntry.TeamColor.Blue, "Debug1", "Mario");

            GameManager.Data.ChangeCharacter("Mario");

            

            pPlayerList.Add(player);

            object[] playerData = new object[] { player.GetComponent<PhotonView>().ViewID };
            photonView.RPC("AddPlayer", RpcTarget.OthersBuffered, playerData);
        }
        else
        {
            object[] puckData = new object[] { playPuck.GetComponent<PhotonView>().ViewID };
            var player = PhotonNetwork.Instantiate("Players", redSpwans[0].position, redSpwans[0].rotation, 0, puckData);

            player.GetComponent<PlayerDelayCompensation>().SetSyncronize(true);
            player.GetComponent<PlayerSetup>().SentSetUp(PlayerEntry.TeamColor.Red, "Debug2", "Mario");
            GameManager.Data.ChangeCharacter("Mario");
            pPlayerList.Add(player);

            object[] playerData = new object[] { player.GetComponent<PhotonView>().ViewID };
            photonView.RPC("AddPlayer", RpcTarget.OthersBuffered, playerData);
        }

        scoreChecker.StartTimer();
    }

    IEnumerator DebugGameSetupDelay()
    {
        yield return new WaitForSeconds(1f);//for server Setup
        SetUpTeam();
        MakePlayPuck();
        MakeBlocker();
        DebugGameStart();
    }

    IEnumerator GameStartTimer()
    {
        SetUpTeam();
        MakePlayPuck();
        MakeBlocker();

        // Time Syncronize by Sever Time
        int loadTime = PhotonNetwork.CurrentRoom.GetLoadTime();

        while (startTimer > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f)
        {
            int remainTime = (int)(startTimer - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f) + 1;
            infoText.text = $"Start Count Down : {remainTime}";
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Game Start!");
        infoText.text = "Game Start!";


        GameStart();

        yield return new WaitForSeconds(0.5f);
        infoText.text = "";
    }

    private void SetUpTeam()
    {

        for (int i = 0; i < GameManager.Data.BlueTeamPlayerNameList.Count; i++)
        {
            blueTeamChecker.Add(true);
        }

        for (int i = 0; i < GameManager.Data.RedTeamPlayerNameList.Count; i++)
        {
            redTeamChecker.Add(true);
        }
    }

    private int PlayerLoadCount()
    {
        int loadCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad())
                loadCount++;
        }
        return loadCount;
    }

    private void MakePlayPuck()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playPuck = PhotonNetwork.Instantiate("Puck", puckPos.position, puckPos.rotation);
            photonView.RPC("RequestPuckID", RpcTarget.OthersBuffered, playPuck.GetComponent<PhotonView>().ViewID);
        }
    }

    private void MakeBlocker()
    {

        PhotonNetwork.InstantiateRoomObject("GoalBlocker", new Vector3(0, 1, 22), Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject("GoalBlocker", new Vector3(0, 1, -22), Quaternion.identity);

    }

    public void ResetRound()
    {
        RespawnPuck();
        ResetPlayers();
    }

    private void ResetPlayers()
    {
        foreach (var player in pPlayerList)
        {
            player.GetComponent<PlayerDelayCompensation>().SetSyncronize(false);

            player.transform.position = player.GetComponent<PlayerSetup>().originPos;
            player.transform.rotation = player.GetComponent<PlayerSetup>().originRot;

            player.GetComponent<PlayerDelayCompensation>().SetSyncronize(true);
        }
    }
    public void RespawnPuck()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(playPuck);

            playPuck = PhotonNetwork.Instantiate("Puck", puckPos.position, puckPos.rotation);

            foreach (var player in pPlayerList)
            {
                player.GetComponent<PlayerAim>().puck = playPuck;
            }

            photonView.RPC("BroadCastingRespawnPuck", RpcTarget.OthersBuffered, playPuck.GetComponent<PhotonView>().ViewID);
        }
    }

    #region RPC Funcs
    [PunRPC]
    private void RequestPuckID(int id)
    {
        playPuck = PhotonView.Find(id).gameObject;
    }

    [PunRPC]
    private void AddPlayer(int viewId)
    {
        var player = PhotonView.Find(viewId).gameObject;
        pPlayerList.Add(player);
    }

    [PunRPC]
    public void BroadCastingRespawnPuck(int id)
    {
        playPuck = PhotonView.Find(id).gameObject;

        foreach (var player in pPlayerList)
        {
            player.GetComponent<PlayerAim>().puck = playPuck;
        }
    }

    #endregion


    #region ServerCallbacks

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions() { IsVisible = false }; // 디버그를 위한방 비공개방으로
        PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //migration
        if (PhotonNetwork.IsMasterClient)
        {
            //StartCoroutine(SpawnStoneRoutine());
        }
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(DebugGameSetupDelay());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected : {cause}");
        SceneManager.LoadScene("LobbyScene"); // Server Disconnected so SceneManger load
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        PhotonNetwork.LoadLevel("LobbyScene"); // Server Connecting so PhotonNetwork.load
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.LOAD))
        {
            //All Player Ready
            if (PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
            {
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.CurrentRoom.SetLoadTime(PhotonNetwork.ServerTimestamp);
            }
            else // Some players are not ready
            {
                //Wait for Some Player
                Debug.Log($"Wait players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}");
                infoText.text = $"Wait players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}";
            }
        }
    }

    public override void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(CustomProperty.LOADTIME))
        {
            StartCoroutine(GameStartTimer());
        }
    }

    #endregion



}
