using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [RequireComponent(typeof(Animator))]
  [RequireComponent(typeof(PhotonView))]
public class NetWorkedAnimation : MonoBehaviourPunCallbacks
{
    #region private fields
    Animator anim;
    #endregion

    #region monobehaviours
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    #endregion

    #region private methods
    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PlayAnimationEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int targetPhotonView = (int)data[0];
            if (targetPhotonView == this.photonView.ViewID)
            {
                string animatorParameter = (string)data[1];
                string parameterType = (string)data[2];
                object parameterValue = (object)data[3];
                switch (parameterType)
                {
                    case "Trigger":
                        {
                            anim = GetComponent<Animator>();
                            anim.SetTrigger(animatorParameter);
                        }
                        break;
                    case "Bool":
                        {
                            anim = GetComponent<Animator>();
                            anim.SetBool(animatorParameter, (bool)parameterValue);
                        }
                        break;
                    case "Float":
                        {
                            anim = GetComponent<Animator>();
                            anim.SetFloat(animatorParameter, (float)parameterValue);
                        }
                        break;
                    case "Int":
                        {
                            anim = GetComponent<Animator>();
                            anim.SetInteger(animatorParameter, (int)parameterValue);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
    #endregion

    #region public methods

    public const byte PlayAnimationEventCode = 1;

    public void SendPlayAnimationEvent(int photonViewID, string animatorParameter, string parameterType, object parameterValue = null)
    {
        object[] content = new object[] { photonViewID, animatorParameter, parameterType, parameterValue };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(PlayAnimationEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    #endregion
}