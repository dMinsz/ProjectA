using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DataManager : MonoBehaviour
{
    [SerializeField] private int level;
    public int Level { get { return level; } set { level = value; } }

    [SerializeField] private float exp;
    private float Exp { get { return exp; } set { exp = value; } }


    [SerializeField]  public List<string> RedTeamPlayerNameList = new List<string>();
    [SerializeField]  public List<string> BlueTeamPlayerNameList = new List<string>();

    private void Awake()
    {
        characters = GameManager.Resource.Load<Characters>("CharactersData").characters;

        var test = characters;
    }

    [SerializeField] private Character curCharacter;
    public Character CurCharacter 
    { 
        get { return curCharacter; } 
        set { curCharacter = value; } 
    }

    [SerializeField] public List<Character> characters;
    
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
        set { blueTeamsPlayer = value;
}
    }

    [SerializeField] private Dictionary<Player, Character> redTeamsPlayer;
    public Dictionary<Player, Character> RedTeamsPlayer
    {
        get { return redTeamsPlayer; }
        set { redTeamsPlayer = value; }
    }
    private void Update()
    {
        //foreach (Player player in blueTeamsPlayer.Keys)
        //{
        //    Debug.Log($"BlueTemasPlayer.NickName : {player.NickName}");
        //    Debug.Log($"BlueTemasPlayer.GetCharacterName : {player.GetCharacterName()}");
        //}

        //foreach (Character character in blueTeamsPlayer.Values)
        //{
        //    Debug.Log($"BlueTemasPlayer.Values.CharacterName : {character.characterName}");
        //}

        //foreach (Player player in redTeamsPlayer.Keys)
        //{
        //    Debug.Log($"redTemasPlayer.NickName : {player.NickName}");
        //    Debug.Log($"redTemasPlayer.GetCharacterName : {player.GetCharacterName()}");
        //}

        //foreach (Character character in redTeamsPlayer.Values)
        //{
        //    Debug.Log($"redTemasPlayer.Values.CharacterName : {character.characterName}");
        //}

    }

    public UnityAction OnChangeCharacter;

    public void ChangeCharacter(string characterName)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characterName == characters[i].characterName)
            {
                CurCharacter = characters[i];
                OnChangeCharacter?.Invoke(); //Player.cs�� ChangePlayerableCharacter
               // Debug.Log($"DataManager: player's current character is {characters[i].characterName}");
            }
        }
    }
}
