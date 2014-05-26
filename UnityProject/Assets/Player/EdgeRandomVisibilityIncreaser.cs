using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Player
{
    public class EdgeRandomVisibilityIncreaser : DependencyResolvingComponent
    {
        public float ScaleParameter;
        public float MaxVisibilityIncrease;

        public void Start()
        {
            TurnController.OnTurnEnd += () =>
            {
                foreach (EdgeData edge in FindObjectsOfType<EdgeData>())
                {
                    edge.applyRandomEdgeVisibilityIncrease(ScaleParameter, MaxVisibilityIncrease);
                }
            };
        }
    }
}
