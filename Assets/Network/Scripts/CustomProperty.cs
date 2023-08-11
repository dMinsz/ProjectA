using Photon.Realtime;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
public static class CustomProperty
{
    public const string READY = "Ready";
    public const string LOAD = "Load";
    public const string NUMBER = "Number";
    public const string CHARACTERNAME = "CharacterName";
    public const string TEAM = "Team";
    public const string BLUETEAMSCOUNT = "BlueTeamsCount";
    public const string REDTEAMSCOUNT = "RedTeamsCount";
    public const string BLUETEAMSPLAYERLIST = "BlueTeamsPlayerList";
    public const string REDTEAMSPLAYERLIST = "RedTeamsPlayerList";

    public const string LOADTIME = "LoadTime";
    
    public static bool GetReady(this Player player)
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey(READY))
            return (bool)property[READY];
        else
            return false;
    }

    public static void SetReady(this Player player, bool ready)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[READY] = ready;
        player.SetCustomProperties(property);
    }

    public static bool GetLoad(this Player player)
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey(LOAD))
            return (bool)property[LOAD];
        else
            return false;
    }

    public static void SetLoad(this Player player, bool load)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[LOAD] = load;
        player.SetCustomProperties(property);
    }

    public static int GetLoadTime(this Room room)
    {
        PhotonHashtable property = room.CustomProperties;
        if (property.ContainsKey(LOADTIME))
            return (int)property[LOADTIME];
        else
            return -1;
    }

    public static void SetLoadTime(this Room room, int loadTime)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[LOADTIME] = loadTime;
        room.SetCustomProperties(property);
    }

    public static int GetTeamColor(this Player player)
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey(TEAM))
            return (int)property[TEAM];
        else
            return 0;
    }

    public static void SetTeamColor(this Player player, int team)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[TEAM] = team;
        player.SetCustomProperties(property);
    }

    public static int GetBlueTeamsCount(this Room room)
    {
        PhotonHashtable property = room.CustomProperties;
        if (property.ContainsKey(BLUETEAMSCOUNT))
            return (int)property[BLUETEAMSCOUNT];
        else
            return -1;
    }

    public static void SetBlueTeamsCount(this Room room, int blueTeamsCount)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[BLUETEAMSCOUNT] = blueTeamsCount;
        room.SetCustomProperties(property);
    }

    public static int GetRedTeamsCount(this Room room)
    {
        PhotonHashtable property = room.CustomProperties;
        if (property.ContainsKey(REDTEAMSCOUNT))
            return (int)property[REDTEAMSCOUNT];
        else
            return -1;
    }

    public static void SetRedTeamsCount(this Room room, int redTeamsCount)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[REDTEAMSCOUNT] = redTeamsCount;
        room.SetCustomProperties(property);
    }

    //public static Array GetBlueTeamPlayerList(this Room room)
    //{
    //    PhotonHashtable property = room.CustomProperties;
    //    if (property.ContainsKey(BLUETEAMSPLAYERLIST))
    //        return (Array)property[BLUETEAMSPLAYERLIST];
    //    else
    //        throw new KeyNotFoundException("CharacterList not found in custom properties.");
    //}

    //public static void SetBlueTeamPlayerList(this Room room, List<string> blueTeamsPlayerList)
    //{
    //    PhotonHashtable property = new PhotonHashtable();
    //    property[BLUETEAMSPLAYERLIST] = blueTeamsPlayerList.ToArray();
    //    room.SetCustomProperties(property);
    //}

    public static string GetCharacterName(this Player player)
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey(CHARACTERNAME))
            return (string)property[CHARACTERNAME];
        else
            return "None";
            //throw new KeyNotFoundException("Character name not found in custom properties.");
    }

    public static void SetCharacterName(this Player player, string characterName)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[CHARACTERNAME] = characterName;
        player.SetCustomProperties(property);
    }
}
