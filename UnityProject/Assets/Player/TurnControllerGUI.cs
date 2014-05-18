using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.HUD;

namespace Assets.Player
{
    public class TurnControllerGUI : MonoBehaviour
    {
        public static TurnControllerGUI instance { get; private set; }

        public GUISkin skin;
        private TurnController turnController;

        private bool showNextTurnPopup;
        private bool isDoActionCheck;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            turnController = GetComponent<TurnController>();

            turnController.OnTurnStart += () =>
                {
                    // Next turn popup
                    if (turnController.CurrentPlayer.IsLocalHumanPlayer)
                    {
                        ScreenBlocker.instance.setBlocking(true);
                        showNextTurnPopup = true;
                    }
                };
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
            GUI.Label(new Rect(x - width, y, width, height), turnController.CurrentPlayer.PlayerName + "'s Turn", playerTurnStyle);

            float buttonWidth = 150;
            float buttonHeight = 50;
            float windowWidth = 600;
            float windowHeight = 180;
            float buttonYOffset = windowHeight * 0.1f;
            float buttonXOffset = buttonWidth * 0.8f;

            if (showNextTurnPopup)
            {
                GUI.Box(GUIUtilities.getRect(windowWidth, windowHeight), "" + turnController.CurrentPlayer.PlayerName + "'s Turn");
                if (GUI.Button(GUIUtilities.getRect(buttonWidth, buttonHeight, 0, buttonYOffset), "Okay"))
                {
                    showNextTurnPopup = false;
                    ScreenBlocker.instance.setBlocking(false);
                }
            }

            if (isDoActionCheck)
            {
                GUI.Box(GUIUtilities.getRect(windowWidth, windowHeight), "You have " + turnController.CurrentPlayer.actionPointsRemaining() + " action points left. Execute anyways?");
                if (GUI.Button(GUIUtilities.getRect(buttonWidth, buttonHeight, -buttonXOffset, buttonYOffset), "Execute"))
                {
                    isDoActionCheck = false;
                    turnController.ExecuteActions();
                    ScreenBlocker.instance.setBlocking(false);
                }

                if (GUI.Button(GUIUtilities.getRect(buttonWidth, buttonHeight, buttonXOffset, buttonYOffset), "Cancel"))
                {
                    isDoActionCheck = false;
                    ScreenBlocker.instance.setBlocking(false);
                }
            }
        }

        public void TryExecuteActions()
        {
            if (turnController.CurrentPlayer.actionPointsRemaining() > 0 && turnController.CurrentPlayer.IsLocalHumanPlayer)
            {
                isDoActionCheck = true;
                ScreenBlocker.instance.setBlocking(true);
            }
            else
            {
                turnController.ExecuteActions();
            }
        }
    }
}
