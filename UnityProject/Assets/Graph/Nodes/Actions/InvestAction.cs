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
        private const int duration = 2;

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
            //first, disable this action's button (and put this on cooldown)
            ActionButton realButton = GetComponent<NodeMenu>().buttons[this].GetComponent<ActionButton>();
            realButton.ActionEnabled = false;
            IsOnCooldown = true;

            //two turns from now, give the current player some extra actions to play with
            //and take this off cd
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
                        realButton.ActionEnabled = true;
                        IsOnCooldown = false;
                        TurnController.instance.OnTurnEnd -= handler;
                    }
                }
            };

            TurnController.instance.OnTurnStart += handler;
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
