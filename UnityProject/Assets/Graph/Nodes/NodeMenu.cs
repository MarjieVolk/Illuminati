using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class NodeMenu : MonoBehaviour {

	public bool isShown { get; private set; }
	public bool isScheduled = false;

	private List<GameObject> buttons;
	private ActionButton prev = null;

	// Use this for initialization
	void Start () {
		buttons = new List<GameObject>();

		Action[] actions = this.gameObject.GetComponents<Action>();
		float angle = 90;
		float dAngle = 360.0f / 6;
		foreach (Action a in actions) {
			GameObject button = a.getButton();
			
			float y = Mathf.Sin((angle * Mathf.PI) / 180.0f);
			float x = Mathf.Cos((angle * Mathf.PI) / 180.0f);
			Vector3 offset = new Vector3(x, y, 0);
			offset.Normalize();
			offset *= 1;
			
			GameObject realButton = (GameObject) Instantiate(button, transform.position + offset, Quaternion.identity);
			realButton.transform.parent = this.gameObject.transform;

            ActionController actionController = FindObjectOfType<ActionController>();
            Action actionCopy = a;
            realButton.GetComponent<ActionButton>().OnClick += () => actionController.selectAction(actionCopy);
			buttons.Add(realButton);
			
			angle -= dAngle;
		}

		hide();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void show() {
		if (isScheduled || gameObject.GetComponent<NodeData>().nTurnsUntilAvailable > 0) {
			return;
		}

		isShown = true;
		foreach (GameObject obj in buttons) {
			obj.SetActive(true);
		}
	}

	public void hide() {
		isShown = false;
		clear();
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
		prev = null;
	}

	void OnMouseOver() {
		RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		foreach (RaycastHit2D hit in hits) {
			GameObject obj = hit.collider.gameObject;
			ActionButton button = obj.GetComponent<ActionButton>();
			if (button != null) {
				if (prev == button) return;

				if (prev != null) prev.OnMouseExit();
				button.OnMouseEnter();
				prev = button;
				return;
			}
		}

		// No button is being hovered over
		if (prev != null) {
			prev.GetComponent<ActionButton>().OnMouseExit();
			prev = null;
		}
	}

	void OnMouseUpAsButton() {
		RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		foreach (RaycastHit2D hit in hits) {
			GameObject obj = hit.collider.gameObject;
			ActionButton button = obj.GetComponent<ActionButton>();
			if (button != null) {
				button.OnMouseUpAsButton();
				return;
			}
		}
	}

	private void clearChildHighlights() {
		foreach (GameObject obj in buttons) {
			obj.GetComponent<ActionButton>().OnMouseExit();
		}
	}
}
