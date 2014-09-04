using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManageDeplacementClass{
    public NetworkViewID viewID;
    public GameObject player;

    public bool playerWantToMoveUp = false;
    public bool playerWantToMoveDown = false;
    public bool playerWantToMoveLeft = false;
    public bool playerWantToMoveRight = false;
    public bool playerWantToJump = false;
    public bool playerCanJump = true;

    public Vector3 playerDirection = new Vector3();

    public ManageDeplacementClass(NetworkViewID id,GameObject player)
    {
        this.viewID = id;
        this.player = player;
    }

    public bool playerWantDoSomething()
    {
        return playerWantToMoveUp || playerWantToMoveDown || playerWantToMoveLeft || playerWantToMoveRight || playerWantToJump;
    }

    /*if (!playerClass.viewID.isMine)
    Debug.Log(playerClass.playerWantToMoveUp + " \n " +
        playerClass.playerWantToMoveDown + " \n " +
        playerClass.playerWantToMoveLeft + " \n " +
        playerClass.playerWantToMoveRight + " \n " +
        playerClass.playerWantToJump + " \n ");*/
}
