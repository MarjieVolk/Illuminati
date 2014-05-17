using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;
using System.Linq;

public class NodeMenu : RadialMenu {

    public Dictionary<Action, GameObject> buttons;

    private NodeData node;

    public override List<GameObject> getButtons() {
        Action[] actions = gameObject.GetComponents<Action>();
        buttons = new Dictionary<Action, GameObject>();

        foreach (Action a in actions) {
            GameObject realButton = (GameObject)Instantiate(a.button, transform.position, Quaternion.identity);
            realButton.transform.parent = this.gameObject.transform;
            buttons[a] = realButton;
        }

        return buttons.Values.ToList();
    }

	public override void Start () {
        base.Start();

		foreach (Action a in buttons.Keys) {
            Action actionCopy = a;
            buttons[a].GetComponent<ActionButton>().OnClick += () => ActionController.instance.selectAction(actionCopy);
		}

        node = GetComponent<NodeData>();
        node.OnHover += () =>
        {
            // Show node menu
            if (node.Owner == TurnController.instance.CurrentPlayer && !ActionController.instance.inSelectionState)
            {
                this.show();
            }
        };
	}

	public override void show() {
		if (node.isScheduled || gameObject.GetComponent<NodeData>().nTurnsUntilAvailable > 0 || TurnController.instance.BetweenTurns ||
            !TurnController.instance.CurrentPlayer.IsLocalHumanPlayer || TurnController.instance.CurrentPlayer.actionPointsRemaining() <= 0) {
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
}
