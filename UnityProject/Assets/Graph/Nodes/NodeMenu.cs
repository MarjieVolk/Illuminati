using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class NodeMenu : MonoBehaviour {

	public bool isShown { get; private set; }
	public bool isScheduled = false;

	private List<GameObject> buttons;

	// Use this for initialization
	void Start () {
		buttons = new List<GameObject>();

		Action[] actions = this.gameObject.GetComponents<Action>();
		float angle = 90;
		float dAngle = 360.0f / 5;
		foreach (Action a in actions) {
			GameObject button = a.getButton();
			
			float y = Mathf.Sin((angle * Mathf.PI) / 180.0f);
			float x = Mathf.Cos((angle * Mathf.PI) / 180.0f);
			Vector3 offset = new Vector3(x, y, 0);
			offset.Normalize();
			offset *= 0.8f;
			
			GameObject realButton = (GameObject) Instantiate(button, transform.position + offset, Quaternion.identity);
			realButton.transform.parent = this.gameObject.transform;
			realButton.GetComponent<ActionButton>().setMenu(this);

            ActionController actionController = FindObjectOfType<ActionController>();
            realButton.GetComponent<ActionButton>().OnClick += () => actionController.selectAction(a);
			buttons.Add(realButton);
			
			angle -= dAngle;
		}

		hide();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void show() {
		if (isScheduled) {
			return;
		}

		isShown = true;
		foreach (GameObject obj in buttons) {
			obj.SetActive(true);
		}
	}

	public void hide() {
		isShown = false;
		foreach (GameObject obj in buttons) {
			obj.SetActive(false);
		}
	}

	public void clear() {
		foreach (GameObject obj in buttons) {
			obj.GetComponent<ActionButton>().clear();
		}
	}

	void OnMouseExit() {
		if (!ActionController.instance.inSelectionState) {
			hide();
		}
		clearChildHighlights();
	}

	void OnMouseOver() {
		clearChildHighlights();

		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		GameObject obj = hit.collider.gameObject;
		ActionButton button = obj.GetComponent<ActionButton>();
		if (button != null) {
			button.OnMouseEnter();
		}
	}

	void OnMouseUpAsButton() {
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		GameObject obj = hit.collider.gameObject;
		ActionButton button = obj.GetComponent<ActionButton>();
		if (button != null) {
			button.OnMouseUpAsButton();
		}
	}

	private void clearChildHighlights() {
		foreach (GameObject obj in buttons) {
			obj.GetComponent<ActionButton>().OnMouseExit();
		}
	}
}
