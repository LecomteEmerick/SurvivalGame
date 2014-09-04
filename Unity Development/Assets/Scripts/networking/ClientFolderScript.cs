using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientFolderScript : MonoBehaviour {

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject camPrefab;

    [SerializeField]
    private GameObject lampPrefab;

    private StaticVariableScript setting;

	// Use this for initialization
	void Start () {
        setting = new StaticVariableScript();
        setting.playerList = new List<ManageDeplacementClass>();
        if (Network.isServer)
            instantiateMyPlayer();
        else
            networkView.RPC("requestPlayerList", RPCMode.Server, Network.player);

	}

    void instantiateMyPlayer()
    {
        GameObject newPlayer = (GameObject)Network.Instantiate(playerPrefab, Vector3.up, Quaternion.identity, 1);
        GameObject cam = (GameObject)Instantiate(camPrefab, Vector3.zero, Quaternion.identity);
        cam.transform.parent = newPlayer.transform;
        cam.transform.localPosition = new Vector3(0, 1, 0);
        GameObject lamp = (GameObject)Network.Instantiate(lampPrefab, Vector3.zero, Quaternion.identity, 2);
        lamp.transform.parent = newPlayer.transform;
        lamp.transform.localPosition = new Vector3(0, 1, 0);
        lamp.GetComponent<MouseLook>().enabled = true;
        //
        networkView.RPC("parentingObject", RPCMode.Others, newPlayer.networkView.viewID, lamp.networkView.viewID);
        //
        setting.playerList.Add(new ManageDeplacementClass(newPlayer.networkView.viewID, newPlayer));
        networkView.RPC("addPlayer", RPCMode.Others, newPlayer.networkView.viewID);
            
    }

    private List<NetworkViewID> getListOfViewId()
    {
        List<NetworkViewID> list = new List<NetworkViewID>();
        foreach(ManageDeplacementClass player in setting.playerList)
        {
            list.Add(player.viewID);
        }
        return list;
    }

    [RPC]
    void requestPlayerList(NetworkPlayer player)
    {
        if(Network.isServer)
        {
            foreach(NetworkViewID id in getListOfViewId())
                networkView.RPC("addPlayer", player, id);
            networkView.RPC("addClientPlayer", player);
        }
    }

    [RPC]
    void addClientPlayer()
    {
        instantiateMyPlayer();
    }

    [RPC]
    void addPlayer(NetworkViewID playerID)
    {
        setting.playerList.Add(new ManageDeplacementClass(playerID, NetworkView.Find(playerID).gameObject));
    }

    [RPC]
    void parentingObject(NetworkViewID objectParent, NetworkViewID objectChild)
    {
        GameObject parent = NetworkView.Find(objectParent).gameObject;
        GameObject child = NetworkView.Find(objectChild).gameObject;
        if (parent != null && child != null)
            child.transform.parent = parent.transform;
        else
            Debug.Log("parent : " + parent + "\nChild : " + child);
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        ManageDeplacementClass myPlayer = StaticVariableScript.findMyPlayer();
        Network.Destroy(myPlayer.player);
        Network.RemoveRPCs(myPlayer.viewID);
        setting.playerList.Remove(myPlayer);
    }
}
