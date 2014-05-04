using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HUD;
using Assets.Player;

public class MainMenu : MonoBehaviour {

    public GUISkin skin;

    private GUIStyle titleStyle;
    private string title = "Enlightened";
    private string play = "<size=32>New Game</size>";
    private string instructText = "Instructions";

    private bool showMenu = false;
    private Instructions instructions;

    private int nPlayers;
    private List<int> playerVals;
    private List<string> playerNames;
    private string[] playerOptions = { "Human", "Computer" };

	// Use this for initialization
	void Start () {
        titleStyle = new GUIStyle();
        titleStyle.fontSize = 120;
        //titleStyle.normal.textColor = new Color(0.15f, 0.3f, 0.05f);
        titleStyle.alignment = TextAnchor.LowerCenter;

        instructions = gameObject.GetComponent<Instructions>();
        instructions.enabled = false;

        resetDefaults();
	}

    private void startGame() {
        for (int i = 0; i < playerVals.Count; i++) {
            if (playerVals[i] == 0) {
                // Human
                createHumanPlayer(playerNames[i], i);
            } else {
                // Computer
                createComputerPlayer(playerNames[i], i);
            }
        }

        Application.LoadLevel("Map");
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
            Rect newGameRect = new Rect((Screen.width / 2.0f) - ((textSize.x + 40) / 2.0f), (Screen.height * 0.9f - textSize.y), textSize.x + 40, textSize.y);
            if (GUI.Button(newGameRect, play)) {
                showMenu = true;
            }

            textSize = skin.button.CalcSize(new GUIContent(instructText));
            Rect instructRect = new Rect((Screen.width / 2.0f) - ((textSize.x + 40) / 2.0f), (Screen.height * 0.95f - textSize.y), textSize.x + 40, textSize.y);
            if (GUI.Button(instructRect, instructText)) {
                instructions.enabled = true;
            }

        } else if (showMenu) {
            float width = Screen.width * 0.6f;
            float height = Screen.height * 0.9f;
            GUI.Window(0, GUIUtilities.getRect(width, height), layoutWindow, "New Game");
        }
    }

    private void layoutWindow(int windowID) {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Number of Players: ");
        nPlayers = (int) Mathf.Round(GUILayout.HorizontalSlider(nPlayers, 2, 4));
        GUILayout.Label("" + nPlayers);
        GUILayout.EndHorizontal();

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
