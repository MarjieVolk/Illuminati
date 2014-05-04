using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Player;
using Assets.Graph.Nodes;
using System.Linq;
using Assets.AI;

public class PlayerData : MonoBehaviour {

    public string Name;
    public int ActionPointsPerTurn;
    private int actionPoints;
    private List<Action> selectedActions;
    public NodeData StartingNode;
    public int NumStartingNodes;
    public bool IsLocalHumanPlayer; //true if this player is human and sitting at this computer, false if it's a human not at this computer or an AI
    public EnlightenedAI AI;

	private GUIStyle style;

	private static List<NodeData> startingNodes;
	private static System.Random gen = new System.Random();

	private const float WIDTH = 50;
	private const float MARGIN = 5;

    void Reset()
    {
        NumStartingNodes = 2;
    }

    void Awake()
    {
        actionPoints = 0;
    }

	// Use this for initialization
	void Start () {
        selectStartingNode();

        selectSecondaryStartingNodes();

		// Increase starting node attacks
		Array values = Enum.GetValues(typeof(DominationType));
		foreach (DominationType type in values) {
			StartingNode.getAttackSkill(type).value += 15;
			StartingNode.getDefenseSkill(type).value += 10;
		}

		style = new GUIStyle();
		style.normal.textColor = Color.black;
		style.fontSize = 16;
		style.normal.background = InvestigateAction.MakeTextureOfColor(Color.gray);
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
            GraphUtility.instance.CaptureNode(node, StartingNode, DominationType.Bribe);
            node.startingOwner = this;
        }
    }

    private void selectStartingNode()
    {
        if (StartingNode == null)
        {
            if (startingNodes == null)
            {
                NodeData[] nodes = UnityEngine.Object.FindObjectsOfType<NodeData>();
                startingNodes = new List<NodeData>();
                foreach (NodeData node in nodes)
                {
                    if (node.isStartNode)
                    {
                        startingNodes.Add(node);
                    }
                }
            }

            NodeData ourStart = startingNodes[gen.Next(startingNodes.Count)];
            startingNodes.Remove(ourStart);
            StartingNode = ourStart;
        }
        StartingNode.startingOwner = this;
        StartingNode.Owner = this;
    }
	
	void Update() {
        if (this == TurnController.instance.CurrentPlayer)
        {
            if (IsLocalHumanPlayer)
            {
                float x = MARGIN;
                foreach (Action a in selectedActions)
                {
                    GameObject tag = a.getListScheduledTag();
                    tag.SetActive(true);
                    Vector3 screenPos = new Vector3(x + (WIDTH / 2.0f), MARGIN + (WIDTH / 2.0f), 0);
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                    tag.transform.position = new Vector3(worldPos.x, worldPos.y, -5);

                    x += WIDTH + MARGIN;
                }
            }
        }
	}

    void OnGUI()
    {
        if (this != TurnController.instance.CurrentPlayer) return;

        GUI.Label(new Rect(0, Screen.height - (Screen.height * 0.115f), 85, 20), "  Actions: <b>" + actionPoints + "</b>", style);
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
            toSelect.getListScheduledTag().GetComponent<ScheduledAction>().player = this;
            toSelect.getMapScheduledTag().GetComponent<ScheduledAction>().player = this;
        }

        return true;
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

        action.getListScheduledTag().SetActive(false);
        action.getMapScheduledTag().SetActive(false);
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
                ActionController.instance.selectAction(toSchedule);
                if (toSchedule.isTargeting)
                {
                    ActionController.instance.scheduleAction(toSchedule.Target);
                }
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
