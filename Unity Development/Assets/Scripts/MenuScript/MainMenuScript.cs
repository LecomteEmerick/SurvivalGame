using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScript : MonoBehaviour {

    private Rect playerConnectZone;
    private Rect mapAfficheZone;
    private Rect centerRef;
    private Rect afficheRect;
    public Vector2 scrollPosition;
    private Texture selectedMap;
    private string selectedMapName;

    private string message = "";
    private string name = "";
    private string pseudo = "";

    private int menuState = 0;

    private StaticVariableScript setting;

    [SerializeField]
    private List<Texture> backgroundList;

    [SerializeField]
    private List<Texture> mapList;

    [SerializeField]
    private Texture arrow_next;

    [SerializeField]
    private Texture arrow_previous;

    [SerializeField]
    private GUITexture currentBackground;

    // Use this for initialization
    void Start()
    {
        playerConnectZone = new Rect(200, Screen.height - 300, Screen.width - 200, 150);
        mapAfficheZone = new Rect(100, 50, Screen.width - 200, Screen.height - 400);
        selectedMapName = "";
        scrollPosition = new Vector2(0, 0);
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
        else if (menuState == 30)
            EquipmentMenu();

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
        if (GUI.Button(afficheRect, "Equipement"))
            menuState = 30;
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
        GUI.Label(afficheRect, "Pseudo : ");
        afficheRect = verticalDown(afficheRect);
        pseudo = GUI.TextArea(afficheRect, pseudo);
        afficheRect = verticalDown(afficheRect);
        GUI.Label(afficheRect, "Name : ");
        GUI.Label(afficheRect, message);
        afficheRect = verticalDown(afficheRect);
        name = GUI.TextArea(afficheRect, name);
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Create"))
        {
            if (name != "" && pseudo != "")
            {
                setting.GameName = name + " partie de : " + pseudo;
                setting.Pseudo = pseudo;
                setting.isServer = true;
                setting.ip = "0";
                connection();
            }
            else
            {
                message = "Please Enter a Name or a pseudo";
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
        GUI.Label(afficheRect, "Pseudo : ");
        afficheRect = verticalDown(afficheRect);
        pseudo = GUI.TextArea(afficheRect, pseudo);
        afficheRect = verticalDown(afficheRect);
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
            if (GUILayout.Button("Connection") && pseudo != "")
            {
                setting.Pseudo = pseudo;
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

    private void EquipmentMenu()
    {
        afficheRect = centerRef;
        GUI.Label(afficheRect, "Aucun Equipement pour le moment");
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Retour"))
            menuState = 0;
    }

    private void ServeurChoiceMenu()
    {
        setBackground(1);
        afficheRect = centerRef;
        listMapAffiche();
        playerConnectedToGame();
        afficheRect.y = Screen.height - 70;
        if (GUI.Button(afficheRect, "Jouer"))
            startScene();
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Retour"))
        {
            menuState = 0;
            closeNetwork();
            setBackground(0);
        }
    }

    private void listMapAffiche()
    {
        GUILayout.BeginArea(mapAfficheZone);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);
        GUILayout.BeginHorizontal();
        foreach (Texture map in mapList)
        {
            if(GUILayout.Button(map))
            {
                selectedMapName = map.name;
                networkView.RPC("selectMap", RPCMode.Others,selectedMapName);
            }
            GUILayout.Space(10);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        if (GUI.Button(new Rect(Screen.width - 100, 50, 100, Screen.height - 400), arrow_next))
            scrollPosition.x += Screen.width - 200;
        if (GUI.Button(new Rect(0, 50, 100, Screen.height - 400), arrow_previous))
            scrollPosition.x -= Screen.width - 200;
    }

    private void playerConnectedToGame()
    {
        GUILayout.BeginArea(playerConnectZone);
        GUILayout.BeginVertical();
        foreach(string ip in setting.ListClient)
        {
            GUILayout.Label(ip);
            GUILayout.Space(10);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        networkView.RPC("requestPseudo", player);
        if(selectedMapName != "")
            networkView.RPC("selectMap",player, selectedMapName);
    }

    [RPC]
    void requestPseudo()
    {
        string pseudo = "UNKNOW";
        if (setting.Pseudo != "")
            pseudo = setting.Pseudo;
        networkView.RPC("addUser", RPCMode.Server,pseudo);
    }

    //call on serveur only
    [RPC]
    void addUser(string pseudo)
    {
        if (Network.isServer)
            setting.ListClient.Add(pseudo);
    }

    [RPC]
    void removeUser(string pseudo)
    {
        if (Network.isServer && setting.ListClient.Contains(pseudo))
            setting.ListClient.Remove(pseudo);
    }

    [RPC]
    void selectMap(string name)
    {
        bool found = false;
        int i = 0;
        selectedMapName = name;
        while(!found && i < mapList.Count)
        {
            if (mapList[i].name == name)
                selectedMap = mapList[i];
            i++;
        }
    }

    private void ClientLobbyMenu()
    {
        afficheRect = centerRef;
        if(selectedMap != null)
        {
            GUI.DrawTexture(new Rect(Screen.width/2 - selectedMap.width/2,50,selectedMap.width,selectedMap.height), selectedMap);
            afficheRect = verticalDown(afficheRect);
        }
        afficheRect.y = Screen.height - 150;
        if (GUI.Button(afficheRect, "Accepter"))
            networkView.RPC("acceptMap", RPCMode.Server,pseudo);
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Refuser"))
            networkView.RPC("rejectMap", RPCMode.Server, pseudo);
        afficheRect = verticalDown(afficheRect);
        if (GUI.Button(afficheRect, "Retour"))
        {
            menuState = 0;
            closeNetwork();
        }
    }

    [RPC]
    void acceptMap(string pseudo)
    {
        if (setting.ListClient.Contains(pseudo))
            setting.ListClient[setting.ListClient.IndexOf(pseudo)] = pseudo + " - Map accepted";
    }

    [RPC]
    void rejectMap(string pseudo)
    {
        if (setting.ListClient.Contains(pseudo))
            setting.ListClient[setting.ListClient.IndexOf(pseudo)] = pseudo + " - Map refused";
    }

    //Helper
    private Rect verticalDown(Rect afficheRect)
    {
        afficheRect.y += afficheRect.height + 10;
        return afficheRect;
    }

    [RPC]
    private void startScene()
    {
        if (Network.isServer)
            networkView.RPC("startScene", RPCMode.Others);
        Application.LoadLevel("AlphaMapScene");
    }

    public void connection()
    {
        Application.runInBackground = true;

        if (setting.isServer)
        {
            Network.InitializeServer(32, 8080, !Network.HavePublicAddress());
            MasterServer.RegisterHost("SurvivalGame", setting.GameName, "");
            setting.ListClient = new List<string>();
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
        if (Network.isClient)
            networkView.RPC("removeUser", RPCMode.Server, setting.Pseudo);
        Network.Disconnect();
    }

    void OnDisconnectedFromServer()
    {
        menuState = 0;
    }
}
