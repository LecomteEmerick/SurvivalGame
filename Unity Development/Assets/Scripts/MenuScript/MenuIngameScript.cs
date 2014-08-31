using UnityEngine;
using System.Collections;

public class MenuIngameScript : MonoBehaviour {

    private bool afficheMenu = false;

    private Rect centerRef;
    private Rect afficheRect;

	// Use this for initialization
	void Start () {
        centerRef = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, 30);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            afficheMenu = !afficheMenu;
        if (!afficheMenu && !Screen.lockCursor)
            Screen.lockCursor = true;
    }

	// Update is called once per frame
	void OnGUI () {
	    if(afficheMenu)
        {
            if (Screen.lockCursor)
                Screen.lockCursor = false;
            afficheRect = centerRef;
            if (GUI.Button(afficheRect, "Return to game"))
                afficheMenu = false;
            afficheRect = verticalDown(afficheRect);
            if (GUI.Button(afficheRect, "Exit Game"))
                Application.Quit();
        }
	}

    private Rect verticalDown(Rect afficheRect)
    {
        afficheRect.y += afficheRect.height + 10;
        return afficheRect;
    }
}
