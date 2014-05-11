using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class RadialMenu : MonoBehaviour {

    public float radius = 1;
	public bool isShown { get; private set; }

	public Dictionary<Action, GameObject> buttons;
	private NodeButton prev = null;

	// Use this for initialization
	public virtual void Start () {
        buttons = new Dictionary<Action, GameObject>();

		Action[] actions = this.gameObject.GetComponents<Action>();
		float angle = 90;
		float dAngle = 360.0f / actions.Length;
		foreach (Action a in actions) {
			GameObject button = a.getButton();
			
			float y = Mathf.Sin((angle * Mathf.PI) / 180.0f);
			float x = Mathf.Cos((angle * Mathf.PI) / 180.0f);
			Vector3 offset = new Vector3(x, y, 0);
			offset.Normalize();
			offset *= radius;
			
			GameObject realButton = (GameObject) Instantiate(button, transform.position + offset, Quaternion.identity);
			realButton.transform.parent = this.gameObject.transform;

			buttons[a] = realButton;
			
			angle -= dAngle;
		}

		hide();
	}

	public virtual void show() {
		isShown = true;
		foreach (GameObject obj in buttons.Values) {
			obj.SetActive(true);
		}
	}

	public void hide() {
		isShown = false;
		clear();
		foreach (GameObject obj in buttons.Values) {
			obj.SetActive(false);
		}
	}

	public void clear() {
		foreach (GameObject obj in buttons.Values) {
			obj.GetComponent<NodeButton>().clear();
		}
	}

	public virtual void OnMouseExit() {
		clearChildHighlights();
		prev = null;
	}

	public void OnMouseOver() {
        if (!isShown) return;

        // Transfer relevant events to buttons
		RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		foreach (RaycastHit2D hit in hits) {
			GameObject obj = hit.collider.gameObject;
			NodeButton button = obj.GetComponent<NodeButton>();
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
			prev.GetComponent<NodeButton>().OnMouseExit();
			prev = null;
		}
	}

	public void OnMouseUpAsButton() {
        if (!isShown) return;

		RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		foreach (RaycastHit2D hit in hits) {
			GameObject obj = hit.collider.gameObject;
			NodeButton button = obj.GetComponent<NodeButton>();
			if (button != null) {
				button.OnMouseUpAsButton();
				return;
			}
		}
	}

	private void clearChildHighlights() {
		foreach (GameObject obj in buttons.Values) {
			obj.GetComponent<NodeButton>().OnMouseExit();
		}
	}
}
