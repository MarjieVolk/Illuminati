using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Assets.Player;

public abstract class Action : MonoBehaviour {

	public GameObject button;
	public GameObject scheduledTag;

    public float CarryingEdgeVisibilityIncreaseScaleParameter = 0.04f;
    public float CarryingEdgeMaxVisibilityIncrease = 0.10f;

    public float PathVisibilityIncreaseScaleParameter = 0.02f;
    public float PathMaxVisibilityIncrease = 0.10f;
	
	public bool isTargeting { get; set;}
	public Targetable Target { get; private set; }

    public bool IsOnCooldown = false;

	private GameObject listTag, mapTag;

	void Start() {
	}

	public GameObject getButton() {
		return button;
	}

	public GameObject getListScheduledTag() {
		if (listTag == null) {
			initTags();
		}
		return listTag;
	}

	public GameObject getMapScheduledTag() {
		if (mapTag == null) {
			initTags();
		}
		return mapTag;
	}

	private void initTags() {
		mapTag = instantiateTag();
		listTag = instantiateTag();
		mapTag.GetComponent<ScheduledAction>().setSister(listTag.GetComponent<ScheduledAction>());
	}

	private GameObject instantiateTag() {
		GameObject tag = (GameObject) Instantiate(scheduledTag, new Vector3(0, 0, -10), Quaternion.identity);
		tag.GetComponent<ScheduledAction>().action = this;
		tag.SetActive(false);
		return tag;
	}

    public void Activate() {
		doActivate(Target);
        GraphUtility.instance.TidyGraph();
        //do applicable visibility increases
        if (isTargeting && Target.GetType() == typeof(NodeData))
        {
            if (GraphUtility.instance.getConnectedNodes(GetComponent<NodeData>()).Contains((NodeData)Target))
            {
                GraphUtility.instance.getConnectingEdge(GetComponent<NodeData>(), (NodeData)Target).applyRandomEdgeVisibilityIncrease(CarryingEdgeVisibilityIncreaseScaleParameter, CarryingEdgeMaxVisibilityIncrease);
            }
        }
        VisitEdgesBetweenNodesWithVisibility(TurnController.instance.CurrentPlayer.StartingNode, GetComponent<NodeData>(), PathVisibilityIncreaseScaleParameter,
            (edge, increaseScaleParameter) => { edge.applyRandomEdgeVisibilityIncrease(increaseScaleParameter, PathMaxVisibilityIncrease); });
		
		clearScheduled();
	}
	
	public abstract List<Targetable> getPossibleTargets();
	public abstract string getAdditionalTextForTarget(Targetable target);
	protected abstract void doActivate(Targetable target);

    public virtual float expectedVisibilityModifier() {
        return 0;
    }

    public bool SetScheduled(Targetable target)
    {
        if (isTargeting && null == target) return false;
        if (!isTargeting && null != target) return false;
        if (isTargeting && !getPossibleTargets().Contains(target)) return false;

        this.Target = target;
        gameObject.GetComponent<NodeMenu>().isScheduled = true;
        if (target != null) target.addScheduledAction(this);

        if (TurnController.instance.CurrentPlayer.IsLocalHumanPlayer)
        {
            gameObject.GetComponent<NodeMenu>().clear();
            gameObject.GetComponent<NodeMenu>().hide();

            GameObject tag = getMapScheduledTag();
            tag.SetActive(true);
            tag.transform.position = gameObject.transform.position + new Vector3(0.5f, 0.3f, -1);
        }

        return true;
    }

    public void clearScheduled()
    {
        if (Target != null) Target.removeScheduledAction(this);
        Target = null;
		gameObject.GetComponent<NodeMenu>().isScheduled = false;
	}

    public void putOnCooldown(int nTurns) {
        putOnCooldown(nTurns, () => { });
    }

    public void putOnCooldown(int nTurns, System.Action onReactivate) {
        // First, disable this action's button (and put this on cooldown)
        ActionButton realButton = GetComponent<NodeMenu>().buttons[this].GetComponent<ActionButton>();
        realButton.ActionEnabled = false;
        IsOnCooldown = true;

        // DURATION turns from now, take this off cd
        PlayerData playerOfInterest = TurnController.instance.CurrentPlayer;
        int numTurnsDelay = nTurns;
        System.Action handler = null;
        handler = () => {
            if (TurnController.instance.CurrentPlayer == playerOfInterest) {
                numTurnsDelay--;

                if (0 == numTurnsDelay) {
                    realButton.ActionEnabled = true;
                    IsOnCooldown = false;
                    TurnController.instance.OnTurnEnd -= handler;

                    onReactivate();
                }
            }
        };

        TurnController.instance.OnTurnStart += handler;
    }

    /// <summary>
    /// Visit each player controlled edge along all possible paths from source to target.  For each edge, the visitor will recieve (1) the EdgeData for
    /// the edge being visited and (2) the probability for a visibility increase on that edge.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="baseIncreaseProbability"></param>
    /// <param name="doStuffHere"></param>
    public static void VisitEdgesBetweenNodesWithVisibility(NodeData source, NodeData target, float baseIncreaseProbability, System.Action<EdgeData, float> visitor)
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
                visitor(edge, increaseProbability);
            }

            //propagate visibility to descendants
            foreach (NodeData node in futureNodes)
            {
                if (!increaseProbabilities.ContainsKey(node)) increaseProbabilities[node] = 0.0f;
                increaseProbabilities[node] += increaseProbability;
            }
        }
    }
}
