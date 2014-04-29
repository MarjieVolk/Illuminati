﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TemporarySupportAction : Action {

	private const int duration = 2;

	void Start () {
		isTargeting = true;
	}
	
	public override List<Targetable> getPossibleTargets() {
		NodeData thisNode = this.gameObject.GetComponent<NodeData>();
		List<NodeData> nodes = GraphUtility.instance.getInfluencingNodes(thisNode);
		nodes.AddRange(GraphUtility.instance.getInfluencedNodes(thisNode));
		
		List<Targetable> targets = new List<Targetable>();
		foreach (NodeData node in nodes) {
			targets.Add(node);
		}
		return targets;
	}
	
	public override string getAdditionalTextForTarget(Targetable target) {
		NodeData thisNode = this.gameObject.GetComponent<NodeData>();
		Array values = Enum.GetValues(typeof(DominationType));
		
		string ret = "";
		foreach (DominationType type in values) {
			int increase = getIncrease(thisNode.getAttackSkill(type).getWorkingValue());
			if (increase > 0) {
				ret += type.ToString() + " Attack +" + increase + "\n";
			}

			increase = getIncrease(thisNode.getDefenseSkill(type).getWorkingValue());			
			if (increase > 0) {
				ret += type.ToString() + " Defense +" + increase + "\n";
			}
		}
		
		return ret.Equals("") ? "--" : (ret + "(" + duration + " turns)");
	}
	
	protected override void doActivate(Targetable target) {
		NodeData otherNode = (NodeData) target;
		NodeData thisNode = gameObject.GetComponent<NodeData>();
		Array values = Enum.GetValues(typeof(DominationType));
		
		foreach (DominationType type in values) {
			int increase = getIncrease(thisNode.getAttackSkill(type).getWorkingValue());
			otherNode.getAttackSkill(type).temporaryIncrease(increase, duration);

			increase = getIncrease(thisNode.getDefenseSkill(type).getWorkingValue());
			otherNode.getDefenseSkill(type).temporaryIncrease(increase, duration);
		}

		// Freeze node for duration
		thisNode.nTurnsUntilAvailable = duration;
	}

	private int getIncrease(int value) {
		return Mathf.CeilToInt(value / 3.0f);
	}
}
