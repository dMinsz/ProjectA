using anstjddn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerrange : MonoBehaviour
{
    [SerializeField] PlayerAimTest player;

    private float size;
    private void Awake()
    {
       
        size = player.attacksize *1f;
      transform.localScale = new Vector3(size, size, size);
    }
}
