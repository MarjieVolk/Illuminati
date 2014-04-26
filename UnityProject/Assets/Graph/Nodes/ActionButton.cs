using UnityEngine;
using System.Collections;

public class ActionButton : MonoBehaviour {

    public event Highlightable.OnClickHandler OnClick;

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

	public void OnMouseEnter() {
		if (!isSelected) {
			Debug.Log("Set to hover");
			spriteRenderer.sprite = hover;
		}
	}

	public void OnMouseExit() {
		if (!isSelected) {
			Debug.Log("Set to normal");
			spriteRenderer.sprite = normal;
		}
	}

	public void OnMouseUpAsButton() {
		Debug.Log("Set to selected");
		isSelected = true;
		spriteRenderer.sprite = selected;
		menu.setActionSelected(true);

        if (null != OnClick) OnClick();
	}
}
