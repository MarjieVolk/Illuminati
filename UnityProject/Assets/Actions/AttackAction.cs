using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Player;

public class AttackAction : Action {

	private static System.Random gen;

    public NodeType strongAgainst;

    private SideEffect effect = null;

    public void setSideEffect(SideEffect effect) {
        this.effect = effect;
    }

	// Use this for initialization
	void Start () {
		IsTargeting = true;
		if (gen == null) gen = new System.Random();
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

        EdgeData edge = GraphUtility.instance.getConnectingEdge(thisNode, otherNode);
        if (edge.direction == EdgeData.EdgeDirection.Unusable) return;
		
        bool isWin = gen.NextDouble() <= getProbabilityOfWin(target);

		// Take node
		if (isWin) GraphUtility.instance.CaptureNode(otherNode, thisNode);

        if (effect != null) effect.additionalEffect(thisNode, target, isWin);
        effect = null;
	}

	public double getProbabilityOfWin(Targetable target) {
		NodeData otherNode = target.GetComponent<NodeData>();
		NodeData thisNode = this.GetComponent<NodeData>();

        bool isStrong = (otherNode.type == strongAgainst);

		// Find attacker attack score and defender defense score
		int defense = otherNode.power;
        int attack = thisNode.power;

		int minAttack = attack / 2;
		int maxAttack = attack * (isStrong ? 3 : 2);
		
		// Determine if node is captured
		double probability = 0;
		
		if (defense <= minAttack) {
			probability = 1;
		} else if (defense >= maxAttack) {
			probability = 0;
		} else {
			probability = ((double) (maxAttack - defense)) / ((double) (maxAttack - minAttack));
		}

		return probability;
	}
}
