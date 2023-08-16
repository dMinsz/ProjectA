using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySoundSelector : MonoBehaviour
{
    public List<AudioClip> backGroundMusics = new List<AudioClip>();

    private AudioSource music;

    private void Awake()
    {
        music = GetComponent<AudioSource>();
    }


    public void PlayCharacterChooseSound() 
    {
        music.Stop();
        music.clip = backGroundMusics[1];
        music.Play();
    }

    public void ResetMusic()
    {
        music.Stop();
        music.clip = backGroundMusics[0];
        music.Play();
    }
}
