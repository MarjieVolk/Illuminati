using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Action : MonoBehaviour {

	public GameObject button;
	
	protected bool isTargeting;
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

        return true;
    }

    public void clearScheduled()
    {
        target = null;
        isScheduled = false;
    }
}
