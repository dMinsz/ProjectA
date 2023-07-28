using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AirHockeyManager : MonoBehaviour
{
    public GameObject striker, player, ai, pS, eS; //pS - playerScore, eS - enemyScore

    //for score
    public float pScore, eScore; //p-player,e-enemy


    // Use this for initialization
    void Start()
    {
        pScore = 0f;
        eScore = 0f;
        //striker = GameObject.FindGameObjectWithTag("Striker");
        //player = GameObject.FindGameObjectWithTag("Player");
        //ai = GameObject.Find("AI");
        //pS = GameObject.Find("P_Score");
        //eS = GameObject.Find("E_Score");
    }

    // Update is called once per frame
    void Update()
    {

        pS.gameObject.GetComponent<TMP_Text>().text = "YOU:" + pScore;
        eS.gameObject.GetComponent<TMP_Text>().text = "OPP:" + eScore;
    }

    public void Reset(int status)
    {
        //Reset all the gameObjects to original position to start a new game session
        if (status == 1) //recieved when striker hits any goal, check Striker.cs
            striker.transform.position = new Vector3(0.06f, striker.transform.position.y, 2.82f);//if  enemy wins, 
                                                                                                 //place on player side
        else
            striker.transform.position = new Vector3(0.06f, striker.transform.position.y, -4.59f);

        striker.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);//stop the striker
        player.transform.position = new Vector3(0.88f, player.transform.position.y, -8.17f);
        ai.transform.position = new Vector3(0.88f, ai.transform.position.y, 6.65f);
    }


}

