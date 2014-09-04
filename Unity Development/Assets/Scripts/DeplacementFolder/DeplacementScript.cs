using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeplacementScript : MonoBehaviour {

    private ManageDeplacementClass myManage;
    private Transform _player;

    [SerializeField]
    private float walkSpeed = 15.0f;

    [SerializeField]
    private float jumpHight = 1000.0f;

    private StaticVariableScript setting;

	// Use this for initialization
	void Start () {
        setting = new StaticVariableScript();
	}

    void Update()
    {
        if (myManage == null && setting.playerList.Count > 0)
            myManage = StaticVariableScript.findMyPlayer();
        else
        {
            checkDeplacement();
            changeDirection();
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        deplacement();
	}

    public void deplacement()
    {
        foreach (ManageDeplacementClass playerClass in setting.playerList)
        {
            Transform playerTransf = playerClass.player.transform;
            Rigidbody playerRigid = playerClass.player.rigidbody;
            playerRigid.MovePosition(playerTransf.position + playerClass.playerDirection *
                    playerClass.walkSpeed * Time.deltaTime);
            if (playerClass.playerWantToJump)
            {
                playerRigid.AddForce(Vector3.up * jumpHight);
                playerClass.playerWantToJump = false;
            }
        }
    }

    public void checkDeplacement()
    {
        _player = StaticVariableScript.findMyPlayer().player.transform;
        if (Network.isClient)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
                networkView.RPC("playerWantToMoveUp", RPCMode.Server, myManage.viewID, true);
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                networkView.RPC("playerWantToMoveDown", RPCMode.Server, myManage.viewID, true);
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
                networkView.RPC("playerWantToMoveLeft", RPCMode.Server, myManage.viewID, true);
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                networkView.RPC("playerWantToMoveRight", RPCMode.Server, myManage.viewID, true);
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.UpArrow))
                networkView.RPC("playerWantToMoveUp", RPCMode.Server, myManage.viewID, false);
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                networkView.RPC("playerWantToMoveDown", RPCMode.Server, myManage.viewID, false);
            if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.LeftArrow))
                networkView.RPC("playerWantToMoveLeft", RPCMode.Server, myManage.viewID, false);
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
                networkView.RPC("playerWantToMoveRight", RPCMode.Server, myManage.viewID, false);
            if (Mathf.Abs(_player.rigidbody.velocity.y) < 0.03 && !myManage.playerCanJump)
                networkView.RPC("playerCanJump", RPCMode.Server, myManage.viewID, true);
            if (Input.GetKey(KeyCode.Space) && myManage.playerCanJump)
            {
                networkView.RPC("playerWantToJump", RPCMode.Server, myManage.viewID, true);
                networkView.RPC("playerCanJump", RPCMode.Server, myManage.viewID, false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
                playerWantToMoveUp(myManage.viewID, true);
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                playerWantToMoveDown(myManage.viewID, true);
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
                playerWantToMoveLeft(myManage.viewID, true);
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                playerWantToMoveRight(myManage.viewID, true);
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.UpArrow))
                playerWantToMoveUp(myManage.viewID, false);
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                playerWantToMoveDown(myManage.viewID, false);
            if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.LeftArrow))
                playerWantToMoveLeft(myManage.viewID, false);
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
                playerWantToMoveRight(myManage.viewID, false);
            if (Mathf.Abs(_player.rigidbody.velocity.y) < 0.03 && !myManage.playerCanJump)
                playerCanJump(myManage.viewID, true);
            if (Input.GetKey(KeyCode.Space) && myManage.playerCanJump)
            {
                playerWantToJump(myManage.viewID, true);
                playerCanJump(myManage.viewID, false);
            }
        }

    }

    private void changeDirection()
    {
        Vector3 direction = Vector3.zero;
        if (myManage.playerWantToMoveUp)
            direction += Camera.main.transform.forward;
        if (myManage.playerWantToMoveDown)
            direction += -Camera.main.transform.forward;
        if (myManage.playerWantToMoveLeft)
            direction += -Camera.main.transform.right;
        if (myManage.playerWantToMoveRight)
            direction += Camera.main.transform.right;
        direction.y = 0;
        if (direction != Vector3.zero || myManage.playerDirection != Vector3.zero)
        {
            if (Network.isServer)
                playerDirectionSet(myManage.viewID, direction);
            else
                networkView.RPC("playerDirectionSet", RPCMode.Server, myManage.viewID, direction);
        }
        myManage.playerDirection = direction;
    }

    [RPC]
    void playerWantToMoveUp(NetworkViewID id, bool want)
    {
        ManageDeplacementClass playerSettings;
        if ((playerSettings = StaticVariableScript.findId(id)) != null)
            playerSettings.playerWantToMoveUp = want;
        if (Network.isServer)
            networkView.RPC("playerWantToMoveUp", RPCMode.Others, id, want);
    }

    [RPC]
    void playerWantToMoveDown(NetworkViewID id, bool want)
    {
        ManageDeplacementClass playerSettings;
        if ((playerSettings = StaticVariableScript.findId(id)) != null)
            playerSettings.playerWantToMoveDown = want;
        if (Network.isServer)
            networkView.RPC("playerWantToMoveDown", RPCMode.Others, id, want);
    }

    [RPC]
    void playerWantToMoveLeft(NetworkViewID id, bool want)
    {
        ManageDeplacementClass playerSettings;
        if ((playerSettings = StaticVariableScript.findId(id)) != null)
            playerSettings.playerWantToMoveLeft = want;
        if (Network.isServer)
            networkView.RPC("playerWantToMoveLeft", RPCMode.Others, id, want);
    }

    [RPC]
    void playerWantToMoveRight(NetworkViewID id, bool want)
    {
        ManageDeplacementClass playerSettings;
        if ((playerSettings = StaticVariableScript.findId(id)) != null)
            playerSettings.playerWantToMoveRight = want;
        if (Network.isServer)
            networkView.RPC("playerWantToMoveRight", RPCMode.Others, id, want);
    }

    [RPC]
    void playerWantToJump(NetworkViewID id, bool want)
    {
        ManageDeplacementClass playerSettings;
        if ((playerSettings = StaticVariableScript.findId(id)) != null)
            playerSettings.playerWantToJump = want;
        if (Network.isServer)
            networkView.RPC("playerWantToJump", RPCMode.Others, id, want);
    }

    [RPC]
    void playerCanJump(NetworkViewID id, bool want)
    {
        ManageDeplacementClass playerSettings;
        if ((playerSettings = StaticVariableScript.findId(id)) != null)
            playerSettings.playerCanJump = want;
        if (Network.isServer)
            networkView.RPC("playerCanJump", RPCMode.Others, id, want);
    }

    [RPC]
    void playerDirectionSet(NetworkViewID id, Vector3 direction)
    {
        ManageDeplacementClass playerSettings;
        if ((playerSettings = StaticVariableScript.findId(id)) != null)
            playerSettings.playerDirection = direction;
        if (Network.isServer)
            networkView.RPC("playerDirectionSet", RPCMode.Others, id, direction);
    }

    [RPC]
    void playerWantToRun(NetworkViewID id, bool want)
    {
        if(
    }
}
