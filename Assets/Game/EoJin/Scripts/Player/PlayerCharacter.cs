using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerCharacter : MonoBehaviour
{
    private DataManager data;
    private PlayerAttacker attacker;
    [SerializeField] Character character;
    Animator anim;
    bool forChange;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        attacker = gameObject.GetComponent<PlayerAttacker>();
        anim = GetComponent<Animator>();
        data.CurCharacter = character;
    }

    public void OnEnable()
    {
        data.OnChangeCharacter += ChangePlayerableCharacter;
    }

    public void OnDisable()
    {
        data.OnChangeCharacter -= ChangePlayerableCharacter;
    }

    public void ChangePlayerableCharacter()
    {
        character = data.CurCharacter;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == data.CurCharacter.characterName)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }

        anim.avatar = data.CurCharacter.avatar;
    }


}
