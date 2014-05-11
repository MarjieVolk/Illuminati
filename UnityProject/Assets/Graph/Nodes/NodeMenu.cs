using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class NodeMenu : RadialMenu {

	public bool isScheduled = false;

	public override void Start () {
        base.Start();

		foreach (Action a in buttons.Keys) {
            Action actionCopy = a;
            buttons[a].GetComponent<ActionButton>().OnClick += () => ActionController.instance.selectAction(actionCopy);
		}
	}

	public override void show() {
		if (isScheduled || gameObject.GetComponent<NodeData>().nTurnsUntilAvailable > 0 || TurnController.instance.BetweenTurns ||
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
