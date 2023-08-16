using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Object/EffectData")]
public class Effect : ScriptableObject
{
    [SerializeField] public GameObject effectPrefab;
    [SerializeField] public float additionalTime;

}
