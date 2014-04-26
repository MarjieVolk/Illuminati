﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Player
{
    public class ActionController : MonoBehaviour
    {
        private Action selectedAction;

        private Dictionary<Highlightable, Highlightable.OnClickHandler> clickHandlers;

        public void selectAction(Action selected)
        {
            //if the action is targeted
            //ask for targets
            if (selected.isTargeting)
            {
                List<Highlightable> possibleTargets = selected.getPossibleTargets();
                //highlight the possible targets
                possibleTargets.ForEach((x) => x.setHighlighted());

                //attach event handlers to the possible targets so they're selected when they're clicked
                possibleTargets.ForEach((x) => { clickHandlers[x] = () => scheduleAction(selected, x); x.OnClicked += clickHandlers[x]; });

                // TODO make a cancel button, attach an event handler to it

                return;
            }
            
            //no target needed
            //add the action to the player
            scheduleAction(selected, null);
        }

        void scheduleAction(Action toSchedule, Highlightable target)
        {
            //the other targets should not be highlighted now
            foreach (Highlightable noLongerATarget in toSchedule.getPossibleTargets())
            {
                noLongerATarget.setUnhighlighted();
                noLongerATarget.OnClicked -= clickHandlers[noLongerATarget];
            }

            PlayerData currentPlayer = this.GetComponent<TurnController>().CurrentPlayer;

            toSchedule.SetScheduled(target);
            if (!currentPlayer.scheduleAction(toSchedule)) toSchedule.clearScheduled();
        }

        // Use this for initialization
        void Start()
        {
            clickHandlers = new Dictionary<Highlightable, Highlightable.OnClickHandler>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
