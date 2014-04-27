using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class NodeStatMenu : MonoBehaviour {

	public GameObject menuBackground;

	private bool isShown = false;
	private GUIStyle style;

	// Use this for initialization
	void Start () {
		style = new GUIStyle();
		style.normal.textColor = Color.black;
		style.fontStyle = FontStyle.Bold;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		if (menuBackground) {
			// Display menu background
		}
		
		NodeData data = gameObject.GetComponent<NodeData>();
		if (canShowMenu(data)) {
			isShown = true;
			foreach (NodeData node in GraphUtility.instance.getConnectedNodes(data)) {
				if (canShowMenu(node)) {
					node.gameObject.GetComponent<NodeStatMenu>().isShown = true;
				}
			}
		}
	}

	void OnMouseExit() {
		if (menuBackground) {
			// Hide menu background
		}
		isShown = false;
		NodeData data = gameObject.GetComponent<NodeData>();
		foreach (NodeData node in GraphUtility.instance.getConnectedNodes(data)) {
			node.gameObject.GetComponent<NodeStatMenu>().isShown = false;
		}
	}

	void OnGUI() {
		if (isShown) {
			GUI.depth = 10;
			NodeData data = gameObject.GetComponent<NodeData>();
			string text = "\t\t\t Att\tDef\n" +
					"Bribe\t\t<color=maroon>" + data.getAttack(DominationType.Bribe) + "\t" + data.getDefense(DominationType.Bribe) + "</color>\n" +
					"Blackmail\t<color=maroon>" + data.getAttack(DominationType.Blackmail) + "\t" + data.getDefense(DominationType.Blackmail) + "</color>\n" +
					"Threaten\t<color=maroon>" + data.getAttack(DominationType.Threaten) + "\t" + data.getDefense(DominationType.Threaten) + "</color>\n";

			Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

			GUI.Label(new Rect(screenPosition.x - 150, Screen.height - screenPosition.y - 30, 300, 100), text, style);
		}
	}

	private bool canShowMenu(NodeData node) {
		if (node.Owner == TurnController.instance.CurrentPlayer) {
			return true;
		}

		List<NodeData> nodes = GraphUtility.instance.getConnectedNodes(node);
		foreach (NodeData otherNode in nodes) {
			if (otherNode.Owner == TurnController.instance.CurrentPlayer) {
				return true;
			}
		}

		return false;
	}
}
