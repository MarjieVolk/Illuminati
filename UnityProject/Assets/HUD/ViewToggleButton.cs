using UnityEngine;
using System.Collections;
using Assets.Player;

public class ViewToggleButton : HUDButton {
	
	override public bool viewAsOwned(VisibilityController.Visibility visibility) {
		return visibility == VisibilityController.Visibility.Private;
	}

	void Awake() {
		x = 0.01f;
		y = 0.01f;
	}

	void OnMouseUpAsButton() {
		VisibilityController.instance.ToggleVisibility();
	}
}
