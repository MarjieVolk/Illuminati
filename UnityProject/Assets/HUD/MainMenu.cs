using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HUD;
using Assets.Player;

public class MainMenu : MonoBehaviour {

    public GUISkin skin;

    private GUIStyle titleStyle;
    private GUIStyle mapOptionStyle;
    private string title = "Enlightened";
    private string play = "<size=32>New Game</size>";
    private string instructText = "Instructions";
    private string quitText = "Quit";

    private bool showMenu = false;
    private Instructions instructions;

    private int nPlayers;
    private List<int> playerVals;
    private List<string> playerNames;
    private string[] playerOptions = { "Human", "Computer" };

    private int mapChoice = 0;
    private GUIContent[] mapOptions = new GUIContent[2];
    private int[] mapMaxPlayers = { 4, 2 };
    private string[] mapScenes = { "Map", "TinyMap" };

	// Use this for initialization
	void Start () {
        titleStyle = new GUIStyle();
        titleStyle.fontSize = 120;
        titleStyle.normal.textColor = new Color(0.07f, 0, 0.01f);
        titleStyle.alignment = TextAnchor.LowerCenter;

        mapOptionStyle = new GUIStyle(skin.button);
        mapOptionStyle.padding.left += 10;
        mapOptionStyle.padding.right += 10;
        mapOptionStyle.padding.top += 10;
        mapOptionStyle.padding.bottom += 10;
        mapOptionStyle.border = skin.window.border;

        instructions = gameObject.GetComponent<Instructions>();
        instructions.enabled = false;

        for (int i = 0; i < mapOptions.Length; i++) {
            mapOptions[i] = new GUIContent();
        }

        mapOptions[0].image = Resources.Load<Texture>("map_icon");
        mapOptions[0].text = "Old Map";
        mapOptions[1].image = Resources.Load<Texture>("tinymap_icon");
        mapOptions[1].text = "Crazy New Map";

        resetDefaults();
	}

    private void startGame() {
        PlayerData[] leftoverPlayers = GameObject.FindObjectsOfType<PlayerData>();
        foreach (PlayerData player in leftoverPlayers) {
            Destroy(player.gameObject);
        }

        for (int i = 0; i < playerVals.Count; i++) {
            if (playerVals[i] == 0) {
                // Human
                createHumanPlayer(playerNames[i], i);
            } else {
                // Computer
                createComputerPlayer(playerNames[i], i);
            }
        }

        Application.LoadLevel(mapScenes[mapChoice]);
    }

    private void resetDefaults() {
        playerVals = new List<int>();
        playerNames = new List<string>();
        nPlayers = 2;
    }

    private void createHumanPlayer(string name, int index) {
        GameObject obj = (GameObject) Resources.Load("HumanPlayer");
        obj = (GameObject) Instantiate(obj);
        PlayerData player = obj.GetComponent<PlayerData>();
        player.PlayerName = name;
        player.turnOrder = index;
        player.IsLocalHumanPlayer = true;
        DontDestroyOnLoad(obj);
    }

    private void createComputerPlayer(string name, int index) {
        GameObject obj = (GameObject)Resources.Load("AIPlayer");
        obj = (GameObject)Instantiate(obj);
        PlayerData player = obj.GetComponent<PlayerData>();
        player.PlayerName = name;
        player.turnOrder = index;
        player.IsLocalHumanPlayer = false;
        DontDestroyOnLoad(obj);
    }

    void OnGUI() {
        GUI.skin = skin;

        if (!showMenu && !instructions.enabled) {
            Vector2 textSize = titleStyle.CalcSize(new GUIContent(title));
            GUI.Label(new Rect((Screen.width / 2.0f) - (textSize.x / 2.0f), (Screen.height * 0.03f), textSize.x, textSize.y), title, titleStyle);

            textSize = skin.button.CalcSize(new GUIContent(play));
            Rect newGameRect = new Rect((Screen.width / 2.0f) - ((textSize.x + 40) / 2.0f), (Screen.height * 0.85f - textSize.y), textSize.x + 40, textSize.y);
            if (GUI.Button(newGameRect, play)) {
                showMenu = true;
            }

            textSize = skin.button.CalcSize(new GUIContent(instructText));
            Rect instructRect = new Rect((Screen.width / 2.0f) - ((textSize.x + 40) / 2.0f), (Screen.height * 0.91f - textSize.y), textSize.x + 40, textSize.y);
            if (GUI.Button(instructRect, instructText)) {
                instructions.enabled = true;
            }

            textSize = skin.button.CalcSize(new GUIContent(quitText));
            Rect quitRect = new Rect((Screen.width / 2.0f) - ((textSize.x + 40) / 2.0f), (Screen.height * 0.96f - textSize.y), textSize.x + 40, textSize.y);
            if (GUI.Button(quitRect, quitText)) {
                Application.Quit();
            }

        } else if (showMenu) {
            float width = 700; //Screen.width * 0.6f;
            float height = 600; //Screen.height * 0.9f;
            GUI.Window(0, GUIUtilities.getRect(width, height), layoutWindow, "New Game");
        }
    }

    private void layoutWindow(int windowID) {
        GUILayout.Label("Map:");
        mapChoice = GUILayout.SelectionGrid(mapChoice, mapOptions, 2, mapOptionStyle, GUILayout.MaxHeight(150));
        GUILayout.Space(30);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Number of Players: ");
        int max = mapMaxPlayers[mapChoice];
        nPlayers = (int) Mathf.Round(GUILayout.HorizontalSlider(Math.Min(nPlayers, max), 2, max));
        GUILayout.Label("" + nPlayers);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        for (int i = 0; i < nPlayers; i++) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Player " + i + ": ");

            while (playerNames.Count <= i) {
                playerNames.Add("");
            }

            GUI.SetNextControlName("player" + i);
            playerNames[i] = GUILayout.TextArea(playerNames[i], GUILayout.MinWidth(200));

            GUILayout.FlexibleSpace();

            while (playerVals.Count <= i) {
                playerVals.Add(0);
            }

            int newVal = GUILayout.SelectionGrid(playerVals[i], playerOptions, 2, GUILayout.MinWidth(300));
            bool isAllComputers = false;
            if (newVal == 1) {
                // Trying to make this player a computer
                // Check whether all other players are computers
                isAllComputers = true;
                for (int index = 0; index < playerVals.Count; index++) {
                    if (index != i && playerVals[index] == 0) {
                        isAllComputers = false;
                        break;
                    }
                }
            }

            // Don't set to new val if doing so would make all players be computers
            if (!isAllComputers) {
                playerVals[i] = newVal;
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("<size=32>Play!</size>")) {
            startGame();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel")) {
            resetDefaults();
            showMenu = false;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        updateDefaultNames();
    }

    private void updateDefaultNames() {
        if (UnityEngine.Event.current.type != EventType.Repaint) {
            return;
        }

        string focused = GUI.GetNameOfFocusedControl();
        int index = -1;
        if (focused.StartsWith("player")) {
            index = int.Parse(focused.Substring(6));
        }

        for (int i = 0; i < playerNames.Count; i++) {
            if (i == index) {
                // focused
                if (playerNames[i] == "Player " + i) playerNames[i] = "";
            } else {
                // not focused
                if (playerNames[i] == "") playerNames[i] = "Player " + i;
            }
        }
    }
}
