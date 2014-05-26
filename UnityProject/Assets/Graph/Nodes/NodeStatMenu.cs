using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;
using Assets.Graph.Nodes;
using Assets;

public class NodeStatMenu : DependencyResolvingComponent
{

	private bool isShown = false;
	private GUIStyle style;

	void Start () {
		style = new GUIStyle();
		style.normal.textColor = Color.black;
		style.fontStyle = FontStyle.Bold;
        style.normal.background = InvestigateAction.MakeTextureOfColor(new Color(0.6f, 0.6f, 0.6f, 0.7f));
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
            string text = " Power: " + data.power;
            
            int increase = data.getWorkingPower() - data.power;
            if (increase != 0) {
                string color = increase > 0 ? "green>+" : "red>";
                text += " <color=" + color + increase + "</color>";
            }

            Vector2 textSize = style.CalcSize(new GUIContent(text));

            bool isNodeMenuShown = GetComponent<NodeMenu>().isShown;
            float radius = GetComponent<CircleCollider2D>().radius;
            float x = transform.position.x - radius + (isNodeMenuShown ? -0.5f : 0);
            Vector3 leftEdgeScreenPosition = Camera.main.WorldToScreenPoint(new Vector3(x, transform.position.y, transform.position.z));
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            Rect r = new Rect(leftEdgeScreenPosition.x - textSize.x, Screen.height - screenPosition.y - textSize.y / 2.0f, textSize.x + 5, textSize.y);

            GUI.Label(r, text, style);
		}
	}

	private bool canShowMenu(NodeData node) {
        if (!TurnController.CurrentPlayer.IsLocalHumanPlayer) return false;
		if (node.Owner == TurnController.CurrentPlayer) {
			return true;
		}

		List<NodeData> nodes = GraphUtility.instance.getConnectedNodes(node);
		foreach (NodeData otherNode in nodes) {
			if (otherNode.Owner == TurnController.CurrentPlayer) {
				return true;
			}
		}

		return false;
	}
}
