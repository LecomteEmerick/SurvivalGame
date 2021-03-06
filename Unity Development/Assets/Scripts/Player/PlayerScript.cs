﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    private PlayerCaracClass carac;

    public delegate IEnumerable DiePlayer(ManageDeplacementClass player, float timeRespawn);
    public static event DiePlayer died;

	// Use this for initialization
	void Start () {
        this.carac = new PlayerCaracClass();
	}

    public void addDamagePhysique(float damage)
    {
        this.carac.takeDamage(damage, true);
        Debug.Log(this.carac.life);
        if (!this.carac.isAlive)
        {
            ManageDeplacementClass deplacement = StaticVariableScript.findId(gameObject.networkView.viewID);
            if (deplacement != null)
                died(deplacement, this.carac.playerRespawnTime);
        }
    }
}
