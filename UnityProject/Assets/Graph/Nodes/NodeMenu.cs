using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;
using System.Linq;
using Assets.Actions;
using Assets.Graph.Nodes;

public class NodeMenu : RadialMenu {

    public Dictionary<Action, GameObject> buttons;

    private NodeData node;

    public override List<GameObject> getButtons() {
        Action[] actions = gameObject.GetComponentsInChildren<Action>();
        buttons = new Dictionary<Action, GameObject>();

        foreach (Action a in actions) {
            GameObject realButton = (GameObject)Instantiate(a.GetComponent<ActionGUI>().button, transform.position, Quaternion.identity);
            realButton.transform.parent = this.gameObject.transform;
            buttons[a] = realButton;

            a.OnStateUpdate += OnActionUpdated;
        }

        return buttons.Values.ToList();
    }

	public override void Start () {
        base.Start();

		foreach (Action a in buttons.Keys) {
            buttons[a].GetComponent<ActionButton>().SetAction(a);
		}

        node = GetComponent<NodeData>();
        GetComponent<NodeGUI>().OnHover += () =>
        {
            // Show node menu
            if (!Input.GetMouseButton(0) && node.Owner == turnController.CurrentPlayer && !ActionController.instance.inSelectionState)
            {
                this.show();
            }
        };
	}

	public override void show() {
		if (node.isScheduled || gameObject.GetComponent<NodeData>().nTurnsUntilAvailable > 0 || turnController.BetweenTurns ||
            !turnController.CurrentPlayer.IsLocalHumanPlayer || turnController.CurrentPlayer.actionPointsRemaining() <= 0) {
			return;
		}

        base.show();
	}

	public override void OnMouseExit() {
		if (!ActionController.instance.inSelectionState) {
			hide();
		}

        base.OnMouseExit();
	}

    private void OnActionUpdated(Action updatedAction)
    {
        if (updatedAction.getNode().isScheduled)
        {
            clear();
            hide();
        }
    }
}
