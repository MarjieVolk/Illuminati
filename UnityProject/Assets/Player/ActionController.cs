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

        public void selectAction(Action toSelect)
        {
            //if the action is targeted
            //ask for targets
            if (toSelect.isTargeting)
            {
                //highlight the possible targets
                toSelect.getPossibleTargets().ForEach((x) => x.setHighlighted());

                //attach event handlers to the possible targets so they're selected when they're clicked
                toSelect.getPossibleTargets().ForEach((x) => x.OnClicked += () => toSelect.scheduleUse(x));

                // TODO make a cancel button, attach an event handler to it

                return;
            }
            
            //no target needed
            //add the action to the player
            toSelect.scheduleUse(null);
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
