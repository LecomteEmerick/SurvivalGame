using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticVariableScript : MonoBehaviour
{
    private string _pseudo;

    public string Pseudo
    {
        get { return _pseudo; }
        set { _pseudo = value; }
    }

    private static bool _isServer;

    public bool isServer
    {
        get { return _isServer; }
        set { _isServer = value; }
    }

    private static string _ip;

    public string ip
    {
        get { return _ip; }
        set { _ip = value; }
    }

    private static HostData _element;

    public HostData Element
    {
        get { return _element; }
        set { _element = value; }
    }

    private static string _gameName;

    public string GameName
    {
        get { return _gameName; }
        set { _gameName = value; }
    }

    private static List<string> _listClient;

    public List<string> ListClient
    {
        get { return _listClient;}
        set 
        {
            _listClient = value;
        }
    }

    private static List<ManageDeplacementClass> _playerList;

    public List<ManageDeplacementClass> playerList
    {
        get { return _playerList; }
        set { _playerList = value; }
    }

    public static ManageDeplacementClass findMyPlayer()
    {
        int i = 0;
        while (i < _playerList.Count)
        {
            if (_playerList[i].viewID.owner == Network.player )
                return _playerList[i];
            i++;
        }
        return null;
    }

    public static ManageDeplacementClass findId(NetworkViewID id)
    {
        int i = 0;
        while (i < _playerList.Count)
        {
            if (_playerList[i].viewID == id)
                return _playerList[i];
            i++;
        }
        return null;
    }
}
