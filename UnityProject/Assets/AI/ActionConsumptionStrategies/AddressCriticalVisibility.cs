using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.AI.ActionConsumptionStrategies
{
    public class AddressCriticalVisibility : ActionConsumptionStrategy
    {
        public float CriticalVisibilityThreshold;
        public bool UseTemporarySupportActions;

        public override List<Action> consumeActions(int numActionsRemaining)
        {
            PlayerData me = TurnController.instance.CurrentPlayer;

            //fetch all the edges
            IEnumerable<EdgeData> myEdges = FindObjectsOfType<EdgeData>()
                //that are mine
                .Where<EdgeData>((edge) => edge.nodeOne.GetComponent<NodeData>().Owner == me && edge.nodeTwo.GetComponent<NodeData>().Owner == me)
                //and order them by visibility
                .OrderByDescending<EdgeData, float>((edge) => edge.Visibility);

            

            //mitigate visibility using temporary support actions where possible and necessary
            List<Action> ret = new List<Action>();
            foreach (EdgeData myEdge in myEdges)
            {
                //if this, the most-visible remaining edge, is not too visible, we're done
                if (myEdge.Visibility < CriticalVisibilityThreshold) return ret;

                //choose the node that will be doing the assisting
                NodeData assistant, assistee;
                if (!myEdge.nodeOne.GetComponent<NodeMenu>().isScheduled)
                {
                    assistee = myEdge.nodeTwo.GetComponent<NodeData>();
                    assistant = myEdge.nodeOne.GetComponent<NodeData>();
                }
                else if (!myEdge.nodeTwo.GetComponent<NodeMenu>().isScheduled)
                {
                    assistee = myEdge.nodeOne.GetComponent<NodeData>();
                    assistant = myEdge.nodeTwo.GetComponent<NodeData>();
                }
                else continue;

                //and schedule the assist action
                Action supportAction = UseTemporarySupportActions ? (Action) assistant.GetComponent<TemporarySupportAction>() : (Action) assistant.GetComponent<PermanentSupportAction>();
                supportAction.SetScheduled(assistee);
                ret.Add(supportAction);
                numActionsRemaining--;
                if (numActionsRemaining <= 0) return ret;
            }

            return ret;
        }
    }
}
