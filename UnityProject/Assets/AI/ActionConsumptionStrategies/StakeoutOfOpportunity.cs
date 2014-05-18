using Assets.Graph.Nodes;
using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.AI.ActionConsumptionStrategies
{
    public class StakeoutOfOpportunity : ActionConsumptionStrategy
    {
        public float VictoryProbabilityThreshold;

        public override List<Action> consumeActions(int numActionsRemaining)
        {
            PlayerData me = TurnController.CurrentPlayer;

            //fetch all the free gov't nodes
            List<NodeData> myGovtNodes = FindObjectsOfType<NodeData>()
                .Where<NodeData>((node) => node.Owner == me 
                    && node.GetComponent<StakeoutAction>() != null 
                    && !node.isScheduled)
                .ToList<NodeData>();

            //fetch all the edges
            IEnumerable<EdgeData> candidateTargets = FindObjectsOfType<EdgeData>()
                // that aren't mine
                .Where<EdgeData>((edge) => edge.nodeOne.GetComponent<NodeData>().Owner != me || edge.nodeTwo.GetComponent<NodeData>().Owner != me)
                // and aren't already blocked
                .Where<EdgeData>((edge)=> edge.direction != EdgeData.EdgeDirection.Unusable)
                // sorted by visibility
                .OrderByDescending<EdgeData, float>((edge) => edge.Visibility);

            //target from highest to lowest vis until they're too low or we have no more gov't nodes
            List<Action> ret = new List<Action>();
            foreach (EdgeData candidate in candidateTargets)
            {
                if (numActionsRemaining <= 0) break;

                //also, pick the best gov't node first, just in case they vary...
                if(myGovtNodes.Count == 0) return ret;
                float bestSuccessChance = myGovtNodes[0].GetComponent<StakeoutAction>().getSuccessProbability(candidate);
                NodeData bestNode = myGovtNodes[0];
                foreach (NodeData govtNode in myGovtNodes)
                {
                    float successChance = govtNode.GetComponent<StakeoutAction>().getSuccessProbability(candidate);
                    if (successChance > bestSuccessChance)
                    {
                        bestSuccessChance = successChance;
                        bestNode = govtNode;
                    }
                }

                if (bestSuccessChance > VictoryProbabilityThreshold)
                {
                    Action selected = bestNode.GetComponent<StakeoutAction>();
                    selected.SetScheduled(candidate);
                    myGovtNodes.Remove(bestNode);
                    ret.Add(selected);
                    numActionsRemaining--;
                }
            }

            return ret;
        }
    }
}
