using UnityEngine;
using System.Collections;
using System.Threading;

public class PlayerCaracClass : MonoBehaviour{

    public bool isAlive;

    public float life;
    private float resistancePhysique;
    private float resistanceMagic;

    public float playerRespawnTime = 10.0f;



    void Start()
    {
        this.isAlive = true;
        this.life = 100;
        this.resistancePhysique = 0.5f;
        this.resistanceMagic = 0.5f;
    }

    public void takeDamage(float damage, bool isPhysique)
    {
        float resistance = resistancePhysique;
        if (!isPhysique)
            resistance = resistanceMagic;

        this.life -= damage * resistance;
        if (this.life < 1)
            isAlive = false;
    }
}
