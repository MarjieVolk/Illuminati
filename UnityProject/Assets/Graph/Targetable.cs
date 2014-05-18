using UnityEngine;
using System.Collections;
using Assets.Graph.Nodes;
using System;
using System.Collections.Generic;

public abstract class Targetable : MonoBehaviour {

    protected List<Action> targetingActions;

	protected virtual void Start () {
        targetingActions = new List<Action>();
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
