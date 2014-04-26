using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Action : MonoBehaviour {

	public bool isTargeting;
	public GameObject button;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract List<Highlightable> getPossibleTargets();

	public abstract bool scheduleUse(Highlightable target);

	public abstract void clearScheduledUse();

	public GameObject getButton() {
		return button;
	}

    public abstract void Activate();
}
