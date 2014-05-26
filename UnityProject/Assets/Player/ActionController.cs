using Assets.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Player
{
    public class ActionController : DependencyResolvingComponent
    {
		public static ActionController instance { get; private set; }

        public bool inSelectionState { get; private set; }
		public Action selected;

        private Dictionary<Targetable, EventHandlers> clickHandlers;

		private bool isFirstFrameAfterSelected = false;

        public void selectAction(Action selected)
        {
            inSelectionState = true;
			isFirstFrameAfterSelected = true;
			this.selected = selected;

            //if the action is targeted, ask for targets
            if (selected.IsTargeting)
            {
                List<Targetable> possibleTargets = selected.getPossibleTargets();
                possibleTargets.ForEach((x) => {
                    //highlight the possible targets
                    Tooltippable guiObj = x.GetComponent<Tooltippable>();
                    guiObj.setHighlighted(true);
                    guiObj.showTargetInfoText(selected.getAdditionalTextForTarget(x));

                    clickHandlers[x] = new EventHandlers();

                    //attach click event handlers to the possible targets so they're selected when they're clicked
                    System.Action clickAction = () => scheduleAction(x);
                    clickHandlers[x].click = clickAction;
                    guiObj.OnClicked += clickAction;

                    //attach hover event handlers to possible targets so that they display edge visibility data when hovered over
                    System.Action hoverAction = () => startHoverForTarget(x);
                    clickHandlers[x].hover = hoverAction;
                    guiObj.OnHover += hoverAction;
                    System.Action endHoverAction = () => endHoverForTarget(x);
                    clickHandlers[x].endHover = endHoverAction;
                    guiObj.OnEndHover += endHoverAction;
                });

                return;
            }
            
            //no target needed
            //add the action to the player
            scheduleAction(null);
        }

        public List<Targetable> getTargetsForScheduledAction() {
            return inSelectionState ? clickHandlers.Keys.ToList<Targetable>() : new List<Targetable>();
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
                selected.transform.parent.GetComponent<NodeMenu>().hide();
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
                Tooltippable guiObj = noLongerATarget.GetComponent<Tooltippable>();
                guiObj.setHighlighted(false);
                guiObj.OnClicked -= clickHandlers[noLongerATarget].click;
                guiObj.OnHover -= clickHandlers[noLongerATarget].hover;
                guiObj.OnEndHover -= clickHandlers[noLongerATarget].endHover;
                guiObj.hideTargetInfoText();
			}

            clickHandlers = new Dictionary<Targetable, EventHandlers>();
            selected.transform.parent.GetComponent<NodeMenu>().clear();

			inSelectionState = false;
			selected = null;
		}

        private void startHoverForTarget(Targetable target) {
            NodeData actionNode = selected.getNode();
            float carryingEdgeExpected = selected.expectedVisibilityModifier();
            EdgeData carryingEdge = null;

            if (target is NodeData) {
                if (GraphUtility.instance.getConnectedNodes(actionNode).Contains((NodeData)target)) {
                    carryingEdge = GraphUtility.instance.getConnectingEdge(actionNode, (NodeData)target);
                    carryingEdgeExpected += carryingEdge.getExpectedVisibilityIncrease(selected.CarryingEdgeVisibilityIncreaseScaleParameter, selected.CarryingEdgeMaxVisibilityIncrease, true);
                }
            }
            bool didCarryingEdge = false;

            Action.VisitEdgesBetweenNodesWithVisibility(TurnController.CurrentPlayer.StartingNode, actionNode, selected.PathVisibilityIncreaseScaleParameter,
                (edge, increaseScaleParameter) => {
                    float expected = edge.getExpectedVisibilityIncrease(increaseScaleParameter, edge.getMaxEdgeVisibilityIncrease(selected.PathMaxVisibilityIncrease));
                    bool isCarryingEdge = edge != null && edge == carryingEdge;
                    if (isCarryingEdge) {
                        expected += carryingEdgeExpected;
                        didCarryingEdge = true;
                    }

                    showEdgeText(edge, expected);
                }
            );

            if (carryingEdge != null && !didCarryingEdge) {
                showEdgeText(carryingEdge, carryingEdgeExpected);
            }
        }

        private void endHoverForTarget(Targetable target) {
            NodeData actionNode = selected.getNode();
            HashSet<EdgeData> edges = GraphUtility.instance.getEdgesBetweenNodes(TurnController.CurrentPlayer.StartingNode, actionNode);
            foreach (EdgeData edge in edges) {
                edge.GetComponent<Tooltippable>().hideTargetInfoText();
            }

            if (target is NodeData && GraphUtility.instance.getConnectedNodes(actionNode).Contains((NodeData)target)) {
                GraphUtility.instance.getConnectingEdge(actionNode, (NodeData)target).GetComponent<Tooltippable>().hideTargetInfoText();
            }
        }

        private void showEdgeText(EdgeData edge, float expectedIncrease) {
            expectedIncrease = Mathf.Max(Mathf.Min(expectedIncrease, 1 - edge.Visibility), -edge.Visibility);

            if (expectedIncrease == 0) {
                return;
            }

            string val = "<size=12>";
            if (expectedIncrease > 0)
            {
                val += "+" + Mathf.CeilToInt(expectedIncrease * 100);
            }
            else if (expectedIncrease < 0)
            {
                val += "-" + Mathf.FloorToInt(expectedIncrease * -100);
            }

            val += "%</size>";
            edge.GetComponent<Tooltippable>().showTargetInfoText(val);
        }

        private class EventHandlers {
            public System.Action click, hover, endHover;
        }
    }
}
