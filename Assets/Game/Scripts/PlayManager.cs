using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using anstjddn;
using UnityEngine.InputSystem;

public class PlayManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] float startTimer;

    [SerializeField] Transform puckPos;
    [SerializeField] List<Transform> blueSpwans;
    [SerializeField] List<Transform> redSpwans;


    private List<bool> blueTeamChecker = new List<bool>();
    private List<bool> redTeamChecker = new List<bool>();


    public GameObject playPuck;

    public List<GameObject> pPlayerList = new List<GameObject>();
    private void SetUpTeam() 
    {

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetTeamColor() == 1) // blue
            {
                blueTeamChecker.Add(true);
            }
            else
            {
                redTeamChecker.Add(true);
            }
        }
    }

    // Start is called before the first frame update
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
            PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
            PhotonNetwork.ConnectUsingSettings();

        }

    }

    private void GameStart()
    {
        //SetUpTeam();
        infoText.text = string.Format("GameStart now team ={0}", PhotonNetwork.LocalPlayer.GetTeamColor());
        //playPuck = PhotonNetwork.InstantiateRoomObject("Puck", puckPos.position, puckPos.rotation);

        if (PhotonNetwork.LocalPlayer.GetTeamColor() == 1) // blue 
        {

           for (int i=0; i< blueTeamChecker.Count ; i++ )
            {
                if (blueTeamChecker[i] == true)
                {
                    object[] puckData = new object[] { playPuck.GetComponent<PhotonView>().ViewID };
                    var player = PhotonNetwork.Instantiate("Player", blueSpwans[i].position, blueSpwans[i].rotation, 0, puckData);

                    player.GetComponent<PlayerSetup>().SentServerColor();
                    blueTeamChecker[i] = false;

                    break;
                }
            }
        }
        else //red
        {
            for (int i = 0; i < redTeamChecker.Count; i++)
            {
                if (redTeamChecker[i] == true)
                {
                    object[] puckData = new object[] { playPuck.GetComponent<PhotonView>().ViewID };
                    var player = PhotonNetwork.Instantiate("Player", redSpwans[i].position, redSpwans[i].rotation, 0, puckData);

                    player.GetComponent<PlayerSetup>().SentServerColor();
                    redTeamChecker[i] = false;

                    break;
                }
            }
        }
    }

    private void DebugGameStart()
    {
        //SetUpTeam();
        //MakePlayPuck();
        //playPuck = PhotonNetwork.InstantiateRoomObject("Puck", puckPos.position, puckPos.rotation);


        if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == 0)
        {
            object[] puckData = new object[]{ playPuck.GetComponent<PhotonView>().ViewID };
            var player = PhotonNetwork.Instantiate("Player", blueSpwans[0].position, blueSpwans[0].rotation, 0, puckData);

            player.GetComponent<PlayerSetup>().SentServerColor();
        }
        else 
        {
            object[] puckData = new object[] { playPuck.GetComponent<PhotonView>().ViewID };
            var player = PhotonNetwork.Instantiate("Player", redSpwans[1].position, redSpwans[1].rotation, 0, puckData);

            player.GetComponent<PlayerSetup>().SentServerColor();
        }

    }

    IEnumerator DebugGameSetupDelay()
    {
        yield return new WaitForSeconds(1f);//for server Setup
        SetUpTeam();
        MakePlayPuck();
        DebugGameStart();
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

    IEnumerator GameStartTimer()
    {
        SetUpTeam();
        MakePlayPuck();

        // Time Syncronize by Sever Time
        int loadTime = PhotonNetwork.CurrentRoom.GetLoadTime();

        while (startTimer > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f)
        {
            int remainTime = (int)(startTimer - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f) + 1;
            infoText.text = $"All Player Loaded, Start count down : {remainTime}";
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Game Start!");
        infoText.text = "Game Start!";
        GameStart();

        yield return new WaitForSeconds(1f);
        infoText.text = "";
    }

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

    private void MakePlayPuck()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playPuck = PhotonNetwork.Instantiate("Puck", puckPos.position, puckPos.rotation);
            photonView.RPC("RequestPuckID", RpcTarget.OthersBuffered, playPuck.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    private void RequestPuckID(int id)
    {
        playPuck = PhotonView.Find(id).gameObject;
    }


}
