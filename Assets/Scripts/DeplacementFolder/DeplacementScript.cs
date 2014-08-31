using UnityEngine;
using System.Collections;

public class DeplacementScript : MonoBehaviour {

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private float walkSpeed = 5.0f;

    private bool playerWantToMoveUp = false;
    private bool playerWantToMoveDown = false;
    private bool playerWantToMoveLeft = false;
    private bool playerWantToMoveRight = false;

	// Use this for initialization
	void Start () {
	    
	}

    void Update()
    {
        checkDeplacement();
    }

	// Update is called once per frame
	void FixedUpdate () {
        deplacement();
	}

    public void deplacement()
    {
        if (playerWantToMoveUp)
            _player.position += Camera.main.transform.forward *
                    walkSpeed * Time.deltaTime;
        if (playerWantToMoveDown)
            _player.position += -Camera.main.transform.forward *
                    walkSpeed * Time.deltaTime;
        if (playerWantToMoveLeft)
            _player.position += -Camera.main.transform.right *
                    walkSpeed * Time.deltaTime;
        if (playerWantToMoveRight)
            _player.position += Camera.main.transform.right *
                    walkSpeed * Time.deltaTime;
    }

    public void checkDeplacement()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
            playerWantToMoveUp = true;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            playerWantToMoveDown = true;
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
            playerWantToMoveLeft = true;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            playerWantToMoveRight = true;
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.UpArrow))
            playerWantToMoveUp = false;
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            playerWantToMoveDown = false;
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.LeftArrow))
            playerWantToMoveLeft = false;
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            playerWantToMoveRight = false;
    }
}
