using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeMenu : MonoBehaviour {

	public bool isShown { get; private set; }

	private bool isActionSelected = false;
	private List<GameObject> createdButtons;

	// Use this for initialization
	void Start () {
		createdButtons = new List<GameObject>();
		isShown = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void show() {
		if (isShown) {
			return;
		}

		isShown = true;
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
			//realButton.transform.parent = this.gameObject.transform;
			realButton.GetComponent<ActionButton>().setMenu(this);
			createdButtons.Add(realButton);

			angle -= dAngle;
		}
	}

	public void hide() {
		isShown = false;
		foreach (GameObject obj in createdButtons) {
			Destroy(obj);
		}
		createdButtons.Clear();
	}

	public void setActionSelected(bool isActionSelected) {
		this.isActionSelected = isActionSelected;
	}

	void OnMouseExit() {
		if (!isActionSelected) {
			//hide();
		}
	}
}
