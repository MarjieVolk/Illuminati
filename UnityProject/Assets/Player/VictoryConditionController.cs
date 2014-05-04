using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;
using Assets.HUD;

public class VictoryConditionController : MonoBehaviour {

	public GameObject winScreen;
    public GUISkin skin;

	private bool won = false;
    private string lossMessage = null;
	private PlayerData winner;

	private GUIStyle style;

	// Use this for initialization
	void Start () {
		TurnController.instance.OnTurnEnd += checkVictoryCondition;
		style = new GUIStyle();
		style.fontSize = 32;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white; //new Color(0.5f, 0, 0);
		style.alignment = TextAnchor.MiddleCenter;
	}

	private void checkVictoryCondition() {
		List<PlayerData> players = new List<PlayerData>();
		players.AddRange(FindObjectsOfType<PlayerData>());
		List<PlayerData> nonLosingPlayers = new List<PlayerData>();

		NodeData[] nodes = FindObjectsOfType<NodeData>();
		foreach (NodeData node in nodes) {
			PlayerData owner = node.Owner;
			foreach (PlayerData player in players) {
				if (player == owner && !(player.StartingNode == node)) {
					nonLosingPlayers.Add(owner);
					players.Remove(owner);
					break;
				}
			}
		}

        if (players.Count > 0) {
            // We have a loser!
            ScreenBlocker.instance.setBlocking(true);
            lossMessage = "";
            foreach (PlayerData losingPlayer in players) {
                lossMessage += losingPlayer.PlayerName + " Lost!\n";
                TurnController.instance.removePlayer(losingPlayer);
            }
        }

		if (nonLosingPlayers.Count == 1) {
			winner = nonLosingPlayers[0];
			Instantiate(winScreen, Camera.main.transform.position + new Vector3(0, 0, 1), Quaternion.identity);
			won = true;
		}
	}

	void OnGUI() {
        GUI.skin = skin;
        if (won) {
            GUI.Label(new Rect(Screen.width / 2.0f - 300, Screen.height / 2.0f - 300, 600, 600), "" + winner.PlayerName + " wins!", style);
        } else if (lossMessage != null) {
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(lossMessage));
            GUI.Window(0, GUIUtilities.getRect(300, size.y + 150), layoutWindow, "Breaking News!");
        }
	}

    private void layoutWindow(int id) {
        GUILayout.Label(lossMessage);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Okay")) {
            lossMessage = null;
            ScreenBlocker.instance.setBlocking(false);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
