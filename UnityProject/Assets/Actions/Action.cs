using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Assets.Player;
using Assets;

public abstract class Action : DependencyResolvingComponent {

    // *****
    // Unity Editor configuration
    // *****

    public float CarryingEdgeVisibilityIncreaseScaleParameter = 0.04f;
    public float CarryingEdgeMaxVisibilityIncrease = 0.10f;

    public float PathVisibilityIncreaseScaleParameter = 0.02f;
    public float PathMaxVisibilityIncrease = 0.10f;

    // *****
    // Runtime configuration
    // *****
	
    // set by the subclass in Start or Awake
	public bool IsTargeting { get; set;}

    // *****
    // State
    // *****

    public System.Action<Action> OnStateUpdate;

    public Targetable Target { get; private set; }
    public bool IsOnCooldown { get; private set; }

    void Awake()
    {
        IsOnCooldown = false;
    }

    public void Activate() {
		doActivate(Target);
        graphUtility.TidyGraph();
        //do applicable visibility increases
        if (IsTargeting && Target.GetType() == typeof(NodeData))
        {
            if (graphUtility.getConnectedNodes(getNode()).Contains((NodeData)Target))
            {
                graphUtility.getConnectingEdge(getNode(), (NodeData)Target).applyRandomEdgeVisibilityIncrease(CarryingEdgeVisibilityIncreaseScaleParameter, CarryingEdgeMaxVisibilityIncrease);
            }
        }
        graphUtility.VisitEdgesBetweenNodesWithVisibility(turnController.CurrentPlayer.StartingNode, getNode(), PathVisibilityIncreaseScaleParameter,
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
        if (IsTargeting && null == target) return false;
        if (!IsTargeting && null != target) return false;
        if (IsTargeting && !getPossibleTargets().Contains(target)) return false;

        this.Target = target;
        getNode().isScheduled = true;
        if (target != null) target.addScheduledAction(this);

        if (OnStateUpdate != null) OnStateUpdate(this);

        return true;
    }

    public void clearScheduled()
    {
        if (Target != null) Target.removeScheduledAction(this);
        Target = null;
		getNode().isScheduled = false;
	}

    public void putOnCooldown(int nTurns) {
        putOnCooldown(nTurns, () => { });
    }

    public void putOnCooldown(int nTurns, System.Action onReactivate) {
        // Put on CD, notify people of that
        IsOnCooldown = true;

        if (OnStateUpdate != null) OnStateUpdate(this);

        // DURATION turns from now, take this off cd
        PlayerData playerOfInterest = turnController.CurrentPlayer;
        int numTurnsDelay = nTurns;
        System.Action handler = null;
        handler = () => {
            if (turnController.CurrentPlayer == playerOfInterest) {
                numTurnsDelay--;

                if (0 == numTurnsDelay) {
                    IsOnCooldown = false;
                    if (OnStateUpdate != null) OnStateUpdate(this);
                    turnController.OnTurnEnd -= handler;

                    onReactivate();
                }
            }
        };

        turnController.OnTurnStart += handler;
    }

    public NodeData getNode() {
        return transform.parent.GetComponent<NodeData>();
    }
}
