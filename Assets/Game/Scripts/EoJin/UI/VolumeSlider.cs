using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;


    public void Music(Slider music)
    {
        soundManager.VolumeControl(SoundManager.Sound.Music, music.value);
    }

    public void Effect(Slider effect)
    {
        soundManager.VolumeControl(SoundManager.Sound.Effect, effect.value);
    }

    public void Voice(Slider voice)
    {
        soundManager.VolumeControl(SoundManager.Sound.Voice, voice.value);
    }
}
