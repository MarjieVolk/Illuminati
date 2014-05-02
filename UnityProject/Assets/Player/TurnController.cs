using UnityEngine;
using System.Collections;
using System;
using Assets.HUD;

namespace Assets.Player
{
    public class TurnController : MonoBehaviour
    {
		public static TurnController instance { get; private set; }

        public event System.Action OnTurnEnd;
        public event System.Action OnTurnStart;

        public GUISkin skin;
        public PlayerData CurrentPlayer;
        public PlayerData OtherPlayer;

        public bool BetweenTurns = false;
        private bool showNextTurnPopup = false;
        private bool isDoActionCheck = false;

        // Use this for initialization
        void Awake()
        {
			instance = this;
        }

        void Start()
        {
            CurrentPlayer.startTurn();
        }

        // Update is called once per frame
        void Update()
        {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit();
			}
        }

        void OnGUI()
        {
            GUI.skin = skin;
			float x = 0.01f * Screen.height;
			float y = 0.218f * Screen.height;
            x = Screen.width - x;

            float width = 2000;
            float height = 50;

            GUIStyle playerTurnStyle = new GUIStyle();
            playerTurnStyle.fontSize = 14;
            playerTurnStyle.alignment = TextAnchor.UpperRight;
            GUI.Label(new Rect(x - width, y, width, height), CurrentPlayer.Name + "'s Turn", playerTurnStyle);

            float buttonWidth = 150;
            float buttonHeight = 50;
            float windowWidth = 600;
            float windowHeight = 180;
            float buttonYOffset = windowHeight * 0.1f;
            float buttonXOffset = buttonWidth * 0.8f;

            if (showNextTurnPopup) {
                GUI.Box(GUIUtilities.getRect(windowWidth, windowHeight), "" + CurrentPlayer.name + "'s Turn");
                if (GUI.Button(GUIUtilities.getRect(buttonWidth, buttonHeight, 0, buttonYOffset), "Okay")) {
                    showNextTurnPopup = false;
                    ScreenBlocker.instance.setBlocking(false);
                }
            }

            if (isDoActionCheck) {
                GUI.Box(GUIUtilities.getRect(windowWidth, windowHeight), "You have " + CurrentPlayer.actionPointsRemaining() + " action points left. Execute anyways?");
                if (GUI.Button(GUIUtilities.getRect(buttonWidth, buttonHeight, -buttonXOffset, buttonYOffset), "Execute")) {
                    ExecuteActions();
                    ScreenBlocker.instance.setBlocking(false);
                }

                if (GUI.Button(GUIUtilities.getRect(buttonWidth, buttonHeight, buttonXOffset, buttonYOffset), "Cancel")) {
                    isDoActionCheck = false;
                    ScreenBlocker.instance.setBlocking(false);
                }
            }
        }

        public void ExecuteActions()
        {
            if (!isDoActionCheck && CurrentPlayer.actionPointsRemaining() > 0) {
                isDoActionCheck = true;
                ScreenBlocker.instance.setBlocking(true);
                return;
            }

            CurrentPlayer.endTurn();

            if (null != OnTurnEnd) OnTurnEnd();

            this.GetComponent<VisibilityController>().ToggleVisibility();
            this.GetComponent<VisibilityController>().ToggleVisibility();

            BetweenTurns = true;
            isDoActionCheck = false;
        }

        public void NextTurn()
        {
            BetweenTurns = false;

            //set visibility to public information only
            this.GetComponent<VisibilityController>().setVisibility(VisibilityController.Visibility.Public);

            //start the next player's turn
            OtherPlayer.startTurn();

            if (null != OnTurnStart) OnTurnStart();

            //announce the turn change ('Player 2's turn' pops up on the screen or something)

            PlayerData temp = OtherPlayer;
            OtherPlayer = CurrentPlayer;
            CurrentPlayer = temp;

            // Next turn popup
            ScreenBlocker.instance.setBlocking(true);
            showNextTurnPopup = true;
        }
    }
}
    