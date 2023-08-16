using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntryColor : MonoBehaviour
{
    [SerializeField] TMP_Text readyText;
    [SerializeField] Image bar;

    private void Start()
    {
    }

    private void SetColorBlue()
    {
        readyText.color = Color.blue;
        bar.color = Color.blue;
    }

    private void SetColorRed()
    {
        readyText.color = Color.red;
        bar.color = Color.red;
    }
}