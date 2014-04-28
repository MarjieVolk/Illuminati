using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Graph.Nodes
{
    //They're too clever to be caught in the act, but maybe we can scare them off...
    //Deactivates an edge for a while
    public class StakeoutAction : Action
    {
        public int NumTurnsTargetDeactivated;
        public float MinimumTargetVisibility;
        public float ProbabilityZeroThreshold;
        public float GuaranteedSuccessThreshold;

        void Reset()
        {
            NumTurnsTargetDeactivated = 5;
            MinimumTargetVisibility = 0.25f;
            ProbabilityZeroThreshold = 0.25f;
            GuaranteedSuccessThreshold = 0.75f;
            PathVisibilityIncreaseProbability = 0.5f;
        }

        public override List<Targetable> getPossibleTargets()
        {
            //can target any edge that does not belong to you with sufficiently high visibility
            List<EdgeData> allEdges = new List<EdgeData>(FindObjectsOfType<EdgeData>());

            PlayerData currentPlayer = TurnController.instance.CurrentPlayer;

            IEnumerable<EdgeData> possibleTargetEdges = allEdges.Where<EdgeData>((x) =>
                {
                    if (x.Visibility < MinimumTargetVisibility) return false;
                    if (x.nodeOne.GetComponent<NodeData>().Owner == currentPlayer || x.nodeTwo.GetComponent<NodeData>().Owner == currentPlayer) return false;
                    return true;
                });

            return possibleTargetEdges.Select<EdgeData, Targetable>((x) => (Targetable)x).ToList<Targetable>();
        }

        public override string getAdditionalTextForTarget(Targetable target)
        {
            return (int)(100 * getSuccessProbability(target)) + "%";
        }

        protected override void doActivate(Targetable target)
        {
            throw new NotImplementedException();
        }

        private float getSuccessProbability(Targetable target)
        {
            float targetVisibility = ((EdgeData)target).Visibility;

            if (targetVisibility < ProbabilityZeroThreshold) return 0;
            if (targetVisibility >= GuaranteedSuccessThreshold) return 1;
            return (targetVisibility - ProbabilityZeroThreshold) / (GuaranteedSuccessThreshold - ProbabilityZeroThreshold);
        }
    }
}
