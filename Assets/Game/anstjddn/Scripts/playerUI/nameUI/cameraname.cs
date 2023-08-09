using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class cameraname : BaseUI
{

    public TMP_Text playername;
    public string nickname;
    protected override void Awake()
    {
        base.Awake();
        playername = texts["Playername"];
        playername.text = nickname;
    }

    private void LateUpdate()
    {
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

    }
}
