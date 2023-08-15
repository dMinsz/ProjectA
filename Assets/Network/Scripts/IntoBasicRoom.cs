using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoBasicRoom : MonoBehaviour
{
    public void IntoBasic()
    {
        StartCoroutine(IntoBasicRoomRoutine());
    }

    IEnumerator IntoBasicRoomRoutine()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;

        yield return new WaitForSeconds(1);

        PhotonNetwork.JoinOrCreateRoom("BasicRoom", roomOptions, TypedLobby.Default);

        yield break;
    }
}
