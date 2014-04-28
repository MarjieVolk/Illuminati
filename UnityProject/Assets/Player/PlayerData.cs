using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Player;
using Assets.Graph.Nodes;

public class PlayerData : MonoBehaviour {

    public string Name;
    public int ActionPointsPerTurn;
    private int actionPoints;
    private List<Action> selectedActions;
    public NodeData StartingNode;

	private GUIStyle style;

	private static List<NodeData> startingNodes;
	private static System.Random gen;

	private const float WIDTH = 50;
	private const float MARGIN = 5;

    void Awake()
    {
        actionPoints = 0;
    }

	// Use this for initialization
	void Start () {
		if (StartingNode == null) {
			if (startingNodes == null) {
				gen = new System.Random();
				NodeData[] nodes = UnityEngine.Object.FindObjectsOfType<NodeData>();
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
		}
		StartingNode.startingOwner = this;
		StartingNode.Owner = this;

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
	
	void Update() {
		if (this != TurnController.instance.CurrentPlayer) return;
		float x = MARGIN;
		foreach (Action a in selectedActions) {
			GameObject tag = a.getListScheduledTag();
			tag.SetActive(true);
			Vector3 screenPos = new Vector3(x + (WIDTH / 2.0f), MARGIN + (WIDTH / 2.0f), 0);
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
			tag.transform.position = new Vector3(worldPos.x, worldPos.y, -5);

			x += WIDTH + MARGIN;
		}
	}

    void OnGUI()
    {
        if (this != TurnController.instance.CurrentPlayer) return;

        GUI.Label(new Rect(0, Screen.height - (Screen.height * 0.115f), 85, 20), "  Actions: <b>" + actionPoints + "</b>", style);
    }

    public void cancelAction(Action toCancel)
    {
        selectedActions.Remove(toCancel);
        toCancel.clearScheduled();
		toCancel.getListScheduledTag().SetActive(false);
		toCancel.getMapScheduledTag().SetActive(false);
        actionPoints++;
    }

    public bool scheduleAction(Action toSelect)
    {
        if (actionPoints <= 0) return false;

        selectedActions.Add(toSelect);
		toSelect.getListScheduledTag().GetComponent<ScheduledAction>().player = this;
		toSelect.getMapScheduledTag().GetComponent<ScheduledAction>().player = this;

        actionPoints--;
        return true;
    }

	public void endTurn() {
        foreach(Action action in selectedActions)
        {
            action.Activate();
			action.getListScheduledTag().SetActive(false);
			action.getMapScheduledTag().SetActive(false);
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
