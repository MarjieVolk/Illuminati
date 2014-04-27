using UnityEngine;
using System.Collections;
using Assets.Player;

public class ViewToggleButton : HUDButton {
	
	override public bool viewAsOwned(VisibilityController.Visibility visibility) {
		return visibility == VisibilityController.Visibility.Private;
	}

	void Awake() {
		x = 10;
		y = 10;
	}

	void OnMouseUpAsButton() {
		VisibilityController.instance.ToggleVisibility();
	}
}
