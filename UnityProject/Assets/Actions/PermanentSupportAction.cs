using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class PermanentSupportAction : Action {

    public float edgeVisModifierMultiplier = 0.9f;

	private const float MIN_PROPORTION = 0.3f;
	private const float MAX_PROPORTION = 0.7f;

	private static System.Random gen;

	void Start () {
		IsTargeting = true;
		if (gen == null) gen = new System.Random();
	}

	public override List<Targetable> getPossibleTargets() {
		NodeData thisNode = getNode();
		List<NodeData> nodes = GraphUtility.instance.getInfluencingNodes(thisNode);
		nodes.AddRange(GraphUtility.instance.getInfluencedNodes(thisNode));

		List<Targetable> targets = new List<Targetable>();
		foreach (NodeData node in nodes) {
			targets.Add(node);
		}
		return targets;
	}

    public override string getAdditionalTextForTarget(Targetable target)
    {
        NodeData node = getNode();
        NodeData other = (NodeData)target;
        int difference = node.getWorkingPower() - other.getWorkingPower();
        if (difference > 0) {
            int min = (int) getMinIncreaseAmount(difference);
            int max = (int) getMaxIncreaseAmount(difference);
            if (max > 0) {
                return "+" + (min == max ? ("" + min) : ("" + min + "-" + max));
            }
        }

        return "--";
	}

	protected override void doActivate(Targetable target) {
        NodeData node = getNode();
        doIncrease(node, (NodeData) target, 1.0f);

        // Decrease edge visibility modifier
        GraphUtility.instance.getConnectingEdge(node, (NodeData) target).visIncreaseModifier *= edgeVisModifierMultiplier;
	}

    public static void doIncrease(NodeData performer, NodeData target, float multiplier) {
        int difference = performer.getWorkingPower() - target.getWorkingPower();
        if (difference > 0) target.power += (int) (getIncreaseAmount(difference) * multiplier);
    }

	public static int getIncreaseAmount(int difference) {
        float min = getMinIncreaseAmount(difference);
        float max = getMaxIncreaseAmount(difference);
		double randomness = gen.NextDouble();
		return (int) (((max - min) * randomness) + min);
	}

    private static float getMinIncreaseAmount(int difference)
    {
        return difference * MIN_PROPORTION;
    }

    private static float getMaxIncreaseAmount(int difference)
    {
        return difference * MAX_PROPORTION;
    }
}
