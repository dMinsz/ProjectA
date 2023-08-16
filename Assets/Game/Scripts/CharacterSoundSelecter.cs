using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundSelecter : MonoBehaviour
{
    public List<AudioClip> voices = new List<AudioClip>();
    private AudioSource mainAudio;


    private void Awake()
    {
        mainAudio = GetComponent<AudioSource>();

        if (GameManager.Data.CurCharacter == null)
        {//for debug mode
            mainAudio.clip = voices[1];
        }
        else 
        {
            if (GameManager.Data.CurCharacter.hasFemaleVoice == true) // women voice
            {
                mainAudio.clip = voices[0];
            }
            else 
            {
                mainAudio.clip = voices[1];
            }
        }
    }




}
