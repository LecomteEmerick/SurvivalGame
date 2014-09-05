using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class ManageDeplacementClass{
    public NetworkViewID viewID;
    public GameObject player;

    private float walkSpeed = 5.0f;

    private List<float> modifierWalk;

    private Dictionary<float, Thread> listThread;

    public bool playerWantToRun = false;
    public bool playerWantToMoveUp = false;
    public bool playerWantToMoveDown = false;
    public bool playerWantToMoveLeft = false;
    public bool playerWantToMoveRight = false;
    public bool playerWantToJump = false;
    public bool playerCanJump = true;

    public Vector3 playerDirection = new Vector3();

    public float getWalkSpeed()
    {
        float modif = 0.0f;
        foreach (float modifier in modifierWalk)
            modif += modifier;
        return this.walkSpeed + modif;
    }


    public ManageDeplacementClass(NetworkViewID id,GameObject player)
    {
        this.viewID = id;
        this.player = player;
        this.modifierWalk = new List<float>();
        this.listThread = new Dictionary<float, Thread>();
    }

    public void addmodifierWalk(float modifier,int timeMillisecond)
    {
        this.modifierWalk.Add(modifier);
        Thread t = new Thread(() => autoRemoveModifier(modifier, timeMillisecond));
        listThread.Add(modifier, t);
        t.Start();
    }

    //Cette méthode ajoute une seconde de décrémentation de la vitesse
    private void autoRemoveModifier(float modifier, int timeMillisecond)
    {
        Thread.Sleep(timeMillisecond);
        float baseModif = modifier;
        for (int i = 0; i < 10;i++ )
        {
            this.modifierWalk[this.modifierWalk.IndexOf(modifier)] -= baseModif / 10;
            modifier -= baseModif / 10;
            Thread.Sleep(100);
        }
        this.modifierWalk.Remove(modifier);
    }

    public void removeModifier(float modifier)
    {
        listThread[modifier].Interrupt();
    }
}
