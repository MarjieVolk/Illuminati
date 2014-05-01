using UnityEngine;
using System.Collections;
using System;

namespace Assets.Player
{
    public class TurnController : MonoBehaviour
    {
		public static TurnController instance { get; private set; }

        public event System.Action OnTurnEnd;
        public event System.Action OnTurnStart;

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
			float x = 0.01f * Screen.height;
			float y = 0.218f * Screen.height;
            x = Screen.width - x;

            float width = 95;
            float height = 20;

            GUI.TextArea(new Rect(x - width, y, width, height), CurrentPlayer.Name + "'s Turn");

            float buttonWidth = Screen.width / 10.0f;
            float buttonHeight = Screen.width / 20.0f;

            if (showNextTurnPopup) {
                GUI.Box(new Rect(Screen.width / 4.0f, Screen.height / 4.0f, Screen.width / 2.0f, Screen.height / 2.0f), "" + CurrentPlayer.name + "'s Turn");
                if (GUI.Button(new Rect((Screen.width / 2.0f) - (buttonWidth / 2.0f), (Screen.height / 2.0f) - (buttonHeight / 2.0f), buttonWidth, buttonHeight), "Okay")) {
                    showNextTurnPopup = false;
                    ScreenBlocker.instance.setBlocking(false);
                }
            }

            if (isDoActionCheck) {
                GUI.Box(new Rect(Screen.width / 4.0f, Screen.height / 4.0f, Screen.width / 2.0f, Screen.height / 2.0f), "You have " + CurrentPlayer.actionPointsRemaining() + " action points left. Execute anyways?");
                if (GUI.Button(new Rect((Screen.width / 2.0f) - (buttonWidth / 2.0f) - (Screen.width * 0.2f), (Screen.height / 2.0f) - (buttonHeight / 2.0f), buttonWidth, buttonHeight), "Execute")) {
                    ExecuteActions();
                }

                if (GUI.Button(new Rect((Screen.width / 2.0f) - (buttonWidth / 2.0f) + (Screen.width * 0.2f), (Screen.height / 2.0f) - (buttonHeight / 2.0f), buttonWidth, buttonHeight), "Cancel")) {
                    isDoActionCheck = false;
                }
            }
        }

        public void ExecuteActions()
        {
            if (!isDoActionCheck && CurrentPlayer.actionPointsRemaining() > 0) {
                isDoActionCheck = true;
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
    