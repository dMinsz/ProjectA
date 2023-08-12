using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DataManager : MonoBehaviour
{
    [SerializeField] private int level;
    public int Level { get { return level; } set { level = value; } }

    [SerializeField] private float exp;
    private float Exp { get { return exp; } set { exp = value; } }


    [SerializeField] private Character curCharacter;
    public Character CurCharacter 
    { 
        get { return curCharacter; } 
        set { curCharacter = value; } 
    }

    [SerializeField] private Character[] characters;
    public Character GetCharacter(string charactorName)
    {
        foreach (Character character in characters)
        {
            if (character.characterName == charactorName)
                return character;
        }
        return characters[0] ;   // None Charactor
    }

    [SerializeField] private Dictionary<Player, Character> blueTeamsPlayer;
    public Dictionary<Player, Character> BlueTeamsPlayer
    {
        get { return blueTeamsPlayer; }
        set { blueTeamsPlayer = value; }
    }

    [SerializeField] private Dictionary<Player, Character> redTeamsPlayer;
    public Dictionary<Player, Character> RedTeamsPlayer
    {
        get { return redTeamsPlayer; }
        set 
        {
            redTeamsPlayer = value;
            //redPlayersForRoom.Clear();
            //foreach (Player player in redTeamsPlayer.Keys)
            //{
            //    redPlayersForRoom.Add(player);
            //}
            
        }
    }

    //private void Update()
    //{
    //    Debug.Log(redPlayersForRoom.Count);
    //}

    //private List<Player> bluePlayersForRoom;

    //private List<Player> redPlayersForRoom = new List<Player>();

    //[SerializeField] public Avatar[] avatars;
    //[SerializeField] public AnimatorController[] animators;
    public UnityAction OnChangeCharacter;

    public void ChangeCharacter(string characterName)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characterName == characters[i].characterName)
            {
                CurCharacter = characters[i];
                OnChangeCharacter?.Invoke(); //Player.csÀÇ ChangePlayerableCharacter
                Debug.Log($"DataManager: player's current character is {characters[i].characterName}");
            }
        }
    }
}
