using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Player;
using Assets.Graph.Nodes;
using System.Linq;
using Assets.AI;
using Assets.Actions;
using Assets;

public class PlayerData : DependencyResolvingComponent {

    public string PlayerName;
    public int ActionPointsPerTurn;
    public int turnOrder = 0;
    public int startingNodeBonus = 7;

    private int actionPoints;
    private List<Action> selectedActions;
    public NodeData StartingNode;
    public int NumStartingNodes;
    public bool IsLocalHumanPlayer; //true if this player is human and sitting at this computer, false if it's a human not at this computer or an AI
    public EnlightenedAI AI;

	private static System.Random gen = new System.Random();

    void Reset()
    {
        NumStartingNodes = 2;
    }

    void Awake()
    {
        actionPoints = 0;
        selectedActions = new List<Action>();
    }

    public void init() {
        selectStartingNode();
        selectSecondaryStartingNodes();

        // Increase starting node power
        StartingNode.power += startingNodeBonus;
    }

    private void selectSecondaryStartingNodes()
    {
        List<NodeData> adjacentNodes = GraphUtility.instance.getConnectedNodes(StartingNode)
            .Where<NodeData>((node) => node.isSecondaryStartNode)
            .ToList<NodeData>();
        List<NodeData> toCapture = new List<NodeData>();
        foreach (NodeData node in adjacentNodes)
        {
            if (node.Owner == null)
            {
                toCapture.Add(node);
                if (toCapture.Count >= NumStartingNodes)
                {
                    break;
                }
            }
        }

        foreach (NodeData node in toCapture)
        {
            GraphUtility.instance.CaptureNode(node, StartingNode);
            node.startingOwner = this;
        }
    }

    private void selectStartingNode()
    {
        if (StartingNode == null)
        {
            NodeData[] nodes = UnityEngine.Object.FindObjectsOfType<NodeData>();
            List<NodeData> startingNodes = new List<NodeData>();
            foreach (NodeData node in nodes)
            {
                if (node.isStartNode && node.startingOwner == null) startingNodes.Add(node);
            }

            NodeData ourStart = startingNodes[gen.Next(startingNodes.Count)];
            StartingNode = ourStart;
        }
        StartingNode.startingOwner = this;
        StartingNode.Owner = this;
    }
	
	void Update() {
        if (TurnController != null && this == TurnController.CurrentPlayer)
        {
            if (IsLocalHumanPlayer)
            {
                for (int i = 0; i < selectedActions.Count; i++) {
                    GameObject tag = selectedActions[i].GetComponent<ActionGUI>().getListScheduledTag();
                    tag.SetActive(true);
                    tag.GetComponent<ScheduledAction>().updateSelfAsListTag(i);
                }
            }
        }
	}

    public int actionPointsRemaining() {
        return actionPoints;
    }

    public void cancelAction(Action toCancel)
    {
        selectedActions.Remove(toCancel);
        toCancel.clearScheduled();
        actionPoints++;

        if (IsLocalHumanPlayer)
        {
            disableScheduledTagsForAction(toCancel);
        }
    }

    public bool scheduleAction(Action toSelect)
    {
        if (actionPoints <= 0) return false;

        selectedActions.Add(toSelect);
        actionPoints--;

        if (IsLocalHumanPlayer)
        {
            toSelect.GetComponent<ActionGUI>().getListScheduledTag().GetComponent<ScheduledAction>().player = this;
            toSelect.GetComponent<ActionGUI>().getMapScheduledTag().GetComponent<ScheduledAction>().player = this;
        }

        return true;
    }

    public void setActionIndex(Action a, int index) {
        if (!selectedActions.Contains(a)) {
            Debug.LogError("Trying to set the index of a non-scheduled action: " + a);
            return;
        }

        index = Mathf.Min(index, selectedActions.Count - 1);
        selectedActions.Remove(a);
        selectedActions.Insert(index, a);
    }

    public int nScheduledActions() {
        return selectedActions.Count;
    }

	public void endTurn() {
        foreach(Action action in selectedActions)
        {
            action.Activate();
            if (IsLocalHumanPlayer)
            {
                disableScheduledTagsForAction(action);
            }
        }

        selectedActions = new List<Action>();
        actionPoints = 0;
	}

    private static void disableScheduledTagsForAction(Action action)
    {

        action.GetComponent<ActionGUI>().getListScheduledTag().SetActive(false);
        action.GetComponent<ActionGUI>().getMapScheduledTag().SetActive(false);
    }

    public bool startTurn()
    {
        actionPoints += ActionPointsPerTurn;
		selectedActions = new List<Action>();

        if (AI != null)
        {
            List<Action> decisions = AI.scheduleActions(actionPoints);
            decisions = decisions.GetRange(0, Math.Min(actionPoints, decisions.Count));

            foreach (Action toSchedule in decisions)
            {
                scheduleAction(toSchedule);
            }

            return true;
        }

        return false;
	}
	
	public void addActionPoints(int numToAdd)
    {
        actionPoints += numToAdd;
    }
}
