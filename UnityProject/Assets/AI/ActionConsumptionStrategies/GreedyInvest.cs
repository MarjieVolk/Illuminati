using Assets.Graph.Nodes;
using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.AI.ActionConsumptionStrategies
{
    public class GreedyInvest : ActionConsumptionStrategy
    {

        public override List<Action> consumeActions(int numActionsRemaining)
        {
            List<Action> ret = new List<Action>();
            List<NodeData> ownedNodes = FindObjectsOfType<NodeData>().Where<NodeData>((node) => node.Owner == turnController.CurrentPlayer).ToList<NodeData>();

            foreach (NodeData node in ownedNodes)
            {
                if (!node.isScheduled)
                {
                    InvestAction action = node.gameObject.GetComponent<InvestAction>();
                    if (action != null && !action.IsOnCooldown)
                    {
                        action.SetScheduled(null);
                        ret.Add(action);
                    }
                }
            }

            return ret;
        }
    }
}
