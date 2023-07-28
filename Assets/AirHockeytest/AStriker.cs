using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStriker : MonoBehaviour
{
    [SerializeField]AirHockeyManager gm;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        //gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Goal_Ai")
        {
            gm.pScore += 1f;
            gm.Reset(1);
        }
        if (c.tag == "Goal_User")
        {
            gm.eScore += 1f;
            gm.Reset(0);
        }
    }
}
