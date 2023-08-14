using TMPro;
using UnityEngine;

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
