using Photon.Realtime;
using System;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
public static class CustomProperty
{
    public const string READY = "Ready";
    public const string LOAD = "Load";
    public const string NUMBER = "Number";
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
        PhotonHashtable property = player.CustomProperties;
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
        PhotonHashtable property = player.CustomProperties;
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
        PhotonHashtable property = room.CustomProperties;
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
        PhotonHashtable property = player.CustomProperties;
        property[TEAM] = team;
        player.SetCustomProperties(property);
    }

    public static int GetTeamPlayersCount(this Room room, PlayerEntry.TeamColor teamColor)
    {
        PhotonHashtable property = room.CustomProperties;
        if (teamColor == PlayerEntry.TeamColor.Blue)
        {
            if (property.ContainsKey(BLUETEAMSCOUNT))
                return (int)property[BLUETEAMSCOUNT];
            else
                return -1;
        }
        else
        {
            if (property.ContainsKey(REDTEAMSCOUNT))
                return (int)property[REDTEAMSCOUNT];
            else
                return -1;
        }
    }

    public static void SetTeamPlayersCount(this Room room, PlayerEntry.TeamColor teamColor, int teamPlayersCount)
    {
        PhotonHashtable property = room.CustomProperties;
        if (teamColor == PlayerEntry.TeamColor.Blue)
        {
            property[BLUETEAMSCOUNT] = teamPlayersCount;
            room.SetCustomProperties(property);
        }
        else if (teamColor == PlayerEntry.TeamColor.Red)
        {
            property[REDTEAMSCOUNT] = teamPlayersCount;
            room.SetCustomProperties(property);
        }
    }
}
