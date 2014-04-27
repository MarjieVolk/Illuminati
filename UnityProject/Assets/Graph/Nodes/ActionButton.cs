using UnityEngine;
using System.Collections;
using Assets.Player;

public class ActionButton : MonoBehaviour {

    public event Highlightable.OnClickHandler OnClick;

	public Sprite normal, hover, selected;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
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
