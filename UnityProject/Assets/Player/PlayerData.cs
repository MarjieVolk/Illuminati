using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Player;

public class PlayerData : MonoBehaviour {

    public string Name;
	private int actionPoints;
    private List<Action> selectedActions;
    public NodeData StartingNode;

	private static List<NodeData> startingNodes;
	private static System.Random gen;

	// Use this for initialization
	void Start () {
        startTurn();

		if (StartingNode == null) {
			if (startingNodes == null) {
				gen = new System.Random();
				NodeData[] nodes = Object.FindObjectsOfType<NodeData>();
				startingNodes = new List<NodeData>();
				foreach (NodeData node in nodes) {
					if (node.isStartNode) {
						startingNodes.Add(node);
					}
				}
			}

			NodeData ourStart = startingNodes[gen.Next(startingNodes.Count)];
			startingNodes.Remove(ourStart);
			StartingNode = ourStart;
			ourStart.Owner = this;
		}
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

        GUI.Label(new Rect(0, 550, 100, 90), "Action points: " + actionPoints);
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
	}

    public void startTurn()
    {
        actionPoints = 4;
        selectedActions = new List<Action>();
    }
}
