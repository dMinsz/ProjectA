using Photon.Pun;
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
    [SerializeField] TMP_Text characterName;
    [SerializeField] Image characterImage;
    [SerializeField] Image localPlayerBackGround;
    //[SerializeField] Button playerReadyButton;
    [SerializeField] Character curCharacter;
    [SerializeField] public DataManager dataManager;
    private Player player;

    public enum TeamColor { Blue, Red }

    private void OnEnable()
    {
        dataManager = FindObjectOfType<DataManager>();
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "Ready" : "";
        //Team(player.GetTeamColor());
        SelectCharacter(player.GetCharacterName());
        //playerReadyButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber);
    }

    public void Ready()
    {
        if (player.GetCharacterName() == "None")
            return;

        bool ready = player.GetReady();
        ready = !ready;
        player.SetReady(ready);
    }

    //public void Team(int team)    // Team Mark
    //{
    //    if (team == (int)TeamColor.Blue)
    //    {
    //        playerTeam.text = "Blue";
    //    }
    //    else
    //    {
    //        playerTeam.text = "Red";
    //    }
    //}

    public void SelectCharacter(string selectCharacterName)
    {
        dataManager.ChangeCharacter(selectCharacterName);
        localPlayerBackGround.enabled = false;

        if (player == PhotonNetwork.LocalPlayer)
        {
            curCharacter = dataManager.CurCharacter;
            localPlayerBackGround.enabled = true;
        }

        characterName.text = dataManager.CurCharacter.name;
        characterImage.sprite = dataManager.CurCharacter.Image;
    }

    public void DebugCharacter()
    {
        Debug.Log(player.GetCharacterName());
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

        //if (property.TryGetValue(CustomProperty.TEAM, out object teamValue))  // Team Mark
        //{
        //    int team = (int)teamValue;
        //    Team(team); 
        //}

        if (property.TryGetValue(CustomProperty.CHARACTERNAME, out object characterValue))
        {
            string character = (string)characterValue;
            SelectCharacter(character);
        }
        else
            Debug.Log("UpdateProperty Error");
    }
}
