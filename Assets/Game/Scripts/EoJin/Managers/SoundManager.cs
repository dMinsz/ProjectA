using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public enum Sound { Master, Music, Effect, Voice };
    string str;

    /// <summary>
    /// value must be over 0, under 1
    /// </summary>
    /// <param name="soundType"></param>
    /// <param name="value"></param>
    public void VolumeControl(Sound soundType, float value)
    {
        switch (soundType)
        {
            case Sound.Master:
                str = "MasterVolume";
                break;
            case Sound.Music:
                str = "MusicVolume";
                break;
            case Sound.Effect:
                str = "EffectVolume";
                break;
            case Sound.Voice:
                str = "VoiceVolume";
                break;
        }

        if (str == null)
            return;

        audioMixer.SetFloat(str, Mathf.Log10(value) * 20);
    }
}
