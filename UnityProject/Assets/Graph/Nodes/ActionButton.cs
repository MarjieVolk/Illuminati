using UnityEngine;
using System.Collections;

public class ActionButton : MonoBehaviour {

    public event Highlightable.OnClickHandler OnClick;

	public Sprite hover, selected;

	private Sprite normal;

	private bool isSelected = false;
	private NodeMenu menu;

	public void setMenu(NodeMenu menu) {
		this.menu = menu;
	}

	// Use this for initialization
	void Start () {
		normal = this.gameObject.GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseEnter() {
		if (!isSelected && hover != null) {
			this.gameObject.GetComponent<SpriteRenderer>().sprite = hover;
		}
	}

	public void OnMouseExit() {
		if (!isSelected && normal != null) {
			this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
		}
	}

	public void OnMouseUpAsButton() {
		isSelected = true;
		if (selected != null) {
			this.gameObject.GetComponent<SpriteRenderer>().sprite = selected;
		}
		menu.setActionSelected(true);

        if (null != OnClick) OnClick();
	}
}
