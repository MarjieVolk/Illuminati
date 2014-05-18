using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.HUD;
using System.Linq;

namespace Assets.Player
{
    public class TurnController : MonoBehaviour
    {
		public static TurnController instance { get; private set; }

        public event System.Action OnTurnEnd;
        public event System.Action OnTurnStart;

        public GUISkin skin;

        public PlayerData CurrentPlayer {
            get {
                return players[current];
            }
        }
        private List<PlayerData> players;
        private int current = 0;

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
            players = UnityEngine.Object.FindObjectsOfType<PlayerData>().ToList<PlayerData>();
            players = players.OrderBy<PlayerData, int>((player) => player.turnOrder).ToList<PlayerData>();

            foreach (PlayerData player in players) {
                player.init();
            }

            CurrentPlayer.startTurn();
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
            GUI.Label(new Rect(x - width, y, width, height), CurrentPlayer.PlayerName + "'s Turn", playerTurnStyle);

            float buttonWidth = 150;
            float buttonHeight = 50;
            float windowWidth = 600;
            float windowHeight = 180;
            float buttonYOffset = windowHeight * 0.1f;
            float buttonXOffset = buttonWidth * 0.8f;

            if (showNextTurnPopup) {
                GUI.Box(GUIUtilities.getRect(windowWidth, windowHeight), "" + CurrentPlayer.PlayerName + "'s Turn");
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
            if (!isDoActionCheck && CurrentPlayer.actionPointsRemaining() > 0 && CurrentPlayer.IsLocalHumanPlayer) {
                isDoActionCheck = true;
                ScreenBlocker.instance.setBlocking(true);
                return;
            }

            CurrentPlayer.endTurn();

            if (null != OnTurnEnd) OnTurnEnd();

            BetweenTurns = true;
            isDoActionCheck = false;
        }

        public void NextTurn()
        {
            BetweenTurns = false;

            // Set to next player
            current = (current + 1) % players.Count;

            // Trigger event
            if (null != OnTurnStart) OnTurnStart();

            // Next turn popup
            if (CurrentPlayer.IsLocalHumanPlayer)
            {
                ScreenBlocker.instance.setBlocking(true);
                showNextTurnPopup = true;
            }

            //start the next player's turn, ready for things like AI actions now
            //if this player was an AI, go ahead and start the next turn right away
            if (CurrentPlayer.startTurn())
            {
                ExecuteActions(); NextTurn();
            }
        }

        public void removePlayer(PlayerData player) {
            int removeIndex = players.IndexOf(player);
            if (current >= removeIndex) {
                current -= 1;
                current %= players.Count;
            }

            players.Remove(player);
            Destroy(player);
        }
    }
}
    