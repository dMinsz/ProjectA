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
        if (forChange)
            data.ChangeCharacter("Mario");
        else
            data.ChangeCharacter("Link");

        forChange = !forChange;
    }
}
