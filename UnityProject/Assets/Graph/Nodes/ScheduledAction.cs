using UnityEngine;
using System.Collections;
using Assets.Player;

public class ScheduledAction : Highlightable {

	public Action action;
	public PlayerData player;

	private ScheduledAction sister;

	public override bool viewAsOwned(VisibilityController.Visibility visibility) {
		return false;
	}

	public void setSister(ScheduledAction other) {
		other.sister = this;
		this.sister = other;
	}

	void OnMouseUpAsButton() {
		// Cancel action
		if (action.target != null) action.target.setHighlighted(false);
		player.cancelAction(action);
	}

	void OnMouseEnter() {
		setHighlighted(true);
		sister.setHighlighted(true);
		if (action.target != null) action.target.setHighlighted(true);
	}

	void OnMouseExit() {
		setHighlighted(false);
		sister.setHighlighted(false);
		if (action.target != null) action.target.setHighlighted(false);
	}
}
