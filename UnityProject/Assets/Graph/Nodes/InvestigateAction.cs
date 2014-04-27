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
        void Start()
        {
            isTargeting = true;
        }

        public float minVisibilityIncrease, maxVisibilityIncrease;

        //can target any edge that does not belong to you
        public override List<Highlightable> getPossibleTargets()
        {
            List<EdgeData> allEdges = new List<EdgeData>(FindObjectsOfType<EdgeData>());
            List<EdgeData> notMyEdges = allEdges.Where<EdgeData>(
                (x) => EdgeData.EdgeDirection.Neutral == x.direction 
                    || (EdgeData.EdgeDirection.Neutral != x.direction && x.nodeOne.GetComponent<NodeData>().Owner != GetComponent<NodeData>().Owner)
                ).ToList<EdgeData>();

            Debug.Log(notMyEdges[0]);

            return notMyEdges.Select<EdgeData, Highlightable>((x) => (Highlightable) x).ToList<Highlightable>();
        }

        protected override void doActivate(Highlightable target)
        {
            //pick a random amount to increase visibility by
            float visibilityIncreaseAmount = UnityEngine.Random.value * (maxVisibilityIncrease - minVisibilityIncrease) + minVisibilityIncrease;

            ((EdgeData)target).Visibility += visibilityIncreaseAmount;
        }
    }
}
