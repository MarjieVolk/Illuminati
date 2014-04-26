using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	private int actionPoints;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void endTurn() {
		actionPoints = 4;
		// Notify controller to advance turn
	}
}
