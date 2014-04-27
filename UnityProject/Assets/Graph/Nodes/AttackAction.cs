using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AttackAction : Action {

	private static System.Random gen;

	public DominationType attackType;

	// Use this for initialization
	void Start () {
		isTargeting = true;
		gen = new System.Random();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public List<Targetable> getPossibleTargets() {
		NodeData thisNode = this.gameObject.GetComponent<NodeData>();
		List<NodeData> nodes = GraphUtility.instance.getNeutralConnectedNodes(thisNode);
		List<Targetable> ret = new List<Targetable>();
		foreach (NodeData node in nodes) {
            ret.Add(node);
		}
		return ret;
	}
	
	override public string getAdditionalTextForTarget(Targetable target) {
		return "" + (int) (100 * getProbabilityOfWin(target)) + "%";
	}
	
	override protected void doActivate(Targetable target) {
		NodeData otherNode = target.GetComponent<NodeData>();
		NodeData thisNode = this.GetComponent<NodeData>();
		EdgeData connection = GraphUtility.instance.getConnectingEdge(otherNode, thisNode);

		// Increase edge visibility, whether you win or not
		connection.triggerEdge(0.5f);
		
		// Take node
		if (gen.NextDouble() <= getProbabilityOfWin(target)) {
			otherNode.Owner = thisNode.Owner;

            //clear only opponent's dominations/connections to this node
			foreach (EdgeData edge in GraphUtility.instance.getConnectedEdges(otherNode)) {
                if (edge.nodeOne.GetComponent<NodeData>().Owner != edge.nodeTwo.GetComponent<NodeData>().Owner)
                {
                    edge.direction = EdgeData.EdgeDirection.Neutral;
                }
			}

			connection.type = attackType;
			connection.direction = connection.nodeOne == thisNode ? EdgeData.EdgeDirection.OneToTwo : EdgeData.EdgeDirection.TwoToOne;
		}
	}

	private double getProbabilityOfWin(Targetable target) {
		NodeData otherNode = target.GetComponent<NodeData>();
		NodeData thisNode = this.GetComponent<NodeData>();
		
		// Find attacker attack score and defender defense score
		int targetDefense = otherNode.getDefense(attackType);
		int attack = thisNode.getDefense(attackType);

		int min = targetDefense / 2;
		int max = targetDefense * 2;
		
		// Determine if node is captured
		double probability = 0;
		
		if (attack <= min) {
			probability = 0;
		} else if (attack >= max) {
			probability = 1;
		} else {
			probability = ((double) (attack - min)) / ((double) (max - min));
		}

		return probability;
	}
}
