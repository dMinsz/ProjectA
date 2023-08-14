using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignUpPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TMP_InputField PWInputField;
    [SerializeField] DBManager DB;
    [SerializeField] LobbyManager Lm;

    bool canUsedId = false;
    private void OnEnable()
    {
        if (DB.isConnect == false)
        {
            DB.Connect();
        }
    }

    public void CheckID() 
    {
        try 
        {
            string id = idInputField.text;
            if (id == null) 
            {
                StatePanel.Instance.AddMessage($"CheckID is null error :");
            }

            string sqlCommand = string.Format("SELECT id FROM user_info WHERE id='{0}'", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, DB.connection);
            DB.reader = cmd.ExecuteReader();

            if (!DB.reader.HasRows)
            {
                canUsedId = true;
                StatePanel.Instance.AddMessage($"That ID can be used.");

                if (!DB.reader.IsClosed)
                    DB.reader.Close();
            }
            else 
            {
                canUsedId = false;
                StatePanel.Instance.AddMessage($"That ID cannot be used.");

                if (!DB.reader.IsClosed)
                    DB.reader.Close();
            }

        }
        catch (Exception ex)
        {
            StatePanel.Instance.AddMessage($"SignUp CheckID error : {ex.Message}");
            Debug.Log(ex.Message);
        }
    }

    public void SignUp() 
    {
        try
        {

            if (canUsedId == false) 
            {
                Debug.Log("Please check id duplicate");
                //StatePanel.Instance.AddMessage("Please check id duplicate");
                return;
            }

            string id = idInputField.text;
            string name = nameInputField.text;
            string pass = PWInputField.text;

            string sqlCommand = string.Format("INSERT INTO user_info (id,nickname,password) VALUES ( '{0}','{1}','{2}')", id , name, pass);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, DB.connection);


            if (cmd.ExecuteNonQuery() == 1)
            {
                //StatePanel.Instance.AddMessage(name + "Member registration complete, id : " + id);
                if (!DB.reader.IsClosed)
                    DB.reader.Close();
            }
            else
            {
                //StatePanel.Instance.AddMessage("registration error");
                if (!DB.reader.IsClosed)
                    DB.reader.Close();
            }

            Lm.SetActivePanel(LobbyManager.Panel.Login);

        }
        catch (Exception ex)
        {
            //StatePanel.Instance.AddMessage($"SignUp error : {ex.Message}");
            Debug.Log(ex.Message);
        }
    }


}
