using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerCharacter : MonoBehaviour
{
    private DataManager data;
    [SerializeField] Character character;
    [SerializeField] GameObject charactersObj;
    Animator anim;
    [SerializeField] RuntimeAnimatorController animatorController;
    bool forChange;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        anim = GetComponent<Animator>();
        animatorController = GetComponent<Animator>().runtimeAnimatorController;
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

        for (int i = 0; i < charactersObj.transform.childCount; i++)
        {
            if (charactersObj.transform.GetChild(i).name == data.CurCharacter.characterName)
                charactersObj.transform.GetChild(i).gameObject.SetActive(true);
            else
                charactersObj.transform.GetChild(i).gameObject.SetActive(false);
        }

        anim.avatar = data.CurCharacter.avatar;
        anim.runtimeAnimatorController = data.CurCharacter.animator;
    }

}
