using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Assets.Graph;

namespace Assets.Graph.Nodes
{
    public class InvestigateAction : Action
    {
        public float minVisibilityIncrease, maxVisibilityIncrease;

        //can target any edge that does not belong to you
        public override List<Highlightable> getPossibleTargets()
        {
            return (List<Highlightable>) new List<EdgeData>(FindObjectsOfType<EdgeData>()).Where<EdgeData>(
                (x) => EdgeData.EdgeDirection.Neutral != x.direction 
                    && x.nodeOne.GetComponent<NodeData>().Owner != GetComponent<NodeData>().Owner
                );
        }

        protected override void doActivate(Highlightable target)
        {
            //pick a random amount to increase visibility by
            float visibilityIncreaseAmount = UnityEngine.Random.value * (maxVisibilityIncrease - minVisibilityIncrease) + minVisibilityIncrease;

            ((EdgeData)target).Visibility += visibilityIncreaseAmount;
        }
    }
}
