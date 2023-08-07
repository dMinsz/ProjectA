using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalChecker : MonoBehaviourPun
{
    public enum team
    {
        blue,
        red,
    }

    public ScoreChecker scoreChecker;
    public team myTeam;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (myTeam == team.blue)
                {
                    photonView.RPC("ScoreRed", RpcTarget.AllViaServer);
                }
                else if (myTeam == team.red)
                {
                    photonView.RPC("ScoreBlue", RpcTarget.AllViaServer);
                }
            }
            //else 
            //{
            //    if (myTeam == team.blue)
            //    {
            //        photonView.RPC("Scoring", RpcTarget.MasterClient , 0);
            //    }
            //    else if (myTeam == team.red)
            //    {
            //        photonView.RPC("Scoring", RpcTarget.MasterClient , 1);
            //    }
            //}
        }
    }


    [PunRPC]
    public void Scoring(int team) 
    {
        if (team == 0) // blue
        {
            photonView.RPC("ScoreRed", RpcTarget.AllViaServer);
        }
        else//red 
        {
            photonView.RPC("ScoreBlue", RpcTarget.AllViaServer);
        }
    }


    [PunRPC]
    public void ScoreRed() 
    {
        scoreChecker.ScoreRed();
    }

    [PunRPC]
    public void ScoreBlue()
    {
        scoreChecker.ScoreBlue();
    }

}
