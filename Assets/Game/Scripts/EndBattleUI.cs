using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndBattleUI : MonoBehaviour
{

    public Sprite[] BattleImage = new Sprite[3]; // blue win, red win , draw
    public GameObject image;
    public void ShowRedWinUI() 
    {
        image.SetActive(true);
        image.GetComponent<Image>().sprite = BattleImage[1];
    }

    public void ShowBlueWinUI() 
    {
        image.SetActive(true);
        image.GetComponent<Image>().sprite = BattleImage[0];
    }
    public void ShowDrawUI()
    {
        image.SetActive(true);
        image.GetComponent<Image>().sprite = BattleImage[2];
    }
}
