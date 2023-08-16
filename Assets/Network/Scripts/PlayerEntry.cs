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
    [SerializeField] Image teamColorBar;
    [SerializeField] TMP_Text characterName;
    [SerializeField] Image characterImage;
    [SerializeField] Image localPlayerBackGround;
    [SerializeField] Character curCharacter;

    private Player player;

    public enum TeamColor { Blue, Red }

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "Ready" : "";
        Team(player.GetTeamColor());
        SelectCharacter(player.GetCharacterName());

        if (player == PhotonNetwork.LocalPlayer)
            localPlayerBackGround.gameObject.SetActive(true);
        else
            localPlayerBackGround.gameObject.SetActive(false);
    }

    public void Ready()
    {
        if (player.GetCharacterName() == "None")
            return;

        bool ready = player.GetReady();
        ready = !ready;
        player.SetReady(ready);
    }

    public void Team(int team)    // Team Mark
    {
        if (team == (int)TeamColor.Blue)
        {
            teamColorBar.color = Color.blue;
            playerReady.color = Color.blue;
        }
        else
        {
            teamColorBar.color = Color.red;
            playerReady.color = Color.red;
        }
    }

    public void SelectCharacter(string selectCharacterName)
    {
        GameManager.Data.GetCharacter(selectCharacterName);

        if (player == PhotonNetwork.LocalPlayer)
        {
            curCharacter = GameManager.Data.CurCharacter;
           
        }

        characterName.text = GameManager.Data.CurCharacter.name;
        characterImage.sprite = GameManager.Data.CurCharacter.Image;
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

        if (property.TryGetValue(CustomProperty.TEAM, out object teamValue))  // Team Mark
        {
            int team = (int)teamValue;
            Team(team); 
        }

        if (property.TryGetValue(CustomProperty.CHARACTERNAME, out object characterValue))
        {
            string character = (string)characterValue;
            SelectCharacter(character);
        }
        else
            Debug.Log("UpdateProperty Error");
    }
}
