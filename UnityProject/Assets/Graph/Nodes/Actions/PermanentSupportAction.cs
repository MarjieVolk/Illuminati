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

    public override string getAdditionalTextForTarget(Targetable target)
    {
        NodeData node = this.gameObject.GetComponent<NodeData>();
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
        NodeData node = this.gameObject.GetComponent<NodeData>();
        NodeData other = (NodeData) target;
        int difference = node.getWorkingPower() - other.getWorkingPower();
        if (difference > 0) other.power += getIncreaseAmount(difference);

        // Decrease edge visibility modifier
        GraphUtility.instance.getConnectingEdge(gameObject.GetComponent<NodeData>(), (NodeData) target).visIncreaseModifier *= edgeVisModifierMultiplier;
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
