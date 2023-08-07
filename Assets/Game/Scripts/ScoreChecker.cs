using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreChecker : MonoBehaviour
{
    public int MaxGoal = 5;
    public TMP_Text blueScore;
    public TMP_Text redScore;
    public TMP_Text timeBoard;

    public TMP_Text testEnd;

    public float roundTime;

    public UnityEvent BlueTeamScoreEvent;
    public UnityEvent RedTeamScoreEvent;

    private int BluePoint = 0;
    private int RedPoint = 0;

    private PlayManager pm;

    Coroutine mainRoutine;
    private void Awake()
    {
        pm = GetComponent<PlayManager>();
        blueScore.text = (BluePoint).ToString();
        redScore.text = (RedPoint).ToString();
    }

    private void OnDisable()
    {
        StopCoroutine(mainRoutine);
    }

    public void StartTimer() 
    {
        mainRoutine = StartCoroutine(TimeTicking());
    }
    public void ScoreBlue() 
    {
        pm.DiablePuck();
        pm.ResetRound();
        var nowScore = ++BluePoint;
        blueScore.text = nowScore.ToString();
    }


    public void ScoreRed()
    {
        pm.DiablePuck();
        pm.ResetRound();
        var nowScore = ++RedPoint;
        redScore.text = nowScore.ToString();
    }

    IEnumerator TimeTicking() 
    {
        // Time Syncronize by Sever Time

        int oldtime = PhotonNetwork.ServerTimestamp;

        while (roundTime > (PhotonNetwork.ServerTimestamp - oldtime) / 1000f)
        {
            int remainTime = (int)(roundTime - (PhotonNetwork.ServerTimestamp - oldtime) / 1000f) + 1;

            var minuate= remainTime/60 % 60;
            string seonds = (remainTime % 60).ToString();
            
            if (seonds == "0")
                seonds = "00";
            
            timeBoard.text = $"{minuate}:{seonds}";
            yield return new WaitForEndOfFrame();


            if (BluePoint >= MaxGoal || RedPoint >= MaxGoal)
            {
                break;
            }

        }
        //end round

        EndBattle();

        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    //just test
    public void EndBattle() 
    {
        pm.DiablePuck();

        testEnd.gameObject.SetActive(true);

        if (BluePoint == RedPoint)
        {
            testEnd.text = "Draw!";
        }
        else if (BluePoint > RedPoint)
        {
            testEnd.text = "Blue Team Win!";
        }
        else 
        {
            testEnd.text = "Red Team Win!";
        }
        
    }
}
