using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    private PlayerCaracClass carac;

	// Use this for initialization
	void Start () {
        this.carac = new PlayerCaracClass();
	}

    public void addDamagePhysique(float damage)
    {
        this.carac.takeDamage(damage, true);
        Debug.Log(this.carac.life);
    }
}
