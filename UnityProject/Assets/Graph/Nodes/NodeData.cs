using UnityEngine;
using System.Collections;

public class NodeData : Highlightable {

	private GameObject[] edges;

	// Used for freezing the node for a certain number of turns after performing an action
	private int nTurnsUntilAvailable;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		// Show node menu
		this.gameObject.GetComponent<NodeMenu>().show();
	}
}
