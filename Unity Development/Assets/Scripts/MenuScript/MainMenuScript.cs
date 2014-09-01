using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScript : MonoBehaviour {

    private Rect centerRef;
    private Rect afficheRect;
    private int ratio;

    private string message = "";
    private string name = "";

    private int menuState = 0;

    private StaticVariableScript setting;

    [SerializeField]
    private List<Texture> backgroundList;

    [SerializeField]
    private List<Texture> mapList;

    [SerializeField]
    private GUITexture currentBackground;

    // Use this for initialization
    void Start()
    {
        ratio = Screen.width / Screen.height;
        setting = new StaticVariableScript();
        setBackground(0);
        centerRef = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, 30);
    }

    void setBackground(int index)
    {
        if (index < backgroundList.Count)
        {
            currentBackground.texture = backgroundList[index];
            currentBackground.pixelInset = new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
        }
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
        else if (menuState == 13)
            ServeurChoiceMenu();
        else if (menuState == 14)
            ClientLobbyMenu();
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

    private void ServeurChoiceMenu()
    {
        setBackground(1);
        afficheRect = centerRef;
        Rect buttonMapRect = new Rect(10, 10, 0, 0);
        for (int i = 0; i < mapList.Count; i++)
        {
            buttonMapRect.height = (mapList[i].height * Screen.height) / Screen.width;
            buttonMapRect.width = (mapList[i].width * Screen.height) / Screen.width;
            if (buttonMapRect.width + buttonMapRect.x > Screen.width - 10)
            {
                buttonMapRect.y += buttonMapRect.height + 10 ;
                buttonMapRect.x = 10;
            }
            if (GUI.Button(buttonMapRect, mapList[i]))
                startScene();
            buttonMapRect.x += 10 + buttonMapRect.width;

        }
        afficheRect.y = Screen.height - 70;
        if (GUI.Button(afficheRect, "Retour"))
        {
            menuState = 0;
            closeNetwork();
        }
    }

    private void ClientLobbyMenu()
    {
        afficheRect = centerRef;
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Retour"))
        {
            menuState = 0;
            closeNetwork();
        }
    }

    //Helper
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
            menuState = 13;
        }
        else
        {
            Network.Connect(setting.Element);
            menuState = 14;
        }
    }

    private void closeNetwork()
    {
        Network.Disconnect();
    }

    void OnDisconnectedFromServer()
    {
        menuState = 0;
    }
}
