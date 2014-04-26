using UnityEngine;
using System.Collections;

public abstract class Action : MonoBehaviour {

	public bool isTargeting;
	public GameObject button;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract GameObject[] getPossibleTargets();

	public abstract bool scheduleUse(GameObject target);

	public abstract void clearScheduledUse();

	public GameObject getButton() {
		return button;
	}

    public abstract void Activate();
}
