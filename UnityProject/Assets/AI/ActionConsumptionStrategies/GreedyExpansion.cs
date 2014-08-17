using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eppy;
using Assets.Player;
using UnityEngine;

namespace Assets.AI.ActionConsumptionStrategies
{
    /// <summary>
    /// Spends its first action on the surest bet for capture.
    /// Spends its second action on the second-surest bet.
    /// Etc.
    /// </summary>
    public class GreedyExpansion : ActionConsumptionStrategy
    {
        public override List<Action> consumeActions(int numActionsRemaining)
        {
            //get the list of attack options we've got
            List<Tuple<NodeData, NodeData, AttackAction, double>> attackPossibilities = generateInitialAttackPossibilities(turnController.CurrentPlayer);

            //put them in a priority queue to keep them sorted by probability of adding a new node
            PriorityQueue<Tuple<NodeData, NodeData, AttackAction, double>, double> queue = new PriorityQueue<Tuple<NodeData, NodeData, AttackAction, double>, double>();
            foreach (var attackPossibility in attackPossibilities)
            {
                queue.Enqueue(attackPossibility, 1.0 - attackPossibility.Item4);
            }

            //keep track of the likelihood that each target node will be captured, for adjusting the keys in the priority queue
            //this ensures the AI will spread its efforts if it makes sense to do so (and won't if it doesn't!)
            Dictionary<NodeData, double> targetedNodeCaptureProbabilities = new Dictionary<NodeData, double>();

            //finally, we're going to build the return list
            List<Action> ret = new List<Action>();
            while(queue.Count() > 0)
            {
                var valueKeyPair = queue.Dequeue();
                var value = valueKeyPair.Item1;
                double key = valueKeyPair.Item2;

                //if this action can still be performed
                if (!value.Item1.isScheduled)
                {
                    //how likely is it that we'll get this node anyways, without executing this action?
                    double currentCaptureProbability = 0;
                    if (targetedNodeCaptureProbabilities.ContainsKey(value.Item2))
                    {
                        currentCaptureProbability = targetedNodeCaptureProbabilities[value.Item2];
                        //back into the queue if the key is out of date! (doesn't account for existing attempts to capture the target node)
                        if (key > currentCaptureProbability + 0.001)
                        {
                            queue.Enqueue(value, currentCaptureProbability);
                            continue;
                        }
                    }

                    //now this is definitely the best thing to do, so let's do it
                    value.Item3.SetScheduled(value.Item2);
                    ret.Add(value.Item3);

                    //update the likelihood that we'll get this node next turn
                    double newCaptureProbability = (1 - currentCaptureProbability) * value.Item4;
                    targetedNodeCaptureProbabilities[value.Item2] = newCaptureProbability;

                    numActionsRemaining--;
                    if (numActionsRemaining <= 0) break;
                }
            }

            return ret;
        }

        private static List<Tuple<NodeData, NodeData, AttackAction, double>> generateInitialAttackPossibilities(PlayerData attackingPlayer)
        {
            List<Tuple<NodeData, NodeData, AttackAction, double>> attackPossibilities = new List<Tuple<NodeData, NodeData, AttackAction, double>>();

            IEnumerable<NodeData> myNodes = FindObjectsOfType<NodeData>()
                .Where<NodeData>((node) => node.Owner == attackingPlayer)
                .Where<NodeData>((node) => !node.isScheduled);

            foreach (NodeData myNode in myNodes)
            {
                foreach (AttackAction attackAction in myNode.GetComponents<AttackAction>())
                {
                    foreach (NodeData notMyNode in attackAction.getPossibleTargets())
                    {
                        double successProbability = attackAction.getProbabilityOfWin(notMyNode);
                        attackPossibilities.Add(new Tuple<NodeData, NodeData, AttackAction, double>(myNode, notMyNode, attackAction, successProbability));
                    }
                }
            }
            return attackPossibilities;
        }
    }
}
