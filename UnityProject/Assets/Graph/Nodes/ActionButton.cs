using UnityEngine;
using System.Collections;
using Assets.Player;

public class ActionButton : MonoBehaviour {

    public event Highlightable.OnClickHandler OnClick;

	public Sprite hover, selected;

	private Sprite normal;

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

	/// <summary>
	/// Returns the sprite to normal, regardless of situation
	/// </summary>
	public void clear() {
		this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
	}

	public void OnMouseEnter() {
		if (!ActionController.instance.inSelectionState && hover != null) {
			this.gameObject.GetComponent<SpriteRenderer>().sprite = hover;
		}
	}

	public void OnMouseExit() {
		if (!ActionController.instance.inSelectionState && normal != null) {
			this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
		}
	}

	public void OnMouseUpAsButton() {
		if (ActionController.instance.inSelectionState) {
			return;
		}

		if (selected != null) {
			this.gameObject.GetComponent<SpriteRenderer>().sprite = selected;
		}

        if (null != OnClick) OnClick();
	}
}
