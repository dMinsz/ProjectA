using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharactersData", menuName = "Scriptable Object/CharactersData")]
public class Characters : ScriptableObject
{
    [SerializeField] public List<Character> characters;
}
