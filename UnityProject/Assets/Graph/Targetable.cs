using UnityEngine;
using System.Collections;
using Assets.Graph.Nodes;
using System;
using System.Collections.Generic;

public abstract class Targetable : MonoBehaviour {

    public event System.Action OnClicked;
    public event System.Action OnHover;
    public event System.Action OnEndHover;

    protected List<Action> targetingActions;

	protected virtual void Start () {
        targetingActions = new List<Action>();
	}

	void OnMouseUpAsButton() {
		if (OnClicked != null) OnClicked();
	}

    void OnMouseEnter() {
        if (OnHover != null) OnHover();
    }

    void OnMouseExit() {
        if (OnEndHover != null) OnEndHover();
    }

    public void addScheduledAction(Action action)
    {
        targetingActions.Add(action);
    }

    public void removeScheduledAction(Action action)
    {
        targetingActions.Remove(action);
    }
}
