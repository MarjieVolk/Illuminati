using UnityEngine;
using System.Collections;
using System;

namespace Assets.Player
{
    public class TurnController : MonoBehaviour
    {
		public static TurnController instance { get; private set; }

        public delegate void OnTurnEndHandler();

        public event OnTurnEndHandler OnTurnEnd;

        public PlayerData CurrentPlayer;
        public PlayerData OtherPlayer;

        // Use this for initialization
        void Awake()
        {
			instance = this;
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
            if (GUI.Button(new Rect(10, 10, 100, 90), "End Turn"))
            {
                NextTurn();
            }

            GUI.TextArea(new Rect(10, 110, 100, 90), CurrentPlayer.Name);
        }

        public void NextTurn()
        {
            //end the current player's turn
            CurrentPlayer.endTurn();

            //set visibility ot public information only
            this.GetComponent<VisibilityController>().setVisibility(VisibilityController.Visibility.Public);

            if (null != OnTurnEnd) OnTurnEnd();

            //start the next player's turn
            OtherPlayer.startTurn();

            //announce the turn change ('Player 2's turn' pops up on the screen or something)

            PlayerData temp = OtherPlayer;
            OtherPlayer = CurrentPlayer;
            CurrentPlayer = temp;
        }
    }
}
    