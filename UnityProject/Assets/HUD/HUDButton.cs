using UnityEngine;
using System.Collections;
using Assets.Player;

public abstract class HUDButton : Highlightable {

	// x and y in screen coordinates (position of top right corner)
	protected float x = 0.01f, y = 0.01f;

	void Update() {
		Vector3 screenPos = new Vector3(Screen.width - (x * Screen.currentResolution.height), Screen.height - (y * Screen.currentResolution.height), 0);
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
}
