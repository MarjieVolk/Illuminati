using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour {

    public string Name;
	private int actionPoints;
    private List<Action> selectedActions;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        //Make buttons for each selected action (to cancel it, you see), indicate how many action points remain
    }

    public void cancelAction(Action toCancel)
    {
        selectedActions.Remove(toCancel);
    }

    public void scheduleAction(Action toSelect)
    {
        selectedActions.Add(toSelect);
    }

	public void endTurn() {
        foreach(Action action in selectedActions)
        {
            action.Activate();
        }
	}

    public void startTurn()
    {
        actionPoints = 4;
        selectedActions = new List<Action>();
    }
}
