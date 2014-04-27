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

	override public List<Highlightable> getPossibleTargets() {
		NodeData thisNode = this.gameObject.GetComponent<NodeData>();
		List<NodeData> nodes = GraphUtility.instance.getNeutralConnectedNodes(thisNode);
		List<Highlightable> ret = new List<Highlightable>();
		foreach (NodeData node in nodes) {
			if (this.gameObject.GetComponent<NodeData>().Owner != node.Owner) {
				ret.Add(node);
			}
		}
		return ret;
	}
	
	override protected void doActivate(Highlightable target) {
		NodeData otherNode = target.GetComponent<NodeData>();
		NodeData thisNode = this.GetComponent<NodeData>();
		
		List<EdgeData> thisEdges = GraphUtility.instance.getConnectedEdges(thisNode);
		EdgeData connection = null;
		foreach (EdgeData edge in thisEdges) {
			if (edge.nodeOne == otherNode.gameObject || edge.nodeTwo == otherNode.gameObject) {
				connection = edge;
				break;
			}
		}

		// Increase edge visibility, whether you win or not
		connection.triggerEdge(0.5f);

		// Find attacker attack score and defender defense score
		int targetDefense = otherNode.getDefense(attackType);
		int attack = thisNode.getDefense(attackType);
		
		int min = targetDefense / 2;
		int max = targetDefense * 2;

		// Determine if node is captured
		bool doCapture = false;

		if (attack <= min) {
			doCapture = false;
		} else if (attack >= max) {
			doCapture = true;
		} else {
			double probability = ((double) (attack - min)) / ((double) (max - min));
			doCapture = gen.NextDouble() <= probability;
		}
		
		// Take node
		if (doCapture) {
			otherNode.Owner = thisNode.Owner;

			foreach (EdgeData edge in GraphUtility.instance.getConnectedEdges(otherNode)) {
				edge.direction = EdgeData.EdgeDirection.Neutral;
			}

			connection.type = attackType;
			connection.direction = connection.nodeOne == thisNode ? EdgeData.EdgeDirection.OneToTwo : EdgeData.EdgeDirection.TwoToOne;
		}
	}
}
