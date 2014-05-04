using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Player
{
    public class ActionController : MonoBehaviour
    {
		public static ActionController instance { get; private set; }

        public bool inSelectionState { get; private set; }
		private Action selected;

        private Dictionary<Targetable, EventHandlers> clickHandlers;

		private bool isFirstFrameAfterSelected = false;

        public void selectAction(Action selected)
        {
            inSelectionState = true;
			isFirstFrameAfterSelected = true;
			this.selected = selected;

            //if the action is targeted, ask for targets
            if (selected.isTargeting)
            {
                List<Targetable> possibleTargets = selected.getPossibleTargets();
                possibleTargets.ForEach((x) => {
                    //highlight the possible targets
                    x.setHighlighted(true);
                    x.showTargetInfoText(selected.getAdditionalTextForTarget(x));

                    clickHandlers[x] = new EventHandlers();

                    //attach click event handlers to the possible targets so they're selected when they're clicked
                    System.Action clickAction = () => scheduleAction(x);
                    clickHandlers[x].click = clickAction;
                    x.OnClicked += clickAction;

                    //attach hover event handlers to possible targets so that they display edge visibility data when hovered over
                    System.Action hoverAction = () => startHoverForTarget(x);
                    clickHandlers[x].hover = hoverAction;
                    x.OnHover += hoverAction;
                    System.Action endHoverAction = () => endHoverForTarget(x);
                    clickHandlers[x].endHover = endHoverAction;
                    x.OnEndHover += endHoverAction;
                });

                return;
            }
            
            //no target needed
            //add the action to the player
            scheduleAction(null);
        }

        public void scheduleAction(Targetable target)
		{
            endHoverForTarget(target);

            PlayerData currentPlayer = this.GetComponent<TurnController>().CurrentPlayer;
			if (currentPlayer.scheduleAction(selected)) {
				selected.SetScheduled(target);
			}

			clearSelectionState();
        }

        // Use this for initialization
        void Start()
        {
            clickHandlers = new Dictionary<Targetable, EventHandlers>();
            inSelectionState = false;
			instance = this;
        }

        // Update is called once per frame
        void Update()
        {
			if (inSelectionState && !isFirstFrameAfterSelected && (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))) {
                // Clicked on something else with action selected -- cancel selection
                selected.gameObject.GetComponent<NodeMenu>().hide();
                clearSelectionState();
			}
			isFirstFrameAfterSelected = false;
        }

		/// <summary>
		/// Called after a selected action is either scheduled or cancelled.
		/// </summary>
		public void clearSelectionState() {
			//the other targets should not be highlighted now
			foreach (Targetable noLongerATarget in selected.getPossibleTargets())
			{
				noLongerATarget.setHighlighted(false);
                noLongerATarget.OnClicked -= clickHandlers[noLongerATarget].click;
                noLongerATarget.OnHover -= clickHandlers[noLongerATarget].hover;
                noLongerATarget.OnEndHover -= clickHandlers[noLongerATarget].endHover;
				noLongerATarget.hideTargetInfoText();
			}

            selected.gameObject.GetComponent<NodeMenu>().clear();

			inSelectionState = false;
			selected = null;
		}

        private void startHoverForTarget(Targetable target) {
            NodeData actionNode = selected.gameObject.GetComponent<NodeData>();
            float carryingEdgeMax = selected.maxVisibilityModifier();
            float carryingEdgeMin = selected.minVisibilityModifier();
            EdgeData carryingEdge = null;

            if (target is NodeData) {
                if (GraphUtility.instance.getConnectedNodes(actionNode).Contains((NodeData)target)) {
                    carryingEdge = GraphUtility.instance.getConnectingEdge(actionNode, (NodeData)target);
                    carryingEdgeMax += selected.CarryingEdgeVisibilityIncreaseProbability > 0 ? carryingEdge.maxVisibilityIncrease : 0;
                }
            }
            bool didCarryingEdge = !actionNode;

            Debug.LogError("Starting visit for " + TurnController.instance.CurrentPlayer.StartingNode + " to " + actionNode);
            Action.VisitEdgesBetweenNodesWithVisibility(TurnController.instance.CurrentPlayer.StartingNode, actionNode, selected.PathVisibilityIncreaseProbability,
                (edge, increaseProbability) => {
                    Debug.LogError("Visiting " + edge);
                    float max = increaseProbability > 0 ? edge.maxVisibilityIncrease : 0;
                    bool isCarryingEdge = edge != null && edge == carryingEdge;
                    if (isCarryingEdge) {
                        max += carryingEdgeMax;
                        didCarryingEdge = true;
                    }

                    showEdgeText(edge, max, isCarryingEdge ? carryingEdgeMin : 0);
                }
            );

            if (carryingEdge != null && !didCarryingEdge) {
                showEdgeText(carryingEdge, carryingEdgeMax, carryingEdgeMin);
            }
        }

        private void endHoverForTarget(Targetable target) {
            NodeData actionNode = selected.gameObject.GetComponent<NodeData>();
            HashSet<EdgeData> edges = GraphUtility.instance.getEdgesBetweenNodes(TurnController.instance.CurrentPlayer.StartingNode, actionNode);
            foreach (EdgeData edge in edges) {
                edge.hideTargetInfoText();
            }

            if (target is NodeData && GraphUtility.instance.getConnectedNodes(actionNode).Contains((NodeData)target)) {
                GraphUtility.instance.getConnectingEdge(actionNode, (NodeData)target).hideTargetInfoText();
            }
        }

        private void showEdgeText(EdgeData edge, float maxIncrease, float minIncrease) {
            if (maxIncrease == 0 && minIncrease == 0) {
                return;
            }

            string val = "<size=12>";
            if (minIncrease >= 0) {
                val += "+ " + (int)(minIncrease * 100) + (maxIncrease == minIncrease ? "" : "-" + (int)(maxIncrease * 100));
            } else if (maxIncrease <= 0) {
                val += "- " + (int)(maxIncrease * -100) + (maxIncrease == minIncrease ? "" : "-" + (int)(minIncrease * -100));
            } else {
                // This is probably hideous, but it should never happen as of right now
                val += (int)(minIncrease * 100) + " to +" + (int)(maxIncrease * 100);
            }

            val += "% visibility</size>";
            edge.showTargetInfoText(val);
        }

        private class EventHandlers {
            public System.Action click, hover, endHover;
        }
    }
}
