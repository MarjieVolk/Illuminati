using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class PermanentSupportAction : Action {

    public float edgeVisModifierMultiplier = 0.9f;

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

    private Action<DominationType, AttackSkill, int> getDisplayTextHandler(String typeLabel, StringBuilder ret)
    {
        return (type, skill, diff) =>
        {
            if (diff > 0)
            {
                int min = (int)(diff * minProportion);
                int max = (int)(diff * maxProportion);
                string increaseStr = min == max ? ("" + min) : ("" + min + "-" + max);
                if (ret.Length != 0) ret.Append("\n");
                ret.Append(type.ToString() + " " + typeLabel + " +" + increaseStr);
            }
        };
    }

    public override string getAdditionalTextForTarget(Targetable target)
    {
        StringBuilder ret = new StringBuilder();

        ForEachDifference(target, getDisplayTextHandler("Attack", ret), getDisplayTextHandler("Defense", ret));

        return ret.Length == 0 ? "--" : ret.ToString();
	}

	protected override void doActivate(Targetable target) {
        Action<DominationType, AttackSkill, int> handler = (type, skill, diff) => 
            {
                if (diff > 0) skill.value += getIncreaseAmount(diff);
            };
        ForEachDifference(target, handler, handler);

        // Decrease edge visibility modifier
        GraphUtility.instance.getConnectingEdge(gameObject.GetComponent<NodeData>(), (NodeData) target).visIncreaseModifier *= edgeVisModifierMultiplier;
	}

    private void ForEachDifference(Targetable target, Action<DominationType, AttackSkill, int> attackHandler, Action<DominationType, AttackSkill, int> defenseHandler)
    {
        NodeData node = (NodeData)target;
        NodeData thisNode = this.gameObject.GetComponent<NodeData>();

        Array values = Enum.GetValues(typeof(DominationType));
        foreach (DominationType type in values)
        {
            AttackSkill targetAttackSkill = node.getAttackSkill(type);
            AttackSkill thisAttackSkill = thisNode.getAttackSkill(type);
            int difference = thisAttackSkill.getWorkingValue() - targetAttackSkill.getWorkingValue();

            attackHandler(type, targetAttackSkill, difference);

            DefenseSkill targetDefenseSkill = node.getDefenseSkill(type);
            DefenseSkill thisDefenseSkill = thisNode.getDefenseSkill(type);
            difference = thisDefenseSkill.getWorkingValue() - targetDefenseSkill.getWorkingValue();

            defenseHandler(type, targetDefenseSkill, difference);
        }
    }

	private int getIncreaseAmount(int difference) {
        float min = getMinIncreaseAmount(difference);
        float max = getMaxIncreaseAmount(difference);
		double randomness = gen.NextDouble();
		return (int) (((max - min) * randomness) + min);
	}

    private float getMinIncreaseAmount(int difference)
    {
        return difference * minProportion;
    }

    private float getMaxIncreaseAmount(int difference)
    {
        return difference * maxProportion;
    }
}
