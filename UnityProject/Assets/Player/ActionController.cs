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

        private Dictionary<Targetable, Targetable.OnClickHandler> clickHandlers;

        public void selectAction(Action selected)
        {
            inSelectionState = true;
			this.selected = selected;

            //if the action is targeted
            //ask for targets
            if (selected.isTargeting)
            {
                List<Targetable> possibleTargets = selected.getPossibleTargets();
                //highlight the possible targets
				possibleTargets.ForEach((x) => { x.setHighlighted(true); x.showTargetInfoText(selected.getAdditionalTextForTarget(x)); });

                //attach event handlers to the possible targets so they're selected when they're clicked
                possibleTargets.ForEach((x) => { clickHandlers[x] = () => scheduleAction(selected, x); x.OnClicked += clickHandlers[x]; });

                // TODO make a cancel button, attach an event handler to it

                return;
            }
            
            //no target needed
            //add the action to the player
            scheduleAction(selected, null);
        }

        void scheduleAction(Action toSchedule, Targetable target)
        {
            PlayerData currentPlayer = this.GetComponent<TurnController>().CurrentPlayer;
			if (currentPlayer.scheduleAction(toSchedule)) {
				toSchedule.SetScheduled(target);
			}

			clearSelectionState(toSchedule);
        }

        // Use this for initialization
        void Start()
        {
            clickHandlers = new Dictionary<Targetable, Targetable.OnClickHandler>();
            inSelectionState = false;
			instance = this;
        }

        // Update is called once per frame
        void Update()
        {
			if (inSelectionState && Input.GetMouseButtonDown(1)) {
				// Clicked on something else with action selected -- cancel selection
				clearSelectionState(selected);
				selected.gameObject.GetComponent<NodeMenu>().hide();
			}
        }

		/// <summary>
		/// Called after a selected action is either scheduled or cancelled.
		/// </summary>
		private void clearSelectionState(Action selected) {
			//the other targets should not be highlighted now
			foreach (Targetable noLongerATarget in selected.getPossibleTargets())
			{
				noLongerATarget.setHighlighted(false);
				noLongerATarget.OnClicked -= clickHandlers[noLongerATarget];
				noLongerATarget.hideTargetInfoText();
			}

			inSelectionState = false;
			selected = null;
		}
    }
}
