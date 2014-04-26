using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Player
{
    public class ActionController : MonoBehaviour
    {
        private Action selectedAction;

        public void selectAction(Action selected)
        {
            //if the action is targeted
            //ask for targets
            if (selected.isTargeting)
            {
                //highlight the possible targets
                selected.getPossibleTargets().ForEach((x) => x.setHighlighted());

                //attach event handlers to the possible targets so they're selected when they're clicked
                selected.getPossibleTargets().ForEach((x) => x.OnClicked += () => scheduleAction(selected, x));

                // TODO make a cancel button, attach an event handler to it

                return;
            }
            
            //no target needed
            //add the action to the player
            scheduleAction(selected, null);
        }

        void scheduleAction(Action toSchedule, Highlightable target)
        {
            PlayerData currentPlayer = this.GetComponent<TurnController>().CurrentPlayer;

            toSchedule.SetScheduled(target);
            if (!currentPlayer.scheduleAction(toSchedule)) toSchedule.clearScheduled();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
