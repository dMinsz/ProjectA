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
    [SerializeField] Character NoneChar;
    [SerializeField] Character AChar;
    [SerializeField] Character BChar;
    [SerializeField] Character CChar;
    [SerializeField] Character DChar;

    [SerializeField] Character curCharacter;
    private List<Character> charactersList = new List<Character>();
    private Player player;

    public enum TeamColor { Blue, Red }

    private void OnEnable()
    {
        charactersList.Add(NoneChar);
        charactersList.Add(AChar);
        charactersList.Add(BChar);
        charactersList.Add(CChar);
        charactersList.Add(DChar);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "Ready" : "";
        Team(player.GetTeamColor());
        //ChangeCharacter(player.GetCharacter());
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

    public void ChangeCharacter(string selectCharacterName)
    {
        foreach (Character character in charactersList)
        {
            if (selectCharacterName == character.name)
            {
                curCharacter = character;
                break;
            }
            else
            {
                Debug.Log("NotFoundCharacterName");
                characterName.text = curCharacter.name;
                characterImage.sprite = curCharacter.Image;
            }
        }

        characterName.text = curCharacter.name;
        characterImage.sprite = curCharacter.Image;
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

        if (property.TryGetValue(CustomProperty.TEAM, out object teamValue))
        {
            int team = (int)teamValue;
            Team(team); 
        }

        //if (property.TryGetValue(CustomProperty.CHARACTERNAME, out object characterValue))
        //{
        //    string character = (string)characterValue;
        //    ChangeCharacter(character);
        //}
    }
}
