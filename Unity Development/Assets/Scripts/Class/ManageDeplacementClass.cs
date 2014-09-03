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

    public ManageDeplacementClass(NetworkViewID id,GameObject player)
    {
        this.viewID = id;
        this.player = player;
    }
}
