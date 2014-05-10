using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Graph.Nodes
{
    public class InvestAction : Action
    {
        public int ActionsPayoff;
        private const int DURATION = 2;

        void Reset()
        {
            ActionsPayoff = 2;
            CarryingEdgeVisibilityIncreaseScaleParameter = 1;
            PathVisibilityIncreaseScaleParameter = 0.5f;
        }

        void Start()
        {
            isTargeting = false;
        }

        protected override void doActivate(Targetable target)
        {
            putOnCooldown(DURATION, () => {
                TurnController.instance.CurrentPlayer.addActionPoints(ActionsPayoff);
            });
        }

        public override List<Targetable> getPossibleTargets()
        {
            return new List<Targetable>();
        }

        public override string getAdditionalTextForTarget(Targetable target)
        {
            return "";
        }
    }
}
