using UnityEngine;
using System.Collections;
using Assets.Player;

public class ExecuteActionsButton : HUDButton {

	bool isClick = false;
	
	void Awake () {
		x = 0.01f;
		y = 0.157f;
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
		TurnController.instance.ExecuteActions();
		OnMouseUp();

		GameObject parent = transform.parent.gameObject;
		ButtonToggler toggler = parent.GetComponent<ButtonToggler>();
		toggler.toggle(gameObject);
	}
}
