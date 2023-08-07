using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerReady;
    [SerializeField] TMP_Text playerTeam;
    [SerializeField] Button playerReadyButton;

    private Player player;

    public enum TeamColor { Blue, Red }

    private void Update()
    {
        //Debug.Log(player.GetTeamColor());
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "Ready" : "";
        Team(player.GetTeamColor());        
        playerReadyButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber);
    }

    public void Ready()
    {
        //int team = player.GetTeamColor();

        //if (team == (int)TeamColor.None)
        //    return;

        bool ready = player.GetReady();
        ready = !ready;
        player.SetReady(ready);
    }

    public void Team(int team)
    {
        if (team == (int)TeamColor.Blue)
        {
            playerTeam.text = "Blue";
            //player.SetTeamColor(team);
        }
        else
        {
            playerTeam.text = "Red";
            //player.SetTeamColor(team);
        }
        //else
        //    playerTeam.text = "None";
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
        //else
        //    playerTeam.text = "None";
    }
}
