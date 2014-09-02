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

    public List<NetworkViewID> playerList;

    private StaticVariableScript setting;

	// Use this for initialization
	void Start () {
        setting = new StaticVariableScript();
        instantiateMyPlayer();
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
    }
}
