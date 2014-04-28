using UnityEngine;
using System.Collections;

public class ButtonToggler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void toggle(GameObject prevChild) {
		foreach (Transform child in transform) {
			if (child != prevChild) {
				child.gameObject.SetActive(true);
				prevChild.SetActive(false);
			}
		}
	}
}
