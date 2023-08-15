using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class StatePanel : MonoBehaviour
{
    //[SerializeField] RectTransform content;
    //[SerializeField] TMP_Text logPrefab;
    [SerializeField] GameObject UI;
    [SerializeField] TMP_Text textArea;

    private ClientState state;

    public static StatePanel Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (state == PhotonNetwork.NetworkClientState)
            return;

        state = PhotonNetwork.NetworkClientState;

        //TMP_Text newLog = Instantiate(logPrefab, content);
        //newLog.text = string.Format("[Photon] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), state);
        //NewText(string.Format("[Photon] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), state));
        Debug.Log(string.Format("[Photon] {0}", state));
    }

    public void AddMessage(string message)
    {
        //TMP_Text newLog = Instantiate(logPrefab, content);
        //newLog.text = string.Format("[Photon] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), message);
        //NewText(string.Format("[Photon] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), message));
        NewText(message);
        Debug.Log(string.Format("[Photon] {0}", message));
    }

    public void NewText(string newText)
    {
        UI.SetActive(true);
        textArea.text = newText;
    }

    public void CloseButton()
    {
        textArea.text = "";
        UI.SetActive(false);
    }
}
