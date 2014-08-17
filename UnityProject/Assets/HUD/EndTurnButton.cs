using UnityEngine;
using Assets.Player;

public class EndTurnButton : HUDButton {

	bool isClick = false;

	void Awake () {
		x = 0.01f;
		y = 0.157f;
		
		// Hide this button
		gameObject.SetActive(false);
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
		turnController.NextTurn();
        OnMouseUp();
        OnMouseExit();
		transform.parent.gameObject.GetComponent<ButtonToggler>().toggle(gameObject);
	}
}
