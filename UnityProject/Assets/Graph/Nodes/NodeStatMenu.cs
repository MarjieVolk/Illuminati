using UnityEngine;
using System.Collections;

public class NodeStatMenu : MonoBehaviour {

	public GameObject menuBackground;

	private bool isShown = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		if (menuBackground) {
			// Display menu background
		}
		isShown = true;
	}

	void OnMouseExit() {
		if (menuBackground) {
			// Hide menu background
		}
		isShown = false;
	}

	void OnGUI() {
		if (isShown) {
			NodeData data = gameObject.GetComponent<NodeData>();
			string text = "\t\tAttack\tDefense\n" +
					"Bribe\t\t\t" + data.getAttack(DominationType.Bribe) + "\t" + data.getDefense(DominationType.Bribe) + "\n" +
					"Blackmail\t" + data.getAttack(DominationType.Blackmail) + "\t" + data.getDefense(DominationType.Blackmail) + "\n" +
					"Threaten\t" + data.getAttack(DominationType.Threaten) + "\t" + data.getDefense(DominationType.Threaten) + "\n";
				
			GUI.Label(new Rect(transform.position.x + 0.7f, transform.position.y + 0.2f, 300, 100), text);
		}
	}
}
