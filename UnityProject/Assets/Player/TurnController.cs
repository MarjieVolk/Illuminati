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
        }

        public void ExecuteActions()
        {
            CurrentPlayer.endTurn();

            //set visibility to public information only
            this.GetComponent<VisibilityController>().setVisibility(VisibilityController.Visibility.Public);

            if (null != OnTurnEnd) OnTurnEnd();
        }

        public void NextTurn()
        {
            //start the next player's turn
            OtherPlayer.startTurn();

            //announce the turn change ('Player 2's turn' pops up on the screen or something)

            PlayerData temp = OtherPlayer;
            OtherPlayer = CurrentPlayer;
            CurrentPlayer = temp;
        }
    }
}
    