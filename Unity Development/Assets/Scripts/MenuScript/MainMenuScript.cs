using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScript : MonoBehaviour {

    private Rect centerRef;
    private Rect afficheRect;

    private string message = "";
    private string name = "";

    private int menuState = 0;

    private StaticVariableScript setting;

    [SerializeField]
    private List<GUITexture> backgroundList;

    private GUITexture currentTexture;

    // Use this for initialization
    void Start()
    {
        setting = new StaticVariableScript();
        background.pixelInset = new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
        centerRef = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, 30);
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (menuState == 0)
            SelectionMenu();
        else if (menuState == 10)
            multiplayerMenu();
        else if (menuState == 11)
            ServerViewMenu();
        else if (menuState == 12)
            ClientViewMenu();
        else if (menuState == 20)
            SuccessMenu();

    }

    private void SelectionMenu()
    {
        afficheRect = centerRef;
        if (GUI.Button(afficheRect, "Jouer : Solo"))
            startScene();
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Jouer : MultiJoueur"))
            menuState = 10;
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Gain et Succes"))
            menuState = 20;
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Quitter"))
            Application.Quit();
    }

    private void multiplayerMenu()
    {
        afficheRect = centerRef;
        if (GUI.Button(afficheRect, "Serveur"))
            menuState = 11;
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Client"))
            menuState = 12;
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Retour"))
            menuState = 0;
    }

    private void ServerViewMenu()
    {
        afficheRect = centerRef;
        GUI.Label(afficheRect, "Name : ");
        afficheRect = verticalDown(afficheRect);
        GUI.Label(afficheRect, message);
        afficheRect = verticalDown(afficheRect);
        name = GUI.TextArea(afficheRect, name);
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Create"))
        {
            if (name != "")
            {
                setting.GameName = name;
                setting.isServer = true;
                setting.ip = "0";
                connection();
            }
            else
            {
                message = "Please Enter a Name";
            }
        }

        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Retour"))
            menuState = 10;
    }

    private void ClientViewMenu()
    {
        MasterServer.RequestHostList("SurvivalGame");
        afficheRect = centerRef;
        afficheRect.height = 50;
        HostData[] data = MasterServer.PollHostList();

        foreach (HostData element in data)
        {
            GUILayout.BeginArea(afficheRect);
            GUILayout.BeginHorizontal();
            string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
            GUILayout.Label(name);
            GUILayout.Space(5);
            string hostInfo;
            hostInfo = "[";
            foreach (string host in element.ip)
                hostInfo = hostInfo + host + ":" + element.port + " ";
            hostInfo = hostInfo + "]";
            GUILayout.Label(hostInfo);
            GUILayout.Space(5);
            GUILayout.Label(element.comment);
            GUILayout.Space(5);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Connection"))
            {
                setting.Element = element;
                connection();
            }
            GUILayout.EndHorizontal();
            afficheRect.y = afficheRect.y + 60;
            GUILayout.EndArea();
        }

        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Retour"))
            menuState = 10;
    }

    private void SuccessMenu()
    {
        afficheRect = centerRef;
        GUI.Label(afficheRect, "Aucun Succès pour le moment");
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Retour"))
            menuState = 0;
    }

    private Rect verticalDown(Rect afficheRect)
    {
        afficheRect.y += afficheRect.height + 10;
        return afficheRect;
    }

    private void startScene()
    {
        Application.LoadLevel("TestScene");
    }

    public void connection()
    {
        Application.runInBackground = true;

        if (setting.isServer)
        {
            Network.InitializeServer(32, 8080, !Network.HavePublicAddress());
            MasterServer.RegisterHost("SurvivalGame", setting.GameName, "");
        }
        else
        {
            Network.Connect(setting.Element);
        }
        startScene();
    }
}
