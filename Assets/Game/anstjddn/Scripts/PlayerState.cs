using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    [SerializeField] public Character character;
    public float playercurhp;
    public float playermaxhp;
    [SerializeField] private UnityEvent ondied;
    private Animator anim;
    private bool isdie;

    public UnityAction die;
    private void Awake()
    {
        playermaxhp = character.stat.hp;
        playercurhp = playermaxhp;
        anim = GetComponent<Animator>();
        isdie = false;
    }

    private void Start()
    {
        StartCoroutine(died());
    }
  private void Update()
    {
      /*  if (!isdie&&playercurhp <= 0)
        {
            Debug.Log("Á×À½");
            Destroy(gameObject,3f);
             isdie=true;
               if (isdie)
               {
                   ondied?.Invoke();
               }
 
        }*/
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

    public void check()
    {
        if (playercurhp > 0)
        {
            return;
        }
        else
        {
            ondied?.Invoke();
        }
    }
    IEnumerator died()
    {
        while (playercurhp>0)
        {
            yield return new WaitForSeconds(0.1f);
            check();
        }
        yield return null;
    }
  
}
