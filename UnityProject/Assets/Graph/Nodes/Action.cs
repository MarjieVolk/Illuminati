using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Assets.Player;

public abstract class Action : MonoBehaviour {

	public GameObject button;
    public float PathVisibilityIncreaseProbability;
    public float CarryingEdgeVisibilityIncreaseProbability;
	
	public bool isTargeting { get; set;}
    private Targetable target;

	public GameObject getButton() {
		return button;
	}

    public void Activate() {
		doActivate(target);
		clearScheduled();
        GraphUtility.instance.TidyGraph();

        //do applicable visibility increases
        if (isTargeting && target.GetType() == typeof(NodeData))
        {
            if (GraphUtility.instance.getConnectedNodes(GetComponent<NodeData>()).Contains((NodeData)target))
            {
                GraphUtility.instance.getConnectingEdge(GetComponent<NodeData>(), (NodeData)target).triggerEdge(CarryingEdgeVisibilityIncreaseProbability);
            }
        }
        IncreaseVisibilityBetweenNodes(TurnController.instance.CurrentPlayer.StartingNode, GetComponent<NodeData>(), PathVisibilityIncreaseProbability);
	}
	
	public abstract List<Targetable> getPossibleTargets();
	public abstract string getAdditionalTextForTarget(Targetable target);
	protected abstract void doActivate(Targetable target);

    public bool SetScheduled(Targetable target)
    {
        if (isTargeting && null == target) return false;
        if (!isTargeting && null != target) return false;
        if (isTargeting && !getPossibleTargets().Contains(target)) return false;

        this.target = target;

		gameObject.GetComponent<NodeMenu>().clear();
		gameObject.GetComponent<NodeMenu>().hide();
		gameObject.GetComponent<NodeMenu>().isScheduled = true;

        return true;
    }

    public void clearScheduled()
    {
        target = null;
		gameObject.GetComponent<NodeMenu>().isScheduled = false;
	}

    public static void IncreaseVisibilityBetweenNodes(NodeData source, NodeData target, float baseIncreaseProbability)
    {
        //find all the edges that are between the source and target nodes
        HashSet<EdgeData> betwixtEdgesClosure = GraphUtility.instance.getEdgesBetweenNodes(source, target);
        List<NodeData> sortedNodes = GraphUtility.instance.TopologicalSortOnEdgeSubset(source, betwixtEdgesClosure);

        //split the visibility increase over all the possible paths
        Dictionary<NodeData, float> increaseProbabilities = new Dictionary<NodeData,float>();
        increaseProbabilities[source] = baseIncreaseProbability;
        foreach(NodeData currentNode in sortedNodes)
        {
            //evenly divide visibility increase likelihood over all child nodes (thus implicitly edges) in the closure
            List<NodeData> futureNodes = GraphUtility.instance.getInfluencedNodes(currentNode);
            //only this in the closure!
            futureNodes.RemoveAll((x) => !sortedNodes.Contains(x));

            float increaseProbability = increaseProbabilities[currentNode] / futureNodes.Count;

            //go ahead and apply this visibility, it's safe b/c we've accumulated everything from all this guy's ancestors (yay topo sort!)
            foreach (EdgeData edge in futureNodes.Select<NodeData, EdgeData>((x) => GraphUtility.instance.getConnectingEdge(currentNode, x)))
            {
                edge.triggerEdge(increaseProbability);
            }

            //propagate visibility to descendants
            foreach (NodeData node in futureNodes)
            {
                Debug.Log(increaseProbability + " likelihood of visibility increase for edge from " + currentNode + " to " + node);
                if (!increaseProbabilities.ContainsKey(node)) increaseProbabilities[node] = 0.0f;
                increaseProbabilities[node] += increaseProbability;
            }
        }
    }

    private static void recIncreaseVisibilityBetweenNodes(NodeData origin, HashSet<EdgeData> validEdges, float increaseProbability, Dictionary<NodeData, float> increaseProbabilities)
    {

    }
}
