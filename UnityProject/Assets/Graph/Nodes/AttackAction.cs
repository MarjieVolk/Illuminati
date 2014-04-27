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
		List<NodeData> nodes = Graph.instance.getNeutralConnectedNodes(thisNode);
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
		
		List<EdgeData> thisEdges = Graph.instance.getConnectedEdges(thisNode);
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
		DefenseSkill[] targetDefenses = target.gameObject.GetComponents<DefenseSkill>();
		DefenseSkill targetDefense = null;
		foreach (DefenseSkill d in targetDefenses) {
			if (d.type == attackType) {
				targetDefense = d;
				break;
			}
		}

		AttackSkill[] attacks = gameObject.GetComponents<AttackSkill>();
		AttackSkill attack = null;
		foreach (AttackSkill a in attacks) {
			if (a.type == attackType) {
				attack = a;
				break;
			}
		}
		
		int min = targetDefense.value / 2;
		int max = targetDefense.value * 2;

		// Determine if node is captured
		bool doCapture = false;

		if (attack.value <= min) {
			doCapture = false;
		} else if (attack.value >= max) {
			doCapture = true;
		} else {
			double probability = ((double) (attack.value - min)) / ((double) (max - min));
			doCapture = gen.NextDouble() <= probability;
		}
		
		// Take node
		if (doCapture) {
			otherNode.Owner = thisNode.Owner;

			connection.type = attackType;
			connection.direction = connection.nodeOne == thisNode ? EdgeData.EdgeDirection.OneToTwo : EdgeData.EdgeDirection.TwoToOne;
		}
	}
}
