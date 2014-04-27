using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PermanentSupportAction : Action {

	private const int maxIncrease = 3;
	private const int maxDifference = 10;

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
		DominationType bestType = DominationType.Bribe;
		bool isAttack = true;
		int maxDifference = 0;

		NodeData node = (NodeData) target;
		NodeData thisNode = this.gameObject.GetComponent<NodeData>();
		Array values = Enum.GetValues(typeof(DominationType));

		foreach (DominationType type in values) {
			AttackSkill targetAttackSkill = node.getAttackSkill(type);
			AttackSkill thisAttackSkill = thisNode.getAttackSkill(type);
			int difference = thisAttackSkill.value - targetAttackSkill.value;

			if (difference > maxDifference) {
				maxDifference = difference;
				bestType = type;
				isAttack = true;
			}
			
			DefenseSkill targetDefenseSkill = node.getDefenseSkill(type);
			DefenseSkill thisDefenseSkill = thisNode.getDefenseSkill(type);
			difference = thisDefenseSkill.value - targetDefenseSkill.value;
			
			if (difference > maxDifference) {
				maxDifference = difference;
				bestType = type;
				isAttack = false;
			}
		}

		if (maxDifference == 0) {
			return "--";
		}

		int increase = getExpectedIncreaseAmount(maxDifference);
		int min = Math.Max(increase - 1, 0);
		int max = Math.Min(increase + 1, maxIncrease);
		string increaseStr = min == max ? ("" + min) : ("" + min + "-" + max);
		return "" + bestType.ToString() + (isAttack ? " Attack" : " Defense") + " +" + increaseStr;
	}

	protected override void doActivate(Targetable target) {
		NodeData node = (NodeData) target;
		NodeData thisNode = this.gameObject.GetComponent<NodeData>();

		Array values = Enum.GetValues(typeof(DominationType));
		foreach (DominationType type in values) {
			AttackSkill targetAttackSkill = node.getAttackSkill(type);
			AttackSkill thisAttackSkill = thisNode.getAttackSkill(type);
			targetAttackSkill.value += getIncreaseAmount( thisAttackSkill.value - targetAttackSkill.value);

			DefenseSkill targetDefenseSkill = node.getDefenseSkill(type);
			DefenseSkill thisDefenseSkill = thisNode.getDefenseSkill(type);
			targetDefenseSkill.value += getIncreaseAmount(thisDefenseSkill.value - targetDefenseSkill.value);
		}
	}

	private int getIncreaseAmount(int difference) {
		int avg = getExpectedIncreaseAmount(difference);
		double randomness = gen.NextDouble();
		if (randomness >= 0.8) {
			avg += 1;
		} else if (randomness <= 0.2) {
			avg -= 1;
		}

		// Bind avg to range 0-maxDifference
		return Math.Max(Math.Min(maxDifference, avg), 0);
	}

	private int getExpectedIncreaseAmount(int difference) {
		if (difference <= 0) {
			return 0;
		} else if (difference >= maxDifference) {
			return maxIncrease;
		} else {
			return difference / maxDifference;
		}
	}
}
