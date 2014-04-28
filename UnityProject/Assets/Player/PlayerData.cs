using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class PlayerData : MonoBehaviour {

    public string Name;
    public int ActionPointsPerTurn;
    private int actionPoints;
    private List<Action> selectedActions;
    public NodeData StartingNode;

	// Use this for initialization
	void Start () {
        actionPoints = 0;
        startTurn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (this != TurnController.instance.CurrentPlayer) return;
        //Make buttons for each selected action (to cancel it, you see), indicate how many action points remain
        int xLocation = 0;
        List<Action> toCancel = new List<Action>();
        foreach (Action action in selectedActions)
        {
            if (GUI.Button(new Rect(xLocation, 650, 100, 90), "" + xLocation))
            {
                toCancel.Add(action);
            }
            xLocation += 110;
        }

        foreach (Action action in toCancel) cancelAction(action);

        GUI.Label(new Rect(0, 550, 100, 90), "Actions Remaining: " + actionPoints);
    }

    public void cancelAction(Action toCancel)
    {
        selectedActions.Remove(toCancel);
        toCancel.clearScheduled();
        actionPoints++;
    }

    public bool scheduleAction(Action toSelect)
    {
        if (actionPoints <= 0) return false;

        selectedActions.Add(toSelect);
        actionPoints--;
        return true;
    }

	public void endTurn() {
        foreach(Action action in selectedActions)
        {
            action.Activate();
        }
        selectedActions = new List<Action>();
        actionPoints = 0;
	}

    public void startTurn()
    {
        actionPoints += ActionPointsPerTurn;
        selectedActions = new List<Action>();
    }

    public void addActionPoints(int numToAdd)
    {
        actionPoints += numToAdd;
    }
}
