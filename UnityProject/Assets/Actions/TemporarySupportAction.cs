﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Player;

public class TemporarySupportAction : Action {

    private static System.Random gen;

    public float maxVisDecrease, minVisDecrease;

	public const int DURATION = 2;

	void Start () {
		IsTargeting = true;
        if (gen == null) gen = new System.Random();
	}
	
	public override List<Targetable> getPossibleTargets() {
		NodeData thisNode = getNode();
		List<NodeData> nodes = graphUtility.getInfluencingNodes(thisNode);
		nodes.AddRange(graphUtility.getInfluencedNodes(thisNode));
		
		List<Targetable> targets = new List<Targetable>();
		foreach (NodeData node in nodes) {
			targets.Add(node);
		}
		return targets;
	}
	
	public override string getAdditionalTextForTarget(Targetable target) {
        NodeData thisNode = getNode();
		
		int increase = getIncrease(thisNode.getWorkingPower());
        if (increase > 0) {
            return "+" + increase + " (" + DURATION + " turns)";
        } else {
            return "--";
        }
	}
	
	protected override void doActivate(Targetable target) {
		NodeData otherNode = (NodeData) target;
        NodeData thisNode = getNode();

        doIncrease(thisNode, otherNode, 1.0f);
        putOnCooldown(DURATION);

        // Decrease edge visibility
        float visDecrease = (float) (gen.NextDouble() * (maxVisDecrease - minVisDecrease)) + minVisDecrease;
        graphUtility.getConnectingEdge(otherNode, thisNode).Visibility -= visDecrease;
	}

    public static void doIncrease(NodeData performer, NodeData target, float multiplier) {
        int increase = getIncrease(performer.getWorkingPower());
        target.temporaryIncrease((int) (increase * multiplier), DURATION);
    }

	public static int getIncrease(int value) {
		return (int) (value / 4.0f);
	}

    public override float expectedVisibilityModifier() {
        return (-minVisDecrease + -maxVisDecrease) / 2;
    }
}
