using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Action : MonoBehaviour {

	public GameObject button;
	
	public bool isTargeting { get; set;}
    private Targetable target;

	public GameObject getButton() {
		return button;
	}

    public void Activate() {
		doActivate(target);
		clearScheduled();
	}
	
	public abstract List<Targetable> getPossibleTargets();
	public abstract string getAdditionalTextForTarget(Targetable target);
	protected abstract void doActivate(Targetable target);

    public bool SetScheduled(Targetable target)
    {
        if (isTargeting && null == target) return false;
        if (!isTargeting && null != target) return false;
        if (isTargeting && !getPossibleTargets().Contains(target)) return false;

		print("Target set to " + target);
        this.target = target;

		gameObject.GetComponent<NodeMenu>().clear();
		gameObject.GetComponent<NodeMenu>().hide();
		gameObject.GetComponent<NodeMenu>().isScheduled = true;

        return true;
    }

    public void clearScheduled()
    {
        target = null;
		gameObject.GetComponent<NodeMenu>().isScheduled = false;
	}
}
