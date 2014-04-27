using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Action : MonoBehaviour {

	public GameObject button;
	
	public bool isTargeting { get; set;}
    private Highlightable target;

	public abstract List<Highlightable> getPossibleTargets();

	public GameObject getButton() {
		return button;
	}

    public void Activate() {
		doActivate(target);
		clearScheduled();
	}

	protected abstract void doActivate(Highlightable target);

    public bool SetScheduled(Highlightable target)
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
