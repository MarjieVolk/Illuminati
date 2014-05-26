using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Assets.Player
{
    public class TurnController : MonoBehaviour
    {
        public event System.Action OnTurnEnd;
        public event System.Action OnTurnStart;

        public PlayerData CurrentPlayer {
            get {
                return players[current];
            }
        }
        private List<PlayerData> players;
        private int current = 0;

        public bool BetweenTurns = false;

        void Start()
        {
            players = UnityEngine.Object.FindObjectsOfType<PlayerData>().ToList<PlayerData>();
            players = players.OrderBy<PlayerData, int>((player) => player.turnOrder).ToList<PlayerData>();

            foreach (PlayerData player in players) {
                player.init();
            }

            CurrentPlayer.startTurn();
        }

        public void ExecuteActions()
        {
            CurrentPlayer.endTurn();

            if (null != OnTurnEnd) OnTurnEnd();

            BetweenTurns = true;
        }

        public void NextTurn()
        {
            BetweenTurns = false;

            // Set to next player
            current = (current + 1) % players.Count;

            // Trigger event
            if (null != OnTurnStart) OnTurnStart();

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
            Destroy(player.gameObject);
        }
    }
}
    