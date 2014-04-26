using UnityEngine;
using System.Collections;

public class ActionButton : MonoBehaviour {

	public Sprite hover, selected;

	private Sprite normal;
	private SpriteRenderer spriteRenderer;

	private bool isSelected = false;
	private NodeMenu menu;

	public void setMenu(NodeMenu menu) {
		this.menu = menu;
	}

	// Use this for initialization
	void Start () {
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
		normal = spriteRenderer.sprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		if (!isSelected) {
			spriteRenderer.sprite = hover;
		}
	}

	void OnMouseExit() {
		if (!isSelected) {
			spriteRenderer.sprite = normal;
		}
	}

	void OnMouseUpAsButton() {
		isSelected = true;
		spriteRenderer.sprite = selected;
		menu.setActionSelected(true);
	}
}
