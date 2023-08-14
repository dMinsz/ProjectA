using UnityEngine;
using Photon.Pun;

public class PuckDelayCompensation : MonoBehaviourPun, IPunObservable
{
    Vector3 latestPos;
    Quaternion latestRot;
    Vector3 latestVel;
    //Lag compensation
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;
    Vector3 velocityAtLastPacket = Vector3.zero;
    Rigidbody rb;

    public bool isSyncronize = true;
    private bool isSyncronizeLastPacket = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.velocity);
            stream.SendNext(isSyncronize);
        }
        else
        {
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            latestVel = (Vector3)stream.ReceiveNext();
            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
            velocityAtLastPacket = rb.velocity;
            isSyncronizeLastPacket = (bool)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {

            if (isSyncronizeLastPacket)
            {
                //Lag compensation
                double timeToReachGoal = currentPacketTime - lastPacketTime;
                currentTime += Time.deltaTime;

                //Update remote player
                transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
                float t = Mathf.Clamp((float)(currentTime / timeToReachGoal), 0f, 0.99f);
                transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, t);
                rb.velocity = Vector3.Lerp(velocityAtLastPacket, latestVel, (float)(currentTime / timeToReachGoal));
            }
            else
            {
                transform.position = latestPos;
                transform.rotation = latestRot;
                rb.velocity = latestVel;
            }
        }

    }

    public void SetSyncronize(bool setSyncro)
    {
        isSyncronize = setSyncro;
    }
}
