using UnityEngine;
using System.Collections;

public class SpwanManagerScript : MonoBehaviour {

    public delegate void SpawnPlayer(Vector3 position);
    public static event SpawnPlayer fireSpawn;

	// Use this for initialization
	void Start () {
        fireSpawn += spawn;
	}

    void spawn(Vector3 position)
    {

    }
}
