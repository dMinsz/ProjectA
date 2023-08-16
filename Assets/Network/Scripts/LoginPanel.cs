using MySql.Data.MySqlClient;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField PWInputField;
    [SerializeField] DBManager DB;
    [SerializeField] LobbyManager Lm;
    [SerializeField] Camera loginPanelCamera;
    [SerializeField] GameObject loginMap;
    [SerializeField] 

    private void OnEnable()
    {
        loginPanelCamera.enabled = true;
        loginMap.SetActive(true);
        idInputField.text = "";
        PWInputField.text = "";
    }

    private void Start()
    {
        DB.Connect();
    }

    private void Update()
    {
        if (idInputField.isFocused && Input.GetKey(KeyCode.Tab))
            Tab();

        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            Enter();
    }

    private void Tab()
    {
        PWInputField.Select();
    }

    private void Enter()
    {
        Login();
    }

    public void Login()
    {
        try
        {
            string id = idInputField.text;
            string pass = PWInputField.text;

            string sqlCommand = string.Format("SELECT id,password FROM user_info WHERE id='{0}'", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, DB.connection);
            DB.reader = cmd.ExecuteReader();

            if (DB.reader.HasRows)
            {
                while (DB.reader.Read())
                {
                    string readID = DB.reader["id"].ToString();
                    string readPass = DB.reader["password"].ToString();

                    //Debug.Log($"id : {readID}, password : {readPass}");

                    if (pass == readPass)
                    {
                        PhotonNetwork.LocalPlayer.NickName = idInputField.text;
                        PhotonNetwork.ConnectUsingSettings();
                        if (!DB.reader.IsClosed)
                            DB.reader.Close();

                        loginPanelCamera.enabled = false;
                        loginMap.SetActive(false);
                        return;
                    }
                    else
                    {
                        StatePanel.Instance.AddMessage($"Wrong Password");
                        Debug.Log("Wrong Password");
                        if (!DB.reader.IsClosed)
                            DB.reader.Close();
                    }
                }
            }
            else
            {
                StatePanel.Instance.AddMessage($"There is no playerID");
                Debug.Log("There is no playerID");
            }
            if (!DB.reader.IsClosed)
                DB.reader.Close();
        }
        catch (Exception ex)
        {
            //StatePanel.Instance.AddMessage($"Login error : {ex.Message}");
            //StatePanel.Instance.AddMessage($"Please check the server");
            Debug.Log(ex.Message);
            if (!DB.reader.IsClosed)
                DB.reader.Close();
        }

    }

    public void GotoSignUp() 
    {
        Lm.SetActivePanel(LobbyManager.Panel.SignUp);
    }

    public void OnExitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
