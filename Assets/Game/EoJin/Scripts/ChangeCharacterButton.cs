using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacterButton : MonoBehaviour
{
    DataManager data;
    bool forChange;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
    }

    public void Change()
    {
        if (!forChange)
            data.ChangeCharacter("Mario"); //character.cs(scriptableObject)의 characterName에 있는 이름을 이용
        else
            data.ChangeCharacter("Link");

        forChange = !forChange;
    }
}
