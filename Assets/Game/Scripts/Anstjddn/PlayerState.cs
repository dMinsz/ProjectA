using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    private Character character;
    public float playercurhp;
    public float playermaxhp;
    [SerializeField] private UnityEvent ondied;
    private Animator anim;
    private bool isdie;

    public UnityAction die;
    private void Awake()
    {
        character = GameManager.Data.CurCharacter;

        if (character == null)
        {
            character = GameManager.Data.characters[1];//mario
        }

        playermaxhp = character.stat.hp;
        playercurhp = playermaxhp;
        anim = GetComponent<Animator>();
        isdie = false;
    }
    private void Update()
    {
        if (!isdie && playercurhp <= 0)
        {
            Destroy(gameObject, 2f);
            isdie = true;
            if (isdie)
            {
                ondied?.Invoke();
            }

        }
        //test
        if (Input.GetKey(KeyCode.Z))
        {
            playercurhp -= 10;
            Debug.Log(playercurhp);
        }
    }
    public void playerdie()
    {
        anim.SetTrigger("die");
    }



}
