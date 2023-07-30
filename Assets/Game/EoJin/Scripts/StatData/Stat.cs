using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatData", menuName = "Scriptable Object/StatData")]
public class Stat : ScriptableObject
{
    [SerializeField] public int stagger; //Ã¼·Â
    [SerializeField] public int power;
    [SerializeField] public int speed;

    public void Init(Stat stat)
    {
        stat.stagger = this.stagger;
        stat.power = this.power;
        stat.speed = this.speed;
    }
}
