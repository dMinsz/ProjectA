using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopUpUI : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;
    [SerializeField] GameObject reconfirmUI;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectSlider;
    [SerializeField] Slider voiceSlider;

    private float musicValue;
    private float effectValue;
    private float voiceValue;

    private void Awake()
    {
        soundManager = GetComponent<SoundManager>();
    }

    private void OnEnable()
    {
        musicValue = musicSlider.value;
        effectValue = effectSlider.value;
        voiceValue = voiceSlider.value;
        reconfirmUI.gameObject.SetActive(false);
    }

    public void OnQuitButton()
    {
        if (musicSlider.value != musicValue || effectSlider.value != effectValue || voiceSlider.value != voiceValue)
            reconfirmUI.gameObject.SetActive(true);
        else
            this.gameObject.SetActive(false);
    }

    public void OnSaveButton()
    {
        this.gameObject.SetActive(false);
    }

    public void OnCansleButton()
    {
        musicSlider.value = musicValue;
        effectSlider.value = effectValue;
        voiceSlider.value = voiceValue;

        this.gameObject.SetActive(false);
    }
}
