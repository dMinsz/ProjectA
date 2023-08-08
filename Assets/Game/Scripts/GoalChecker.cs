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
        if (PhotonNetwork.IsMasterClient)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
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
