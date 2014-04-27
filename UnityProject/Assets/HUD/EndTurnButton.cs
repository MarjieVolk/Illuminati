using UnityEngine;
using Assets.Player;

public class EndTurnButton : HUDButton {

	bool isClick = false;

	void Awake () {
		x = 10;
		y = 155;
	}
	
	override public bool viewAsOwned(VisibilityController.Visibility visibility) {
		return isClick;
	}

	void OnMouseDown() {
		isClick = true;
		updateSprites();
	}

	void OnMouseUp() {
		isClick = false;
		updateSprites();
	}

	void OnMouseUpAsButton() {
		TurnController.instance.NextTurn();
	}
}
