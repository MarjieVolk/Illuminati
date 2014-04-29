using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PermanentSupportAction : Action {

	private const float minProportion = 0.2f;
	private const float maxProportion = 0.6f;

	private static System.Random gen;

	void Start () {
		isTargeting = true;
		if (gen == null) gen = new System.Random();
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
		NodeData node = (NodeData) target;
		NodeData thisNode = this.gameObject.GetComponent<NodeData>();
		Array values = Enum.GetValues(typeof(DominationType));

		string ret = "";
		foreach (DominationType type in values) {
			AttackSkill targetAttackSkill = node.getAttackSkill(type);
			AttackSkill thisAttackSkill = thisNode.getAttackSkill(type);
			int difference = thisAttackSkill.getWorkingValue() - targetAttackSkill.getWorkingValue();

			if (difference > 0) {
				int min = (int) (difference * minProportion);
				int max = (int) (difference * maxProportion);
				string increaseStr = min == max ? ("" + min) : ("" + min + "-" + max);
                if ("" != ret) ret += "\n";
				ret += type.ToString() + " Attack +" + increaseStr;
			}
			
			DefenseSkill targetDefenseSkill = node.getDefenseSkill(type);
			DefenseSkill thisDefenseSkill = thisNode.getDefenseSkill(type);
			difference = thisDefenseSkill.getWorkingValue() - targetDefenseSkill.getWorkingValue();
			
			if (difference > 0) {
				int min = (int) (difference * minProportion);
				int max = (int) (difference * maxProportion);
                string increaseStr = min == max ? ("" + min) : ("" + min + "-" + max);
                if ("" != ret) ret += "\n";
				ret += type.ToString() + " Defense +" + increaseStr;
			}
		}

		return ret == null ? "--" : ret;
	}

	protected override void doActivate(Targetable target) {
		NodeData node = (NodeData) target;
		NodeData thisNode = this.gameObject.GetComponent<NodeData>();

		Array values = Enum.GetValues(typeof(DominationType));
		foreach (DominationType type in values) {
			AttackSkill targetAttackSkill = node.getAttackSkill(type);
			AttackSkill thisAttackSkill = thisNode.getAttackSkill(type);
			targetAttackSkill.value += getIncreaseAmount( thisAttackSkill.getWorkingValue() - targetAttackSkill.getWorkingValue());

			DefenseSkill targetDefenseSkill = node.getDefenseSkill(type);
			DefenseSkill thisDefenseSkill = thisNode.getDefenseSkill(type);
			targetDefenseSkill.value += getIncreaseAmount(thisDefenseSkill.getWorkingValue() - targetDefenseSkill.getWorkingValue());
		}
	}

	private int getIncreaseAmount(int difference) {
		float min = difference * minProportion;
		float max = difference * maxProportion;
		double randomness = gen.NextDouble();
		return (int) (((max - min) * randomness) + min);
	}
}
