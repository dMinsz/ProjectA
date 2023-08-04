using LitJson;
using MySql.Data.MySqlClient;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DBManager : MonoBehaviour, IPunObservable
{
    public MySqlConnection connection;
    public MySqlDataReader reader;

    public bool isConnect = false;


    public void Connect()
    {
        try 
        {
            string DB_PATH = System.IO.Directory.GetCurrentDirectory() + "/ServerData";
            string serverinfo = SetServer(DB_PATH);

            if (serverinfo == string.Empty) 
            {
                Debug.Log("SQL_SERVER Null");
                return;
            }
            
            connection = new MySqlConnection(serverinfo);
            connection.Open();
            Debug.Log("SQL OPEN");
            isConnect = true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
       
    }

    private string SetServer(string path) 
    {
        try 
        {
            if (!File.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }

            string jsonstring = File.ReadAllText(path+"/Config.json");
            JsonData itemdata = JsonMapper.ToObject<JsonData>(jsonstring);
          

            string serverinfo = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3}; Port={4}; CharSet=utf8;",
                itemdata["IP"].ToString(), itemdata["DataBase"].ToString(), itemdata["ID"].ToString(),
                itemdata["PW"].ToString(), itemdata["Port"].ToString());

            return serverinfo;
        }
        catch(Exception e) 
        {
            Debug.LogError(e.Message);
            return string.Empty;
        }
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }
}
