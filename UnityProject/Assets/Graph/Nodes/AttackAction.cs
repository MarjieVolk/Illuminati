using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttackAction : Action {

	private static System.Random gen;

	public DominationType attackType;

	// Use this for initialization
	void Start () {
		isTargeting = true;
		if (gen == null) gen = new System.Random();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public List<Targetable> getPossibleTargets() {
		NodeData thisNode = this.gameObject.GetComponent<NodeData>();
		List<NodeData> nodes = GraphUtility.instance.getNeutralConnectedNodes(thisNode);
		List<Targetable> ret = new List<Targetable>();
		foreach (NodeData node in nodes) {
            //make sure captures can't create a cycle
            if (GraphUtility.instance.getNodesReachableFrom(node, false).Contains(thisNode))
                continue;
            if (FindObjectsOfType<PlayerData>().Any((player) => player.StartingNode == node))
                continue;
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
		
		// Take node
		if (gen.NextDouble() <= getProbabilityOfWin(target)) {
            GraphUtility.instance.CaptureNode(otherNode, thisNode, attackType);
		}
	}

	private double getProbabilityOfWin(Targetable target) {
		NodeData otherNode = target.GetComponent<NodeData>();
		NodeData thisNode = this.GetComponent<NodeData>();
		
		// Find attacker attack score and defender defense score
		int targetDefense = otherNode.getDefense(attackType);
		int attack = thisNode.getAttack(attackType);

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
