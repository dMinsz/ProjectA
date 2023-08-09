using Photon.Realtime;
using System;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
public static class CustomProperty
{
    public const string READY = "Ready";
    public const string LOAD = "Load";
    public const string NUMBER = "Number";
    public const string CHARACTER = "Character";
    public const string TEAM = "Team";
    public const string BLUETEAMSCOUNT = "BlueTeamsCount";
    public const string REDTEAMSCOUNT = "RedTeamsCount";

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

    public static int GetRedTeamsCount(this Room room)
    {
        PhotonHashtable property = room.CustomProperties;
        if (property.ContainsKey(REDTEAMSCOUNT))
            return (int)property[REDTEAMSCOUNT];
        else
            return -1;
    }

    public static void SetBlueTeamsCount(this Room room, int blueTeamsCount)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[BLUETEAMSCOUNT] = blueTeamsCount;
        room.SetCustomProperties(property);
    }

    public static void SetRedTeamsCount(this Room room, int redTeamsCount)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[REDTEAMSCOUNT] = redTeamsCount;
        room.SetCustomProperties(property);
    }

    public static string GetCharacter(this Player player)   // int로 하는게 좋을 것 같음
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey(CHARACTER))
            return (string)property[CHARACTER];
        else
            return "";
    }

    public static void SetCharacter(this Player player, string character)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[CHARACTER] = character;
        player.SetCustomProperties(property);
    }
}
