using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeMenu : MonoBehaviour {

	bool isActionSelected = false;

	private List<Object> createdButtons;

	// Use this for initialization
	void Start () {
		createdButtons = new List<Object>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void show() {
		Action[] actions = this.gameObject.GetComponents<Action>();
		float angle = 90;
		float dAngle = 360.0f / 5;
		foreach (Action a in actions) {
			GameObject button = a.getButton();
			button.GetComponent<ActionButton>().setMenu(this);
			float y = Mathf.Sin((angle * Mathf.PI) / 180.0f);
			float x = Mathf.Cos((angle * Mathf.PI) / 180.0f);
			Vector3 offset = new Vector3(x, y, 0);
			offset.Normalize();
			offset *= 0.8f;
			createdButtons.Add(Instantiate(button, transform.position + offset, Quaternion.identity));
			angle -= dAngle;
		}
	}

	public void hide() {
		foreach (Object obj in createdButtons) {
			Destroy(obj);
		}
		createdButtons.Clear();
	}

	public void setActionSelected(bool isActionSelected) {
		this.isActionSelected = isActionSelected;
	}

	void OnMouseExit() {
		if (!isActionSelected) {
			hide();
		}
	}
}
