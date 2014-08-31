using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

    private Rect centerRef;
    private Rect afficheRect;

    [SerializeField]
    private GUITexture background;

    // Use this for initialization
    void Start()
    {
        background.pixelInset = new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
        centerRef = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, 30);
    }

    void Update()
    {

    }

    // Update is called once per frame
    void OnGUI()
    {
        afficheRect = centerRef;
        if (GUI.Button(afficheRect, "Play"))
            Application.LoadLevel("TestScene");
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Exit"))
            Application.Quit();
    }

    private Rect verticalDown(Rect afficheRect)
    {
        afficheRect.y += afficheRect.height + 10;
        return afficheRect;
    }
}
