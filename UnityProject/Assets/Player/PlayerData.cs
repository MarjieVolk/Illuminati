using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

    public string Name;
	private int actionPoints;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void endTurn() {
		actionPoints = 4;
	}

    public void startTurn()
    {

    }
}
