using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BribeAttackAction : Action {

	// Use this for initialization
	void Start () {
		isTargeting = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public List<Highlightable> getPossibleTargets() {
		return null;
	}
	
	override public bool scheduleUse(Highlightable target) {
		return false;
	}
	
	override public void clearScheduledUse() {

	}
	
	override public void Activate() {

	}
}
