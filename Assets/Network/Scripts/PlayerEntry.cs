using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerReady;
    [SerializeField] TMP_Text playerTeam;
    [SerializeField] TMP_Text characterName;
    [SerializeField] Image characterImage;
    [SerializeField] Button playerReadyButton;

    //DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Imports/Character");
    //List<FileInfo> charcterList = new List<FileInfo>();
    private Player player;

    public enum TeamColor { Blue, Red }

    private void OnEnable()
    {   
        //foreach (FileInfo file in directoryInfo.GetFiles())
        //{
        //    if (file.Extension.ToLower().CompareTo(".png") == 0)
        //        charcterList.Add(file);
        //}
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "Ready" : "";
        //Team(player.GetTeamColor());        
        playerReadyButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber);
    }

    public void Ready()
    {
        bool ready = player.GetReady();
        ready = !ready;
        player.SetReady(ready);
    }

    public void Team(int team)
    {
        if (team == (int)TeamColor.Blue)
        {
            playerTeam.text = "Blue";
        }
        else
        {
            playerTeam.text = "Red";
        }
    }

    public void Character(string character)
    {
        //string character = player.GetCharacter();
        //characterImage
        //resoursece, scriptableobject
        //if (charcterList.FindIndex(0, 1, FileInfo => FileInfo.FullName = character))
        characterName.text = character;
    }

    public void ChangeCustomProperty(PhotonHashtable property)
    {
        if (property.TryGetValue(CustomProperty.READY, out object value))
        {
            bool ready = (bool)value;
            playerReady.text = ready ? "Ready" : "";
        }
        else
        {
            playerReady.text = "";
        }

        if (property.TryGetValue(CustomProperty.TEAM, out object teamvalue))
        {
            int team = (int)teamvalue;
            Team(team); 
        }

        if (property.TryGetValue(CustomProperty.CHARACTER, out object character))
        {
            Character((string)character);
        }
    }
}
