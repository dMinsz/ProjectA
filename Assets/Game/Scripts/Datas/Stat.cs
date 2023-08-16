using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatData", menuName = "Scriptable Object/StatData")]
public class Stat : ScriptableObject
{
    [SerializeField] public int hp; //ü��
    [SerializeField] public int speed;

}
