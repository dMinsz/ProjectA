
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
                OnChangeCharacter?.Invoke(); //Player.cs?? ChangePlayerableCharacter
                Debug.Log($"DataManager: player's current character is {characters[i].characterName}");
            }
        }
    }
}