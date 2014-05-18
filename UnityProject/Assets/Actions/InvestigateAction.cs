using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Assets.Graph;
using Assets.Player;

namespace Assets.Graph.Nodes
{
    public class InvestigateAction : Action
    {
        public static Texture2D MakeTextureOfColor(Color color)
        {
            var tex = new Texture2D(2, 2);
            var colors = new Color[4];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            tex.SetPixels(colors);
            tex.Apply();

            return tex;
        }

        void Start()
        {
            IsTargeting = true;
        }

        public float minVisIncrease, maxVisIncreaseUnowned, maxVisIncreaseOwned;

        //can target any edge that does not belong to you
        public override List<Targetable> getPossibleTargets()
        {
            List<EdgeData> allEdges = new List<EdgeData>(FindObjectsOfType<EdgeData>());
            List<EdgeData> notMyEdges = allEdges.Where<EdgeData>(
                (x) => EdgeData.EdgeDirection.Neutral == x.direction 
                    || (EdgeData.EdgeDirection.Neutral != x.direction && x.nodeOne.GetComponent<NodeData>().Owner != getNode().Owner)
                ).ToList<EdgeData>();

            Debug.Log(notMyEdges[0]);

            return notMyEdges.Select<EdgeData, Targetable>((x) => (Targetable) x).ToList<Targetable>();
        }

        protected override void doActivate(Targetable target)
        {
            //pick a random amount to increase visibility by
			EdgeData.EdgeDirection dir = ((EdgeData) target).direction;
			bool owned = dir == EdgeData.EdgeDirection.OneToTwo || dir == EdgeData.EdgeDirection.TwoToOne;
			float max = owned ? maxVisIncreaseOwned : maxVisIncreaseUnowned;
            float visibilityIncreaseAmount = UnityEngine.Random.value * (max - minVisIncrease) + minVisIncrease;

            ((EdgeData)target).Visibility += visibilityIncreaseAmount;
        }
		
		public override string getAdditionalTextForTarget(Targetable target) {
			return "";
		}
    }
}
