using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerState : MonoBehaviourPun
{

    public List<GameObject> deactivatedObjects = new List<GameObject>();

    private Character character;
    public float playercurhp = 1;
    public float playermaxhp;
    [SerializeField] private UnityEvent ondied;
    [SerializeField] private UnityEvent onRespawn;
    private Animator anim;
    private bool isdie =false;


    PlayerSetup setup;

    Coroutine Routine;

    NetWorkedAnimation nAnim;
    private void Awake()
    {
        nAnim = GetComponent<NetWorkedAnimation>();
        setup = GetComponent<PlayerSetup>();
    }

    public void SetUp(Character nowCharacter) 
    {
        //anim = GetComponent<Animator>();
        //character = GameManager.Data.GetCharacter(nowCharacter.characterName);
        ////anim.runtimeAnimatorController = nowCharacter.animator;

        //playermaxhp = character.stat.hp;
        //playercurhp = playermaxhp;
        //isdie = false;

        photonView.RPC("ResultSetUp", RpcTarget.All, nowCharacter.characterName);
    }

    [PunRPC]
    private void ResultSetUp(string characterName) 
    {
        anim = GetComponent<Animator>();
        character = GameManager.Data.GetCharacter(characterName);
        //anim.runtimeAnimatorController = nowCharacter.animator;

        playermaxhp = character.stat.hp;
        playercurhp = playermaxhp;
        isdie = false;
    }

    private void Update()
    {
        if (!isdie && playercurhp <= 0)
        {
            //Destroy(gameObject, 2f);
            isdie = true;
            if (isdie)
            {
                if (GetComponent<playercontroll>().mainRoutine != null) // dash Remove
                {
                    StopCoroutine(GetComponent<playercontroll>().mainRoutine);
                }

                nAnim.SendPlayAnimationEvent(photonView.ViewID, "die", "Trigger");
                ondied?.Invoke();
            }
            Routine = StartCoroutine(Respawn());
            Deactivate();

        }
        //test
        //if (Input.GetKey(KeyCode.Z))
        //{
        //    playercurhp -= 10;
        //    Debug.Log(playercurhp);
        //}
    }

    [PunRPC]
    private void playerdie(string name)
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger(name);
    }


    public void Deactivate() 
    {
        foreach (var item in deactivatedObjects)
        {
            item.SetActive(false);
        }
    }

    public void Activate()
    {
        foreach (var item in deactivatedObjects)
        {
            item.SetActive(true);
        }
    }


    IEnumerator Respawn() 
    {
        GetComponent<PlayerDelayCompensation>().SetSyncronize(false);
        yield return new WaitForSeconds(1f);

        playercurhp = playermaxhp;


        transform.position = GetComponent<PlayerSetup>().originPos;
        transform.rotation = GetComponent<PlayerSetup>().originRot;

        GetComponent<PlayerDelayCompensation>().SetSyncronize(true);
        Activate();

        setup.ReSetUp();
        nAnim.SendPlayAnimationEvent(photonView.ViewID, "move", "Bool", true);
        isdie = false;
        onRespawn?.Invoke();

    }


}
