using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerState : MonoBehaviour
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

    private void Awake()
    {
        setup = GetComponent<PlayerSetup>();
    }

    public void SetUp(Character nowCharacter) 
    {
        character = nowCharacter;

        playermaxhp = character.stat.hp;
        playercurhp = playermaxhp;
        anim = GetComponent<Animator>();
        isdie = false;
    }

    private void Update()
    {
        if (!isdie && playercurhp <= 0)
        {
            //Destroy(gameObject, 2f);
            Routine = StartCoroutine(Respawn());
            Deactivate();
            isdie = true;
            if (isdie)
            {
                ondied?.Invoke();
            }

        }
        //test
        //if (Input.GetKey(KeyCode.Z))
        //{
        //    playercurhp -= 10;
        //    Debug.Log(playercurhp);
        //}
    }
    public void playerdie()
    {
        anim.SetTrigger("die");
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

        isdie = false;
        onRespawn?.Invoke();

    }


}
