using anstjddn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerrange : MonoBehaviour
{
    [SerializeField] PlayerAim player;

    private float size;
    private void Awake()
    {
       
        size = player.attacksize *0.4f;
      transform.localScale = new Vector3(size, size, size);
    }
}
