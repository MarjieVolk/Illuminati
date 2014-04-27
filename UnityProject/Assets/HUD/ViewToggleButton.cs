using UnityEngine;
using System.Collections;
using Assets.Player;

public class ViewToggleButton : Highlightable {

	override public bool viewAsOwned(VisibilityController.Visibility visibility) {
		return visibility == VisibilityController.Visibility.Private;
	}

	void Update() {
		Vector3 screenPos = new Vector3(Screen.width - 10, Screen.height - 10, 0);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

		Bounds bounds = gameObject.GetComponent<SpriteRenderer>().bounds;
		float width = bounds.extents.x;
		float height = bounds.extents.y;

		transform.position = new Vector3(worldPos.x - width, worldPos.y - height, -5);
	}

	void OnMouseEnter() {
		setHighlighted(true);
	}

	void OnMouseExit() {
		setHighlighted(false);
	}

	void OnMouseUpAsButton() {
		VisibilityController.instance.ToggleVisibility();
	}
}
