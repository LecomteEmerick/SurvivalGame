using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientFolderScript : MonoBehaviour {

    [SerializeField]
    private GameObject playerPrefab;

    public List<NetworkViewID> playerList;

    private StaticVariableScript setting;

	// Use this for initialization
	void Start () {
        setting = new StaticVariableScript();
        if (Network.isClient || Network.isServer)
        {
            playerList = new List<NetworkViewID>();
        }
	}

    void OnPlayerConnected()
    {
        if(Network.isServer)
        {
            GameObject newPlayer = (GameObject)Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 1);
            playerList.Add(newPlayer.networkView.viewID);
            networkView.RPC("addId", RPCMode.Others, newPlayer.networkView.viewID);
        }

    }

    [RPC]
    void addId()
    {
    }
}
