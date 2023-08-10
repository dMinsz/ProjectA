using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacterButton : MonoBehaviour
{
    DataManager data;
    int index;

    public void Awake()
    {
        data = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
    }

    public void Change()
    {
        data.ChangeCharacter(data.characters[index].characterName);
        index++;

        if (index >= data.characters.Length)
            index = 0;
    }
}
