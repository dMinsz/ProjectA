using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class cameraHpbar : BaseUI
{
    [SerializeField] public Slider hpbar;
    [SerializeField] public PlayerState playerState;

    protected override void Awake()
    {
        base.Awake();
        hpbar = sliders["HpSlider"];
       

    }
    private void Start()
    {
        hpbar.minValue = 0;
        hpbar.maxValue = playerState.playermaxhp;
        hpbar.value = playerState.playercurhp;
    }
    private void Update()
    {
        hpbar.value = playerState.playercurhp;
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
