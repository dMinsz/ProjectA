using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] public Character[] characters;
    [SerializeField] public Avatar[] avatars;
    public UnityAction OnChangeCharacter;

    public void ChangeCharacter(string characterName)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characterName == characters[i].characterName)
            {
                CurCharacter = characters[i];
                OnChangeCharacter?.Invoke();
            }
        }
    }
}
