using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Graph.Nodes
{
    public class InvestAction : Action
    {
        public int ActionsPayoff;
        private const int duration = 2;

        void Reset()
        {
            ActionsPayoff = 2;
            CarryingEdgeVisibilityIncreaseProbability = 1;
            PathVisibilityIncreaseProbability = 0.5f;
        }

        void Start()
        {
            isTargeting = false;
        }

        protected override void doActivate(Targetable target)
        {
            //two turns from now, give the current player some extra actions to play with
            PlayerData playerOfInterest = TurnController.instance.CurrentPlayer;
            int numTurnsDelay = duration;
            System.Action handler = null;
            handler = () =>
            {
                if (TurnController.instance.CurrentPlayer == playerOfInterest)
                {
                    numTurnsDelay--;

                    if (0 == numTurnsDelay)
                    {
                        TurnController.instance.CurrentPlayer.addActionPoints(ActionsPayoff);
                        TurnController.instance.OnTurnEnd -= handler;
                    }
                }
            };

            TurnController.instance.OnTurnStart += handler;

            //but we aren't available until that day comes
            GetComponent<NodeData>().nTurnsUntilAvailable = duration;
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
