using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class VictoryConditionController : MonoBehaviour {

	public GameObject winScreen;

	private bool won = false;
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
				if (player == owner) {
					nonLosingPlayers.Add(owner);
					players.Remove(owner);
					break;
				}
			}
		}

		if (nonLosingPlayers.Count == 1) {
			winner = nonLosingPlayers[0];
			Instantiate(winScreen, Camera.main.transform.position + new Vector3(0, 0, 1), Quaternion.identity);
			won = true;
		}
	}

	void OnGUI() {
		if (won) {
			GUI.Label(new Rect(Screen.width / 2.0f - 300, Screen.height / 2.0f - 300, 600, 600), "" + winner.Name + " wins!", style);
		}
	}
}
