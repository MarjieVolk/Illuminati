using UnityEngine;
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
		return "";
	}
	
	protected override void doActivate(Targetable target) {
		NodeData otherNode = (NodeData) target;
		NodeData thisNode = gameObject.GetComponent<NodeData>();
		Array values = Enum.GetValues(typeof(DominationType));
		
		foreach (DominationType type in values) {
			int increase = Mathf.CeilToInt(thisNode.getAttackSkill(type).getWorkingValue() / 3.0f);
			otherNode.getAttackSkill(type).temporaryIncrease(increase, duration);

			increase = Mathf.CeilToInt(thisNode.getDefenseSkill(type).getWorkingValue() / 3.0f);
			otherNode.getDefenseSkill(type).temporaryIncrease(increase, duration);
		}
	}
}
