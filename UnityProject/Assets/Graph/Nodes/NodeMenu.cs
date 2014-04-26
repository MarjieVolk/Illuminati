using UnityEngine;
using System.Collections;

public class NodeMenu : MonoBehaviour {

	bool isActionSelected = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void show() {
		Action[] actions = this.gameObject.GetComponents<Action>();
		float angle = 0;
		float dAngle = 360.0f / 5;
		foreach (Action a in actions) {
			GameObject button = a.getButton();
			float y = Mathf.Sin(angle);
			float x = Mathf.Cos(angle);
			Vector3 offset = new Vector3(x, y, 0);
			offset.Normalize();
			offset *= 2;
			Instantiate(button, transform.position + offset, Quaternion.identity);
			angle += dAngle;
		}
	}

	public void hide() {

	}

	void OnMouseExit() {
		if (!isActionSelected) {
			hide();
		}
	}
}
