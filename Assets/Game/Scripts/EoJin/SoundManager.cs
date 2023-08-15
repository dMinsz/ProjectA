using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sound { Music, Effect, Voice };
    [SerializeField] public List<AudioClip> effectSounds = new List<AudioClip>();

    public void FindSoundByName(List<AudioClip> soundList, string name)
    {
        for (int i = 0; i < soundList.Count; i++)
        {
        }
    }

}
