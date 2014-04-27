using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Action : MonoBehaviour {

	public GameObject button;
	
	public bool isTargeting { get; set;}
    protected bool isScheduled;
    protected Highlightable target;

	public abstract List<Highlightable> getPossibleTargets();

	public GameObject getButton() {
		return button;
	}

    public abstract void Activate();

    public bool SetScheduled(Highlightable target)
    {
        if (isTargeting && null == target) return false;
        if (!isTargeting && null != target) return false;
        if (isTargeting && !getPossibleTargets().Contains(target)) return false;

        this.target = target;
        isScheduled = true;

		gameObject.GetComponent<NodeMenu>().clear();
		gameObject.GetComponent<NodeMenu>().hide();

        return true;
    }

    public void clearScheduled()
    {
        target = null;
        isScheduled = false;
    }
}
