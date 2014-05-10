using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;
using Assets.Graph.Nodes;

public class NodeStatMenu : MonoBehaviour {

	private bool isShown = false;
	private GUIStyle style;

	// Use this for initialization
	void Start () {
		style = new GUIStyle();
		style.normal.textColor = Color.black;
		style.fontStyle = FontStyle.Bold;
        style.normal.background = InvestigateAction.MakeTextureOfColor(new Color(0.5f, 0.5f, 0.5f, 0.9f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {		
		NodeData data = gameObject.GetComponent<NodeData>();
		if (canShowMenu(data)) {
			isShown = true;
		}
	}

	void OnMouseExit() {
		isShown = false;
	}

	void OnGUI() {
		if (isShown) {
			GUI.depth = -9001;
			NodeData data = gameObject.GetComponent<NodeData>();
            int increase = data.getWorkingPower() - data.power;
			string text = "  Power: " + data.power + (increase > 0 ? " <color=green>+" + increase + "</color>" : "");

            Vector2 textSize = style.CalcSize(new GUIContent(text));

            //float radius = GetComponent<CircleCollider2D>().radius;
            //Vector3 leftEdgeScreenPosition = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x - radius - 0.5f, transform.position.y, transform.position.z));
            //Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            //new Rect(leftEdgeScreenPosition.x - textSize.x, Screen.height - screenPosition.y - textSize.y / 2.0f, textSize.x + 5, textSize.y)

            GUI.Label(new Rect(Screen.width - textSize.x - 15, Screen.height - textSize.y - 10, textSize.x + 5, textSize.y), text, style);
		}
	}

	private bool canShowMenu(NodeData node) {
        if (!TurnController.instance.CurrentPlayer.IsLocalHumanPlayer) return false;
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
